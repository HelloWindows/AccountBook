using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.Transform {
    public class JTweenTransformLocalQuaternion : JTweenBase {
        private Quaternion m_beginRotation = Quaternion.identity;
        private Quaternion m_toRotate = Quaternion.identity;
        private UnityEngine.Transform m_Transform;

        public JTweenTransformLocalQuaternion() {
            m_tweenType = (int)JTweenTransform.LocalQuaternion;
            m_tweenElement = JTweenElement.Transform;
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

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("quaternion")) {
                Vector4 quaternion = JTweenUtils.JsonToVector4(json.GetNode("quaternion"));
                m_toRotate = new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
            } // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            Vector4 quaternion = new Vector4(m_toRotate.x, m_toRotate.y, m_toRotate.z, m_toRotate.w);
            json.SetNode("quaternion", JTweenUtils.Vector4Json(quaternion));
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
