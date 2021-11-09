using DeRoso.Core.Health;
using DeRoso.ViewModels;
using System.Collections.ObjectModel;
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
            this.DataContext =  new DataViewModel( ((App)Application.Current).DeRossoData );

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

        private void OnDeleteCurrentSectionButtonClick(object sender, RoutedEventArgs e)
        {
            /*DataViewModel vm = (DataViewModel)this.DataContext;
            if (vm.SelectedSection != null)
            {
                string msg = string.Format("Вы действительно хотите удалить секцию <{0}>?", vm.SelectedSection.Title);
                MessageBoxResult res = MessageBox.Show(msg, "УДАЛЕНИЕ", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                    vm.Sections.Remove(vm.SelectedSection);
            }
            */
        }

        private void OnAddNewSectionButtonClick(object sender, RoutedEventArgs e)
        {
            /*
            DataViewModel vm = (DataViewModel)this.DataContext;
            HealthTestSection sec = new HealthTestSection() { Title = "Новая секция", Description = "Описание новой секции" };
            vm.Sections.Add(sec);
            vm.SelectedSection = sec;
            */
        }

        private void OnDeleteCurrentGroupButtonClick(object sender, RoutedEventArgs e)
        {
            /*DataViewModel vm = (DataViewModel)this.DataContext;
            if (vm.SelectedGroup != null)
            {
                string msg = string.Format("Вы действительно хотите удалить раздел <{0}>?", vm.SelectedGroup.Title);
                MessageBoxResult res = MessageBox.Show(msg, "УДАЛЕНИЕ", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    vm.SelectedSection.Groups.Remove(vm.SelectedGroup);
                    vm.Groups.Remove(vm.SelectedGroup);
                }                    
            }
            */
        }

        private void OnAddNewGroupButtonClick(object sender, RoutedEventArgs e)
        {
            /*
            DataViewModel vm = (DataViewModel)this.DataContext;
            HealthTestGroup gr = new HealthTestGroup() { Title = "Новая группа", Description = "Описание новой группы" };

            if (vm.SelectedSection != null)
            {
                if (vm.SelectedSection.Groups == null)
                    vm.SelectedSection.Groups = new ObservableCollection<HealthTestGroup>();

                vm.SelectedSection.Groups.Add(gr);
                vm.SelectedGroup = gr;
            }  
            */
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
    }
}
