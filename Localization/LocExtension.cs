// <copyright file="LocExtension.cs" company="Liebl">
//     Simon Liebl 2017
// </copyright>

namespace Localization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Data;

    /// <summary>
    /// The Localization Extension for WPF
    /// </summary>
    public class LocExtension : Binding
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LocExtension"/> class.
        /// </summary>
        /// <param name="name">The name</param>
        public LocExtension(string name)
        : base("[" + name + "]")
            {
                this.Source = LocDict.Instance;
                this.Mode = BindingMode.OneWay;
            }
        #endregion
    }
}
