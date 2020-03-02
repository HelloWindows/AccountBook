using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.RectTransform {
    public class JTweenRectTransformAnchorMax : JTweenBase {
        private Vector2 m_beginAnchorMax = Vector2.zero;
        private Vector2 m_toAnchorMax = Vector2.zero;
        private UnityEngine.RectTransform m_RectTransform;

        public JTweenRectTransformAnchorMax() {
            m_tweenType = (int)JTweenRectTransform.AnchorMax;
            m_tweenElement = JTweenElement.RectTransform;
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

        protected override void JsonTo(JsonData json) {
            if (json.Contains("anchorMax")) m_toAnchorMax = JTweenUtils.JsonToVector2(json["anchorMax"]);
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["anchorMax"] = JTweenUtils.Vector2Json(m_toAnchorMax);
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
