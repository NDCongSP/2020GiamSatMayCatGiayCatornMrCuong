using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MayCatServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            UserControl1 userControl = new UserControl1();

            string url = $"http://*:{MayCatServer.Properties.Settings.Default["port"].ToString()}";
            WebApp.Start(url);
            base.OnStartup(e);
        }
    }
}
