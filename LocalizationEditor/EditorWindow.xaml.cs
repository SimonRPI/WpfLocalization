// <copyright file="EditorWindow.xaml.cs" company="Liebl">
//     Simon Liebl 2017
// </copyright>

namespace LocalizationEditor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;

    using Localization;

    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window, INotifyPropertyChanged
    {
        #region Attributes
        /// <summary>
        /// The localization IO handler to read and write the localization file
        /// </summary>
        private ILocalizationIOHandler fileIOHandler;

        /// <summary>
        /// The file path
        /// </summary>
        private string filePath;

        /// <summary>
        /// The localization file
        /// </summary>
        private LocalizationFile locFile;

        /// <summary>
        /// The search filter
        /// </summary>
        private string filter = string.Empty;

        /// <summary>
        /// The localization file changed
        /// </summary>
        private bool fileChanged = false;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorWindow"/> class.
        /// </summary>
        public EditorWindow()
        {
            this.InitializeComponent();
            this.fileIOHandler = new LocalizationXmlIOHandler();
            this.lstViewDict.SelectionChanged += this.LstViewDict_SelectionChanged;
            this.DictSorting.Items.Add(new LocAlphaAscSorting());
            this.DictSorting.Items.Add(new LocAlphaDescSorting());
            this.DictSorting.Items.Add(new LocAssemblyAscSorting());
            this.DictSorting.Items.Add(new LocAssemblyDescSorting());
            this.DictSorting.SelectionChanged += this.DictSorting_SelectionChanged;
            this.DictSorting.SelectedIndex = 0;

            var args = Environment.GetCommandLineArgs();
            if (args.Length == 2 && File.Exists(args[1]))
            {
                this.OpenFile(args[1]);
            }
        }

        #region Events
        /// <summary>
        /// The property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the localization file
        /// </summary>
        public LocalizationFile LocFile
        {
            get { return this.locFile; }
            set
            {
                this.locFile = value;
                this.ReSetItemSource();
                this.DictSorting_SelectionChanged(null, null);
                this.OnPropertyChanged();
                this.OnPropertyChanged("FilteredDictionary");
            }
        }

        /// <summary>
        /// Gets the localization dictionary
        /// </summary>
        public List<LocalizationValue> Dictionary
        {
            get { return this.LocFile == null ? null : this.LocFile.Values; }
        }

        /// <summary>
        /// Gets the filtered dictionary
        /// </summary>
        public List<LocalizationValue> FilteredDictionary
        {
            get
            {
                if (this.Dictionary != null)
                {
                    return this.Dictionary.Where(x => x.Key.ToLower().StartsWith(this.Filter.ToLower())).ToList();
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the search filter
        /// </summary>
        public string Filter
        {
            get { return this.filter; }
            set
            {
                this.filter = value;
                this.OnPropertyChanged();
                this.ReSetItemSource();
            }
        }

        /// <summary>
        /// Gets or sets the file path
        /// </summary>
        public string FilePath
        {
            get { return this.filePath; }
            set
            {
                this.filePath = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged("IsFileLoaded");
            }
        }

        /// <summary>
        /// Gets a value indicating whether a file is loaded
        /// </summary>
        public bool IsFileLoaded
        {
            get { return this.FilePath != null; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// The on property changed
        /// </summary>
        /// <param name="propertyName">The property name</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// The sorting changed
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void DictSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sorting = this.DictSorting.SelectedItem as LocDictSortingBase;
            sorting.SortLocDict(this.Dictionary);
            this.ReSetItemSource();
        }

        /// <summary>
        /// The selected localization key changed 
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void LstViewDict_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LocalValuesGrid.Children.Clear();
            this.LocalCol.Width = new GridLength(this.DefaultCol.ActualWidth);
            var item = this.lstViewDict.SelectedItem as LocalizationValue;
            if (item != null)
            {
                foreach (var culture in item.LocalValues)
                {
                    this.LocalValuesGrid.RowDefinitions.Add(new RowDefinition());
                    var textblock = new TextBlock();
                    textblock.Margin = new Thickness(0, 5, 0, 0);
                    textblock.Text = culture.Culture;
                    this.LocalValuesGrid.Children.Add(textblock);
                    Grid.SetRow(textblock, this.LocalValuesGrid.RowDefinitions.Count - 1);
                    var textBox = new TextBox();
                    textBox.Margin = new Thickness(5, 5, 0, 0);
                    var binding = new Binding();
                    binding.Path = new PropertyPath("Value");
                    binding.Source = culture;
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
                    this.LocalValuesGrid.Children.Add(textBox);
                    Grid.SetRow(textBox, this.LocalValuesGrid.RowDefinitions.Count - 1);
                    Grid.SetColumn(textBox, 1);
                }
            }
        }

        /// <summary>
        /// Sets and resets the itemsource of the listview
        /// </summary>
        private void ReSetItemSource()
        {
            this.lstViewDict.ItemsSource = null;
            this.lstViewDict.ItemsSource = this.FilteredDictionary;
        }

        /// <summary>
        /// Open file clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "XML Files (*.xml)|*.xml";

            if (ofd.ShowDialog() == true)
            {
                this.OpenFile(ofd.FileName);
            }
        }

        /// <summary>
        /// Loads a localization file
        /// </summary>
        /// <param name="path">The file path</param>
        private void OpenFile(string path)
        {
            this.FilePath = path;
            this.LocFile = this.fileIOHandler.ReadFile(this.FilePath);
            this.LocFile.SubscribePropertyChanged();
            this.LocFile.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
            {
                this.fileChanged = true;
            };

            this.DeleteLanguage.Items.Clear();
            if (this.Dictionary.Count > 0)
            {
                foreach (var culture in this.Dictionary.FirstOrDefault().LocalValues)
                {
                    var item = new MenuItem();
                    item.Header = culture.Culture;
                    item.Click += this.OnRemoveLanguage_Click;
                    this.DeleteLanguage.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Saves the current dictionary
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void OnSave_Click(object sender, RoutedEventArgs e)
        {
            this.fileIOHandler.WriteFile(this.FilePath, this.LocFile);
            this.fileChanged = false;
        }

        /// <summary>
        /// Adds a language to the dictionary
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void OnAddLanguage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialogs.AddLanguageWindow();
            if (dialog.ShowDialog() == true)
            {
                var item = new MenuItem();
                item.Header = dialog.NewLanguage;
                item.Click += this.OnRemoveLanguage_Click;
                this.DeleteLanguage.Items.Add(item);
                this.AddCulture(dialog.NewLanguage);
            }
        }

        /// <summary>
        /// Adds a langugage to every key
        /// </summary>
        /// <param name="culture">The language</param>
        private void AddCulture(string culture)
        {
            foreach (var key in this.Dictionary)
            {
                if (!key.LocalValues.Any(x => x.Culture == culture))
                {
                    key.LocalValues.Add(new LocalizationValue.LocalValue(culture, string.Empty));
                }
            }

            this.fileIOHandler.WriteFile(this.FilePath, this.LocFile);
        }

        /// <summary>
        /// Removes a language from the dictionary
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void OnRemoveLanguage_Click(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as MenuItem;
            if (MessageBox.Show(LocDict.Instance.Get(nameof(Properties.Resources.MsgRemoveLanguage), item.Header.ToString()), string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                this.RemoveCulture(item.Header.ToString());
                this.DeleteLanguage.Items.Remove(item);
            }
        }

        /// <summary>
        /// Removes a language form the dictionary
        /// </summary>
        /// <param name="culture">The language</param>
        /// <returns>True if language exists</returns>
        private bool RemoveCulture(string culture)
        {
            bool ret = false;
            foreach (var key in this.Dictionary)
            {
                var val = key.LocalValues.FirstOrDefault(x => x.Culture == culture);
                if (val != null)
                {
                    key.LocalValues.Remove(val);
                    ret = true;
                }
            }

            this.fileIOHandler.WriteFile(this.FilePath, this.LocFile);

            return ret;
        }

        /// <summary>
        /// Deletes a key from the dictionary
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void OnDeleteKey_Click(object sender, RoutedEventArgs e)
        {
            if (this.lstViewDict.SelectedItem != null)
            {
                var value = (LocalizationValue)this.lstViewDict.SelectedItem;
                if (MessageBox.Show(LocDict.Instance.Get(nameof(Properties.Resources.MsgDeleteKey), value.Key), string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    this.Dictionary.Remove(value);
                    this.ReSetItemSource();
                    this.fileChanged = true;
                }
            }
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
        /// Opens read me
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void OnHowTo_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("ReadMe.txt");
        }

        /// <summary>
        /// Listen to pressed keys
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void Root_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                this.OnSave_Click(null, null);
            }
        }

        /// <summary>
        /// The on window closing
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The e</param>
        private void Root_Closing(object sender, CancelEventArgs e)
        {
            if (this.fileChanged == true)
            {
                if (MessageBox.Show(LocDict.Instance.Get(nameof(Properties.Resources.MsgSaveFile)), string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    this.OnSave_Click(null, null);
                }
            }
        }
        #endregion
    }
}
