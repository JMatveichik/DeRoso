using DeRoso.Core.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Health
{
    [Table("Patient")]
    public class Patient : ViewModelBase
    {
        /// <summary>
        /// Идентификатор пациента
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя пациента
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName == value)
                    return;

                _firstName = value;
                OnPropertyChanged();
                OnPropertyChanged("FullName");
                OnPropertyChanged("ShortName");
            }
        }
        private string _firstName;


        /// <summary>
        /// Отчество пациента
        /// </summary>
        public string SecondName
        {
            get { return _secondName; }
            set
            {
                if (_secondName == value)
                    return;

                _secondName = value;
                OnPropertyChanged();
                OnPropertyChanged("FullName");
                OnPropertyChanged("ShortName");
            }
        }
        private string _secondName;


        /// <summary>
        /// Фамилия пациента
        /// </summary>
        public string FamilyName
        {
            get { return _familyName; }
            set
            {
                if (_familyName == value)
                    return;

                _familyName = value;
                OnPropertyChanged();
                OnPropertyChanged("FullName");
                OnPropertyChanged("ShortName");

            }
        }
        private string _familyName;


        /// <summary>
        /// Пол пациента
        /// </summary>
        public EnumPatientGender Gender
        {
            get { return _gender; }
            set
            {
                if (_gender == value)
                    return;

                _gender = value;
                OnPropertyChanged();
            }
        }
        private EnumPatientGender _gender;


        /// <summary>
        /// День рождения
        /// </summary>
        public DateTime BirthDay
        {
            get { return _birthDay; }
            set
            {
                if (_birthDay == value)
                    return;

                _birthDay = value;
                OnPropertyChanged();
            }
        }
        private DateTime _birthDay;


        [NotMapped]
        /// <summary>
        /// Полное имя пациента Фамилия Имя Отчество
        /// </summary>
        public string FullName
        {
            get
            {
                return string.Format("{0} {1} {2}", FamilyName, FirstName, SecondName);
            }
        }

        [NotMapped]
        /// <summary>
        /// Сокращенное имя пациента Фамилия И. О.
        /// </summary>
        public string ShortName
        {
            get
            {
                char f = ' ';
                char s = ' ';

                if (FirstName != null)
                    f = FirstName.Length == 0 ? ' ' : FirstName[0];

                if (SecondName != null)
                    s = SecondName.Length == 0 ? ' ' : SecondName[0];

                return string.Format("{0} {1}. {2}.", FamilyName, f, s);
            }
        }


        public static Patient Add(string family = null, string fname = null, string sname = null, EnumPatientGender gender = EnumPatientGender.Male, DateTime? birth = null)
        {
            using (DeRosoContext db = new DeRosoContext())
            {
                Patient patient = new Patient()
                {
                    FirstName = fname == null ? "Имя" : fname,
                    FamilyName = family == null ? "Фамилия" : family,
                    SecondName = sname == null ? "Отчество" : sname,
                    Gender = gender,
                    BirthDay = birth.HasValue ? birth.Value : DateTime.Now
                };

                db.Patients.Add(patient);
                db.SaveChanges();

                return patient;
            }
        }

        public static ObservableCollection<Patient> All ()
        {
            using (DeRosoContext db = new DeRosoContext())
            {
                ObservableCollection<Patient> all = new ObservableCollection<Patient>(db.Patients.ToList());
                return all;
            }
        }

        public static void Remove(Patient patient)
        {
            using (DeRosoContext db = new DeRosoContext())
            {
                db.Patients.Remove(patient);
                db.SaveChanges();
            }
        }
    }
}
