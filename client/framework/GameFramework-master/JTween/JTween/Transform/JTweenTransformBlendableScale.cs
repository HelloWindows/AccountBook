using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.Transform {
    public class JTweenTransformBlendableScale : JTweenBase {
        private Vector3 m_byScale = Vector3.zero;
        private UnityEngine.Transform m_Transform;

        public JTweenTransformBlendableScale() {
            m_tweenType = (int)JTweenTransform.BlendableScale;
            m_tweenElement = JTweenElement.Transform;
        }

        public Vector3 ByScale {
            get {
                return m_byScale;
            }
            set {
                m_byScale = value;
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
            return m_Transform.DOBlendableScaleBy(m_byScale, m_duration);
        }

        public override void Restore() {
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("scale")) m_byScale = JTweenUtils.JsonToVector3(json.GetNode("scale"));
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("scale", JTweenUtils.Vector3Json(m_byScale));
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
