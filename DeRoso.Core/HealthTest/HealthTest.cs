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

        /*
        /// <summary>
        /// Идентификатор секции  тестов
        /// </summary>
        public int HealthTestSectionId { get; set; }

        /// <summary>
        /// Секция к которой принадлежит тест
        /// </summary>
        public HealthTestSection Section { get; set; }
        */
            
        /// <summary>
        /// Набор препаратов для теста
        /// </summary>
        public ObservableCollection<HealthTestDrug> Drugs { get; set; }        


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
        /// Значение измеряемого параметра перед выполнение теста 
        /// </summary>
        public double MeassurmentBefore
        {
            get { return _meassurmentBefore; }
            set
            {
                if (Math.Abs(value - _meassurmentBefore) < 0.000001)
                    return;

                _meassurmentBefore = value;
                OnPropertyChanged();
            }
        }

        private double _meassurmentBefore;

        /// <summary>
        /// Значение измеряемого параметра перед выполнение теста 
        /// </summary>
        public double MeassurmentAfter
        {
            get { return _meassurmentAfter; }
            set
            {
                if (Math.Abs(value - _meassurmentAfter) < 0.000001)
                    return;

                _meassurmentAfter = value;
                OnPropertyChanged();
            }
        }

        private double _meassurmentAfter;

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
