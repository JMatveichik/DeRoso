using System.ComponentModel.DataAnnotations.Schema;


namespace DeRoso.Core.Health
{
    /// <summary>
    /// Таблица выбранных тестов
    /// </summary>
    [Table("Selected")]
    public class HealthTestSelected
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int Id { get; set; }


        public int HealthTestId
        {
            get;
            set;
        }

        public HealthTest Test { get; set; }

        
    }
}
