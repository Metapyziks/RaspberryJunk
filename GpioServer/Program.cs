using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Bcm2835;

namespace GpioServer
{
    class Program : IDisposable
    {
        static void Main( string[] args )
        {
            using ( var program = new Program() )
            {
                program.Run();
            }
        }

        class Client
        {
            private readonly Program Program;
            private readonly TcpClient TcpClient;
            private readonly BinaryReader Reader;
            private readonly EndPoint EndPoint;

            public Client( Program program, TcpClient client )
            {
                Program = program;
                TcpClient = client;
                EndPoint = TcpClient.Client.RemoteEndPoint;
                Reader = new BinaryReader( TcpClient.GetStream() );
            }

            public bool Update()
            {
                if ( !TcpClient.Connected ) return false;

                while ( TcpClient.Available > 0 )
                {
                    switch ( (CommandType) Reader.ReadByte() )
                    {
                        case CommandType.Toggle:
                            Program.Toggle( (GpioPin) Reader.ReadByte() );
                            break;
                        case CommandType.Set:
                            Program.Set( (GpioPin) Reader.ReadByte(), Reader.ReadBoolean() );
                            break;
                        case CommandType.StartPwm:
                            Program.StartPwm();
                            break;
                        case CommandType.SetPwmRange:
                            Program.SetPwmRange( Reader.ReadUInt32() );
                            break;
                        case CommandType.SetPwmData:
                            Program.SetPwmData( Reader.ReadUInt32() );
                            break;
                        case CommandType.StopPwm:
                            Program.StopPwm();
                            break;
                        case CommandType.Disconnect:
                            TcpClient.Close();
                            return false;
                        case CommandType.StopServer:
                            Program.Stop();
                            break;
                    }
                }

                return true;
            }

            public override string ToString()
            {
                return EndPoint.ToString();
            }
        }

        readonly TcpListener Listener = new TcpListener( IPAddress.Any, 28015 );

        readonly List<Client> Disconnected = new List<Client>(); 
        readonly List<Client> Clients = new List<Client>();

        readonly Dictionary<GpioPin, GpioFunction> ConnectedPins = new Dictionary<GpioPin, GpioFunction>();
        readonly Dictionary<GpioPin, bool> PinStates = new Dictionary<GpioPin, bool>();

        private int ClientCount => Clients.Count;

        private bool Listening = false;

        void AcceptClient()
        {
            var client = new Client( this, Listener.AcceptTcpClient() );
            Console.WriteLine($"Client connected: {client}");
            Clients.Add( client );
        }

        void RemoveClient( Client client )
        {
            Console.WriteLine($"Client disconnected: {client}");
            Clients.Remove( client );
        }

        void UpdateClients()
        {
            foreach ( var client in Clients )
            {
                if ( !client.Update() )
                {
                    Disconnected.Add( client );
                }
            }

            foreach ( var client in Disconnected )
            {
                RemoveClient( client );
            }

            Disconnected.Clear();
        }

        void EnsurePinFunction( GpioPin pin, GpioFunction function )
        {
            GpioFunction oldFunction;
            if ( ConnectedPins.TryGetValue( pin, out oldFunction ) )
            {
                if ( oldFunction == function ) return;
                ConnectedPins[pin] = function;
            }
            else
            {
                ConnectedPins.Add( pin, function );
                PinStates.Add( pin, false );
            }

            Gpio.SetFunction( pin, function );
        }

        private bool GetNoEnsure( GpioPin pin )
        {
            bool value;
            return PinStates.TryGetValue( pin, out value ) && value;
        }

        public void Toggle( GpioPin pin )
        {
            Set( pin, !GetNoEnsure( pin ) );
        }
        
        public void Set( GpioPin pin, bool state )
        {
            EnsurePinFunction( pin, GpioFunction.Output );

            PinStates[pin] = state;
            Gpio.Write( pin, state );
        }

        public void StartPwm()
        {
            EnsurePinFunction( RpiV1Gpio.Port1Pin12, GpioFunction.Alt5);
            Pwm.SetClock( PwmClockDivisor.Div16 );
            Pwm.SetMode( PwmChannel.Channel0, true, true );
        }

        public void SetPwmRange( uint range )
        {
            EnsurePinFunction( RpiV1Gpio.Port1Pin12, GpioFunction.Alt5);
            Pwm.SetRange( PwmChannel.Channel0, range );
        }

        public void SetPwmData( uint data )
        {
            EnsurePinFunction( RpiV1Gpio.Port1Pin12, GpioFunction.Alt5);
            Pwm.SetData( PwmChannel.Channel0, data );
        }

        public void StopPwm()
        {
            EnsurePinFunction( RpiV1Gpio.Port1Pin12, GpioFunction.Alt5);
            Pwm.SetMode( PwmChannel.Channel0, false, false );
        }

        public void Stop()
        {
            Listening = false;
        }

        public void Run()
        {
            if ( !Rpi.Initialize() )
            {
                throw new Exception("Unable to initialize Bcm2835.");
            }

            Listening = true;
            Listener.Start();

            while ( Listening )
            {
                while ( Listener.Pending() )
                {
                    AcceptClient();
                }

                UpdateClients();

                Thread.Sleep( ClientCount == 0 ? 250 : 10 );
            }

            Listening = false;
            Listener.Stop();

            if ( !Rpi.Close() )
            {
                throw new Exception("Unable to close Bcm2835.");
            }
        }

        public void Dispose()
        {
            Listener.Stop();
        }
    }
}
