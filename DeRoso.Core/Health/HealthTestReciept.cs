using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Health
{
    [Table("Reciepts")]
    public class HealthTestReciept
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int Id
        {
            get;
            set;
        }
    }
}
