using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.RectTransform {
    public class JTweenRectTransformSizeDelta : JTweenBase {
        private Vector2 m_beginSizeDelta = Vector2.zero;
        private Vector2 m_toSizeDelta = Vector2.zero;
        private UnityEngine.RectTransform m_rectTransform;

        public JTweenRectTransformSizeDelta() {
            m_tweenType = (int)JTweenRectTransform.SizeDelta;
            m_tweenElement = JTweenElement.RectTransform;
        }

        public Vector2 BeginSizeDelta {
            get {
                return m_beginSizeDelta;
            }
            set {
                m_beginSizeDelta = value;
            }
        }

        public Vector2 ToSizeDelta {
            get {
                return m_toSizeDelta;
            }
            set {
                m_toSizeDelta = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_rectTransform = m_target.GetComponent<UnityEngine.RectTransform>();
            if (null == m_rectTransform) return;
            // end if
            m_beginSizeDelta = m_rectTransform.sizeDelta;
        }

        protected override Tween DOPlay() {
            if (null == m_rectTransform) return null;
            // end if
            return m_rectTransform.DOSizeDelta(m_toSizeDelta, m_duration, m_isSnapping);
        }

        public override void Restore() {
            if (null == m_rectTransform) return;
            // end if
            m_rectTransform.sizeDelta = m_beginSizeDelta;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginSizeDelta")) BeginSizeDelta = JTweenUtils.JsonToVector2(json.GetNode("beginSizeDelta"));
            // end if
            if (json.Contains("size")) m_toSizeDelta = JTweenUtils.JsonToVector2(json.GetNode("size"));
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginSizeDelta", JTweenUtils.Vector2Json(m_beginSizeDelta));
            json.SetNode("size", JTweenUtils.Vector2Json(m_toSizeDelta));
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_rectTransform) {
                errorInfo = GetType().FullName + " GetComponent<RectTransform> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
