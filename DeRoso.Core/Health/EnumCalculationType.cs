using DeRoso.Core.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
