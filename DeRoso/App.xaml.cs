using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
            ContentManager.Instance.RegisterCreator("TESTING", new CreatorViewTesting());


            var optionsBuilder = new DbContextOptionsBuilder<DeRosoContext>();
            var options = optionsBuilder.UseSqlite("Filename=DeRoso.db").Options;

            DeRossoData = new DeRosoContext(options);
            DeRossoData.Load();

            Device = new DeviceProvider();

            //DeRossoDBTools.Instance.InitDB(DeRossoData, "db.xml", "ProfLeng.txt");
            //DeRossoDBTools.Instance.TraceDB(DeRossoData, "outtree.txt");

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

        public DeviceProvider Device
        {
            get;
            private set;
        }

        
    }
}
