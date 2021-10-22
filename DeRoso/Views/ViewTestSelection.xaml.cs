﻿using DeRoso.Core.Health;
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
            
            if (!test.ContainValidDrugs())
                return false;

            TestSelectionViewModel model = (TestSelectionViewModel)this.DataContext;
            if (model.SelectedTests.Contains(test))
                return false;

            return true;
        }

        private void SelectedTestsListBoxDrop(object sender, DragEventArgs e)
        {
            ListBox parent = (ListBox)sender;

            
            HealthTest test = (HealthTest)e.Data.GetData(typeof(HealthTest));
            TestSelectionViewModel model = (TestSelectionViewModel)this.DataContext;

            if (CanAddToSelectedTests(test))
                model.SelectedTests.Add (test);
        }

        private void OnTestsListBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ListBox parent = (ListBox)sender;
                HealthTest test = (HealthTest)GetDataFromListBox(parent, e.GetPosition(parent));
                TestSelectionViewModel model = (TestSelectionViewModel)this.DataContext;

                if (CanAddToSelectedTests(test))
                    model.SelectedTests.Add(test);
            }
            e.Handled = false;
        }
    }
}