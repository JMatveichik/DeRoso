using System;

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
