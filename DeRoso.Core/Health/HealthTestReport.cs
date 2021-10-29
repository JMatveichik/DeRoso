using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Health
{
    public class HealthTestReport : ViewModelBase
    {
        
        /// <summary>
        /// Тестируемый пациент
        /// </summary>
        public Patient Patient
        {
            get { return _pacient; }
            set
            {
                if (_pacient == value)
                    return;

                _pacient = value;

                //Отчет изменен
                IsModified = true;
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
        /// Дата и время отчета
        /// </summary>
        public DateTime ReportDate
        {
            get { return _reportDate; }
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
            get { return _isModified; }
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
    }
}
