using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
        public int HealthTestDrugId { get; set; }

        /// <summary>
        /// Оптимальный препарат теста
        /// </summary>
        public HealthTestDrug Drug { get; set; }

        /// <summary>
        /// Шкала лекарства
        /// </summary>
        public float Scale
        {
            get => _scale;
            private set
            {
                if (value == _scale)
                    return;

                _scale = value;
                OnPropertyChanged();
            }
        }
        private float _scale = 0.0f;

        /// <summary>
        /// Значение измеряемого параметра перед выдачей препарата
        /// </summary>
        public float MeassurmentBefore
        {
            get => _meassurmentBefore;
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
            get => _meassurmentAfter;
            set
            {
                if (Math.Abs(value - _meassurmentAfter) < 0.000001)
                    return;

                _meassurmentAfter = value;
                OnPropertyChanged();
            }
        }
        private float _meassurmentAfter;
       
        [NotMapped]
        /// <summary>
        /// Буфер измерений
        /// </summary>
        public ObservableCollection<HealthTestDrugResult> Meassurments
        {
            get;
            private set;
        } = new ObservableCollection<HealthTestDrugResult>();

        [NotMapped]
        /// <summary>
        /// Данный тест простой т.е. содержащит один рецепт
        /// </summary>
        public bool IsSingle => Test.Reciepts.Count == 1;

        /// <summary>
        /// Выбор оптимального препарата послепроведения теста
        /// </summary>
        public void SelectOptimalDrug()
        {
            switch(Test.CalculationType)
            {
                case EnumCalculationType.Maximum:
                    {
                        //упорядочиваие по убыванию разницы в измерениях и выбор последнего элемента
                        /*var res = Meassurments.OrderBy(d => Math.Abs(d.MeassurmentBefore - d.MeassurmentAfter)).Last();

                        HealthTestDrugId = res.HealthTestDrugId;
                        Drug = res.Drug;
                        res.IsOptimal = true;
                        MeassurmentAfter = res.MeassurmentAfter;*/

                        //оптимальный препарат
                        HealthTestDrugResult optimal = Meassurments.FirstOrDefault();

                        //тетс с одним препаратом
                        if (Meassurments.Count == 1)
                        {
                            optimal = Meassurments[0];
                            ///шкала как результат измерения после
                            Scale = Meassurments[0].MeassurmentAfter;
                        }
                        ///тест с несколькими прапаратами
                        else
                        { 
                            int scale = 0;
                            double maxDiff = double.MinValue;
                            int ind = 0;

                            ///перебор всех измерений
                            foreach (HealthTestDrugResult dr in Meassurments)
                            {
                                double diff = Math.Abs(dr.MeassurmentBefore - dr.MeassurmentAfter);
                                if (diff > maxDiff)
                                {
                                    maxDiff = diff;
                                    scale = ind;
                                    optimal = dr;
                                }
                                ind++;
                            }

                            ///шкала как индекс оптимального препарата
                            Scale = scale + 1;
                        }

                        
                        ///на будущее для сохранения в базу данных
                        HealthTestDrugId = optimal.HealthTestDrugId;
                        Drug = optimal.Drug;
                        
                        //для текущего теста
                        MeassurmentAfter = optimal.MeassurmentAfter;
                        optimal.IsOptimal = true;
                        
                        
                    }
                    break;

                case EnumCalculationType.Minimum:
                    {
                        //упорядочиваие по возрастанию разницы в измерениях и выбор первого элемента
                        /*var res = Meassurments.OrderBy(d => Math.Abs(d.MeassurmentBefore - d.MeassurmentAfter)).First();

                        HealthDrugId = res.HealthTestDrugId;
                        Drug = res.Drug;
                        res.IsOptimal = true;
                        MeassurmentAfter = res.MeassurmentAfter;*/

                        //оптимальный препарат
                        HealthTestDrugResult optimal = Meassurments.FirstOrDefault();

                        //тетс с одним препаратом
                        if (Meassurments.Count == 1)
                        {
                            optimal = Meassurments[0];
                            ///шкала как результат измерения после
                            Scale = Meassurments[0].MeassurmentAfter;
                        }
                        ///тест с несколькими прапаратами
                        else
                        {
                            int scale = 0;
                            double maxDiff = double.MaxValue;
                            int ind = 0;

                            ///перебор всех измерений
                            foreach (HealthTestDrugResult dr in Meassurments)
                            {
                                double diff = Math.Abs(dr.MeassurmentBefore - dr.MeassurmentAfter);
                                if (diff < maxDiff)
                                {
                                    maxDiff = diff;
                                    scale = ind;
                                    optimal = dr;
                                }
                                ind++;
                            }

                            ///шкала как индекс оптимального препарата
                            Scale = scale + 1;
                        }


                        ///на будущее для сохранения в базу данных
                        HealthTestDrugId = optimal.HealthTestDrugId;
                        Drug = optimal.Drug;

                        //для текущего теста
                        MeassurmentAfter = optimal.MeassurmentAfter;
                        optimal.IsOptimal = true;
                    }
                    break;

                case EnumCalculationType.Medium:
                    {
                        //среднее отклонение для всех измерений
                        var averageGlobal = Meassurments.Select(d => Math.Abs(d.MeassurmentBefore - d.MeassurmentAfter)).Average();

                        //первая разница она же минимальная
                        /*HealthTestDrugResult res = Meassurments.First();
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
                        MeassurmentAfter = res.MeassurmentAfter;*/

                        //оптимальный препарат
                        HealthTestDrugResult optimal = Meassurments.FirstOrDefault();

                        //тетс с одним препаратом
                        if (Meassurments.Count == 1)
                        {
                            optimal = Meassurments[0];
                            ///шкала как результат измерения после
                            Scale = Meassurments[0].MeassurmentAfter;
                        }
                        ///тест с несколькими прапаратами
                        else
                        {
                            int scale = 0;
                            double maxDiff = double.MaxValue;
                            int ind = 0;

                            ///перебор всех измерений
                            foreach (HealthTestDrugResult dr in Meassurments)
                            {
                                //отклонение от среднего
                                double diff = Math.Abs(averageGlobal - Math.Abs(dr.MeassurmentBefore - dr.MeassurmentAfter));
                                if (diff < maxDiff)
                                {
                                    maxDiff = diff;
                                    scale = ind;
                                    optimal = dr;
                                }
                                ind++;
                            }

                            ///шкала как индекс оптимального препарата
                            Scale = scale + 1;
                        }


                        ///на будущее для сохранения в базу данных
                        HealthTestDrugId = optimal.HealthTestDrugId;
                        Drug = optimal.Drug;

                        //для текущего теста
                        MeassurmentAfter = optimal.MeassurmentAfter;
                        optimal.IsOptimal = true;

                    }
                    break;
            }
        }

    }
}
