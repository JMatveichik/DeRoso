using System.ComponentModel;

namespace DeRoso.Core.Device
{
    public enum DeviceCommands : byte
    {
        [Description ("Включить импульс")]
        ImpulsOn = 0x40,

        [Description("Выключить импульс")]
        ImpulsOff = 0x41,

        [Description("Включить селектор")]
        SelectorOn = 0x42,

        [Description("Задание частоты HV")]
        SetFrequency = 0x44,

        [Description("Выключить селектор")]
        SelectorOff = 0x43,

        [Description("Включить диагностику")]
        DiagnosticOn = 0x45,

        [Description("Добавить препарат")]
        AddDrug = 0x46,

        ImpulsTest = 0x47,
        OutDrugStart = 0x48,
        OutDrugStop = 0x49,
        ReleCalibrationOn = 0x4a,
        ReleCalibrationOff = 0x4b,
        ReleSelectorOn = 0x4c,
        ReleSelectorOff = 0x4d,
        ReleDiagnosticOn = 0x4e,
        ReleDiagnosticOff = 0x4f,


        MeteringOn = 0x60,
        MeteringOff = 0x61,
        AllOff = 0x62,
        SelectorTest = 0x63,
        ReadSelectorTest = 0x64
    }
}
