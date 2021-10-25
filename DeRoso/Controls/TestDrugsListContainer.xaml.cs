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
    /// Interaction logic for DrugsListContainer.xaml
    /// </summary>
    public partial class TestDrugsListContainer : UserControl
    {
        public TestDrugsListContainer()
        {
            InitializeComponent();
        }
       

        private void TestDrugsListBoxDrop(object sender, DragEventArgs e)
        {
            ListBox parent = (ListBox)sender;

            DataViewModel model = (DataViewModel)this.DataContext;
            HealthTest test = model.SelectedTest;
            HealthTestDrug drug = (HealthTestDrug)e.Data.GetData(typeof(HealthTestDrug));

            //если пустой список препаратов создаем его
            if (test.Drugs == null)
                test.Drugs = new ObservableCollection<HealthTestDrug>();


            if (test != null)
            {
                test.Drugs.Add(drug);
                model.SelectedDrug = drug;
            }
            
        }

        private void TestDrugsListBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DataViewModel model = (DataViewModel)this.DataContext;

                HealthTest      test = model.SelectedTest;
                HealthTestDrug  drug = model.SelectedDrug;
                
                if (test!= null && drug != null)
                {
                    test.Drugs.Remove(drug);
                    
                }
                    
            }
        }
    }
}
