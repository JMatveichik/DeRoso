using DeRoso.Core.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Health
{
    public class HealthTestEventArgs : EventArgs
    {
        public HealthTestEventArgs(HealthTestItem test, EnumHealthTestStep step, TimeSpan left)
        {
            TestItem = test;
            CurrentStep = step;
            OperationLeftTime = left;

        }

        public HealthTestItem TestItem
        {
            get;
            private set;
        }

        public EnumHealthTestStep CurrentStep
        {
            get;
            private set;
        }

        public TimeSpan OperationLeftTime
        {
            get;
            private set;
        }
    }
}
