using DeRoso.Core;
using DeRoso.Core.Device;
using DeRoso.Core.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.ViewModels
{
    public class CurrentTestProcessViewModel : ViewModelBase
    {

        public CurrentTestProcessViewModel(DeviceProvider device)
        {

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
                return _currentTest.Group.Title;
            }
            
        }

        /// <summary>
        /// Заголовок текущей секции
        /// </summary>
        public string SectionName
        {
            get
            {
                return _currentTest.Group.Section.Title;
            }

        }



    }
}
