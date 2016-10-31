namespace Bcm2835
{
    public enum PwmChannel : byte
    {
        Channel0,
        Channel1
    }

    public enum PwmClockDivisor : uint
    {
        Div2048 = 2048,
        Div1024 = 1024,
        Div512 = 512,
        Div256 = 256,
        Div128 = 128,
        Div64 = 64,
        Div32 = 32,
        Div16 = 16,
        Div8 = 8,
        Div4 = 4,
        Div2 = 2,
        Div1 = 1
    }

    public static class Pwm
    {
        public static void SetClock( PwmClockDivisor divisor )
        {
            Native.bcm2835_pwm_set_clock( (uint) divisor );
        }

        public static void SetMode( PwmChannel channel, bool markspace, bool enabled )
        {
            Native.bcm2835_pwm_set_mode( (byte) channel, markspace, enabled );
        }

        public static void SetRange( PwmChannel channel, uint range )
        {
            Native.bcm2835_pwm_set_range( (byte) channel, range );
        }

        public static void SetData( PwmChannel channel, uint data )
        {
            Native.bcm2835_pwm_set_data( (byte) channel, data );
        }
    }
}
