using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Health
{
    [Table ("Tests")]
    public class HealthTest : HealthTestItem
    {

        /// <summary>
        /// Идентификатор группы тестов
        /// </summary>
        public int HealthTestGroupId { get; set; }

        /// <summary>
        /// Группа к которой принадлежит тест
        /// </summary>
        public HealthTestGroup Group { get; set; }

            
        /// <summary>
        /// Набор препаратов для теста
        /// </summary>
        public ObservableCollection<HealthTestDrug> Drugs
        {
            get
            {
                return _drugs;
            }
            set
            {
                if (value == _drugs)
                    return;

                _drugs = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<HealthTestDrug> _drugs = null;


        public bool ContainValidDrugs()
        {
            if (Drugs == null)
                return false;

            if (Drugs.Count == 0)
                return false;

            foreach(HealthTestDrug d in Drugs)
            {
                if (d.Title.Contains("Нет"))
                    return false;
            }

            return true;
        }

        
        /// <summary>
        /// Использовать HV после выполнения теста
        /// </summary>        
        public bool UseHV
        {
            get => _useHV;
            set
            {
                if (value == _useHV)
                    return;

                _useHV = value;
                OnPropertyChanged();
            }
        }
        private bool _useHV = true;

        /// <summary>
        /// Частота HV
        /// </summary>
        public double FrequencyHV
        {
            get => _frequencyHV;
            set
            {
                if (Math.Abs(value - _frequencyHV) < 0.000001)
                    return;

                _frequencyHV = value;
                OnPropertyChanged();
            }
        }
        private double _frequencyHV = 25.0;


        /// <summary>
        /// Пауза перед HV
        /// </summary>
        public TimeSpan PauseBeforeHV
        {
            get => _pauseBeforeHV;
            set
            {
                if (value == _pauseBeforeHV)
                    return;

                _pauseBeforeHV = value;
                OnPropertyChanged();
            }
        }
        private TimeSpan _pauseBeforeHV = TimeSpan.FromSeconds(3.0);


        /// <summary>
        /// Пауза перед запуском теста
        /// </summary>
        public TimeSpan PauseBeforeStart
        {
            get => _pauseBeforeStart;
            set
            {
                if (value == _pauseBeforeStart)
                    return;

                _pauseBeforeStart = value;
                OnPropertyChanged();
            }
        }
        private TimeSpan _pauseBeforeStart = TimeSpan.FromSeconds(3.0);

        /// <summary>
        /// Пауза перед приемом препарата
        /// </summary>
        public TimeSpan PauseBeforeDrug
        {
            get => _pauseBefore;
            set
            {
                if (value == _pauseBefore)
                    return;

                _pauseBefore = value;
                OnPropertyChanged();
            }
        }
        private TimeSpan _pauseBefore = TimeSpan.FromSeconds(3.0);

        /// <summary>
        /// Пауза после применения препарата
        /// </summary>
        public TimeSpan PauseAfterDrug
        {
            get => _pauseAfter;
            set
            {
                if (value == _pauseAfter)
                    return;

                _pauseAfter = value;
                OnPropertyChanged();
            }
        }
        private TimeSpan _pauseAfter = TimeSpan.FromSeconds(3.0);

        /// <summary>
        /// Время применения препарата
        /// </summary>
        public TimeSpan DrugDuration
        {
            get => _duration;
            set
            {
                if (value == _duration)
                    return;

                _duration = value;
                OnPropertyChanged();
            }
        }
        private TimeSpan _duration = TimeSpan.FromSeconds(3.0);


        /// <summary>
        /// Минимально нормальное показания измеряемого параметра 
        /// </summary>
        public double LowLimit
        {
            get { return _lowLimit; }
            set
            {
                if (Math.Abs(value - _lowLimit) < 0.000001)
                    return;

                _lowLimit = value;
                OnPropertyChanged();
            }
        }
        private double _lowLimit = 0.0;

        /// <summary>
        ///  Максимально нормальное показания измеряемого параметра 
        /// </summary>
        public double HighLimit
        {
            get { return _highLimit; }
            set
            {
                if (Math.Abs(value - _highLimit) < 0.000001)
                    return;

                _highLimit = value;
                OnPropertyChanged();
            }
        }
        private double _highLimit = 100.0;

        /// <summary>
        /// Тип рассчета параметров
        /// </summary>
        public EnumCalculationType CalculationType
        {
            get { return _calcType; }
            set
            {
                if (value == _calcType)
                    return;

                _calcType = value;
                OnPropertyChanged();
            }
        }
        private EnumCalculationType _calcType = EnumCalculationType.Medium;

    }
}
