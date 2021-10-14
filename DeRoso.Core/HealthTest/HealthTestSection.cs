using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Health
{
    [Table ("Sections")]
    public class HealthTestSection : HealthTestItem
    {
        public List<HealthTestGroup> Groups { get; set; }
    }
}
