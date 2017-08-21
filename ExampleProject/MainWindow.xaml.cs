using System;
using System.Collections.Generic;
using System.Globalization;
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

using Localization;

namespace ExampleProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Changes the language to English
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void OnEnglish_Click(object sender, RoutedEventArgs e)
        {
            LocDict.Instance.CurrentCulture = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Changes the language to German
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void OnGerman_Click(object sender, RoutedEventArgs e)
        {
            LocDict.Instance.CurrentCulture = new CultureInfo("de-DE");
        }

        /// <summary>
        /// Opens the localization editor
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void OnOpenEditor_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("LocalizationEditor.exe", "Translations.xml");
        }

        /// <summary>
        /// Click!
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void OnClick_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(LocDict.Instance.Get(nameof(Properties.Resources.MsgChangeLanguage)));
            // for those people who have to use C#5.0
            //MessageBox.Show(LocDict.Instance.Get(LocDict.GetNameOf(() => Properties.Resources.MsgChangeLanguage)));
        }

        /// <summary>
        /// Here also!
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void OnClickParameter_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(LocDict.Instance.Get(nameof(Properties.Resources.MsgWithParameter), nameof(Properties.Resources.UiGerman), "Not translatable parameter"));
        }
    }
}
