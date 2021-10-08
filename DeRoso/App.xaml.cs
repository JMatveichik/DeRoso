using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using DeRoso.Core;

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
            Programs.Load("D:\\Source\\FREE\\db.xml");
        }

        public DevicePrograms Programs = new DevicePrograms();

        
    }
}
