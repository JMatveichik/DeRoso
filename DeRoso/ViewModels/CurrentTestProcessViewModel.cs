using DeRoso.Core;
using DeRoso.Core.Device;
using DeRoso.Core.Health;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeRoso.ViewModels
{
    public class CurrentTestProcessViewModel : ViewModelBase
    {

        public CurrentTestProcessViewModel(DeviceProvider device)
        {
            device.HealthTestStarted += DeviceHealthTestStarted;
            device.HealthTestTick += DeviceHealthTestTick;
            device.HealthTestComplete += DeviceHealthTestComplete;
            device.HealthTestFailed += DeviceHealthTestFailed;

        }

        private void DeviceHealthTestFailed(object sennder, HealthTestEventArgs args)
        {
            CurrentOperation = EnumHelper.GetDescription(args.CurrentStep);
            OnPropertyChanged("CurrentOperation");
        }

        private void DeviceHealthTestComplete(object sennder, HealthTestEventArgs args)
        {
            CurrentOperation = EnumHelper.GetDescription(args.CurrentStep);
            OnPropertyChanged("CurrentOperation");
        }

        private void DeviceHealthTestTick(object sender, HealthTestEventArgs args)
        {
            CurrentOperation = EnumHelper.GetDescription(args.CurrentStep);
            OnPropertyChanged("CurrentOperation");

            TimeLeft = args.OperationLeftTime.TotalSeconds < 0.0 ? 0.0 : args.OperationLeftTime.TotalSeconds;
            OnPropertyChanged("TimeLeft");

            switch(args.CurrentStep)
            {
                case EnumHealthTestStep.AddDrug:
                    HealthTestResult lastRes = Results.Last();
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => lastRes.Meassurments.Add((HealthTestDrugResult)args.TestItem)));
                    break;

                case EnumHealthTestStep.MeassureBefore:
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => Results.Add((HealthTestResult)args.TestItem)));
                    break;
            }
            
        }

        private void DeviceHealthTestStarted(object sender, HealthTestEventArgs args)
        {
            CurrentTest = (HealthTest)args.TestItem;
            CurrentOperation = EnumHelper.GetDescription(args.CurrentStep);
            OnPropertyChanged("CurrentOperation");
        }

        public double TimeLeft
        {
            get;
            private set;
        }

        public string CurrentOperation
        {
            get;
            private set;
        }

        /// <summary>
        /// Текущий тест
        /// </summary>
        public HealthTest CurrentTest
        {
            get
            {
                return _currentTest;
            }
            set
            {
                if (value == _currentTest)
                    return;

                _currentTest = value;
                OnPropertyChanged();
                OnPropertyChanged("GroupName");
                OnPropertyChanged("SectionName");
            }
        }
        private HealthTest _currentTest = null;

        /// <summary>
        /// Заголовок текущей группы
        /// </summary>
        public string GroupName
        {
            get
            {
                return _currentTest?.Group.Title;
            }
            
        }

        /// <summary>
        /// Заголовок текущей секции
        /// </summary>
        public string SectionName
        {
            get
            {
                return _currentTest?.Group.Section.Title;
            }

        }

        public ObservableCollection<HealthTestResult> Results
        {
            get;
            private set;
        } = new ObservableCollection<HealthTestResult>();



    }
}
