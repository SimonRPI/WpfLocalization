// <copyright file="AddLanguageWindow.xaml.cs" company="Liebl">
//     Simon Liebl 2017
// </copyright>

namespace LocalizationEditor.Dialogs
{
    using System;
    using System.Collections.Generic;
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
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for AddLanguageWindow.xaml
    /// </summary>
    public partial class AddLanguageWindow : Window
    {
        /// <summary>
        /// The new language
        /// </summary>
        private string newLanguage = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddLanguageWindow"/> class
        /// </summary>
        public AddLanguageWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the new language
        /// </summary>
        public string NewLanguage
        {
            get { return this.newLanguage; }
            set { this.newLanguage = value; }
        }

        /// <summary>
        /// The on OK click
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
