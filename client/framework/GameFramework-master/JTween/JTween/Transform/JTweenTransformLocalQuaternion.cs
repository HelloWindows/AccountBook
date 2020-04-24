using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Transform {
    public class JTweenTransformLocalQuaternion : JTweenBase {
        private Quaternion m_beginRotation = Quaternion.identity;
        private Quaternion m_toRotate = Quaternion.identity;
        private UnityEngine.Transform m_Transform;

        public JTweenTransformLocalQuaternion() {
            m_tweenType = (int)JTweenTransform.LocalQuaternion;
            m_tweenElement = JTweenElement.Transform;
        }

        public Quaternion BeginRotation {
            get {
                return m_beginRotation;
            }
            set {
                m_beginRotation = value;
                if (m_Transform != null) {
                    m_Transform.localRotation = m_beginRotation;
                } // end if
            }
        }

        public Quaternion ToRotate {
            get {
                return m_toRotate;
            }
            set {
                m_toRotate = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Transform = m_target.GetComponent<UnityEngine.Transform>();
            if (null == m_Transform) return;
            // end if
            m_beginRotation = m_Transform.localRotation;
        }

        protected override Tween DOPlay() {
            if (null == m_Transform) return null;
            // end if
            return m_Transform.DOLocalRotateQuaternion(m_toRotate, m_duration);
        }

        public override void Restore() {
            if (null == m_Transform) return;
            // end if
            m_Transform.localRotation = m_beginRotation;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginRotation")) {
                Vector4 quaternion = JTweenUtils.JsonToVector4(json["beginRotation"]);
                BeginRotation = new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
            } // end if
            if (json.Contains("quaternion")) {
                Vector4 quaternion = JTweenUtils.JsonToVector4(json["quaternion"]);
                m_toRotate = new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
            } // end if
        }

        protected override void ToJson(ref JsonData json) {
            Vector4 quaternion = new Vector4(m_beginRotation.x, m_beginRotation.y, m_beginRotation.z, m_beginRotation.w);
            json["beginRotation"] = JTweenUtils.Vector4Json(quaternion);
            quaternion = new Vector4(m_toRotate.x, m_toRotate.y, m_toRotate.z, m_toRotate.w);
            json["quaternion"] = JTweenUtils.Vector4Json(quaternion);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Transform) {
                errorInfo = GetType().FullName + " GetComponent<Transform> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
