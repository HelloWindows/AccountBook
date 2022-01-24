using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.Transform {
    public class JTweenTransformBlendableLocalMove : JTweenBase {
        private Vector3 m_byPosition = Vector3.zero;
        private UnityEngine.Transform m_Transform;

        public JTweenTransformBlendableLocalMove() {
            m_tweenType = (int)JTweenTransform.BlendableLocalMove;
            m_tweenElement = JTweenElement.Transform;
        }

        public Vector3 ByPosition {
            get {
                return m_byPosition;
            }
            set {
                m_byPosition = value;
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
            return m_Transform.DOBlendableLocalMoveBy(ByPosition, m_duration, m_isSnapping);
        }

        public override void Restore() {
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("move")) ByPosition = JTweenUtils.JsonToVector3(json.GetNode("move")); 
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("move", JTweenUtils.Vector3Json(m_byPosition));
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
