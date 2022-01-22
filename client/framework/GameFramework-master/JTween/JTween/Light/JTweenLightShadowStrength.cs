using Json;
using DG.Tweening;

namespace JTween.Light {
    public class JTweenLightShadowStrength : JTweenBase {
        private float m_beginStrength = 0;
        private float m_toStrength = 0;
        private UnityEngine.Light m_Light;

        public JTweenLightShadowStrength() {
            m_tweenType = (int)JTweenLight.ShadowStrength;
            m_tweenElement = JTweenElement.Light;
        }

        public float BeginStrength {
            get {
                return m_beginStrength;
            }
            set {
                m_beginStrength = value;
            }
        }

        public float ToStrength {
            get {
                return m_toStrength;
            }
            set {
                m_toStrength = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Light = m_target.GetComponent<UnityEngine.Light>();
            if (null == m_Light) return;
            // end if
            m_beginStrength = m_Light.shadowStrength;
        }

        protected override Tween DOPlay() {
            if (null == m_Light) return null;
            // end if
            return m_Light.DOShadowStrength(m_toStrength, m_duration);
        }

        public override void Restore() {
            if (null == m_Light) return;
            // end if
            m_Light.shadowStrength = m_beginStrength;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginStrength")) BeginStrength = json.GetFloat("beginStrength"); 
            // end if
            if (json.Contains("strength")) m_toStrength = json.GetFloat("strength");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetFloat("beginStrength", m_beginStrength);
            json.SetFloat("strength", m_toStrength);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Light) {
                errorInfo = GetType().FullName + " GetComponent<Light> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
