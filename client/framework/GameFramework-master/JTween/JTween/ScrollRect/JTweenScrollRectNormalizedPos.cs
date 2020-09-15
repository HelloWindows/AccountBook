using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

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
                if (m_scrollRect != null) {
                    m_scrollRect.normalizedPosition = m_beginNormalizedPos;
                } // end if
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

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginNormalizedPos")) BeginNormalizedPos = JTweenUtils.JsonToVector2(json["beginNormalizedPos"]);
            // end if
            if (json.Contains("pos")) m_toNormalizedPos = JTweenUtils.JsonToVector2(json["pos"]);
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["beginNormalizedPos"] = JTweenUtils.Vector2Json(m_beginNormalizedPos);
            json["pos"] = JTweenUtils.Vector2Json(m_toNormalizedPos);
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
