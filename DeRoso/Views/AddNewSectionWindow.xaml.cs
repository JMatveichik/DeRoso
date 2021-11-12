using System.Windows;
using DeRoso.Core.Health;

namespace DeRoso.Views
{
    /// <summary>
    /// Interaction logic for AddNewSectionWindow.xaml
    /// </summary>
    public partial class AddNewSectionWindow : Window
    {
        public AddNewSectionWindow()
        {
            InitializeComponent();
            NewSection  = new HealthTestSection()
            {
                Title = "Новая секция",
                Description = "Краткое описание секции"
            };

            this.DataContext = NewSection;
        }

        public HealthTestSection NewSection
        {
            get;
            private set;
        }

        private void OnAddNewSectionButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            NewSection = null;
            DialogResult = false;
            this.Close();
        }

    }
}
