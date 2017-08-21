// <copyright file="LocBase.cs" company="Liebl">
//     Simon Liebl 2017
// </copyright>

namespace Localization
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The localization base
    /// </summary>
    public class LocBase : INotifyPropertyChanged
    {
        /// <summary>
        /// The property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
    }
}
