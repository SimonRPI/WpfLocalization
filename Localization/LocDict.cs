// <copyright file="LocDict.cs" company="Liebl">
//     Simon Liebl 2017
// </copyright>

namespace Localization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The Localization dictionary
    /// </summary>
    public class LocDict : INotifyPropertyChanged
    {
        #region Attributes
        /// <summary>
        /// The loc dictionary singleton instance
        /// </summary>
        private static LocDict instance = new LocDict();

        /// <summary>
        /// The source file name
        /// </summary>
        private string fileName = "Translations.xml";

        /// <summary>
        /// The localization IO handler to read and write the localization file
        /// </summary>
        private ILocalizationIOHandler fileIOHandler;

        /// <summary>
        /// The localization file
        /// </summary>
        private LocalizationFile locFile;

        /// <summary>
        /// The current application culture
        /// </summary>
        private CultureInfo currentCulture = CultureInfo.CurrentCulture;
        
        /// <summary>
        /// The list of local cultures
        /// To create empty fields in the file
        /// </summary>
        private List<string> localCultures = new List<string>();
        #endregion

        #region Constructors
        /// <summary>
        /// Prevents a default instance of the <see cref="LocDict"/> class from being created.
        /// </summary>
        private LocDict()
        {
        }
        #endregion

        #region Events
        /// <summary>
        /// The property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the Localization Dictionary Singleton instance
        /// </summary>
        public static LocDict Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets the list of local cultures
        /// To create empty fields in the file
        /// </summary>
        public List<string> LocalCultures
        {
            get { return this.localCultures; }
        }

        /// <summary>
        /// Gets or sets the current application culture
        /// </summary>
        public CultureInfo CurrentCulture
        {
            get
            {
                return this.currentCulture;
            }

            set
            {
                if (this.currentCulture != value)
                {
                    this.currentCulture = value;
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the localization dictionary
        /// </summary>
        private List<LocalizationValue> Dictionary
        {
            get { return this.locFile.Values; }
        }

        /// <summary>
        /// Gets the value from a given key depending on the current culture
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The value</returns>
        public string this[string key]
        {
            get
            {
                var item = this.Dictionary.FirstOrDefault(x => x.Key == key);
                if (item == null)
                {
                    return key;
                }

                return item.GetLocalValue(this.CurrentCulture); // get local value if available
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the name of a given variable/resource/parameter
        /// Usage: LocDict.GetNameOf(() => Properties.Resources.MyUiKey)
        /// </summary>
        /// <typeparam name="T">Lamda expression</typeparam>
        /// <param name="property">The variable/resource/paramter</param>
        /// <returns>The name</returns>
        public static string GetNameOf<T>(Expression<Func<T>> property)
        {
            return (property.Body as MemberExpression).Member.Name;
        }

        /// <summary>
        /// Gets the value from a given key depending on the current culture
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="parameter1">The optional parameter1</param>
        /// <param name="parameter2">The optional parameter2</param>
        /// <param name="parameter3">The optional parameter3</param>
        /// <param name="parameter4">The optional parameter4</param>
        /// <param name="parameter5">The optional parameter5</param>
        /// <param name="parameter6">The optional parameter6</param>
        /// <param name="parameter7">The optional parameter7</param>
        /// <param name="parameter8">The optional parameter8</param>
        /// <returns>The value</returns>
        public string Get(string key, string parameter1 = null, string parameter2 = null, string parameter3 = null, string parameter4 = null, string parameter5 = null, string parameter6 = null, string parameter7 = null, string parameter8 = null)
        {
            var parameters = new List<string>() { parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7, parameter8 };
            parameters.RemoveAll(x => x == null);
            for (int i = 0; i < parameters.Count; i++)
            {
                parameters[i] = this[parameters[i]];
            }

            return string.Format(this[key], parameters.ToArray());
        }

        /// <summary>
        /// Initializes the LocalizationDictionary
        /// Reads a existing translations file
        /// Reads all resources from assemblies
        /// Updates the translations file
        /// </summary>
        /// <param name="translationFileName">Name of the localization file</param>
        /// <param name="assemblies">List of assemblies to search for resources</param>
        public void Initialize(string translationFileName, List<Assembly> assemblies)
        {
            this.fileName = translationFileName;

            // reads the translations source file
            this.fileIOHandler = new LocalizationXmlIOHandler();
            var locFilePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            locFilePath = locFilePath.Remove(locFilePath.LastIndexOf(@"\") + 1);
            locFilePath += this.fileName;
            this.locFile = this.fileIOHandler.ReadFile(locFilePath);
            this.localCultures.AddRange(this.Dictionary.SelectMany(x => x.LocalValues).Select(x => x.Culture).Distinct());

            // get all resources from the assemblies
            foreach (var assembly in assemblies)
            {
                if (!assembly.IsDynamic)
                {
                    var resourceNames = assembly.GetManifestResourceNames();
                    if (resourceNames.Contains(assembly.GetName().Name + ".Properties.Resources.resources"))
                    {
                        var rm = new ResourceManager(assembly.GetName().Name + ".Properties.Resources", assembly);
                        ResourceSet resourceSet = rm.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
                        foreach (DictionaryEntry entry in resourceSet)
                        {
                            var resourceKey = entry.Key.ToString();
                            if (this.Dictionary.All(x => x.Key != resourceKey))
                            {
                                if (entry.Value is string)
                                {
                                    var resourceValue = (string)entry.Value;
                                    this.Dictionary.Add(new LocalizationValue(resourceKey, resourceValue, assembly.FullName, this.LocalCultures)); // add new key to dict
                                }
                            }
                        }
                    }
                }
            }

            // update the source file
            this.fileIOHandler.WriteFile(this.fileName, this.locFile);

            // get project folder to update the localization source file 
            var projectPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
            if (projectPath != null)
            {
                var filePath = Directory.GetFiles(projectPath, this.fileName, SearchOption.AllDirectories).FirstOrDefault(x => Path.GetDirectoryName(x) != Environment.CurrentDirectory);
                if (filePath != null)
                {
                    File.Copy(this.fileName, filePath, true);
                }
            }
        }
        
        /// <summary>
        /// The on property changed event
        /// </summary>
        /// <param name="propertyName">The property name</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
