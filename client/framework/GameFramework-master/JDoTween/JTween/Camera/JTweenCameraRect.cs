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

        public Rect ToRect {
            get {
                return m_toRect;
            }
            set {
                m_toRect = value;
            }
        }

        public override void Init() {
            if (null == m_Target) return;
            // end if
            m_Camera = m_Target.GetComponent<UnityEngine.Camera>();
            if (null == m_Camera) return;
            // end if
            m_beginRect = m_Camera.rect;
        }

        protected override Tween DOPlay() {
            if (null == m_Camera) return null;
            // end if
            return m_Camera.DORect(m_toRect, m_Duration);
        }

        protected override void Restore() {
            if (null == m_Camera) return;
            // end if
            m_Camera.rect = m_beginRect;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("rect")) {
                Vector4 rect = Utility.Utils.JsonToVector4(json["rect"]);
                m_toRect = new Rect(rect.x, rect.y, rect.z, rect.w);
            } // end if
        }

        protected override void ToJson(ref JsonData json) {
            Vector4 rect = new Vector4(m_toRect.x, m_toRect.y, m_toRect.width, m_toRect.height);
            json["rect"] = Utility.Utils.Vector4Json(rect);
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
