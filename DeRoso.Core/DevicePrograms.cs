using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace DeRoso.Core
{
    public class DevicePrograms : ViewModelBase
    {        
        public void Load(string path)
        {
            Programs.Clear();

            XmlRootAttribute xRoot = new XmlRootAttribute {ElementName = "DevicePrograms", IsNullable = false};

            XmlSerializer formatter = new XmlSerializer(typeof(List<DeviceProgram>), xRoot);
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                Programs = ( List<DeviceProgram>) formatter.Deserialize(fs);
            }

            /*foreach (DeviceProgram p in Programs)
            {
                Debug.WriteLine($"Раздел: {p.SectionTitle} --- Секция: {p.SubsectionTitle} --- Название: {p.Title}");
            }
            */

            ProgramsFES = Programs.Where(p => p.SectionTitle == "Результаты ФЭС").Select(p=>p).ToList();
            ProgramsStress = Programs.Where(p => p.SectionTitle == "Результаты стресс отягощений").Select(p => p).ToList();
            ProgramsEnergo = Programs.Where(p => p.SectionTitle == "Результаты энергосостояния").Select(p => p).ToList();
        }

        public void Save(string path)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(DeviceProgram[]));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Programs);
            }
        }

        public List<DeviceProgram> Programs
        {
            get;
            private set;
        } = new List<DeviceProgram>();


        public List<DeviceProgram> ProgramsFES
        {
            get;
            private set;
        } = new List<DeviceProgram>();


        public List<DeviceProgram> ProgramsStress
        {
            get;
            private set;
        } = new List<DeviceProgram>();


        public List<DeviceProgram> ProgramsEnergo
        {
            get;
            private set;
        } = new List<DeviceProgram>();


        public DeviceProgram Selected
        {
            get => _selectedProg;
            set
            {
                if (value == _selectedProg)
                    return;

                _selectedProg = value;
                OnPropertyChanged();
            }
        }

        private DeviceProgram _selectedProg = null;
    }

}
