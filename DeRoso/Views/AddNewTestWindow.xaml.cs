using System.Windows;
using DeRoso.Core.Health;

namespace DeRoso.Views
{
    /// <summary>
    /// Interaction logic for AddNewTestWindow.xaml
    /// </summary>
    public partial class AddNewTestWindow : Window
    {
        public AddNewTestWindow(HealthTestGroup parent)
        {
            InitializeComponent();
            NewTest = new HealthTest()
            {
                Title = "Новый тест",
                Description = "Краткое описание теста",
                HealthTestGroupId = parent.Id
            };

            this.DataContext = NewTest;
        }

        public HealthTest NewTest
        {
            get;
            private set;
        }

        private void OnAddNewTestButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            NewTest = null;
            DialogResult = false;
            this.Close();
        }

    }
}
