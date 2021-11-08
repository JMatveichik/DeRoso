using DeRoso.Core.Health;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DeRoso.Core.Data
{
    public static class DeRossoDataWorker
    {

        public static DeRosoContext DB
        {
            get;
            set;
        }

        public static List<Patient> GetAllPatients()
        {
            return DB.Patients.ToList();
        }

        public static void AddPatient(Patient newPatient)
        {
            //если уже присутствует в списке не добавляем
            /*
            var result = DB.Patients.Select(l => l.Id).ToList();
            if (result.Contains(newPatient.Id))
                return;
            */

            DB.Patients.Add(newPatient);
            DB.SaveChanges();
        }

        #region РАБОТА С ТАБЛИЦЕЙ ПОСЛЕДНИХ ВЫБРАННВХ ТЕСТОВ

        /// <summary>
        /// Получение всего списка последних выбранных тестов 
        /// </summary>
        /// <returns></returns>
        public static List<HealthTest> GetLastSelectedTests()
        {
            return DB.LastSelected.Select(l => l.Test).ToList();            
        }

        /// <summary>
        /// Очистить список последних выбранных тестов 
        /// </summary>
        public static void ClearLastSelectedTests()
        {            
            var rng = DB.LastSelected;
            DB.LastSelected.RemoveRange(rng);
            DB.SaveChanges();
            
        }

        /// <summary>
        /// Удалить тест из списка
        /// </summary>
        /// <param name="test"></param>
        public static void RemoveFromLastSelectedTests(HealthTest test)
        {            
            var result = DB.LastSelected.Select(l => l).Where(l => l.HealthTestId == test.Id).ToList();
            DB.LastSelected.RemoveRange(result);
            DB.SaveChanges();
            
        }

        /// <summary>
        /// Добавить тест если он еще не присутствует в базе
        /// </summary>
        /// <param name="test"></param>
        public static void AddToLastSelectedTests(HealthTest test)
        {            
            //если уже присутствует в списке не добавляем
            var result = DB.LastSelected.Select(l => l.Test.Id).ToList();
            if (result.Contains(test.Id))
                return;

            DB.LastSelected.Add(
                new HealthTestSelected()
                {
                    HealthTestId = test.Id                        
                }
            );

            DB.SaveChanges();
           
        }

        /// <summary>
        /// Добавить группу тестов. Если тест из списка присутствует в таблице он не добавляется
        /// </summary>
        /// <param name="test"></param>
        public static void AddToLastSelectedTests(IEnumerable< HealthTest > tests)
        {
            
            foreach (HealthTest test in tests)
            {
                //проверка на валидность препаратов в тесте
                if (!test.ContainValidReciepts())
                    continue;

                //если уже присутствует в списке не добавляем
                var result = DB.LastSelected.Select(l => l.Test.Id).ToList();
                if (result.Contains(test.Id))
                    continue;

                ///добавляем тест в список выбранных
                DB.LastSelected.Add(
                    new HealthTestSelected()
                    {
                        HealthTestId = test.Id
                    }
                );

                Debug.WriteLine(test.ToString());
            }

            DB.SaveChanges();
            
        }

        /// <summary>
        /// Таблица выбранных тестов пустая
        /// </summary>
        /// <returns></returns>
        public static bool IsEmptySelectedTests()
        {            
            return DB.LastSelected.Count() == 0;           
        }

        #endregion
    }
}
