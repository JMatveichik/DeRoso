using DeRoso.Core;
using DeRoso.Core.Data;
using DeRoso.Core.Health;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

            PropertyChanged += DataViewModel_PropertyChanged;
        }

        private void DataViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == " ")
            {

            }
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

        public void AddDrug(HealthTest test, HealthTestDrug drug)
        {
            if (test == null || drug == null )
                return;

            if (test.Reciepts == null)
                test.Reciepts = new ObservableCollection<HealthTestReciept>();

            var drugs = test.Reciepts.Select(d => d.Drug);

            if (drugs.Contains(drug))
                return;

            HealthTestReciept reciept = new HealthTestReciept()
            {
                Drug = drug,
                HealthTestDrugId = drug.Id,
                Order = test.Reciepts.Count + 1
            };

            test.Reciepts.Add(reciept);

            
        }

        /// <summary>
        /// Текущий разделов тестов
        /// </summary>
        public HealthTestSection SelectedSection
        {
            get => _selectedSection;
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
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                if (value != null)
                    GroupTests = SelectedGroup.Tests;

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
            get => _selectedTest;
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
            get => _selectedDrug;
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
            get => _currentTestPath;
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
            get => _drugMinimalAddress;
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

        /// <summary>
        /// Таблица тестов
        /// </summary>
        public ObservableCollection<HealthTest> GroupTests
        {
            get => _groupTest;
            private set
            {                
                _groupTest = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<HealthTest> _groupTest = null;



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
        public ObservableCollection<HealthTestSection> Sections => new ObservableCollection<HealthTestSection>(DeRossoData.Sections.ToList());

        /// <summary>
        /// Таблица групп тестов
        /// </summary>
        public ObservableCollection<HealthTestGroup> Groups => new ObservableCollection<HealthTestGroup>(DeRossoData.Groups.ToList());

        /// <summary>
        /// Таблица препаратов
        /// </summary>
        public ObservableCollection<HealthTestDrug> Drugs => new ObservableCollection<HealthTestDrug>(DeRossoData.Drugs.ToList());

        /// <summary>
        /// Таблица препаратов
        /// </summary>
        public ObservableCollection<HealthTestReciept> Reciepts => new ObservableCollection<HealthTestReciept>(DeRossoData.Reciepts.ToList());

        /// <summary>
        /// Таблица тестов
        /// </summary>
        public ObservableCollection<HealthTest> Tests => new ObservableCollection<HealthTest>(DeRossoData.Tests.ToList());
    }
}
