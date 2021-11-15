using DeRoso.Core;
using DeRoso.Core.Data;
using DeRoso.Core.Health;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace DeRoso.ViewModels
{
    public class DataViewModel : ViewModelBase
    {
        public DataViewModel()
        {
            //Drugs
            FilteredDrugs = (CollectionView)CollectionViewSource.GetDefaultView(Drugs);
            FilteredDrugs.Filter = DrugFilter;

            Update();
        }

        /// <summary>
        /// Обновляем данные из базы
        /// </summary>
        public void Update(HealthTestSection selSection = null, HealthTestGroup selGroup = null, HealthTest selTest = null)
        {
            //обновляем список разделов
            Sections = DeRossoDataWorker.GetAllSections();

            //выбираем первый раздел если не задана выбранная секция
            SelectedSection = selSection ?? Sections.FirstOrDefault();

            //получаем 
            Groups = DeRossoDataWorker.GetAllGroups(SelectedSection);

            //выбираем первую группу в разделе если не задан выбранный раздел
            SelectedGroup = selGroup ?? Groups.FirstOrDefault();

            //Выбираем тест 
            SelectedTest = selTest ?? SelectedGroup?.Tests?.FirstOrDefault();
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
        /// Таблица разделов
        /// </summary>
        public List<HealthTestGroup> Groups
        {
            get => _groups;
            private set
            {
                if (value == _groups)
                    return;

                _groups = value;
                OnPropertyChanged();
            }
        }
        private List<HealthTestGroup> _groups = null;

        /// <summary>
        /// Таблица разделов
        /// </summary>
        public List<HealthTestSection> Sections
        {
            get => _sections;
            private set
            {
                if (value == _sections)
                    return;


                _sections = value;
                OnPropertyChanged();
            }
        }
        private List<HealthTestSection> _sections = null;


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

                //получаем 
                Groups = DeRossoDataWorker.GetAllGroups(SelectedSection);

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
                if (value == _selectedGroup)
                    return;

                _selectedGroup = value;
                GroupTests = _selectedGroup?.Tests;

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

        public string DrugFilterString
        {
            get => _drugFilterString;
            set
            {
                if (value == _drugFilterString)
                    return;

                _drugFilterString = value;
                CollectionViewSource.GetDefaultView(FilteredDrugs).Refresh();
                OnPropertyChanged();
            }

        }
        private string _drugFilterString = "";

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


        /// <summary>
        /// Фильтр препаратов
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool DrugFilter(object item)
        {
            HealthTestDrug d = (HealthTestDrug)item;

            if (string.IsNullOrEmpty(DrugFilterString))
                return true;

            string[] filter = DrugFilterString.Split(new char[] { ' ', ',', ':', '-'});

            int address = 0;
            int cell = 0;

            if (filter.Length == 1 && int.TryParse(filter[0], out address))
            {
                if (d.Address >=  address)
                    return true;
            }
            else if (filter.Length == 2)
            {
                //если ячейка пустая проверяем адресс
                if (string.IsNullOrEmpty(filter[1]))
                {
                    if (int.TryParse(filter[0], out address) && d.Address >= address)
                        return true;
                }
                else if(int.TryParse(filter[0], out address) && int.TryParse(filter[1], out cell))
                {
                    if (d.Address >= address && d.Cell == cell)
                        return true;
                }
            }
           

            return false;
        }


        /// <summary>
        /// Таблица препаратов
        /// </summary>
        public List<HealthTestDrug> Drugs => DeRossoDataWorker.GetAllDrugs();


        /*
        /// <summary>
        /// Таблица разделов тестов
        /// </summary>
        public List<HealthTestSection> Sections => DeRossoDataWorker.GetAllSections();

        /// <summary>
        /// Таблица групп тестов
        /// </summary>
        public List<HealthTestGroup> Groups => DeRossoDataWorker.GetAllGroups();

        

        /// <summary>
        /// Таблица всех препаратов
        /// </summary>
        public List<HealthTestReciept> Reciepts => DeRossoDataWorker.GetAllReciepts();

        /// <summary>
        /// Таблица тестов
        /// </summary>
        public List<HealthTest> Tests => DeRossoDataWorker.GetAllTests();
        */
    }
}
