using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Device
{
    public class DeviceException : Exception
    {
        public DeviceException(string message,  byte[] data) : base(message)
        {
            DeviceData = data;
        }

        public byte[] DeviceData
        {
            get;
            private set;
        }
    }
}
