// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Collections.Generic;
using UnityEditor.Connect;
using UnityEngine;
using UnityAssetStoreUtils = UnityEditor.AssetStoreUtils;
using UnityAssetStorePackageInfo = UnityEditor.PackageInfo;

namespace UnityEditor.PackageManager.UI
{
    internal sealed class AssetStoreUtils
    {
        static IAssetStoreUtils s_Instance = null;
        public static IAssetStoreUtils instance => s_Instance ?? AssetStoreUtilsInternal.instance;

        public static Dictionary<string, object> ParseResponseAsDictionary(IAsyncHTTPClient request, Action<string> errorMessageCallback)
        {
            string errorMessage;
            if (request.IsSuccess() && request.responseCode == 200)
            {
                try
                {
                    var response = Json.Deserialize(request.text) as Dictionary<string, object>;
                    if (response != null)
                        return response;

                    errorMessage = ApplicationUtil.instance.GetTranslationForText("Failed to parse JSON.");
                }
                catch (Exception e)
                {
                    errorMessage = string.Format(ApplicationUtil.instance.GetTranslationForText("Failed to parse JSON: {0}"), e.Message);
                }
            }
            else
            {
                if (request.responseCode == 0)
                    errorMessage = L10n.Tr("Failed to parse response.");
                else
                {
                    var text = request.text.Length <= 128 ? request.text : request.text.Substring(0, 128) + "...";
                    errorMessage = string.Format(L10n.Tr("Failed to parse response: Code {0} \"{1}\""), request.responseCode, text);
                }
            }
            errorMessageCallback?.Invoke(errorMessage);
            return null;
        }

        private class AssetStoreUtilsInternal : IAssetStoreUtils
        {
            private static AssetStoreUtilsInternal s_Instance;
            public static AssetStoreUtilsInternal instance => s_Instance ?? (s_Instance = new AssetStoreUtilsInternal());

            private AssetStoreUtilsInternal()
            {
            }

            public void Download(string id, string url, string[] destination, string key, string jsonData, bool resumeOK)
            {
                UnityAssetStoreUtils.Download(id, url, destination, key, jsonData, resumeOK);
            }

            public string CheckDownload(string id, string url, string[] destination, string key)
            {
                return UnityAssetStoreUtils.CheckDownload(id, url, destination, key);
            }

            public bool AbortDownload(string id, string[] destination)
            {
                return UnityAssetStoreUtils.AbortDownload(id, destination);
            }

            public void RegisterDownloadDelegate(ScriptableObject d)
            {
                UnityAssetStoreUtils.RegisterDownloadDelegate(d);
            }

            public void UnRegisterDownloadDelegate(ScriptableObject d)
            {
                UnityAssetStoreUtils.UnRegisterDownloadDelegate(d);
            }

            public UnityAssetStorePackageInfo[] GetLocalPackageList()
            {
                return UnityAssetStorePackageInfo.GetPackageList();
            }

            public string assetStoreUrl => UnityConnect.instance.GetConfigurationURL(CloudConfigUrl.CloudAssetStoreUrl);
        }
    }
}
