using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Health
{
    [Table("Drugs")]
    public class HealthTestDrug : HealthTestItem
    {

        /// <summary>
        /// Идентификатор теста
        /// </summary>
        public int? HealthTestId { get; set; }

        /// <summary>
        /// Тест которому принадлежит препарат
        /// </summary>
        public HealthTest Test { get; set; }


        /// <summary>
        /// Адрес препарата в устройстве
        /// </summary>        
        public int Address
        {
            get => _address;
            set
            {
                if (value == _address)
                    return;

                _address = value;
                OnPropertyChanged();
            }
        }
        private int _address;

        /// <summary>
        /// Ячейка препарата в устройстве
        /// </summary>        
        public int Cell
        {
            get => _cell;
            set
            {
                if (value == _cell)
                    return;

                _cell = value;
                OnPropertyChanged();
            }
        }
        private int _cell;

        /// <summary>
        /// Пауза перед приемом препарата
        /// </summary>
        public TimeSpan PauseBefore
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
        public TimeSpan PauseAfter
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
        public TimeSpan Duration
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
    }
}
