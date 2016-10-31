using System.Runtime.InteropServices;

namespace Bcm2835
{
    internal static class Native
    {
        private const string Lib = "bcm2835.so";

        [DllImport( Lib )]
        public static extern int bcm2835_init();

        [DllImport( Lib )]
        public static extern int bcm2835_close();

        [DllImport( Lib )]
        public static extern uint bcm2835_version();

        [DllImport( Lib )]
        public static extern void bcm2835_gpio_fsel( byte pin, byte mode );

        [DllImport( Lib )]
        public static extern void bcm2835_gpio_set( byte pin );

        [DllImport( Lib )]
        public static extern void bcm2835_gpio_clr( byte pin );

        [DllImport( Lib )]
        public static extern void bcm2835_gpio_write( byte pin, bool on );

        [DllImport( Lib )]
        public static extern void bcm2835_delay( uint millis );

        [DllImport( Lib )]
        public static extern void bcm2835_delayMicroseconds( ulong micros );

        [DllImport( Lib )]
        public static extern void bcm2835_pwm_set_clock( uint divisor );

        [DllImport( Lib )]
        public static extern void bcm2835_pwm_set_mode( byte channel, bool markspace, bool enabled );

        [DllImport( Lib )]
        public static extern void bcm2835_pwm_set_range( byte channel, uint range );

        [DllImport( Lib )]
        public static extern void bcm2835_pwm_set_data( byte channel, uint data );
    }
}
