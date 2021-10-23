using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Health
{
    static public class HealthTestSelected
    {
        static HealthTestSelected()
        {

        }

        static public ObservableCollection<HealthTest> Tests
        {
            get;
            private set;
        } = new ObservableCollection<HealthTest>();
    }
}
