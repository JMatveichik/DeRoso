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
            HealthTestDrugResult optimalResult = null;
            HealthTestReciept optimalReciept = null;

            //optimal.IsOptimal = true;

            switch (Test.CalculationType)
            {
                case EnumCalculationType.Maximum:
                    {
                        //упорядочиваие по убыванию разницы в измерениях и выбор последнего элемента
                        //оптимальный препарат
                        optimalResult  = Meassurments.OrderBy(d => d.MeassurmentAfter).Last();
                        optimalReciept = Test.Reciepts.Select(r => r).FirstOrDefault(r => r.Id == optimalResult.HealthTestDrugId);

                        #region СТАРЫЕ РАСЧЕТЫ

                        ///тест с несколькими прапаратами
                        /*
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
                        */

                        #endregion

                    }
                    break;

                case EnumCalculationType.Minimum:
                    {
                        //упорядочиваие по убыванию разницы в измерениях и выбор последнего элемента
                        //оптимальный препарат
                        optimalResult = Meassurments.OrderBy(d => d.MeassurmentAfter).First();
                        optimalReciept = Test.Reciepts.Select(r => r).FirstOrDefault(r => r.Id == optimalResult.HealthTestDrugId);

                        #region СТАРЫЕ РАСЧЕТЫ
                        /*
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
                        */

                        #endregion
                    }
                    break;

                case EnumCalculationType.Medium:
                    {
                        //среднее отклонение для всех измерений
                        var averageGlobal = Meassurments.Select(d => d.MeassurmentAfter).Average();

                        //первая разница она же минимальная
                        optimalResult = Meassurments.First();
                        double minDiff = Math.Abs( optimalResult.MeassurmentAfter - averageGlobal);

                        foreach (HealthTestDrugResult r in Meassurments)
                        {
                            var diff = Math.Abs(r.MeassurmentAfter - averageGlobal);
                            if (diff < minDiff)
                            {
                                optimalResult = r;
                                minDiff = diff;
                            }                                
                        }

                        optimalReciept = Test.Reciepts.Select(r => r).FirstOrDefault(r => r.Id == optimalResult.HealthTestDrugId);

                        #region  СТАРЫЕ РАСЧЕТЫ

                        /*
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
                        */

                        #endregion


                    }
                    break;
            }


            //тетс с одним препаратом
            if (Meassurments.Count == 1)
            {
                ///шкала как результат измерения после
                Scale = optimalResult.MeassurmentAfter;
            }
            else
            {
                ///шкала как порядок оптимального теста 
                Scale = optimalReciept?.Order ?? 0;
            }


            ///на будущее для сохранения в базу данных
            HealthTestDrugId = optimalResult.HealthTestDrugId;
            Drug = optimalResult.Drug;

            //для текущего теста
            MeassurmentAfter = optimalResult.MeassurmentAfter;
            optimalResult.IsOptimal = true;
        }

    }
}
