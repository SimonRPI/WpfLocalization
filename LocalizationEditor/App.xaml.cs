// <copyright file="App.xaml.cs" company="Liebl">
//     Simon Liebl 2017
// </copyright>

namespace LocalizationEditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;

    using Localization;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The application startup
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var assemblies = new List<Assembly>();
            assemblies.Add(typeof(App).Assembly);
            LocDict.Instance.Initialize("LocalizationEditorTranslations.xml", assemblies);

            var main = new EditorWindow();
            main.Show();
        }
    }
}
