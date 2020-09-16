using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Camera {
    public class JTweenCameraRect : JTweenBase {
        private Rect m_beginRect = Rect.zero;
        private Rect m_toRect = Rect.zero;
        private UnityEngine.Camera m_Camera;

        public JTweenCameraRect() {
            m_tweenType = (int)JTweenCamera.Rect;
            m_tweenElement = JTweenElement.Camera;
        }

        public Rect BeginRect {
            get {
                return m_beginRect;
            }
            set {
                m_beginRect = value;
            }
        }

        public Rect ToRect {
            get {
                return m_toRect;
            }
            set {
                m_toRect = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Camera = m_target.GetComponent<UnityEngine.Camera>();
            if (null == m_Camera) return;
            // end if
            m_beginRect = m_Camera.rect;
        }

        protected override Tween DOPlay() {
            if (null == m_Camera) return null;
            // end if
            return m_Camera.DORect(m_toRect, m_duration);
        }

        public override void Restore() {
            if (null == m_Camera) return;
            // end if
            m_Camera.rect = m_beginRect;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginRect")) {
                Vector4 rect = JTweenUtils.JsonToVector4(json["beginRect"]);
                m_beginRect = new Rect(rect.x, rect.y, rect.z, rect.w);
            } // end if
            if (json.Contains("rect")) {
                Vector4 rect = JTweenUtils.JsonToVector4(json["rect"]);
                m_toRect = new Rect(rect.x, rect.y, rect.z, rect.w);
            } // end if
            Restore();
        }

        protected override void ToJson(ref JsonData json) {
            Vector4 rect = new Vector4(m_beginRect.x, m_beginRect.y, m_beginRect.width, m_beginRect.height);
            json["beginRect"] = JTweenUtils.Vector4Json(rect);
            rect = new Vector4(m_toRect.x, m_toRect.y, m_toRect.width, m_toRect.height);
            json["rect"] = JTweenUtils.Vector4Json(rect);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Camera) {
                errorInfo = GetType().FullName + " GetComponent<Camera> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
