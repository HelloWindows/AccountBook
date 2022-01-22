using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.RectTransform {
    public class JTweenRectTransformAnchorMax : JTweenBase {
        private Vector2 m_beginAnchorMax = Vector2.zero;
        private Vector2 m_toAnchorMax = Vector2.zero;
        private UnityEngine.RectTransform m_RectTransform;

        public JTweenRectTransformAnchorMax() {
            m_tweenType = (int)JTweenRectTransform.AnchorMax;
            m_tweenElement = JTweenElement.RectTransform;
        }

        public Vector2 BeginAnchorMax {
            get {
                return m_beginAnchorMax;
            }
            set {
                m_beginAnchorMax = value;
            }
        }

        public Vector2 ToAnchorMax {
            get {
                return m_toAnchorMax;
            }
            set {
                m_toAnchorMax = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_RectTransform = m_target.GetComponent<UnityEngine.RectTransform>();
            if (null == m_RectTransform) return;
            // end if
            m_beginAnchorMax = m_RectTransform.anchorMax;
        }

        protected override Tween DOPlay() {
            if (null == m_RectTransform) return null;
            // end if
            return m_RectTransform.DOAnchorMax(m_toAnchorMax, m_duration, m_isSnapping);
        }

        public override void Restore() {
            if (null == m_RectTransform) return;
            // end if
            m_RectTransform.anchorMax = m_beginAnchorMax;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginAnchorMax")) m_beginAnchorMax = JTweenUtils.JsonToVector2(json.GetNode("beginAnchorMax"));
            // end if
            if (json.Contains("anchorMax")) m_toAnchorMax = JTweenUtils.JsonToVector2(json.GetNode("anchorMax"));
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginAnchorMax", JTweenUtils.Vector2Json(m_beginAnchorMax));
            json.SetNode("anchorMax", JTweenUtils.Vector2Json(m_toAnchorMax));
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_RectTransform) {
                errorInfo = GetType().FullName + " GetComponent<RectTransform> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
