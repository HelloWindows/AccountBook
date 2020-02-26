// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using UnityEngine.Bindings;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEditor.PackageManager
{
    [NativeHeader("Modules/PackageManager/Editor/Public/PackageManager.h")]
    [StaticAccessor("PackageManager", StaticAccessorType.DoubleColon)]
    class NativeClient
    {
        public static extern NativeStatusCode List([Out] out long operationId, bool offlineMode, bool includeIndirectDependencies);

        public static extern NativeStatusCode Add([Out] out long operationId, string packageId);

        public extern static NativeStatusCode Embed([Out] out long operationId, string packageId);

        public static extern NativeStatusCode Remove([Out] out long operationId, string packageId);

        public static extern NativeStatusCode GetPackageInfo([Out] out long operationId, string packageId, bool offlineMode);

        public static extern NativeStatusCode GetAllPackageInfo([Out] out long operationId, bool offlineMode);

        public static extern NativeStatusCode ResetToEditorDefaults([Out] out long operationId);

        public static extern NativeStatusCode Pack([Out] out long operationId, string packageFolder, string targetFolder);

        public static extern void Resolve();

        public static extern NativeStatusCode Search([Out] out long operationId, SearchOptions options);

        public static extern NativeStatusCode GetOperationStatus(long operationId);

        public static extern NativeStatusCode GetRegistries([Out] out long operationId);

        [ThreadAndSerializationSafe]
        public static extern void ReleaseCompletedOperation(long operationId);

        public static extern Error GetOperationError(long operationId);

        public static extern OperationStatus GetListOperationData(long operationId);

        public static extern PackageInfo GetAddOperationData(long operationId);

        extern public static PackageInfo GetEmbedOperationData(long operationId);

        public static extern PackageInfo[] GetGetPackageInfoOperationData(long operationId);

        public static extern PackOperationResult GetPackOperationData(long operationId);

        public static extern SearchResults GetSearchOperationData(long operationId);

        public static extern RegistryInfo[] GetGetRegistriesOperationData(long operationId);
    }

    [NativeHeader("Modules/PackageManager/Editor/Public/PackageManager.h")]
    [NativeHeader("Modules/PackageManager/Editor/PackageManagerFolders.h")]
    [StaticAccessor("PackageManager", StaticAccessorType.DoubleColon)]
    class Folders
    {
        public static extern string GetPackagesPath();
        public static extern bool IsPackagedAssetPath(string path);
        public static extern string[] GetPackagesPaths();
    }

    [NativeHeader("Modules/PackageManager/Editor/Public/PackageManager.h")]
    [StaticAccessor("PackageManager", StaticAccessorType.DoubleColon)]
    public partial class PackageInfo
    {
        [NativeName("GetAllPackages")]
        internal static extern PackageInfo[] GetAll();

        [NativeName("GetPredefinedPackageTypes")]
        internal static extern string[] GetPredefinedPackageTypes();

        [NativeName("GetPredefinedHiddenByDefaultPackageTypes")]
        internal static extern string[] GetPredefinedHiddenByDefaultPackageTypes();

        [NativeName("GetPackageByAssetPath")]
        private static extern bool TryGetForAssetPath(string assetPath, [Out][NotNull] PackageInfo packageInfo);
    }
}
