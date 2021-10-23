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

        [Description("Измерение перед")]
        MeassureBefore = 0x03,

        [Description("Ожидание перед выдачей препарата")]
        WaitBeforeDrugDespencing = 0x04,        

        [Description("Выдача препарата")]
        DrugDespencing = 0x05,

        [Description("Ожидание после выдачи препарата")]
        WaitAfterDrugDespencing = 0x06,

        [Description("Измерение после")]
        MeassureAfter = 0x07,

        [Description("Ожидание импульса HV")]
        WaitHV = 0x08,

        [Description("Импульс HV")]
        ImpulseHV = 0x09,

        [Description("Тест завершен")]
        Complete = 0x10,

        [Description("Провален")]
        Failed = 0x11
    }
}
