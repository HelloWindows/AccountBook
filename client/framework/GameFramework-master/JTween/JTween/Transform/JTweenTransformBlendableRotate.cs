using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.Transform {
    public class JTweenTransformBlendableRotate : JTweenBase {
        private Vector3 m_byRotate = Vector3.zero;
        private UnityEngine.Transform m_Transform;

        public JTweenTransformBlendableRotate() {
            m_tweenType = (int)JTweenTransform.BlendableRotate;
            m_tweenElement = JTweenElement.Transform;
        }
        public Vector3 ByRotate {
            get {
                return m_byRotate;
            }
            set {
                m_byRotate = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Transform = m_target.GetComponent<UnityEngine.Transform>();
        }

        protected override Tween DOPlay() {
            if (null == m_Transform) return null;
            // end if
            return m_Transform.DOBlendableRotateBy(m_byRotate, m_duration);
        }

        public override void Restore() {

        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("rotate")) m_byRotate = JTweenUtils.JsonToVector3(json.GetNode("rotate"));
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("rotate", JTweenUtils.Vector3Json(m_byRotate));
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
