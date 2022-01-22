using Json;
using DG.Tweening;

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

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginNCP")) m_beginNCP = json.GetFloat("beginNCP"); 
            // end if
            if (json.Contains("NCP")) m_toNCP = json.GetFloat("NCP");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetFloat("beginNCP", m_beginNCP);
            json.SetFloat("NCP", m_toNCP);
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
