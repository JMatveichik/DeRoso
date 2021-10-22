using DeRoso.Core;
using DeRoso.Core.Data;
using DeRoso.Core.Health;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.ViewModels
{
    class TestSelectionViewModel :  ViewModelBase
    {

        public TestSelectionViewModel(DeRosoContext data)
        {
            DeRossoData = data;
        }

        public DeRosoContext DeRossoData
        {
            get;
            set;
        }

        /// <summary>
        /// Текущий разделов тестов
        /// </summary>
        public HealthTestSection SelectedSection
        {
            get
            {
                return _selectedSection;
            }
            set
            {
                if (value == _selectedSection)
                    return;

                _selectedSection = value;
                OnPropertyChanged();               
            }
        }
        private HealthTestSection _selectedSection = null;

        /// <summary>
        /// Текущая группа тестов
        /// </summary>
        public HealthTestGroup SelectedGroup
        {
            get
            {
                return _selectedGroup;
            }
            set
            {
                if (value == _selectedGroup)
                    return;

                _selectedGroup = value;
                OnPropertyChanged();
            }
        }
        private HealthTestGroup _selectedGroup = null;
       

        /// <summary>
        /// Таблица разделов тестов
        /// </summary>
        public List<HealthTestSection> Sections
        {
            get
            {
                return DeRossoData.Sections.ToList();
            }
        }

        /// <summary>
        /// Таблица групп тестов
        /// </summary>
        public List<HealthTestGroup> Groups
        {
            get
            {
                return DeRossoData.Groups.ToList();
            }
        }
       

        /// <summary>
        /// Таблица тестов
        /// </summary>
        public List<HealthTest> Tests
        {
            get
            {
                return DeRossoData.Tests.ToList();
            }
        }

        /// <summary>
        /// Выбранные тесты
        /// </summary>
        public ObservableCollection<HealthTest> SelectedTests
        {
            get;
            private set;
        } = new ObservableCollection<HealthTest>();
    }
}
