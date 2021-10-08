using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using DeRoso.Core;
using DeRoso.Views;

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
            ContentManager.Instance.RegisterCreator("PROGRAMMS", new CreatorViewProgramms());
            ContentManager.Instance.RegisterCreator("HELP", new CreatorViewHelp());
            ContentManager.Instance.RegisterCreator("ARCHIVE", new CreatorViewArchive());
            ContentManager.Instance.RegisterCreator("TESTING", new CreatorViewTesting());


            Programs.Load("D:\\Source\\FREE\\db.xml");
        }

        public DevicePrograms Programs = new DevicePrograms();

        
    }
}
