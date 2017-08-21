// <copyright file="LocalizationXmlIOHandler.cs" company="Liebl">
//     Simon Liebl 2017
// </copyright>

namespace Localization
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// The localization Xml IO handler
    /// </summary>
    public class LocalizationXmlIOHandler : ILocalizationIOHandler
    {
        #region Methods
        /// <summary>
        /// Reads a translations file
        /// </summary>
        /// <param name="path">The file path</param>
        /// <returns>Localization file</returns>
        public LocalizationFile ReadFile(string path)
        {
            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LocalizationFile));
                FileStream fs = new FileStream(path, FileMode.Open);
                var file = (LocalizationFile)serializer.Deserialize(fs);
                fs.Close();

                file.Values.ForEach(x => x.SubscribePropertyChanged());

                return file;
            }

            return new LocalizationFile(); 
        }

        /// <summary>
        /// Writes a translation file
        /// </summary>
        /// <param name="path">The file path</param>
        /// <param name="locFile">The localization file</param>
        public void WriteFile(string path, LocalizationFile locFile)
        {
            if (!File.Exists(path))
            {
                var file = File.Create(path);
                file.Close();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(LocalizationFile));
            TextWriter writer = new StreamWriter(path);
            serializer.Serialize(writer, locFile);
            writer.Close();
        }
        #endregion
    }
}
