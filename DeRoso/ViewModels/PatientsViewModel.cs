using DeRoso.Core;
using DeRoso.Core.Data;
using DeRoso.Core.Health;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace DeRoso.ViewModels
{
    public class PatientsViewModel : ViewModelBase
    {
        public PatientsViewModel(DeRosoContext db)
        {
            DeRossoData = db;
            DeRossoData.Patients.Load();
            Patients = DeRossoData.Patients.Local.ToBindingList();
        }

        public DeRosoContext DeRossoData
        {
            get;
            private set;
        }

        /// <summary>
        /// Таблица пациентов
        /// </summary>
        public BindingList <Patient> Patients
        {
            get;
            private set;
        }
        


        /// <summary>
        /// Выбранный пациент
        /// </summary>
        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                if (_selectedPatient == value)
                    return;

                _selectedPatient = value;
                OnPropertyChanged();
            }
        }
        private Patient _selectedPatient;

        

    }
}
