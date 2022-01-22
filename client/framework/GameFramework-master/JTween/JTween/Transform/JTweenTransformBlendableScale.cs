using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.Transform {
    public class JTweenTransformBlendableScale : JTweenBase {
        private Vector3 m_beginScale = Vector3.zero;
        private Vector3 m_toScale = Vector3.zero;
        private UnityEngine.Transform m_Transform;

        public JTweenTransformBlendableScale() {
            m_tweenType = (int)JTweenTransform.BlendableScale;
            m_tweenElement = JTweenElement.Transform;
        }

        public Vector3 BeginScale {
            get {
                return m_beginScale;
            }
            set {
                m_beginScale = value;
            }
        }

        public Vector3 ToScale {
            get {
                return m_toScale;
            }
            set {
                m_toScale = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Transform = m_target.GetComponent<UnityEngine.Transform>();
            if (null == m_Transform) return;
            // end if
            m_beginScale = m_Transform.localScale;
        }

        protected override Tween DOPlay() {
            if (null == m_Transform) return null;
            // end if
            return m_Transform.DOBlendableScaleBy(m_toScale, m_duration);
        }

        public override void Restore() {
            if (null == m_Transform) return;
            // end if
            m_Transform.localScale = m_beginScale;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginScale")) BeginScale = JTweenUtils.JsonToVector3(json.GetNode("beginScale"));
            // end if
            if (json.Contains("scale")) m_toScale = JTweenUtils.JsonToVector3(json.GetNode("scale"));
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginScale", JTweenUtils.Vector3Json(m_beginScale));
            json.SetNode("scale", JTweenUtils.Vector3Json(m_toScale));
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
