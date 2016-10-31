using System;

namespace Bcm2835
{
    public static class Rpi
    {
        public static bool Initialize()
        {
            return Native.bcm2835_init() != 0;
        }

        public static bool Close()
        {
            return Native.bcm2835_close() != 0;
        }

        public static void Delay( TimeSpan time )
        {
            Native.bcm2835_delayMicroseconds( (ulong) (time.TotalMilliseconds*1000) );
        }

        public static uint Version => Native.bcm2835_version();
    }
}
