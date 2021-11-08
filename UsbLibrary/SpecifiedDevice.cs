using System;

namespace UsbLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class DataRecievedEventArgs : EventArgs
    {

        public readonly byte[] Data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public DataRecievedEventArgs(byte[] data)
        {
            this.Data = data;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataSendEventArgs : EventArgs
    {
        public readonly byte[] Data;

        public DataSendEventArgs(byte[] data)
        {
            this.Data = data;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void DataRecievedEventHandler(object sender, DataRecievedEventArgs args);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void DataSendEventHandler(object sender, DataSendEventArgs args);

    /// <summary>
    /// 
    /// </summary>
    public class SpecifiedDevice : HidDevice
    {
        /// <summary>
        /// 
        /// </summary>
        public event DataRecievedEventHandler DataRecieved;
        /// <summary>
        /// 
        /// </summary>
        public event DataSendEventHandler DataSend;


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override InputReport CreateInputReport() => new SpecifiedInputReport(this);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static SpecifiedDevice FindSpecifiedDevice(int vendorId, int productId) => (SpecifiedDevice)FindDevice(vendorId, productId, typeof(SpecifiedDevice));


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oInRep"></param>
        protected override void HandleDataReceived(InputReport oInRep)
        {
            // Fire the event handler if assigned
            if (DataRecieved == null)
                return;

            SpecifiedInputReport report = (SpecifiedInputReport)oInRep;
            DataRecieved(this, new DataRecievedEventArgs(report.Data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SendData(byte[] data)
        {
            SpecifiedOutputReport oRep = new SpecifiedOutputReport(this);	// create output report
            oRep.SendData(data);	// set the lights states
            try
            {
                Write(oRep); // write the output report
                DataSend?.Invoke(this, new DataSendEventArgs(data));
            }
            catch (HidDeviceException ex)
            {
                // Device may have been removed!
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bDisposing"></param>
        protected override void Dispose(bool bDisposing)
        {
            if (bDisposing)
            {
                // to do's before exit
            }
            base.Dispose(bDisposing);
        }
    }
}
