using DeRoso.Core.Data;
using DeRoso.Core.Health;
using DeRoso.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        

        private void SelectedTestsListBoxDrop(object sender, DragEventArgs e)
        {
            HealthTest test = (HealthTest)e.Data.GetData(typeof(HealthTest));

            DeRossoDataWorker.AddToLastSelectedTests(test);
            TestSelectionViewModel vm = this.DataContext as TestSelectionViewModel;
            vm.Update();
        }

        private void TestsListBoxOnPreviewMouseMove(object sender, MouseEventArgs e)
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

        private void OnTestsListBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ListBox parent = (ListBox)sender;
                HealthTest test = (HealthTest)GetDataFromListBox(parent, e.GetPosition(parent));
                
                DeRossoDataWorker.AddToLastSelectedTests(test);

                TestSelectionViewModel vm = this.DataContext as TestSelectionViewModel;
                vm.Update();
            }
            e.Handled = false;
        }

        private void OnButtonAddAllAvailableTests(object sender, RoutedEventArgs e)
        {
            TestSelectionViewModel vm = this.DataContext as TestSelectionViewModel;
            DeRossoDataWorker.AddToLastSelectedTests(vm.SelectedGroup.Tests);
            
            vm.Update();
        }

        private void OnButtonAddAllSectionTests(object sender, RoutedEventArgs e)
        {
            TestSelectionViewModel vm = this.DataContext as TestSelectionViewModel;
            DeRossoDataWorker.ClearLastSelectedTests();

            foreach (HealthTestGroup gr in vm.SelectedSection.Groups)
                DeRossoDataWorker.AddToLastSelectedTests(gr.Tests);
            
            vm.Update();
        }

        private void OnButtonAddAllGroupTests(object sender, RoutedEventArgs e)
        {
            TestSelectionViewModel vm = this.DataContext as TestSelectionViewModel;
            DeRossoDataWorker.AddToLastSelectedTests(vm.SelectedGroup.Tests);            
            vm.Update();
        }


        private void OnButtonClearTargetTests(object sender, RoutedEventArgs e)
        {
            TestSelectionViewModel vm = this.DataContext as TestSelectionViewModel;
            DeRossoDataWorker.ClearLastSelectedTests();
            vm.Update();
        }

        private void SelectedTargetTestsListKeyDown(object sender, KeyEventArgs e)
        {
            TestSelectionViewModel vm = this.DataContext as TestSelectionViewModel;
            DeRossoDataWorker.RemoveFromLastSelectedTests(vm.SelectedTargetTest);
        }

        private void OnItemDeleteClick(object sender, RoutedEventArgs e)
        {
            Button btn = e.OriginalSource as Button;

            if (btn == null)
                return;

            TestSelectionViewModel vm = this.DataContext as TestSelectionViewModel;

            HealthTest test = (HealthTest)btn.DataContext;
            DeRossoDataWorker.RemoveFromLastSelectedTests(test);

            vm.Update();
        }

       
    }
}
