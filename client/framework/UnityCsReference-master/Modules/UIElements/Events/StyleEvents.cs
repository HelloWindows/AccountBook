// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
    public class CustomStyleResolvedEvent : EventBase<CustomStyleResolvedEvent>
    {
        public ICustomStyle customStyle
        {
            get { return (target as VisualElement)?.customStyle; }
        }
    }
}
