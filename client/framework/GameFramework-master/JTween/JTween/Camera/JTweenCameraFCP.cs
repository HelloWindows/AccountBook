using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Camera {
    public class JTweenCameraFCP : JTweenBase {
        private float m_beginFCP = 0;
        private float m_toFCP = 0;
        private UnityEngine.Camera m_Camera;

        public JTweenCameraFCP() {
            m_tweenType = (int)JTweenCamera.FCP;
            m_tweenElement = JTweenElement.Camera;
        }

        public float BeginFCP {
            get {
                return m_beginFCP;
            }
            set {
                m_beginFCP = value;
                if (m_Camera != null) {
                    m_Camera.farClipPlane = m_beginFCP;
                } // end if
            }
        }

        public float ToFCP {
            get {
                return m_toFCP;
            }
            set {
                m_toFCP = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Camera = m_target.GetComponent<UnityEngine.Camera>();
            if (null == m_Camera) return;
            // end if
            m_beginFCP = m_Camera.farClipPlane;
        }

        protected override Tween DOPlay() {
            if (null == m_Camera) return null;
            // end if
            return m_Camera.DOFarClipPlane(m_toFCP, m_duration);
        }

        public override void Restore() {
            if (null == m_Camera) return;
            // end if
            m_Camera.farClipPlane = m_beginFCP;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginFCP")) m_beginFCP = (float)json["beginFCP"];
            // end if
            if (json.Contains("FCP")) m_toFCP = (float)json["FCP"];
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["beginFCP"] = m_beginFCP;
            json["FCP"] = m_toFCP;
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
