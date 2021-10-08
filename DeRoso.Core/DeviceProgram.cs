using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;


namespace DeRoso.Core
{


    [Serializable, XmlRoot("DeviceProgram")]
    public class DeviceProgram : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }



        /// <summary>
        /// Название Раздела
        /// </summary>
        [XmlAttribute(AttributeName = "SectionTitle")]
        public string SectionTitle
        {
            get => _sectionTitle;
            set
            {
                if (value == _sectionTitle)
                    return;

                _sectionTitle = value;
                OnPropertyChanged();
            }
        }
        private string _sectionTitle;


        /// <summary>
        /// Название Подраздела
        /// </summary>
        [XmlAttribute(AttributeName = "SubsectionTitle")]
        public string SubsectionTitle
        {
            get => _subSectionTitle;
            set
            {
                if (value == _subSectionTitle)
                    return;

                _subSectionTitle = value;
                OnPropertyChanged();
            }
        }
        private string _subSectionTitle;


        /// <summary>
        /// Название параметра
        /// </summary>
        [XmlAttribute(AttributeName = "Title")]
        public string Title
        {
            get => _title;
            set
            {
                if (value == _title)
                    return;

                _title = value;
                OnPropertyChanged();
            }
        }
        private string _title;


        /// <summary>
        /// Описание параметра
        /// </summary>
        public string Description
        {
            get => _desc;
            set
            {
                if (value == _desc)
                    return;

                _desc = value;
                OnPropertyChanged();
            }
        }
        private string _desc;


        /// <summary>
        /// Адрес 
        /// </summary>
        [XmlAttribute(AttributeName = "Address")]
        public int Address
        {
            get => _address;
            set
            {
                if (value == _address)
                    return;

                _address = value;
                OnPropertyChanged();
            }
        }
        private int _address;

        /// <summary>
        /// Ячейка 
        /// </summary>
        [XmlAttribute(AttributeName = "Cell")]
        public int Cell
        {
            get => _cell;
            set
            {
                if (value == _cell)
                    return;

                _cell = value;
                OnPropertyChanged();
            }
        }
        private int _cell;

        /// <summary>
        /// Использовать HV
        /// </summary>
        [XmlAttribute(AttributeName = "UseHV")]
        public bool UseHV
        {
            get => _useHV;
            set
            {
                if (value == _useHV)
                    return;

                _useHV = value;
                OnPropertyChanged();
            }
        }
        private bool _useHV = true;

        /// <summary>
        /// Частота HV
        /// </summary>
        public double FrequencyHV
        {
            get => _frequencyHV;
            set
            {
                if (Math.Abs(value - _frequencyHV) < 0.000001)
                    return;

                _frequencyHV = value;
                OnPropertyChanged();
            }
        }
        private double _frequencyHV = 25.0;


        /// <summary>
        /// Пауза перед HV
        /// </summary>
        public TimeSpan PauseBeforeHV
        {
            get => _pauseBeforeHV;
            set
            {
                if (value == _pauseBeforeHV)
                    return;

                _pauseBeforeHV = value;
                OnPropertyChanged();
            }
        }
        private TimeSpan _pauseBeforeHV = TimeSpan.FromSeconds(3.0);

        /// <summary>
        /// Пауза перед выполнением
        /// </summary>
        public TimeSpan PauseBefore
        {
            get => _pauseBefore;
            set
            {
                if (value == _pauseBefore)
                    return;

                _pauseBefore = value;
                OnPropertyChanged();
            }
        }
        private TimeSpan _pauseBefore = TimeSpan.FromSeconds(3.0);

        /// <summary>
        /// Пауза после выполнения
        /// </summary>
        public TimeSpan PauseAfter
        {
            get => _pauseAfter;
            set
            {
                if (value == _pauseAfter)
                    return;

                _pauseAfter = value;
                OnPropertyChanged();
            }
        }
        private TimeSpan _pauseAfter = TimeSpan.FromSeconds(3.0);

        /// <summary>
        /// Время выполнения
        /// </summary>
        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                if (value == _duration)
                    return;

                _duration = value;
                OnPropertyChanged();
            }
        }
        private TimeSpan _duration = TimeSpan.FromSeconds(3.0);

    }
}
