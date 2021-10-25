using DeRoso.Core.Health;
using DeRoso.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        private void OnDrugListBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ListBox parent = (ListBox)sender;
                object data = GetDataFromListBox(parent, e.GetPosition(parent));

                if (data != null)
                {
                    DragDrop.DoDragDrop(parent, data, DragDropEffects.All);
                }
                e.Handled = false;
            }
        }

        private static object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
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
    }
}
