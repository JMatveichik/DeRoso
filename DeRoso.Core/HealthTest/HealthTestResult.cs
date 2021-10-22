using System;
using System.Collections.Generic;
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
        public List<float> Meassurments
        {
            get;
            private set;
        } = new List<float>();

        /// <summary>
        /// Значение измеряемого параметра перед выполнение теста 
        /// </summary>
        public float MeassurmentBefore
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

        private float _meassurmentBefore;

        /// <summary>
        /// Значение измеряемого параметра перед выполнение теста 
        /// </summary>
        public float MeassurmentAfter
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

        private float _meassurmentAfter;
    }
}
