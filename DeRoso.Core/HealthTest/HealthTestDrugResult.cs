using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Health
{
    public class HealthTestDrugResult : HealthTestItem
    {
        /// <summary>
        /// Идентификатор теста с которым связан результат
        /// </summary>
        public int HealthTestId { get; set; }

        /// <summary>
        /// Тест с которым связан результат
        /// </summary>
        public HealthTest Test { get; set; }

        /// <summary>
        /// Идентификатор препарата
        /// </summary>
        public int HealthTestDrugId { get; set; }

        /// <summary>
        /// Препарат с которым связан результат
        /// </summary>
        public HealthTestDrug Drug { get; set; }

        
        /// <summary>
        /// Оптимальный препарат в тесте
        /// </summary>
        public bool IsOptimal
        {
            get { return _isOptimal; }
            set
            {
                if (value == _isOptimal)
                    return;

                _isOptimal = value;
                OnPropertyChanged();
            }
        }

        private bool _isOptimal;



        /// <summary>
        /// Значение измеряемого параметра перед выдачей препарата
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
        /// Значение измеряемого параметра поле выдачи препарата
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
