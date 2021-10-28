using DeRoso.Core.Health;
using DeRoso.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ViewTestSelection.xaml
    /// </summary>
    public partial class ViewTestSelection : UserControl
    {
        public ViewTestSelection()
        {
            InitializeComponent();
            this.DataContext = new TestSelectionViewModel(((App)Application.Current).DeRossoData);
        }


        private void OnTestsListBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            object data = GetDataFromListBox(parent, e.GetPosition(parent));

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.All);
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

        private bool  CanAddToSelectedTests(HealthTest test)
        {           

            if (test == null)
                return false;
            
            if (!test.ContainValidReciepts())
                return false;

            if (HealthTestSelected.Tests.Contains(test))
                return false;

            return true;
        }

        private void SelectedTestsListBoxDrop(object sender, DragEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            HealthTest test = (HealthTest)e.Data.GetData(typeof(HealthTest));
            
            if (CanAddToSelectedTests(test))
                HealthTestSelected.Tests.Add (test);
        }

        private void OnTestsListBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ListBox parent = (ListBox)sender;
                HealthTest test = (HealthTest)GetDataFromListBox(parent, e.GetPosition(parent));
                
                if (CanAddToSelectedTests(test))
                    HealthTestSelected.Tests.Add(test);
            }
            e.Handled = false;
        }

        private void OnButtonAddAllAvailableTests(object sender, RoutedEventArgs e)
        {
            TestSelectionViewModel vm = this.DataContext as TestSelectionViewModel;
            
            foreach (HealthTest t in vm.SelectedGroup.Tests)
            {
                if (CanAddToSelectedTests(t))
                    HealthTestSelected.Tests.Add(t);
            }
        }

        private void OnButtonAddAllSectionTests(object sender, RoutedEventArgs e)
        {
            TestSelectionViewModel vm = this.DataContext as TestSelectionViewModel;
            HealthTestSelected.Tests.Clear();

            foreach (HealthTestGroup gr in vm.SelectedSection.Groups)
            {
                foreach (HealthTest t in gr.Tests)
                {
                    if (CanAddToSelectedTests(t))
                        HealthTestSelected.Tests.Add(t);
                }
            }
            
        }

        private void OnButtonAddAllGroupTests(object sender, RoutedEventArgs e)
        {
            TestSelectionViewModel vm = this.DataContext as TestSelectionViewModel;                        
            foreach (HealthTest t in vm.SelectedGroup.Tests)
            {
                if (CanAddToSelectedTests(t))
                    HealthTestSelected.Tests.Add(t);
            }            
        }


        private void OnButtonClearTargetTests(object sender, RoutedEventArgs e)
        {
            HealthTestSelected.Tests.Clear();
        }

        private void SelectedTargetTestsListKeyDown(object sender, KeyEventArgs e)
        {
            TestSelectionViewModel vm = this.DataContext as TestSelectionViewModel;
            HealthTestSelected.Tests.Remove(vm.SelectedTargetTest);
        }

        private void OnItemDeleteClick(object sender, RoutedEventArgs e)
        {
            Button btn = e.OriginalSource as Button;

            if (btn == null)
                return;

            HealthTest test = (HealthTest)btn.DataContext;
            HealthTestSelected.Tests.Remove(test);            
        }
    }
}
