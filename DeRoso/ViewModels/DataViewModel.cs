using DeRoso.Core;
using DeRoso.Core.Data;
using DeRoso.Core.Health;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DeRoso.ViewModels
{
    public class DataViewModel : ViewModelBase
    {
        public DataViewModel(DeRosoContext data)
        {
            DeRossoData = data;

            FilteredDrugs = (CollectionView)CollectionViewSource.GetDefaultView(Drugs);
            FilteredDrugs.Filter = DrugFilter;

            SelectedSection = Sections.FirstOrDefault();
            SelectedGroup = SelectedSection?.Groups.FirstOrDefault();
        }

        public DeRosoContext DeRossoData
        {
            get;
            set;
        }


        private string buildTetsPath()
        {
            string res = "";

            res = SelectedSection?.Title + " > " + SelectedGroup?.Title + " > " + SelectedTest?.Title;

            return res;
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

                CurrentTestPath = buildTetsPath();
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
                CurrentTestPath = buildTetsPath();
            }
        }
        private HealthTestGroup _selectedGroup = null;


        /// <summary>
        /// Текущий тест
        /// </summary>
        public HealthTest SelectedTest
        {
            get
            {
                return _selectedTest;
            }
            set
            {
                if (value == _selectedTest)
                    return;

                _selectedTest = value;
                OnPropertyChanged();
                CurrentTestPath = buildTetsPath();
            }
        }
        private HealthTest _selectedTest = null;


        /// <summary>
        /// Текущий выбранный препарат
        /// </summary>
        public HealthTestDrug SelectedDrug
        {
            get
            {
                return _selectedDrug;
            }
            set
            {
                if (value == _selectedDrug)
                    return;

                _selectedDrug = value;
                OnPropertyChanged();
            }
        }
        private HealthTestDrug _selectedDrug = null;


        public string CurrentTestPath
        {
            get
            {
                return _currentTestPath;
            }
            private set
            {
                if (value == _currentTestPath)
                    return;

                _currentTestPath = value;
                OnPropertyChanged();
            }
        }
        private string _currentTestPath = null;



        public ICollectionView FilteredDrugs
        {
            get;
            private set;
        }

        public int DrugMinimalAddress
        {
            get
            {
                return _drugMinimalAddress;
            }
            set
            {
                if (value == _drugMinimalAddress)
                    return;

                _drugMinimalAddress = value;
                CollectionViewSource.GetDefaultView(FilteredDrugs).Refresh();
                OnPropertyChanged();
            }

        }
        private int _drugMinimalAddress = 0;

        private bool DrugFilter(object item)
        {
            HealthTestDrug d = (HealthTestDrug)item;
            if (d.Address >= DrugMinimalAddress)
                return true;

            return false;
        }
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
        /// Таблица препаратов
        /// </summary>
        public List<HealthTestDrug> Drugs
        {
            get
            {
                return DeRossoData.Drugs.ToList();
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


    }
}
