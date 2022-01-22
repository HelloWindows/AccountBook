using Json;
using DG.Tweening;

namespace JTween.Camera {
    public class JTweenCameraOrthoSize : JTweenBase {
        private float m_beginOrthoSize = 0;
        private float m_toOrthoSize = 0;
        private UnityEngine.Camera m_Camera;

        public JTweenCameraOrthoSize() {
            m_tweenType = (int)JTweenCamera.OrthoSize;
            m_tweenElement = JTweenElement.Camera;
        }

        public float BeginOrthoSize {
            get {
                return m_beginOrthoSize;
            }
            set {
                m_beginOrthoSize = value;
            }
        }

        public float ToOrthoSize {
            get {
                return m_toOrthoSize;
            }
            set {
                m_toOrthoSize = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Camera = m_target.GetComponent<UnityEngine.Camera>();
            if (null == m_Camera) return;
            // end if
            m_beginOrthoSize = m_Camera.orthographicSize;
        }

        protected override Tween DOPlay() {
            if (null == m_Camera) return null;
            // end if
            return m_Camera.DOOrthoSize(m_toOrthoSize, m_duration);
        }

        public override void Restore() {
            if (null == m_Camera) return;
            // end if
            m_Camera.farClipPlane = m_beginOrthoSize;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginOrthoSize")) m_beginOrthoSize = json.GetFloat("beginOrthoSize");
            // end if
            if (json.Contains("orthoSize")) m_toOrthoSize = json.GetFloat("orthoSize");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetFloat("beginOrthoSize", m_beginOrthoSize);
            json.SetFloat("orthoSize", m_toOrthoSize);
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
