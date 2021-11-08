using System.ComponentModel.DataAnnotations.Schema;

namespace DeRoso.Core.Health
{
    [Table("Reciepts")]
    public class HealthTestReciept
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификатор препарата
        /// </summary>
        public int HealthTestDrugId
        {
            get;
            set;
        }
        

        /// <summary>
        /// Навигационное свойство препарата
        /// </summary>
        public HealthTestDrug Drug
        {
            get;
            set;
        }

        public int HealthTestId
        {
            get;
            set;
        }

        public HealthTest Test
        {
            get;
            set;
        } 

        /// <summary>
        /// Порядковый номер препарата в ячейке
        /// </summary>
        public int Order
        {
            get;
            set;
        } 
        
    }
}
