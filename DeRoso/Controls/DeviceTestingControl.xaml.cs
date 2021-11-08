using DeRoso.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace DeRoso.Controls
{
    /// <summary>
    /// Interaction logic for DeviceTestingControl.xaml
    /// </summary>
    public partial class DeviceTestingControl : UserControl
    {
        public DeviceTestingControl()
        {
            InitializeComponent();
            App app = (App)Application.Current;
            DataContext = new DeviceTestingViewModel( app.Device);
        }
    }
}
