using DeRoso.Core.Health;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Health
{
    [Table("Results")]
    public class HealthTestResult : HealthTestItem
    {

        /// <summary>
        /// Идентификатор теста
        /// </summary>
        public int HealthTestId { get; set; }

        /// <summary>
        /// Тест с к которым связан принадлежит результат
        /// </summary>
        public HealthTest Test { get; set; }

        /// <summary>
        /// Идентификатор оптимального препарата теста
        /// </summary>
        public int HealthDrugId { get; set; }

        /// <summary>
        /// Оптимальный препарат теста
        /// </summary>
        public HealthTestDrug Drug { get; set; }



        [NotMapped]
        /// <summary>
        /// Буфер измерений
        /// </summary>
        public ObservableCollection<HealthTestDrugResult> Meassurments
        {
            get;
            private set;
        } = new ObservableCollection<HealthTestDrugResult>();

        
    }
}
