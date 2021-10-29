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

namespace DeRoso.Controls
{
    /// <summary>
    /// Interaction logic for ProgrammEditor.xaml
    /// </summary>
    public partial class TestEditor : UserControl
    {
        public TestEditor()
        {
            InitializeComponent();
        }

        private void TestDrugsListBoxDrop(object sender, DragEventArgs e)
        {
            HealthTest test = (HealthTest)this.DataContext;
            HealthTestDrug drug = (HealthTestDrug)e.Data.GetData(typeof(HealthTestDrug));

            if (test == null)
                return;

            //если пустой список препаратов создаем его
            if (test.Reciepts == null)
                test.Reciepts = new ObservableCollection<HealthTestReciept>();

            //выборр препаратов из рецептов
            var drugs = test.Reciepts.Select(d => d.Drug);

            if (drugs == null)
                return;

            if (drugs.Contains(drug))
                return;

            HealthTestReciept reciept = new HealthTestReciept()
            {
                Drug = drug,
                HealthTestDrugId = drug.Id,
                Order = test.Reciepts.Count + 1
            };            

            test.Reciepts.Add(reciept);          
        }

        private void TestDrugsListBoxKeyDown(object sender, KeyEventArgs e)
        {            
            if (e.Key == Key.Delete)
            {
                HealthTestReciept reciept = (HealthTestReciept)this.DataContext;
                HealthTest test = reciept.Test;

                if (test != null )
                    test.Reciepts.Remove(reciept);

            }
        }

        private void OnButtonClearTetsDrugs(object sender, RoutedEventArgs e)
        {
            HealthTest test = (HealthTest)this.DataContext;
            test.Reciepts.Clear();
        }

        private void OnItemDeleteClick(object sender, RoutedEventArgs e)
        {
            Button btn = e.OriginalSource as Button;

            if (btn == null)
                return;

            HealthTestReciept reciept = (HealthTestReciept)btn.DataContext;           
            HealthTest test = (HealthTest)this.DataContext;

            test.Reciepts.Remove(reciept);
            lbxReciepts.Items?.Refresh();
        }
    }
}
