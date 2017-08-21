using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

using Localization;

namespace ExampleProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var assemblies = new List<Assembly>();
            assemblies.Add(typeof(App).Assembly);
            LocDict.Instance.Initialize("Translations.xml", assemblies);

            var main = new MainWindow();
            main.Show();
        }
    }
}
