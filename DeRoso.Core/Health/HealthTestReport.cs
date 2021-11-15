using DeRoso.Core.Data;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace DeRoso.Core.Health
{
    public class HealthTestReport : ViewModelBase
    {        
        /// <summary>
        /// Тестируемый пациент
        /// </summary>
        public Patient Patient
        {
            get => _pacient;
            set
            {
                if (_pacient == value)
                    return;

                _pacient = value;

                //Отчет изменен
                //IsModified = true;
                OnPropertyChanged();
            }
        }
        private Patient _pacient;

        /// <summary>
        /// Результаты всех тестов
        /// </summary>
        public ObservableCollection<HealthTestResult> Results
        {
            get;
            private set;
        } = new ObservableCollection<HealthTestResult>();        

        /// <summary>
        /// Очистить список результатов
        /// </summary>
        public void Clear()
        {
            Results.Clear();
            IsModified = false;
        }
        
        /// <summary>
        /// Флаг наполнения результатов
        /// </summary>
        public bool IsEmpty => Results.Count > 0;



        /// <summary>
        /// Дата и время отчета
        /// </summary>
        public DateTime ReportDate
        {
            get => _reportDate;
            set
            {
                if (_reportDate == value)
                    return;

                _reportDate = value;

                //отчет изменен
                IsModified = true;
                OnPropertyChanged();
            }
        }
        private DateTime _reportDate = DateTime.Now;

        /// <summary>
        /// Отчет изменен
        /// </summary>
        public bool IsModified
        {
            get => _isModified;
            set
            {
                if (_isModified == value)
                    return;

                _isModified = value;
                OnPropertyChanged();
            }
        }
        private bool _isModified = false;

        /// <summary>
        /// Результат тестирования
        /// </summary>
        /// <param name="tr"></param>
        public void AddTestResult(HealthTestResult tr)
        {
            Results.Add(tr);
            IsModified = true;
        }



        /// <summary>
        /// Сохранение по умолчанию в файл Excel
        /// </summary>
        public bool Save(bool showResults = true)
        {
            string patientDir = Directory.GetCurrentDirectory() + "\\";
            string patientName = null;

            if (Patient != null)
            {
                patientDir += Patient.ShortName;
                patientName = Patient.FamilyName;
            }
            else
            {
                patientDir += "Незарегистрированные";
                patientName = "Пациент";
            }
            
            Directory.CreateDirectory(patientDir);
            ReportDate = DateTime.Now;

            string fileName =
                $"{patientName}_{ReportDate.ToShortDateString()}_{ReportDate.Hour}_{ReportDate.Minute}{".xlsx"}";

            string path = patientDir + "\\" + fileName;
            
            if (Save(new ExcelResultsSaver(path), showResults))
            {
                IsModified = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Сохранением с использованием конкретного интерфейса
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="showResults">Отобразить результаты после сохранения</param>
        public bool Save(IResultsSaver sp, bool showResults)
        {
            return sp.Save(this, showResults);
        }
    }
}
