// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using UnityEditor.PackageManager.Requests;

namespace UnityEditor.PackageManager.UI
{
    internal class UpmEmbedOperation : UpmBaseOperation<EmbedRequest>
    {
        public override RefreshOptions refreshOptions => RefreshOptions.None;

        public void Embed(string packageName, string packageUniqueId = null)
        {
            m_PackageName = packageName;
            m_PackageUniqueId = packageUniqueId ?? packageName;
            Start();
        }

        protected override EmbedRequest CreateRequest()
        {
            return Client.Embed(packageName);
        }
    }
}
