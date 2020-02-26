// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Linq;
using UnityEditor.PackageManager;

namespace UnityEditor
{
    /// <summary>
    /// PackageManager helper class.
    /// </summary>
    internal static class PackageManagerUtilityInternal
    {
        /// <summary>
        /// Returns visibles packages, it excludes modules and non-root dependencies (used in project browser)
        /// </summary>
        /// <param name="skipHiddenPackages">Whether or not to skip hidden packages (packages with property hideInEditor set to true, except embedded packages). Default is true</param>
        /// <returns>an array of package information ordered by display name.</returns>
        public static PackageManager.PackageInfo[] GetAllVisiblePackages(bool skipHiddenPackages = true)
        {
            return PackageManager.PackageInfo.GetAll().Where(info => info.type != "module" &&
                (!skipHiddenPackages || !info.hideInEditor ||
                    info.source == PackageSource.Embedded ||
                    info.source == PackageSource.Local)).
                OrderBy(info => string.IsNullOrEmpty(info.displayName) ? info.name : info.displayName,
                    StringComparer.InvariantCultureIgnoreCase).ToArray();
        }

        /// <summary>
        /// Determines whether or not a path belongs to a visible package (packages with property hideInEditor set to false, or embedded packages).
        /// <param name="path">The path to check.</param>
        /// <returns>A boolean, true if path belongs to a visible package. False otherwise.</returns>
        /// </summary>
        public static bool IsPathInVisiblePackage(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            var package = PackageManager.PackageInfo.FindForAssetPath(path);
            if (package == null)
                return true;

            return package.type != "module" && (!package.hideInEditor ||
                package.source == PackageSource.Embedded ||
                package.source == PackageSource.Local);
        }

        /// <summary>
        /// Count of hidden packages. (packages with property hideInEditor set to true, except embedded packages)
        /// </summary>
        public static int HiddenPackagesCount
        {
            get
            {
                return PackageManager.PackageInfo.GetAll().Count(info => info.type != "module" &&
                    info.source != PackageSource.Embedded &&
                    info.source != PackageSource.Local &&
                    info.hideInEditor);
            }
        }
    }
}
