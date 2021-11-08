using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeRoso.Core.Health
{
    [Table ("Sections")]
    public class HealthTestSection : HealthTestItem
    {
        public ObservableCollection<HealthTestGroup> Groups { get; set; }
    }
}
