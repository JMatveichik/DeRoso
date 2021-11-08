namespace UsbLibrary
{
    public class SpecifiedOutputReport : OutputReport
    {
        public SpecifiedOutputReport(HidDevice oDev) : base(oDev)
        {

        }

        public bool SendData(byte[] data)
        {
            byte[] arrBuff = Buffer; //new byte[Buffer.Length];
            for (int i = 1; i < arrBuff.Length; i++)
            {
                arrBuff[i] = data[i - 1];
            }

            //Buffer = arrBuff;

            //returns false if the data does not fit in the buffer. else true
            if (arrBuff.Length < data.Length)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
