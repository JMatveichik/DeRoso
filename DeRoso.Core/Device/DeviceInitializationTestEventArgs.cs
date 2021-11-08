using System;

namespace DeRoso.Core.Device
{
    public class DeviceInitializationTestEventArgs : EventArgs
    {
        public DeviceInitializationTestEventArgs(string initTestDescription, DeviceInitializationTestState state)
        {
            TestDescription = initTestDescription;
            State = state;
        }

        public string TestDescription
        {
            get;
            private set;
        }

        public DeviceInitializationTestState State
        {
            get;
            private set;
        }

    }
}
