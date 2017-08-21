// <copyright file="LocalizationValue.cs" company="Liebl">
//     Simon Liebl 2017
// </copyright>

namespace Localization
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    /// <summary>
    /// The Localization value
    /// </summary>
    public class LocalizationValue : LocBase
    {
        #region Attributes
        /// <summary>
        /// The culture key
        /// </summary>
        private string key = string.Empty;

        /// <summary>
        /// The default value of a key
        /// </summary>
        private string defaultValue = string.Empty;

        /// <summary>
        /// The list of local values 
        /// </summary>
        private List<LocalValue> localValues = new List<LocalValue>();

        /// <summary>
        /// The name of the belonging assembly
        /// </summary>
        private string assemblyName;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationValue"/> class.
        /// </summary>
        public LocalizationValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationValue"/> class.
        /// </summary>
        /// <param name="key">The culture</param>
        /// <param name="defaultValue">The default value</param>
        /// <param name="assemblyName">The assemlby name</param>
        /// <param name="defaultLocalValues">The default local values</param>
        public LocalizationValue(string key, string defaultValue, string assemblyName, List<string> defaultLocalValues)
        {
            this.Key = key;
            this.DefaultValue = defaultValue;
            this.AssemblyName = assemblyName;
            foreach (var loc in defaultLocalValues)
            {
                this.LocalValues.Add(new LocalValue(loc, string.Empty));
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the culture
        /// </summary>
        public string Key
        {
            get { return this.key; }
            set
            {
                this.key = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the default value
        /// </summary>
        public string DefaultValue
        {
            get { return this.defaultValue; }
            set
            {
                this.defaultValue = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the dictionary of local values
        /// </summary>
        public List<LocalValue> LocalValues
        {
            get { return this.localValues; }
            set
            {
                this.localValues = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the name of the belonging assembly
        /// </summary>
        public string AssemblyName
        {
            get { return this.assemblyName; }
            set
            {
                this.assemblyName = value;
                this.OnPropertyChanged();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the local value to a given culture
        /// </summary>
        /// <param name="culture">The culture</param>
        /// <returns>The local value or default value</returns>
        public string GetLocalValue(CultureInfo culture)
        {
            var item = this.LocalValues.FirstOrDefault(x => x.Culture == culture.Name);
            if (culture == CultureInfo.InvariantCulture || item == null || string.IsNullOrEmpty(item.Value))
            {
                if (string.IsNullOrEmpty(this.DefaultValue))
                {
                    return this.Key;
                }

                return this.DefaultValue;
            }

            return item.Value;
        }

        /// <summary>
        /// Subscribes the property changed event to each local value
        /// </summary>
        public void SubscribePropertyChanged()
        {
            foreach (var loc in this.LocalValues)
            {
                loc.PropertyChanged += delegate(object s, PropertyChangedEventArgs e)
                {
                    this.OnPropertyChanged(e.PropertyName);
                };
            }
        }
        #endregion

        #region Classes
        /// <summary>
        /// The LocalValue class to reproduce a dictionary
        /// Dictinary is in XML not supported
        /// </summary>
        public class LocalValue : LocBase
        {
            /// <summary>
            /// The culture
            /// </summary>
            private string culture;

            /// <summary>
            /// The value
            /// </summary>
            private string value;

            /// <summary>
            /// Initializes a new instance of the <see cref="LocalValue"/> class.
            /// </summary>
            public LocalValue()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="LocalValue"/> class.
            /// </summary>
            /// <param name="culture">The culture</param>
            /// <param name="value">The value</param>
            public LocalValue(string culture, string value)
            {
                this.Culture = culture;
                this.Value = value;
            }

            /// <summary>
            /// Gets or sets the culture string
            /// </summary>
            [XmlAttribute]
            public string Culture
            {
                get { return this.culture; }
                set
                {
                    this.culture = value;
                    this.OnPropertyChanged();
                }
            }

            /// <summary>
            /// Gets or sets the value
            /// </summary>
            [XmlText]
            public string Value
            {
                get { return this.value; }
                set
                {
                    this.value = value;
                    this.OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}
