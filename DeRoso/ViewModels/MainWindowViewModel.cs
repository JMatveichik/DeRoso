using DeRoso.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DeRoso.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
        {
            CurrentContent = ContentManager.Instance.GetContent("HOME");
        }

        private UserControl _currentContent;

        public UserControl CurrentContent
        {
            get { return _currentContent; }
            set
            {
                if (_currentContent == value)
                    return;

                _currentContent = value;
                OnPropertyChanged();
            }
        } 


        public static RoutedUICommand Home
        {
            get;
        } = new RoutedUICommand("Go to home page", "Home", typeof(MainWindowViewModel), null);

        public static RoutedUICommand Programms
        {
            get;
        } = new RoutedUICommand("Go to programms page", "Programms", typeof(MainWindowViewModel), null);

        public static RoutedUICommand Archive
        {
            get;
        } = new RoutedUICommand("Go to archive page", "Archive", typeof(MainWindowViewModel), null);

        public static RoutedUICommand Help
        {
            get;
        } = new RoutedUICommand("Go to help page", "Home", typeof(MainWindowViewModel), null);

        public static RoutedUICommand Testing
        {
            get;
        } = new RoutedUICommand("Go to testing page", "Testing", typeof(MainWindowViewModel), null);

    }
}
