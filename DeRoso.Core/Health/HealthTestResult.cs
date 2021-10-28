using DeRoso.Core.Data;
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

        public HealthTestResult()
        {
            Date = DateTime.Now;
        }

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

        /// <summary>
        /// Шкала лекарства
        /// </summary>
        public float Scale
        {
            get
            {
                return _scale;
            }
            private set
            {
                if (value == _scale)
                    return;

                _scale = value;
                OnPropertyChanged();
            }
        }
        private float _scale = float.PositiveInfinity;


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


        public DateTime Date
        {
            get;
            private set;
        }

        [NotMapped]
        /// <summary>
        /// Буфер измерений
        /// </summary>
        public ObservableCollection<HealthTestDrugResult> Meassurments
        {
            get;
            private set;
        } = new ObservableCollection<HealthTestDrugResult>();

        /// <summary>
        /// Выбор оптимального препарата для теста
        /// </summary>
        public void SelectOptimalResult()
        {
            switch(Test.CalculationType)
            {
                case EnumCalculationType.Maximum:
                    {
                        //упорядочиваие по убыванию разницы в измерениях и выбор последнего элемента
                        var res = Meassurments.OrderBy(d => Math.Abs(d.MeassurmentBefore - d.MeassurmentAfter)).Last();

                        HealthDrugId = res.HealthTestDrugId;
                        Drug = res.Drug;
                        res.IsOptimal = true;
                        MeassurmentAfter = res.MeassurmentAfter;
                    }
                    break;

                case EnumCalculationType.Minimum:
                    {
                        //упорядочиваие по возрастанию разницы в измерениях и выбор первого элемента
                        var res = Meassurments.OrderBy(d => Math.Abs(d.MeassurmentBefore - d.MeassurmentAfter)).First();

                        HealthDrugId = res.HealthTestDrugId;
                        Drug = res.Drug;
                        res.IsOptimal = true;
                        MeassurmentAfter = res.MeassurmentAfter;
                    }
                    break;

                case EnumCalculationType.Medium:
                    {
                        //среднее отклонение для всех измерений
                        var averageGlobal = Meassurments.Select(d => Math.Abs(d.MeassurmentBefore - d.MeassurmentAfter)).Average();
                       
                        //первая разница она же минимальная
                        HealthTestDrugResult res = Meassurments.First();
                        double minDiff = Math.Abs(res.MeassurmentBefore - res.MeassurmentAfter);

                        foreach (HealthTestDrugResult r in Meassurments)
                        {
                            var diff = Math.Abs(r.MeassurmentBefore - r.MeassurmentAfter);
                            if (diff < minDiff)
                            {
                                res = r;
                                minDiff = diff;
                            }                                
                        }

                        HealthDrugId = res.HealthTestDrugId;
                        Drug = res.Drug;
                        res.IsOptimal = true;
                        MeassurmentAfter = res.MeassurmentAfter;
                    }
                    break;
            }
        }

    }
}
