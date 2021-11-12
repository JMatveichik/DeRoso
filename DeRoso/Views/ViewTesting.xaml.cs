using DeRoso.Core.Data;
using DeRoso.Core.Health;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
                    pr.CurrentPatient = pr.Patients.Select(p => p).FirstOrDefault(p => p.FullName == wnd.NewPatient.FullName); 
                }                    
                
            }
        }

        private void OnPacientFolderButtonClick(object sender, RoutedEventArgs e)
        {
            Process prFolder = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            HealthTestsProcessor pr = (HealthTestsProcessor)this.DataContext;

            string patientDir = Directory.GetCurrentDirectory() + "\\";

            if (pr.Report.Patient != null)
                patientDir += pr.Report.Patient.ShortName;
            else
                patientDir += "Незарегистрированные";


            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.FileName = "explorer";
            psi.Arguments = patientDir;
            prFolder.StartInfo = psi;
            prFolder.Start();
        }

        private void OnTestingViewIsVisible(object sender, DependencyPropertyChangedEventArgs e)
        {
            var pr = (HealthTestsProcessor)this.DataContext;
            pr.Update();
        }

        private void OnViewTestingLoaded(object sender, RoutedEventArgs e)
        {
            var pr = (HealthTestsProcessor)this.DataContext;
            pr.Report.Clear();
        }
    }
}
