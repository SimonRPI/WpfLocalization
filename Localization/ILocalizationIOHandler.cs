// <copyright file="ILocalizationIOHandler.cs" company="Liebl">
//     Simon Liebl 2017
// </copyright>

namespace Localization
{
    using System.Collections.Generic;

    /// <summary>
    /// The localization IO handler
    /// </summary>
    public interface ILocalizationIOHandler
    {
        #region Methods
        /// <summary>
        /// Reads a translations file
        /// </summary>
        /// <param name="path">The file path</param>
        /// <returns>Localization file</returns>
        LocalizationFile ReadFile(string path);

        /// <summary>
        /// Writes a translation file
        /// </summary>
        /// <param name="path">The file path</param>
        /// <param name="locFile">The localization file</param>
        void WriteFile(string path, LocalizationFile locFile);
        #endregion
    }
}