using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeRoso.Core.Health;

namespace DeRoso.Core.Data
{
    public abstract class FileOutputResultSaver : IResultsSaver
    {
        /// <summary>
        /// Конструктор класса сохранения результатов во внешний файл
        /// </summary>
        /// <param name="targetPath"></param>
        public FileOutputResultSaver(string targetPath)
        {
            TargetFilePath = targetPath;
        }
        

        /// <summary>
        /// Путь к файлу сохранения
        /// </summary>
        public string TargetFilePath
        {
            get;
            private set;
        }

        /// <summary>
        /// Сохранение результатов
        /// </summary>
        /// <param name="results"></param>
        abstract public bool Save(IEnumerable <HealthTestResult> results);
        
    }
}
