namespace Bcm2835
{
    public enum GpioFunction : byte
    {
        Input = 0x00,
        Output = 0x01,
        Alt0 = 0x04,
        Alt1 = 0x05,
        Alt2 = 0x06,
        Alt3 = 0x07,
        Alt4 = 0x03,
        Alt5 = 0x02,
        Mask = 0x07
    }

    public static class Gpio
    {
        public static void SetFunction( GpioPin pin, GpioFunction function )
        {
            Native.bcm2835_gpio_fsel( pin, (byte) function );
        }

        public static void Set( GpioPin pin )
        {
            Native.bcm2835_gpio_set( pin );
        }

        public static void Clear( GpioPin pin )
        {
            Native.bcm2835_gpio_clr( pin );
        }

        public static void Write( GpioPin pin, bool value )
        {
            Native.bcm2835_gpio_write( pin, value );
        }
    }
}
