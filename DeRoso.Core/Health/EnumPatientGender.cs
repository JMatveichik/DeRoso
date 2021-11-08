using System.ComponentModel;

namespace DeRoso.Core.Health
{
    public enum EnumPatientGender
    {
        [Description("Мужской")]
        Male = 0x01,

        [Description("Женский")]
        Female = 0x02
    }
}
