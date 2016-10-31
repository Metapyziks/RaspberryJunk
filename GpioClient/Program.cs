using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Bcm2835;
using GpioServer;

namespace GpioClient
{
    class Program : IDisposable
    {
        static void Main( string[] args )
        {
            using ( var program = new Program() )
            {
                program.Connect( "192.168.0.5", 28015 );
                
                program.StartPwm();
                program.SetPwmRange( 1024 );

                var timer = new Stopwatch();
                timer.Start();

                while ( timer.Elapsed.TotalSeconds < 10 )
                {
                    var time = timer.Elapsed.TotalSeconds;
                    var value = Math.Sin( time*Math.PI/2 ) * 0.5 + 0.5;

                    Console.WriteLine( (uint) (value*1024) );
                    program.SetPwmData( (uint) (value*1024) );

                    Thread.Sleep( 10 );
                }

                program.StopPwm();
            }
        }

        private readonly TcpClient Client = new TcpClient();

        private BinaryWriter Writer;

        public void Toggle( GpioPin pin )
        {
            Writer.Write( (byte) CommandType.Toggle );
            Writer.Write( (byte) pin );
            Writer.Flush();
        }

        public void Set( GpioPin pin, bool state )
        {
            Writer.Write( (byte) CommandType.Set );
            Writer.Write( (byte) pin );
            Writer.Write( state );
            Writer.Flush();
        }

        public void StartPwm()
        {
            Writer.Write( (byte) CommandType.StartPwm );
            Writer.Flush();
        }

        public void SetPwmRange( uint range )
        {
            Writer.Write( (byte) CommandType.SetPwmRange );
            Writer.Write( range );
            Writer.Flush();
        }

        public void SetPwmData( uint data )
        {
            Writer.Write( (byte) CommandType.SetPwmData );
            Writer.Write( data );
            Writer.Flush();
        }

        public void StopPwm()
        {
            Writer.Write( (byte) CommandType.StopPwm );
            Writer.Flush();
        }

        public void StopServer()
        {
            Writer.Write( (byte) CommandType.StopServer );
            Writer.Flush();
        }

        public void Connect(string hostname, int port)
        {
            Client.Connect( hostname, port );
            Writer = new BinaryWriter( Client.GetStream() );
        }

        public void Close()
        {
            if ( !Client.Connected ) return;

            Writer.Write( (byte) CommandType.Disconnect );
            Client.Close();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
