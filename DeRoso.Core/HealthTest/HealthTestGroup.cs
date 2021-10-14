using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Health
{
    [Table("Groups")]
    public class HealthTestGroup : HealthTestItem
    {
        public int HealthTestSectionId { get; set; }

        public HealthTestSection Section { get; set; }

        public List<HealthTest> Tests { get; set; }
    }
}
