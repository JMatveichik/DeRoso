namespace UsbLibrary
{
    public class SpecifiedInputReport : InputReport
    {
        private byte[] _arrData;

        public SpecifiedInputReport(HidDevice oDev) : base(oDev)
        {

        }

        public override void ProcessData()
        {
            this._arrData = Buffer;
        }

        public byte[] Data
        {
            get
            {
                return _arrData;
            }
        }
    }
}
