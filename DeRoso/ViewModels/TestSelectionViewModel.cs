using DeRoso.Core;
using DeRoso.Core.Data;
using DeRoso.Core.Health;
using System.Collections.Generic;
using System.Linq;

namespace DeRoso.ViewModels
{
    class TestSelectionViewModel :  ViewModelBase
    {

        public TestSelectionViewModel(DeRosoContext data)
        {
            DeRossoData = data;
            SelectedSection = Sections?.FirstOrDefault();
            SelectedGroup = SelectedSection.Groups?.FirstOrDefault();

        }

        public void Update()
        {
            SelectedTargetTests = DeRossoDataWorker.GetLastSelectedTests();
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
        public List<HealthTest> SelectedTargetTests
        {
            get
            {
                return _selectedTargetTests;
            }
            private set
            {
                if (value == _selectedTargetTests)
                    return;

                _selectedTargetTests = value;
                OnPropertyChanged();
            }
        }
        public List<HealthTest> _selectedTargetTests = DeRossoDataWorker.GetLastSelectedTests();

        /// <summary>
        /// Выбранный тест  в целевой группе
        /// </summary>
        public HealthTest SelectedTargetTest
        {
            get
            {
                return _selectedTargetTest;
            }
            set
            {
                if (value == _selectedTargetTest)
                    return;

                _selectedTargetTest = value;
                OnPropertyChanged();
            }
        }
        private HealthTest _selectedTargetTest = null;

        /// <summary>
        /// Выбранный тест  в исходной группе
        /// </summary>
        public HealthTest SelectedSourceTest
        {
            get
            {
                return _selectedSourceTest;
            }
            set
            {
                if (value == _selectedSourceTest)
                    return;

                _selectedSourceTest = value;
                OnPropertyChanged();
            }
        }
        private HealthTest _selectedSourceTest = null;
    }
}
