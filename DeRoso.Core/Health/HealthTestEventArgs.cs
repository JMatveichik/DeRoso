using System;

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
