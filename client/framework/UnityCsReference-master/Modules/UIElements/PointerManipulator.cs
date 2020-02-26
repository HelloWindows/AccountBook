// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

namespace UnityEngine.UIElements
{
    public abstract class PointerManipulator : MouseManipulator
    {
        private int m_CurrentPointerId;

        protected bool CanStartManipulation(IPointerEvent e)
        {
            foreach (var activator in activators)
            {
                if (activator.Matches(e))
                {
                    m_CurrentPointerId = e.pointerId;
                    return true;
                }
            }

            return false;
        }

        protected bool CanStopManipulation(IPointerEvent e)
        {
            if (e == null)
                return false;

            return e.pointerId == m_CurrentPointerId;
        }
    }
}
