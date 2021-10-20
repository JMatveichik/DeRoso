using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Health
{
    [Table ("Sections")]
    public class HealthTestSection : HealthTestItem
    {
        public ObservableCollection<HealthTestGroup> Groups { get; set; }
    }
}
