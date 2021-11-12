using DeRoso.Core.Data;
using DeRoso.Core.Health;
using DeRoso.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DeRoso.Views
{
    /// <summary>
    /// Interaction logic for ViewTestsEditor.xaml
    /// </summary>
    public partial class ViewTestsEditor : UserControl
    {
        public ViewTestsEditor()
        {
            InitializeComponent();
            this.DataContext =  new DataViewModel( );

            DataViewModel vm = (DataViewModel)this.DataContext;
            vm.PropertyChanged += OnModelPropertyChanged;

            
        }

        private void OnModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ( e.PropertyName == "SelectedTest" )
            {
                DataViewModel vm = (DataViewModel)this.DataContext;
                var drugs = vm.SelectedTest?.Reciepts;

                if (vm.SelectedTest == null)
                    return;

                if (drugs == null)
                    drugs = vm.SelectedTest.Reciepts = new ObservableCollection<HealthTestReciept>();

                drugs.CollectionChanged += DrugsCollectionChanged;

            }
        }

        private void DrugsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {           
            lstboxGruopTest.Items.Refresh();            
        }       

        private static object GetDataFromListBox(ListBox source, Point point)
        {
            if (source.InputHitTest(point) is UIElement element)
            {
                object data = DependencyProperty.UnsetValue;

                while (data == DependencyProperty.UnsetValue)
                {
                    
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                        element = VisualTreeHelper.GetParent(element) as UIElement;

                    if (element == null)
                        return null;

                    if (element.Equals(source))
                        return null;

                }

                if (data != DependencyProperty.UnsetValue)
                    return data;
            }

            return null;
        }

       private void OnDrugsListBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataViewModel vm = (DataViewModel)this.DataContext;

            if (e.ChangedButton == MouseButton.Left)
            {

                ListBox parent = (ListBox)sender;
                HealthTestDrug drug = (HealthTestDrug)GetDataFromListBox(parent, e.GetPosition(parent));

                vm.AddDrug(vm.SelectedTest, drug);
            }
        }

        private void OnDrugsListBoxPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ListBox parent = (ListBox)sender;
                object data = GetDataFromListBox(parent, e.GetPosition(parent));

                if (data != null)
                {
                    DragDrop.DoDragDrop(parent, data, DragDropEffects.All);
                }
            }
        }

        private void OnAddNewSectionButtonClick(object sender, RoutedEventArgs e)
        {
            AddNewSectionWindow wnd = new AddNewSectionWindow();
            bool? res = wnd.ShowDialog();

            //добавляем нового пациента
            if (res.HasValue && res.Value == true)
            {
                if (wnd.NewSection != null)
                {
                    //добавить новую секцию в базу данных
                    DeRossoDataWorker.AddSection(wnd.NewSection);
                    //выбираем вновь добавленную секцию
                    HealthTestSection sel = DeRossoDataWorker.GetSection(wnd.NewSection.Title);

                    //обновляем данные модели
                    DataViewModel vm = (DataViewModel)this.DataContext;
                    vm.Update(sel);
                    
                }
            }
        }

        private void OnDeleteCurrentSectionButtonClick(object sender, RoutedEventArgs e)
        {
            DataViewModel vm = (DataViewModel)this.DataContext;
            if (vm.SelectedSection != null)
            {
                string msg = "Удаление секции приведет к каскадному удалению разделов входящих в секцию,\n" +
                             "а так же всех тестов входящих в эти разделы!\n\n" +
                             $"Вы действительно хотите удалить секцию <{vm.SelectedSection.Title}>?";


                MessageBoxResult res = MessageBox.Show(msg, "УДАЛЕНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (res == MessageBoxResult.Yes)
                {
                    DeRossoDataWorker.RemoveSection(vm.SelectedSection);
                    vm.Update();
                }
            }
            
        }

        private void OnAddNewGroupButtonClick(object sender, RoutedEventArgs e)
        {
            DataViewModel vm = (DataViewModel)this.DataContext;
            HealthTestSection sec = vm.SelectedSection;

            if (sec == null)
                return;

            AddNewGroupWindow wnd = new AddNewGroupWindow(vm.SelectedSection);
            bool? res = wnd.ShowDialog();

            //добавляем нового пациента
            if (res.HasValue && res.Value == true)
            {
                if (wnd.NewTestGroup != null)
                {
                    //добавить новую секцию в базу данных
                    DeRossoDataWorker.AddGroup(wnd.NewTestGroup);

                    //выбираем вновь добавленную секцию
                    HealthTestGroup gr = DeRossoDataWorker.GetGroup(wnd.NewTestGroup.Title);

                    //обновляем данные модели
                    vm.Update(sec, gr);
                }
            }
        }

        private void OnDeleteCurrentGroupButtonClick(object sender, RoutedEventArgs e)
        {
            DataViewModel vm = (DataViewModel)this.DataContext;
            HealthTestSection sec = vm.SelectedSection;

            if (vm.SelectedGroup != null)
            {
                string msg = "Удаление группы приведет к каскадному удалению тестов,\n" +
                             "входящих в данную группу\n\n" +
                             $"Вы действительно хотите удалить группу <{vm.SelectedGroup.Title}>?";


                MessageBoxResult res = MessageBox.Show(msg, "УДАЛЕНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (res == MessageBoxResult.Yes)
                {
                    DeRossoDataWorker.RemoveGroup(vm.SelectedGroup);
                    vm.Update(sec);
                }
            }
        }


        private void OnAddNewTestButtonClick(object sender, RoutedEventArgs e)
        {
            DataViewModel vm = (DataViewModel)this.DataContext;
            HealthTestSection sec = vm.SelectedSection;
            HealthTestGroup  gr = vm.SelectedGroup;

            if (sec == null || gr == null)
                return;

            AddNewTestWindow wnd = new AddNewTestWindow(gr);
            bool? res = wnd.ShowDialog();

            //добавляем нового пациента
            if (res.HasValue && res.Value == true)
            {
                if (wnd.NewTest != null)
                {
                    //добавить новую секцию в базу данных
                    DeRossoDataWorker.AddTest(wnd.NewTest);

                    //выбираем вновь добавленную секцию
                    HealthTest test = DeRossoDataWorker.GetTest(wnd.NewTest.Title);

                    //обновляем данные модели
                    vm.Update(sec, gr, test);
                }
            }
        }

        private void OnDeleteCurrentTestButtonClick(object sender, RoutedEventArgs e)
        {
            DataViewModel vm = (DataViewModel)this.DataContext;

            if (vm.SelectedTest != null)
            {
                string msg = $"Вы действительно хотите удалить выбранный тест   <{vm.SelectedTest.Title}> ?";


                MessageBoxResult res = MessageBox.Show(msg, "УДАЛЕНИЕ", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);


                if (res == MessageBoxResult.Yes)
                {
                    DeRossoDataWorker.RemoveTest(vm.SelectedTest);

                    HealthTestSection sec = vm.SelectedSection;
                    HealthTestGroup gr = vm.SelectedGroup;

                    vm.Update(sec, gr);
                }
            }
        }
    }
}
