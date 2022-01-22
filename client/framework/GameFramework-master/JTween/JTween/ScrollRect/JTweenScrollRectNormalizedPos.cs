using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.ScrollRect {
    public class JTweenScrollRectNormalizedPos : JTweenBase {
        private Vector2 m_beginNormalizedPos = Vector2.zero;
        private Vector2 m_toNormalizedPos = Vector2.zero;
        private UnityEngine.UI.ScrollRect m_scrollRect;

        public JTweenScrollRectNormalizedPos() {
            m_tweenType = (int)JTweenScrollRect.NormalizedPos;
            m_tweenElement = JTweenElement.ScrollRect;
        }

        public Vector2 BeginNormalizedPos {
            get {
                return m_beginNormalizedPos;
            }
            set {
                m_beginNormalizedPos = value;
            }
        }

        public Vector2 ToNormalizedPos {
            get {
                return m_toNormalizedPos;
            }
            set {
                m_toNormalizedPos = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_scrollRect = m_target.GetComponent<UnityEngine.UI.ScrollRect>();
            if (null == m_scrollRect) return;
            // end if
            m_beginNormalizedPos = m_scrollRect.normalizedPosition;
        }

        protected override Tween DOPlay() {
            if (null == m_scrollRect) return null;
            // end if
            return m_scrollRect.DONormalizedPos(m_toNormalizedPos, m_duration, m_isSnapping);
        }

        public override void Restore() {
            if (null == m_scrollRect) return;
            // end if
            m_scrollRect.normalizedPosition = m_beginNormalizedPos;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginNormalizedPos")) BeginNormalizedPos = JTweenUtils.JsonToVector2(json.GetNode("beginNormalizedPos"));
            // end if
            if (json.Contains("pos")) m_toNormalizedPos = JTweenUtils.JsonToVector2(json.GetNode("pos"));
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginNormalizedPos", JTweenUtils.Vector2Json(m_beginNormalizedPos));
            json.SetNode("pos", JTweenUtils.Vector2Json(m_toNormalizedPos));
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_scrollRect) {
                errorInfo = GetType().FullName + " GetComponent<ScrollRect> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
