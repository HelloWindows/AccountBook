using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Camera {
    public class JTweenCameraAspect : JTweenBase {
        private float m_beginAspect = 0;
        private float m_toAspect = 0;
        private UnityEngine.Camera m_Camera;

        public JTweenCameraAspect() {
            m_tweenType = (int)JTweenCamera.Aspect;
            m_tweenElement = JTweenElement.Camera;
        }

        public float BeginAspect {
            get {
                return m_beginAspect;
            }
            set {
                m_beginAspect = value;
                if (m_Camera != null) {
                    m_Camera.aspect = m_beginAspect;
                } // end if
            }
        }

        public float ToAspect {
            get {
                return m_toAspect;
            }
            set {
                m_toAspect = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Camera = m_target.GetComponent<UnityEngine.Camera>();
            if (null == m_Camera) return;
            // end if
            m_beginAspect = m_Camera.aspect;
        }

        protected override Tween DOPlay() {
            if (null == m_Camera) return null;
            // end if
            return m_Camera.DOAspect(m_toAspect, m_duration);
        }

        public override void Restore() {
            if (null == m_Camera) return;
            // end if
            m_Camera.aspect = m_beginAspect;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginAspect")) BeginAspect = (float)json["beginAspect"];
            // end if
            if (json.Contains("aspect")) m_toAspect = (float)json["aspect"];
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["beginAspect"] = m_beginAspect;
            json["aspect"] = m_toAspect;
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