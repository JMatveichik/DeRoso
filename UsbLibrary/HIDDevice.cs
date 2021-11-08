using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace UsbLibrary
{
    #region Custom exception
    /// <summary>
    /// Generic HID device exception
    /// </summary>
    public class HidDeviceException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strMessage"></param>
        public HidDeviceException(string strMessage) : base(strMessage)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        public static HidDeviceException GenerateWithWinError(string strMessage)
        {
            return new HidDeviceException(string.Format("Msg:{0} WinEr:{1:X8}", strMessage, Marshal.GetLastWin32Error()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        public static HidDeviceException GenerateError(string strMessage)
        {
            return new HidDeviceException(string.Format("Msg:{0}", strMessage));
        }
    }
    #endregion
    /// <summary>
    /// Abstract HID device : Derive your new device controller class from this
    /// </summary>
    public abstract class HidDevice : Win32Usb, IDisposable
    {
        #region Privates variables
        /// <summary>Filestream we can use to read/write from</summary>
        private FileStream _mOFile;
        /// <summary>Length of input report : device gives us this</summary>
        private int _mNInputReportLength;
        /// <summary>Length if output report : device gives us this</summary>
        private int _mNOutputReportLength;
        /// <summary>Handle to the device</summary>
        private IntPtr _mHHandle;
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Disposer called by both dispose and finalise
        /// </summary>
        /// <param name="bDisposing">True if disposing</param>
        protected virtual void Dispose(bool bDisposing)
        {
            try
            {
                if (bDisposing)	// if we are disposing, need to close the managed resources
                {
                    if (_mOFile != null)
                    {
                        _mOFile.Close();
                        _mOFile = null;
                    }
                }
                if (_mHHandle != IntPtr.Zero)	// Dispose and finalize, get rid of unmanaged resources
                {

                    CloseHandle(_mHHandle);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region Privates/protected
        /// <summary>
        /// Initialises the device
        /// </summary>
        /// <param name="strPath">Path to the device</param>
        private void Initialise(string strPath)
        {
            // Create the file from the device path
            _mHHandle = CreateFile(strPath, GenericRead | GenericWrite, 0, IntPtr.Zero, OpenExisting, FileFlagOverlapped, IntPtr.Zero);

            if (_mHHandle != InvalidHandleValue || _mHHandle == null)   // if the open worked...
            {
                IntPtr lpData;
                if (HidD_GetPreparsedData(_mHHandle, out lpData))   // get windows to read the device data into an internal buffer
                {
                    try
                    {
                        HidCaps oCaps;
                        HidP_GetCaps(lpData, out oCaps);	// extract the device capabilities from the internal buffer
                        _mNInputReportLength = oCaps.InputReportByteLength;	// get the input...
                        _mNOutputReportLength = oCaps.OutputReportByteLength;	// ... and output report lengths

                        //m_oFile = new FileStream(m_hHandle, FileAccess.Read | FileAccess.Write, true, m_nInputReportLength, true);
                        _mOFile = new FileStream(new SafeFileHandle(_mHHandle, false), FileAccess.Read | FileAccess.Write, _mNInputReportLength, true);

                        BeginAsyncRead();	// kick off the first asynchronous read                              
                    }
                    catch (Exception ex)
                    {
                        throw HidDeviceException.GenerateWithWinError("Failed to get the detailed data from the hid.");
                    }
                    finally
                    {
                        HidD_FreePreparsedData(ref lpData); // before we quit the funtion, we must free the internal buffer reserved in GetPreparsedData
                    }
                }
                else    // GetPreparsedData failed? Chuck an exception
                {
                    throw HidDeviceException.GenerateWithWinError("GetPreparsedData failed");
                }
            }
            else    // File open failed? Chuck an exception
            {
                _mHHandle = IntPtr.Zero;
                throw HidDeviceException.GenerateWithWinError("Failed to create device file");
            }
        }
        /// <summary>
        /// Kicks off an asynchronous read which completes when data is read or when the device
        /// is disconnected. Uses a callback.
        /// </summary>
        private void BeginAsyncRead()
        {
            byte[] arrInputReport = new byte[_mNInputReportLength];
            // put the buff we used to receive the stuff as the async state then we can get at it when the read completes

            _mOFile.BeginRead(arrInputReport, 0, _mNInputReportLength, new AsyncCallback(ReadCompleted), arrInputReport);
        }
        /// <summary>
        /// Callback for above. Care with this as it will be called on the background thread from the async read
        /// </summary>
        /// <param name="iResult">Async result parameter</param>
        protected void ReadCompleted(IAsyncResult iResult)
        {
            byte[] arrBuff = (byte[])iResult.AsyncState;	// retrieve the read buffer
            try
            {
                _mOFile.EndRead(iResult);	// call end read : this throws any exceptions that happened during the read
                try
                {
                    InputReport oInRep = CreateInputReport();   // Create the input report for the device
                    oInRep.SetData(arrBuff);	// and set the data portion - this processes the data received into a more easily understood format depending upon the report type
                    HandleDataReceived(oInRep);	// pass the new input report on to the higher level handler
                }
                finally
                {
                    BeginAsyncRead();	// when all that is done, kick off another read for the next report
                }
            }
            catch (IOException ex)	// if we got an IO exception, the device was removed
            {
                HandleDeviceRemoved();
                if (OnDeviceRemoved != null)
                {
                    OnDeviceRemoved(this, new EventArgs());
                }
                Dispose();
            }
        }
        /// <summary>
        /// Write an output report to the device.
        /// </summary>
        /// <param name="oOutRep">Output report to write</param>
        protected void Write(OutputReport oOutRep)
        {
            try
            {
                _mOFile.Write(oOutRep.Buffer, 0, oOutRep.BufferLength);
            }
            catch (IOException ex)
            {
                //Console.WriteLine(ex.ToString());
                // The device was removed!
                throw new HidDeviceException("Probbaly the device was removed");
            }
            catch (Exception exx)
            {
                Console.WriteLine(exx.ToString());
            }
        }
        /// <summary>
        /// virtual handler for any action to be taken when data is received. Override to use.
        /// </summary>
        /// <param name="oInRep">The input report that was received</param>
        protected virtual void HandleDataReceived(InputReport oInRep)
        {
        }
        /// <summary>
        /// Virtual handler for any action to be taken when a device is removed. Override to use.
        /// </summary>
        protected virtual void HandleDeviceRemoved()
        {
        }
        /// <summary>
        /// Helper method to return the device path given a DeviceInterfaceData structure and an InfoSet handle.
        /// Used in 'FindDevice' so check that method out to see how to get an InfoSet handle and a DeviceInterfaceData.
        /// </summary>
        /// <param name="hInfoSet">Handle to the InfoSet</param>
        /// <param name="oInterface">DeviceInterfaceData structure</param>
        /// <returns>The device path or null if there was some problem</returns>
        private static string GetDevicePath(IntPtr hInfoSet, ref DeviceInterfaceData oInterface)
        {
            uint nRequiredSize = 0;
            // Get the device interface details
            if (!SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, IntPtr.Zero, 0, ref nRequiredSize, IntPtr.Zero))
            {
                DeviceInterfaceDetailData oDetail = new DeviceInterfaceDetailData();
                oDetail.Size = 5;   // hardcoded to 5! Sorry, but this works and trying more future proof versions by setting the size to the struct sizeof failed miserably. If you manage to sort it, mail me! Thx
                if (SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, ref oDetail, nRequiredSize, ref nRequiredSize, IntPtr.Zero))
                {
                    return oDetail.DevicePath;
                }
            }
            return null;
        }
        #endregion

        #region Public static
        /// <summary>
        /// Finds a device given its PID and VID
        /// </summary>
        /// <param name="nVid">Vendor id for device (VID)</param>
        /// <param name="nPid">Product id for device (PID)</param>
        /// <param name="oType">Type of device class to create</param>
        /// <returns>A new device class of the given type or null</returns>
        public static HidDevice FindDevice(int nVid, int nPid, Type oType)
        {
            string strPath = string.Empty;
            string strSearch = string.Format("vid_{0:x4}&pid_{1:x4}", nVid, nPid); // first, build the path search string
            Guid gHid = HidGuid;
            //HidD_GetHidGuid(out gHid);	// next, get the GUID from Windows that it uses to represent the HID USB interface
            IntPtr hInfoSet = SetupDiGetClassDevs(ref gHid, null, IntPtr.Zero, DigcfDeviceinterface | DigcfPresent);	// this gets a list of all HID devices currently connected to the computer (InfoSet)
            try
            {
                DeviceInterfaceData oInterface = new DeviceInterfaceData();	// build up a device interface data block
                oInterface.Size = Marshal.SizeOf(oInterface);
                // Now iterate through the InfoSet memory block assigned within Windows in the call to SetupDiGetClassDevs
                // to get device details for each device connected
                int nIndex = 0;
                while (SetupDiEnumDeviceInterfaces(hInfoSet, 0, ref gHid, (uint)nIndex, ref oInterface))	// this gets the device interface information for a device at index 'nIndex' in the memory block
                {
                    string strDevicePath = GetDevicePath(hInfoSet, ref oInterface);	// get the device path (see helper method 'GetDevicePath')
                    if (strDevicePath.IndexOf(strSearch) >= 0)	// do a string search, if we find the VID/PID string then we found our device!
                    {
                        HidDevice oNewDevice = (HidDevice)Activator.CreateInstance(oType);	// create an instance of the class for this device
                        oNewDevice.Initialise(strDevicePath);	// initialise it with the device path
                        return oNewDevice;	// and return it
                    }
                    nIndex++;	// if we get here, we didn't find our device. So move on to the next one.
                }
            }
            catch (Exception ex)
            {
                throw HidDeviceException.GenerateError(ex.ToString());
                //Console.WriteLine(ex.ToString());
            }
            finally
            {
                // Before we go, we have to free up the InfoSet memory reserved by SetupDiGetClassDevs
                SetupDiDestroyDeviceInfoList(hInfoSet);
            }
            return null;	// oops, didn't find our device
        }
        #endregion

        #region Publics
        /// <summary>
        /// Event handler called when device has been removed
        /// </summary>
        public event EventHandler OnDeviceRemoved;
        /// <summary>
        /// Accessor for output report length
        /// </summary>
        public int OutputReportLength
        {
            get
            {
                return _mNOutputReportLength;
            }
        }
        /// <summary>
        /// Accessor for input report length
        /// </summary>
        public int InputReportLength
        {
            get
            {
                return _mNInputReportLength;
            }
        }
        /// <summary>
        /// Virtual method to create an input report for this device. Override to use.
        /// </summary>
        /// <returns>A shiny new input report</returns>
        public virtual InputReport CreateInputReport()
        {
            return null;
        }
        #endregion
    }
}
