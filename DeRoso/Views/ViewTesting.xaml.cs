using DeRoso.Core.Data;
using DeRoso.Core.Health;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for ViewTesting.xaml
    /// </summary>
    public partial class ViewTesting : UserControl
    {
        public ViewTesting()
        {
            InitializeComponent();
            App app = (App)Application.Current;
            DataContext = app.TestProcessor;
        }

        private void OnAddPacientButtonClick(object sender, RoutedEventArgs e)
        {
            AddNewPacientWindow wnd = new AddNewPacientWindow();
            bool? res = wnd.ShowDialog();
            
            //добавляем нового пациента
            if (res.HasValue && res.Value == true)
            {
                if (wnd.NewPatient != null)
                {
                    DeRossoDataWorker.AddPatient(wnd.NewPatient);
                    HealthTestsProcessor pr = (HealthTestsProcessor)this.DataContext;
                    pr.Update();
                    pr.CurrentPatient = pr.Patients.Select(p => p).Where(p => p.FullName == wnd.NewPatient.FullName).FirstOrDefault(); 
                }                    
                
            }
        }

        private void OnPacientFolderButtonClick(object sender, RoutedEventArgs e)
        {
            Process PrFolder = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            HealthTestsProcessor pr = (HealthTestsProcessor)this.DataContext;

            //string file = @"C:\sample.txt";

            string patientDir = Directory.GetCurrentDirectory() + "\\";

            if (pr.Report.Patient != null)
                patientDir += pr.Report.Patient.ShortName;
            else
                patientDir += "Незарегистрированные";


            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.FileName = "explorer";
            psi.Arguments = patientDir;
            PrFolder.StartInfo = psi;
            PrFolder.Start();
        }
    }
}
