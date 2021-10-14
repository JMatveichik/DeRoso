using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DeRoso.Core;
using DeRoso.Core.Device;
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
            App app = (App)System.Windows.Application.Current;
            this.DataContext = new MainWindowViewModel();

            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.Home, HomeExecute, HomeCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.Help, HelpExecute, HelpCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.Archive, ArchiveExecute, ArchiveCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.Programms, ProgrammsExecute, ProgrammsCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.Testing, TestingExecute, TestingCanExecute));

            //byte[] buf = DeviceCommand.CreateCommand(EnumDeviceCommands.MeteringOn, new byte[] { 1, 2, 3, 4 });
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

        /*

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            HwndSource src = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            src.AddHook(new HwndSourceHook(WndProc));
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            Message m = new Message();
            m.HWnd = hwnd;
            m.Msg = msg;
            m.WParam = wParam;
            m.LParam = lParam;

            //usb.ParseMessages(ref m);
            return IntPtr.Zero;
        }

        private void OnMainWindowSourceInitialized(object sender, EventArgs e)
        {
            base.OnSourceInitialized(e);
            var handle = new WindowInteropHelper(this).Handle;

           usb.RegisterHandle(handle);
        }
        */
    }
}
