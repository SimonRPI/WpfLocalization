// <copyright file="LocalizationFile.cs" company="Liebl">
//     Simon Liebl 2017
// </copyright>

namespace Localization
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The localization file class
    /// </summary>
    public class LocalizationFile : LocBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationFile"/> class.
        /// </summary>
        public LocalizationFile()
        {
            this.Values = new List<LocalizationValue>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationFile"/> class.
        /// </summary>
        /// <param name="values">The values</param>
        public LocalizationFile(List<LocalizationValue> values)
        {
            this.Values = values;
        }

        /// <summary>
        /// Gets or sets the list of values
        /// </summary>
        public List<LocalizationValue> Values { get; set; }

        /// <summary>
        /// Subscribes the property changed event to each local value
        /// </summary>
        public void SubscribePropertyChanged()
        {
            foreach (var val in this.Values)
            {
                val.PropertyChanged += delegate(object s, PropertyChangedEventArgs e)
                {
                    this.OnPropertyChanged(e.PropertyName);
                };
            }
        }
    }
}
