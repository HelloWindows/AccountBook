// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.UIElements
{
    class DefaultWindowBackend : IWindowBackend
    {
        protected Panel m_Panel;
        protected IWindowModel m_Model;

        protected IMGUIContainer imguiContainer;

        public object visualTree => m_Panel.visualTree;

        public virtual void OnCreate(IWindowModel model)
        {
            m_Model = model;
            m_Panel = EditorPanel.FindOrCreate(model as ScriptableObject);

            m_Panel.visualTree.SetSize(m_Model.size);
            m_Panel.IMGUIEventInterests = m_Model.eventInterests;

            imguiContainer = new IMGUIContainer(m_Model.onGUIHandler) { useOwnerObjectGUIState = true };
            imguiContainer.StretchToParentSize();
            imguiContainer.viewDataKey = "Dockarea";
            imguiContainer.name = VisualElementUtils.GetUniqueName("Dockarea");
            imguiContainer.tabIndex = -1;
            imguiContainer.focusOnlyIfHasFocusableControls = false;

            m_Panel.visualTree.Insert(0, imguiContainer);

            m_Model.sizeChanged = OnSizeChanged;
            m_Model.eventInterestsChanged = OnEventsInterestsChanged;
        }

        private void OnSizeChanged()
        {
            m_Panel.visualTree.SetSize(m_Model.size);
        }

        private void OnEventsInterestsChanged()
        {
            m_Panel.IMGUIEventInterests = m_Model.eventInterests;
        }

        public virtual void OnDestroy(IWindowModel model)
        {
            if (imguiContainer != null)
            {
                if (imguiContainer.HasMouseCapture())
                    imguiContainer.ReleaseMouse();
                imguiContainer.RemoveFromHierarchy();
                imguiContainer = null;
            }

            if (m_Model != null)
            {
                m_Model.sizeChanged = OnSizeChanged;
                m_Model.eventInterestsChanged = OnEventsInterestsChanged;
                m_Model = null;
            }
            m_Panel.Dispose();
        }

        public bool GetTooltip(Vector2 windowMouseCoordinates, out string tooltip, out Rect screenRectPosition)
        {
            tooltip = string.Empty;
            screenRectPosition = Rect.zero;

            VisualElement target = m_Panel.Pick(windowMouseCoordinates);
            if (target != null)
            {
                using (var tooltipEvent = TooltipEvent.GetPooled())
                {
                    tooltipEvent.target = target;
                    tooltipEvent.tooltip = null;
                    tooltipEvent.rect = Rect.zero;
                    target.SendEvent(tooltipEvent);

                    if (!string.IsNullOrEmpty(tooltipEvent.tooltip) && !tooltipEvent.isDefaultPrevented)
                    {
                        tooltip = tooltipEvent.tooltip;
                        screenRectPosition = tooltipEvent.rect;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
