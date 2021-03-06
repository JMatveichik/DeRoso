using System.ComponentModel.DataAnnotations.Schema;

namespace DeRoso.Core.Health
{
    [Table("Drugs")]
    public class HealthTestDrug : HealthTestItem
    {
       
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
    }
}
