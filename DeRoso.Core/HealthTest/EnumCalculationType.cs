using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Health
{
    public enum EnumCalculationType : byte
    {
        [Description ("Вычислять минимальный показатель")]
        Minimum = 0x01,

        [Description("Вычислять средний показатель")]
        Medium  = 0x02,

        [Description("Вычислять максимальный показатель")]
        Maximum = 0x03
    }
}
