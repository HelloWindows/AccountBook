using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Camera {
    public class JTweenCameraNCP : JTweenBase {
        private float m_beginNCP = 0;
        private float m_toNCP = 0;
        private UnityEngine.Camera m_Camera;

        public JTweenCameraNCP() {
            m_tweenType = (int)JTweenCamera.NCP;
            m_tweenElement = JTweenElement.Camera;
        }

        public float BeginNCP {
            get {
                return m_beginNCP;
            }
            set {
                m_beginNCP = value;
                if (m_Camera != null) {
                    m_Camera.nearClipPlane = m_beginNCP;
                } // end if
            }
        }

        public float ToNCP {
            get {
                return m_toNCP;
            }
            set {
                m_toNCP = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Camera = m_target.GetComponent<UnityEngine.Camera>();
            if (null == m_Camera) return;
            // end if
            m_beginNCP = m_Camera.nearClipPlane;
        }

        protected override Tween DOPlay() {
            if (null == m_Camera) return null;
            // end if
            return m_Camera.DONearClipPlane(m_toNCP, m_duration);
        }

        public override void Restore() {
            if (null == m_Camera) return;
            // end if
            m_Camera.nearClipPlane = m_beginNCP;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginNCP")) m_beginNCP = (float)json["beginNCP"];
            // end if
            if (json.Contains("NCP")) m_toNCP = (float)json["NCP"];
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["beginNCP"] = m_beginNCP;
            json["NCP"] = m_toNCP;
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
