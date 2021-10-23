using DeRoso.Core.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DeRoso.Selectors
{
    public class TestDrugTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NormalTestDrugDataTemplate { get; set; }
        public DataTemplate OptimalTestDrugDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            HealthTestDrugResult res = (HealthTestDrugResult)item;

            if (res.IsOptimal)
                return OptimalTestDrugDataTemplate;

            return NormalTestDrugDataTemplate;
        }
    }
}
