namespace UsbLibrary
{
    /// <summary>
    /// Base class for report types. Simply wraps a byte buffer.
    /// </summary>
    public abstract class Report
    {
        #region Member variables
        /// <summary>Buffer for raw report bytes</summary>
        private byte[] _mArrBuffer;
        /// <summary>Length of the report</summary>
        private int _mNLength;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oDev">Constructing device</param>
        public Report(HidDevice oDev)
        {
            // Do nothing
        }

        /// <summary>
        /// Sets the raw byte array.
        /// </summary>
        /// <param name="arrBytes">Raw report bytes</param>
        protected void SetBuffer(byte[] arrBytes)
        {
            _mArrBuffer = arrBytes;
            _mNLength = _mArrBuffer.Length;
        }

        /// <summary>
        /// Accessor for the raw byte buffer
        /// </summary>
        public byte[] Buffer
        {
            get => _mArrBuffer;
            set { this._mArrBuffer = value; }
        }
        /// <summary>
        /// Accessor for the buffer length
        /// </summary>
        public int BufferLength => _mNLength;
    }
    /// <summary>
    /// Defines a base class for output reports. To use output reports, just put the bytes into the raw buffer.
    /// </summary>
    public abstract class OutputReport : Report
    {
        /// <summary>
        /// Construction. Setup the buffer with the correct output report length dictated by the device
        /// </summary>
        /// <param name="oDev">Creating device</param>
        public OutputReport(HidDevice oDev) : base(oDev)
        {
            SetBuffer(new byte[oDev.OutputReportLength]);
        }
    }
    /// <summary>
    /// Defines a base class for input reports. To use input reports, use the SetData method and override the 
    /// ProcessData method.
    /// </summary>
    public abstract class InputReport : Report
    {
        /// <summary>
        /// Construction. Do nothing
        /// </summary>
        /// <param name="oDev">Creating device</param>
        public InputReport(HidDevice oDev) : base(oDev)
        {
        }
        /// <summary>
        /// Call this to set the buffer given a raw input report. Calls an overridable method to
        /// should automatically parse the bytes into meaningul structures.
        /// </summary>
        /// <param name="arrData">Raw input report.</param>
        public void SetData(byte[] arrData)
        {
            SetBuffer(arrData);
            ProcessData();
        }
        /// <summary>
        /// Override this to process the input report into something useful
        /// </summary>
        public abstract void ProcessData();
    }
}
