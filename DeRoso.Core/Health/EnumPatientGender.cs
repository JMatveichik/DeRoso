using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
