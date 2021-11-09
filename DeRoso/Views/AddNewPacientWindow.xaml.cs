using DeRoso.Core.Health;
using System;
using System.Windows;

namespace DeRoso.Views
{
    /// <summary>
    /// Interaction logic for AddNewPacientWindow.xaml
    /// </summary>
    public partial class AddNewPacientWindow : Window
    {


        public AddNewPacientWindow()
        {
            InitializeComponent();
            NewPatient = new Patient() { FamilyName = "Новый", FirstName="Пациент", BirthDay = DateTime.Now, Gender = EnumPatientGender.Female };
            this.DataContext = NewPatient;
        }        

        public Patient NewPatient
        {
            get => _newPatient;
            private set
            {
                if (_newPatient == value)
                    return;

                _newPatient = value;
            }
        }
        private Patient _newPatient;

        private void OnAddNewPatientButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            NewPatient = null;
            DialogResult = false;
            this.Close();
        }
        
    }
}
