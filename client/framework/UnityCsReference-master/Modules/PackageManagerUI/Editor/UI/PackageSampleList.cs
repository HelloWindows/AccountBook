// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System.Linq;
using UnityEngine.UIElements;

namespace UnityEditor.PackageManager.UI
{
    internal class PackageSampleList : VisualElement
    {
        internal new class UxmlFactory : UxmlFactory<PackageSampleList> {}

        public PackageSampleList()
        {
            var root = Resources.GetTemplate("PackageSampleList.uxml");
            Add(root);
            cache = new VisualElementCache(root);
        }

        public void SetPackageVersion(IPackageVersion version)
        {
            importStatusContainer.Clear();
            nameLabelContainer.Clear();
            sizeLabelContainer.Clear();
            importButtonContainer.Clear();

            if (version == null || version.samples == null || !version.samples.Any())
            {
                UIUtils.SetElementDisplay(this, false);
                return;
            }
            UIUtils.SetElementDisplay(this, true);
            foreach (var sample in version.samples)
            {
                var sampleItem = new PackageSampleItem(version, sample);
                importStatusContainer.Add(sampleItem.importStatus);
                nameLabelContainer.Add(sampleItem.nameLabel);
                sizeLabelContainer.Add(sampleItem.sizeLabel);
                importButtonContainer.Add(sampleItem.importButton);
                sampleItem.importButton.SetEnabled(version.isInstalled);
            }
        }

        private VisualElementCache cache { get; set; }

        internal VisualElement importStatusContainer { get { return cache.Get<VisualElement>("importStatusContainer"); } }
        internal VisualElement nameLabelContainer { get { return cache.Get<VisualElement>("nameLabelContainer"); } }
        internal VisualElement sizeLabelContainer { get { return cache.Get<VisualElement>("sizeLabelContainer"); } }
        internal VisualElement importButtonContainer { get { return cache.Get<VisualElement>("importButtonContainer"); } }
    }
}
