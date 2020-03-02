using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.RectTransform {
    public class JTweenRectTransformSizeDelta : JTweenBase {
        private Vector2 m_beginSizeDelta = Vector2.zero;
        private Vector2 m_toSizeDelta = Vector2.zero;
        private UnityEngine.RectTransform m_rectTransform;

        public JTweenRectTransformSizeDelta() {
            m_tweenType = (int)JTweenRectTransform.SizeDelta;
            m_tweenElement = JTweenElement.RectTransform;
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

        protected override void JsonTo(JsonData json) {
            if (json.Contains("size")) m_toSizeDelta = JTweenUtils.JsonToVector2(json["size"]);
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["size"] = JTweenUtils.Vector2Json(m_toSizeDelta);
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
