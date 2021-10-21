using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
