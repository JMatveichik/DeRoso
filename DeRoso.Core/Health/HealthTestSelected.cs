using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
