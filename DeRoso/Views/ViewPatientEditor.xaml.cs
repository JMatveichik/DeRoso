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
    /// Interaction logic for ViewUsersEditor.xaml
    /// </summary>
    public partial class ViewPatientEditor : UserControl
    {
        public ViewPatientEditor()
        {
            InitializeComponent();
            PatientsViewModel vm = new PatientsViewModel(((App)Application.Current).DeRossoData);
            this.DataContext = vm;

            vm.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedPatient")
            {
                PatientsViewModel vm = (PatientsViewModel)this.DataContext;
                vm.SelectedPatient.PropertyChanged += OnSelectedPatientPropertyChanged; ;
            }
        }

        private void OnSelectedPatientPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Gender")
            {
                lstPacients.Items.Refresh();
            }
        }

        private void OnAddPatientButtonClick(object sender, RoutedEventArgs e)
        {
            Patient patient = new Patient() { FamilyName = "Новый", FirstName = "Пациент" };
            PatientsViewModel vm = (PatientsViewModel)this.DataContext;

            vm.Patients.Add(patient);            
            vm.SelectedPatient = patient;
        }

        private void OnPatientListBoxButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = e.OriginalSource as Button;

            if (btn == null)
                return;

            Patient patient = (Patient)btn.DataContext;
            PatientsViewModel vm = (PatientsViewModel)this.DataContext;
            vm.Patients.Remove(patient);
            

        }
    }
}
