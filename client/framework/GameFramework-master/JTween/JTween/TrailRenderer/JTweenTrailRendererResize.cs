using DG.Tweening;
using Json;

namespace JTween.TrailRenderer {
    public class JTweenTrailRendererResize : JTweenBase {
        private float m_beginStartWidth = 0;
        private float m_beginEndWidth = 0;
        private float m_startWidth = 0;
        private float m_endWidth = 0;
        private UnityEngine.TrailRenderer m_TrailRenderer;

        public JTweenTrailRendererResize() {
            m_tweenType = (int)JTweenTrailRenderer.Resize;
            m_tweenElement = JTweenElement.TrailRenderer;
        }

        public float BeginStartWidth {
            get {
                return m_beginStartWidth;
            }
            set {
                m_beginStartWidth = value;
            }
        }

        public float BeginEndWidth {
            get {
                return m_beginEndWidth;
            }
            set {
                m_beginEndWidth = value;
            }
        }

        public float StartWidth {
            get {
                return m_startWidth;
            }
            set {
                m_startWidth = value;
            }
        }

        public float EndWidth {
            get {
                return m_endWidth;
            }
            set {
                m_endWidth = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_TrailRenderer = m_target.GetComponent<UnityEngine.TrailRenderer>();
            if (null == m_TrailRenderer) return;
            // end if
            m_beginStartWidth = m_TrailRenderer.startWidth;
            m_beginEndWidth = m_TrailRenderer.endWidth;
        }

        protected override Tween DOPlay() {
            if (null == m_TrailRenderer) return null;
            // end if
            return m_TrailRenderer.DOResize(m_startWidth, m_endWidth, m_duration);
        }

        public override void Restore() {
            if (null == m_TrailRenderer) return;
            // end if
            m_TrailRenderer.startWidth = m_beginStartWidth;
            m_TrailRenderer.endWidth = m_beginEndWidth;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginStartWidth")) BeginStartWidth = json.GetFloat("beginStartWidth");
            // end if
            if (json.Contains("beginEndWidth")) BeginEndWidth = json.GetFloat("beginEndWidth"); 
            // end if
            if (json.Contains("startWidth")) m_startWidth = json.GetFloat("startWidth");
            // end if
            if (json.Contains("endWidth")) m_endWidth = json.GetFloat("endWidth");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetFloat("beginStartWidth", m_beginStartWidth);
            json.SetFloat("beginEndWidth", m_beginEndWidth);
            json.SetFloat("startWidth", m_startWidth);
            json.SetFloat("endWidth", m_endWidth);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_TrailRenderer) {
                errorInfo = GetType().FullName + " GetComponent<TrailRenderer> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
