using DeRoso.Core.Health;
using System.Windows;
using System.Windows.Controls;

namespace DeRoso.Selectors
{
    public class TestListBoxItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ValidTestDrugsDataTemplate { get; set; }
        public DataTemplate NonValidTestDrugsDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            HealthTest test = (HealthTest)item;
            if (test.ContainValidReciepts())
                return ValidTestDrugsDataTemplate;

            return NonValidTestDrugsDataTemplate;
        }
    }
}
