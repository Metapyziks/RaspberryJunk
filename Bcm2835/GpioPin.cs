using System;

namespace Bcm2835
{
    public enum RpiV1Gpio : byte
    {
        Port1Pin3 = 0,
        Port1Pin5 = 1,
        Port1Pin7 = 4,
        Port1Pin8 = 14,
        Port1Pin10 = 15,
        Port1Pin11 = 17,
        Port1Pin12 = 18,
        Port1Pin13 = 21,
        Port1Pin15 = 22,
        Port1Pin16 = 23,
        Port1Pin18 = 24,
        Port1Pin19 = 10,
        Port1Pin21 = 9,
        Port1Pin22 = 25,
        Port1Pin23 = 11,
        Port1Pin24 = 8,
        Port1Pin26 = 7
    }
    
    public enum RpiV2Gpio : byte
    {
        Port1Pin3 = 2,
        Port1Pin5 = 3,
        Port1Pin7 = 4,
        Port1Pin8 = 14,
        Port1Pin10 = 15,
        Port1Pin11 = 17,
        Port1Pin12 = 18,
        Port1Pin13 = 27,
        Port1Pin15 = 22,
        Port1Pin16 = 23,
        Port1Pin18 = 24,
        Port1Pin19 = 10,
        Port1Pin21 = 9,
        Port1Pin22 = 25,
        Port1Pin23 = 11,
        Port1Pin24 = 8,
        Port1Pin26 = 7,
        Port1Pin29 = 5,
        Port1Pin31 = 6,
        Port1Pin32 = 12,
        Port1Pin33 = 13,
        Port1Pin35 = 19,
        Port1Pin36 = 16,
        Port1Pin37 = 26,
        Port1Pin38 = 20,
        Port1Pin40 = 21,
        Port5Pin03 = 28,
        Port5Pin04 = 29,
        Port5Pin05 = 30,
        Port5Pin06 = 31
    }

    public struct GpioPin : IEquatable<GpioPin>
    {
        public static bool operator ==( GpioPin a, GpioPin b )
        {
            return a.Value == b.Value;
        }

        public static bool operator !=( GpioPin a, GpioPin b )
        {
            return a.Value != b.Value;
        }

        public static implicit operator byte( GpioPin pin )
        {
            return pin.Value;
        }

        public static explicit operator GpioPin( byte value )
        {
            return new GpioPin( value );
        }

        public static implicit operator GpioPin( RpiV1Gpio value )
        {
            return new GpioPin( (byte) value );
        }

        public static implicit operator GpioPin( RpiV2Gpio value )
        {
            return new GpioPin( (byte) value );
        }

        public readonly byte Value;

        internal GpioPin( byte value )
        {
            Value = value;
        }

        public bool Equals( GpioPin other )
        {
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override bool Equals( object obj )
        {
            return obj is GpioPin && Equals( (GpioPin) obj );
        }
    }
}
