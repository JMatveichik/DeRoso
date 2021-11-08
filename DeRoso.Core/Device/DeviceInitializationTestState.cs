using System.ComponentModel;

namespace DeRoso.Core.Device
{
    public enum DeviceInitializationTestState
    {
        [Description("Тест не начат")]
        None = 0x00,

        [Description("Тест запущен")]
        Started = 0x01,

        [Description("Тест удачно закончен")]
        Complete = 0x02,

        [Description("Тест провален")]
        Failed = 0x03

    }
}
