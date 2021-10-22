using DeRoso.Core;
using DeRoso.Core.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.ViewModels
{
    public class DeviceTestingViewModel : ViewModelBase
    {
        public DeviceTestingViewModel(DeviceProvider device)
        {
            device.DeviceTestStarted += OnDeviceTestEvent;
            device.DeviceTestFailed += OnDeviceTestEvent;
            device.DeviceTestComplete += OnDeviceTestEvent;
        }


        public string DeviceTestingPartDescription
        {
            get;
            set;
        }

        public DeviceInitializationTestState TestState
        {
            get;
            set;
        }

        private void OnDeviceTestEvent(object sender, DeviceInitializationTestEventArgs args)
        {
            DeviceTestingPartDescription = args.TestDescription;
            TestState = args.State;

            OnPropertyChanged("DeviceTestingPartDescription");
            OnPropertyChanged("TestState");
        }

        
    }
}
