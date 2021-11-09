using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeRoso.Core.Health
{
    [Table("Groups")]
    public class HealthTestGroup : HealthTestItem
    {
        public int HealthTestSectionId { get; set; }

        public HealthTestSection Section { get; set; }

        public ObservableCollection<HealthTest> Tests
        {
            get => _tests;
            set
            {
                if (value == _tests)
                    return;

                _tests = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<HealthTest> _tests = null;
    }
}
