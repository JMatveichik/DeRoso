using System;
using System.Windows;
using DeRoso.Core;
using DeRoso.Core.Data;
using DeRoso.Core.Device;
using DeRoso.Core.Health;
using DeRoso.Views;
using Microsoft.EntityFrameworkCore;

namespace DeRoso
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ContentManager.Instance.RegisterCreator("HOME", new CreatorViewHome());
            ContentManager.Instance.RegisterCreator("HELP", new CreatorViewHelp());
            ContentManager.Instance.RegisterCreator("ARCHIVE", new CreatorViewArchive());
            ContentManager.Instance.RegisterCreator("EDIT", new CreatorViewTestsEditor());
            ContentManager.Instance.RegisterCreator("SELECTION", new CreatorViewTestSelection());
            ContentManager.Instance.RegisterCreator("TESTING", new CreatorViewTesting());
            ContentManager.Instance.RegisterCreator("EDITSECTIONS", new CreatorViewSectionEditor());
            ContentManager.Instance.RegisterCreator("EDITPATIENTS", new CreatorViewPatientEditor());


            var optionsBuilder = new DbContextOptionsBuilder<DeRosoContext>();
            var options = optionsBuilder.UseSqlite("Filename=DeRoso.db").Options;


            bool init = false;

           

            DeRossoDataWorker.DB = new DeRosoContext(options, init); 

            Device = new DeviceProvider();
            TestProcessor = new HealthTestsProcessor(Device);

            
            if (init)
            {
                DeRossoDBTools.Instance.InitDB(DeRossoData, "db.xml", "ProfLeng.txt");
                DeRossoDBTools.Instance.TraceDB(DeRossoData, "outtree.txt");

                DeRossoData.Patients.Add(
                    new Patient()
                    {
                        FirstName = "Евгений",
                        SecondName = "Альфредович",
                        FamilyName = "Матвейчик",
                        Gender = EnumPatientGender.Male,
                        BirthDay = new DateTime(1975, 06, 28)
                    }
                );
                DeRossoData.SaveChanges();
         
            }
            else
            {
                DeRossoDataWorker.DB.Load();
            }

            DeRossoData = DeRossoDataWorker.DB;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            DeRossoData.SaveChanges();
            base.OnExit(e);
        }

      

        public DeRosoContext DeRossoData {
            get;
            private set;
        }

        public HealthTestsProcessor TestProcessor
        {
            get;
            private set;
        }

        public DeviceProvider Device
        {
            get;
            private set;
        }

        
    }
}
