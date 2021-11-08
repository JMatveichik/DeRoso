using DeRoso.Core.Converters;
using System.ComponentModel;

namespace DeRoso.Core.Health
{
    [TypeConverter(typeof(EnumDescriptionConverter))]
    public enum EnumCalculationType : byte
    {
        [Description ("Минимальный")]
        Minimum = 0x01,

        [Description("Средний")]
        Medium  = 0x02,

        [Description("Максимальный")]
        Maximum = 0x03
    }
}
