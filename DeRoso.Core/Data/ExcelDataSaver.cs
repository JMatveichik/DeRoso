using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeRoso.Core.Health;

namespace DeRoso.Core.Data
{
    public class ExcelResultsSaver : FileOutputResultSaver
    {
        /// <summary>
        /// Конструктор класса сохранения в Файл Excel
        /// </summary>
        /// <param name="targetPath">Путь к файлу сохранения</param>
        public ExcelResultsSaver(string targetPath) : base(targetPath)
        {

        }

        /// <summary>
        /// Сохранение результатов 
        /// </summary>
        /// <param name="results"></param>
        public override bool Save(HealthTestResult results)
        {
            throw new NotImplementedException();
        }
        
    }
}
