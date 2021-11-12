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

        /// <summary>
        /// Получаем список секций из базы данных
        /// </summary>
        /// <returns></returns>
        public static List<HealthTestSection> GetAllSections()
        {
            return DB.Sections.ToList();
        }

        public static HealthTestSection GetSection(string name)
        {
            return DB.Sections.Select(s => s).FirstOrDefault(s => s.Title == name);
        }

        /// <summary>
        /// Добавление новой секции в базу данных
        /// </summary>
        /// <param name="newSection">Новая секция</param>
        public static void AddSection(HealthTestSection newSection)
        {
            DB.Sections.Add(newSection);
            DB.SaveChanges();
        }

        /// <summary>
        /// Удаляет секцию из базы данных
        /// </summary>
        /// <param name="section"></param>
        public static void RemoveSection(HealthTestSection section)
        {
            var result = DB.Sections.Select(l => l).Where(l => l.Id == section.Id).ToList();
            DB.Sections.RemoveRange(result);
            DB.SaveChanges();
        }


        /// <summary>
        /// Получаем список групп в выбранной секции из базы данных
        /// </summary>
        /// <returns></returns>
        public static List<HealthTestGroup> GetAllGroups(HealthTestSection section)
        {
           return DB.Groups.Select(g => g).Where(g => g.HealthTestSectionId == section.Id).ToList(); ;
        }

        public static HealthTestGroup GetGroup(string name)
        {
            return DB.Groups.Select(s => s).FirstOrDefault(s => s.Title == name);
        }

        /// <summary>
        /// Получаем список всех групп из базы данных
        /// </summary>
        /// <returns></returns>
        public static List<HealthTestGroup> GetAllGroups()
        {
            return DB.Groups.ToList(); ;
        }


        /// <summary>
        /// Добавитьт новую группу
        /// </summary>
        /// <param name="newGroup"></param>
        public static void AddGroup(HealthTestGroup newGroup)
        {
            DB.Groups.Add(newGroup);
            DB.SaveChanges();
        }

        /// <summary>
        /// Удаляет группу из базы данных
        /// </summary>
        /// <param name="group"></param>
        public static void RemoveGroup(HealthTestGroup group)
        {
            var result = DB.Groups.Select(l => l).Where(l => l.Id == group.Id).ToList();
            DB.Groups.RemoveRange(result);
            DB.SaveChanges();
        }


        /// <summary>
        /// Получаем список тестов в выбранной группе из базы данных
        /// </summary>
        /// <returns></returns>
        public static List<HealthTest> GetAllTests(HealthTestGroup group)
        {
            return DB.Tests.Select(g => g).Where(g => g.HealthTestGroupId == group.Id).ToList(); ;
        }

        /// <summary>
        /// Получаем список всех тестов из базы данных
        /// </summary>
        /// <returns></returns>
        public static List<HealthTest> GetAllTests()
        {
            return DB.Tests.ToList(); ;
        }


        public static HealthTest GetTest(string name)
        {
            return DB.Tests.Select(s => s).FirstOrDefault(s => s.Title == name);
        }

        /// <summary>
        /// Добавить тест в базу данных
        /// </summary>
        /// <param name="newTest">Новый тест</param>
        public static void AddTest(HealthTest newTest)
        {
            DB.Tests.Add(newTest);
            DB.SaveChanges();
        }

        /// <summary>
        /// Удаляет тест из базы данных
        /// </summary>
        /// <param name="test">Тест для удаления</param>
        public static void RemoveTest(HealthTest test)
        {
            var result = DB.Tests.Select(l => l).Where(l => l.Id == test.Id).ToList();
            DB.Tests.RemoveRange(result);
            DB.SaveChanges();
        }

        
        /// <summary>
        /// Получить список всех препаратов из базы данных
        /// </summary>
        /// <returns></returns>
        public static List<HealthTestDrug> GetAllDrugs()
        {
            return DB.Drugs.ToList();
        }

        /// <summary>
        /// Получаем список всех рецептов из базы данных
        /// </summary>
        /// <returns></returns>
        public static List<HealthTestReciept> GetAllReciepts()
        {
            return DB.Reciepts.ToList(); ;
        }

        #region РАБОТА С ТАБЛИЦЕЙ ПОСЛЕДНИХ ВЫБРАННЫХ ТЕСТОВ

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

        

        #endregion
    }
}
