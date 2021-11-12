using System.Windows;
using DeRoso.Core.Health;

namespace DeRoso.Views
{
    /// <summary>
    /// Interaction logic for AddNewGroupWindow.xaml
    /// </summary>
    public partial class AddNewGroupWindow : Window
    {
        public AddNewGroupWindow(HealthTestSection parent)
        {
            InitializeComponent();
            NewTestGroup = new HealthTestGroup()
            {
                Title = "Новая группа",
                Description = "Краткое описание группы",
                HealthTestSectionId = parent.Id
            };
            this.DataContext = NewTestGroup;
        }

        public HealthTestGroup NewTestGroup
        {
            get;
            private set;
        }

        private void OnAddNewGroupButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            NewTestGroup = null;
            DialogResult = false;
            this.Close();
        }

    }
}
