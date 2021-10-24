using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
using DeRoso.Core.Health;
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


            //привязка команд
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.HomePage, HomePageExecute, HomePageCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.HelpPage, HelpPageExecute, HelpPageCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.ArchivePage, ArchivePageExecute, ArchivePageCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.EditDBPage, EditDBPageExecute, EditDBPageCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.TestingPage, TestingPageExecute, TestingPageCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.SelectTestsPage, SelectTestsPagesExecute, SelectTestsPageCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.StartTestDevice, StartTestDeviceExecute, StartTestDeviceCanExecute));

            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.StartTest, StartTestExecute, StartTestCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.StopTest, StopTestExecute, StopTestCanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindowViewModel.PauseTest, PauseTestExecute, PauseTestCanExecute));

        }

        /// <summary>
        /// Возможно начать процесс тестирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseTestCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void PauseTestExecute(object sender, ExecutedRoutedEventArgs e)
        {
            DeviceProvider dev = ((App)App.Current).Device;

            //Task test = new Task(() => dev.Do(HealthTestSelected.Tests));
            //test.Start();
        }

        /// <summary>
        /// Возможно начать процесс тестирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopTestCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void StopTestExecute(object sender, ExecutedRoutedEventArgs e)
        {
            DeviceProvider dev = ((App)App.Current).Device;

            //Task test = new Task(() => dev.Do(HealthTestSelected.Tests));
            //test.Start();
        }



        /// <summary>
        /// Возможно начать процесс тестирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartTestCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = HealthTestSelected.Tests.Count == 0 ? false : true;
        }

        /// <summary>
        /// Начать процесс тестирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartTestExecute(object sender, ExecutedRoutedEventArgs e)
        {
            DeviceProvider dev = ((App)App.Current).Device;            

            Task test = new Task(() => dev.Do(HealthTestSelected.Tests));
            test.Start();
        }


        /// <summary>
        /// Запуск тестирования прибора возможен
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartTestDeviceCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Запуск процедуры тестирования прибора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartTestDeviceExecute(object sender, ExecutedRoutedEventArgs e)
        {
            DeviceProvider dev = ((App)App.Current).Device;
            dev.Reset();

            Task<bool> test = new Task<bool>(() => dev.Test());
            test.Start();
        }



        /// <summary>
        /// Возможность перехода на страницу выбора тестов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectTestsPageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Переход на страницу запуска выбора тестов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void SelectTestsPagesExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;
            vm.CurrentContent = ContentManager.Instance.GetContent("SELECTION");
        }

        /// <summary>
        /// Возможность перехода на страницу запуска тестирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestingPageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Переход на страницу запуска тестирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void TestingPageExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;
            vm.CurrentContent = ContentManager.Instance.GetContent("TESTING");
        }

        /// <summary>
        /// Возможность перехода на страницу редактирования базы данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditDBPageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Переход на страницу редактирования базы данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>  
        private void EditDBPageExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;
            vm.CurrentContent = ContentManager.Instance.GetContent("EDIT");
        }

        /// <summary>
        /// Возможность перехода на страницу помощи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArchivePageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        /// <summary>
        /// Переход на страницу архива данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>  
        private void ArchivePageExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;
            vm.CurrentContent = ContentManager.Instance.GetContent("HELP");
        }


        /// <summary>
        /// Возможность перехода на страницу помощи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpPageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }


        /// <summary>
        /// Переход на страницу помощи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void HelpPageExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;
            vm.CurrentContent = ContentManager.Instance.GetContent("HELP");
        }



        /// <summary>
        /// Возможность перехода на главную страницу 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomePageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Переход на главную страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomePageExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;
            vm.CurrentContent = ContentManager.Instance.GetContent("HOME");
        }


        /*
        private void SelectTestsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SelectTestsExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;
            vm.CurrentContent = ContentManager.Instance.GetContent("SELECTION");
        }

        private void TestDeviceCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void TestDeviceExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;
            vm.CurrentContent = ContentManager.Instance.GetContent("TESTING");

           
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

        private void EditDBCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void EditDBExecute(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindowViewModel vm = this.DataContext as MainWindowViewModel;
            vm.CurrentContent = ContentManager.Instance.GetContent("EDIT");
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

        */


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
