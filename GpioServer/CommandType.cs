namespace GpioServer
{
    enum CommandType : byte
    {
        Toggle,
        Set,
        StartPwm,
        SetPwmRange,
        SetPwmData,
        StopPwm,
        Disconnect = 254,
        StopServer = 255
    }
}
