using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DeRoso.Core;
using DeRoso.ViewModels;
using MahApps.Metro.Controls;


namespace DeRoso
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            App app = (App)Application.Current;
            this.DataContext = new MainWindowViewModel();

            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.Home, HomeExecute, HomeCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.Help, HelpExecute, HelpCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.Archive, ArchiveExecute, ArchiveCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.Programms, ProgrammsExecute, ProgrammsCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.Testing, TestingExecute, TestingCanExecute));
        }

        private void TestingCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void TestingExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;
            vm.CurrentContent = ContentManager.Instance.GetContent("TESTING");
        }

        private void ProgrammsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ProgrammsExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;
            vm.CurrentContent = ContentManager.Instance.GetContent("PROGRAMMS");
        }

        private void ArchiveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        private void ArchiveExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;
            vm.CurrentContent = ContentManager.Instance.GetContent("ARCHIVE");
        }

        private void HomeExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;
            vm.CurrentContent = ContentManager.Instance.GetContent("HOME");
        }

        private void HomeCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void HelpExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;
            vm.CurrentContent = ContentManager.Instance.GetContent("HELP");
        }
        private void HelpCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }
    }
}
