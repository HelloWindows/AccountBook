using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.ScrollRect {
    public class JTweenScrollHorizontalPos : JTweenBase {
        private float m_beginHorizontalPos = 0;
        private float m_toHorizontalPos = 0;
        private UnityEngine.UI.ScrollRect m_scrollRect;

        public float ToHorizontalPos {
            get {
                return m_toHorizontalPos;
            }
            set {
                m_toHorizontalPos = value;
            }
        }

        public override void Init() {
            if (null == m_target) return;
            // end if
            m_scrollRect = m_target.GetComponent<UnityEngine.UI.ScrollRect>();
            if (null == m_scrollRect) return;
            // end if
            m_beginHorizontalPos = m_scrollRect.horizontalNormalizedPosition;
        }

        protected override Tween DOPlay() {
            if (null == m_scrollRect) return null;
            // end if
            return m_scrollRect.DOHorizontalNormalizedPos(m_toHorizontalPos, m_duration, m_isSnapping);
        }

        protected override void Restore() {
            if (null == m_scrollRect) return;
            // end if
            m_scrollRect.horizontalNormalizedPosition = m_beginHorizontalPos;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("horizontal")) m_toHorizontalPos = json["horizontal"].ToFloat();
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["horizontal"] = m_toHorizontalPos;
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
