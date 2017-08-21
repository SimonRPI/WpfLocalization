// <copyright file="LocDictSorting.cs" company="Liebl">
//     Simon Liebl 2017
// </copyright>

namespace LocalizationEditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Localization;
    using Properties;

    /// <summary>
    /// The Dictionary Sorting class
    /// </summary>
    public abstract class LocDictSortingBase
    {
        /// <summary>
        /// Sorts the given localization dictionary
        /// </summary>
        /// <param name="dict">The dict</param>
        public abstract void SortLocDict(List<LocalizationValue> dict);

        /// <summary>
        /// Returns the name of the sorting
        /// </summary>
        /// <returns>The name</returns>
        public abstract override string ToString();
    }
    
    /// <summary>
    /// The localization alphabetically ascending sorting class
    /// </summary>
    public class LocAlphaAscSorting : LocDictSortingBase
    {
        /// <summary>
        /// Sorts the given localization dictionary
        /// </summary>
        /// <param name="dict">The dict</param>
        public override void SortLocDict(List<LocalizationValue> dict)
        {
            if (dict != null)
            {
                dict.Sort((x, y) => string.Compare(x.Key, y.Key));
            }
        }

        /// <summary>
        /// Returns the name of the sorting
        /// </summary>
        /// <returns>The name</returns>
        public override string ToString()
        {
            return LocDict.Instance.Get(nameof(Resources.UiAlphabetASC));
        }
    }

    /// <summary>
    /// The localization alphabetically descending sorting class
    /// </summary>
    public class LocAlphaDescSorting : LocDictSortingBase
    {
        /// <summary>
        /// Sorts the given localization dictionary
        /// </summary>
        /// <param name="dict">The dict</param>
        public override void SortLocDict(List<LocalizationValue> dict)
        {
            if (dict != null)
            {
                dict.Sort((x, y) => string.Compare(x.Key, y.Key));
                dict.Reverse();
            }
        }

        /// <summary>
        /// Returns the name of the sorting
        /// </summary>
        /// <returns>The name</returns>
        public override string ToString()
        {
            return LocDict.Instance.Get(nameof(Resources.UiAlphabetDESC));
        }
    }

    /// <summary>
    /// The localization assembly ascending sorting class
    /// </summary>
    public class LocAssemblyAscSorting : LocDictSortingBase
    {
        /// <summary>
        /// Sorts the given localization dictionary
        /// </summary>
        /// <param name="dict">The dict</param>
        public override void SortLocDict(List<LocalizationValue> dict)
        {
            if (dict != null)
            {
                dict.Sort((x, y) =>
                {
                    var byAssembly = string.Compare(x.AssemblyName, y.AssemblyName);
                    return byAssembly == 0 ? string.Compare(x.Key, y.Key) : byAssembly;
                });
            }
        }

        /// <summary>
        /// Returns the name of the sorting
        /// </summary>
        /// <returns>The name</returns>
        public override string ToString()
        {
            return LocDict.Instance.Get(nameof(Resources.UiAssemblyASC));
        }
    }

    /// <summary>
    /// The localization assembly descending sorting class
    /// </summary>
    public class LocAssemblyDescSorting : LocDictSortingBase
    {
        /// <summary>
        /// Sorts the given localization dictionary
        /// </summary>
        /// <param name="dict">The dict</param>
        public override void SortLocDict(List<LocalizationValue> dict)
        {
            if (dict != null)
            {
                dict.Sort((x, y) =>
                {
                    var byAssembly = string.Compare(x.AssemblyName, y.AssemblyName);
                    return byAssembly == 0 ? string.Compare(x.Key, y.Key) : byAssembly;
                });
                dict.Reverse();
            }
        }

        /// <summary>
        /// Returns the name of the sorting
        /// </summary>
        /// <returns>The name</returns>
        public override string ToString()
        {
            return LocDict.Instance.Get(nameof(Resources.UiAssemblyDESC));
        }
    }
}
