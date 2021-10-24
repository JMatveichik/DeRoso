using DeRoso.Core.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Data
{
    interface IResultatSaver
    {

        /// <summary>
        /// Сохранение результатов тесирования
        /// </summary>
        /// <param name="results"></param>
        bool Save(HealthTestResult results);
    }
}
