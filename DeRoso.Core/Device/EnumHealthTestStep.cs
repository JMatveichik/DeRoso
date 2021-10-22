using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Device
{
    public enum EnumHealthTestStep
    {
        [Description("Не активен")]
        None = 0x01,

        [Description("Запущен")]
        Started = 0x02,

        [Description("Ожидание измерения")]
        WaitMeassure = 0x03,

        [Description("Измерение перед")]
        MeassureBefore = 0x04,

        [Description("Выдача препарата")]
        DrugDespencing = 0x05,

        [Description("Измерение после")]
        MeassureAfter = 0x06,

        [Description("Ожидание импульса HV")]
        WaitHV = 0x07,

        [Description("Импульс HV")]
        ImpulseHV = 0x08,

        [Description("Завершен")]
        Complete = 0x09,

        [Description("Провален")]
        Failed = 0x10
    }
}
