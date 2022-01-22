using Json;
using DG.Tweening;

namespace JTween.AudioSource {
    public class JTweenAudioSourceFade : JTweenBase {
        private float m_beginVolume = 0;
        private float m_toVolume = 0;
        private UnityEngine.AudioSource m_AudioSource;

        public JTweenAudioSourceFade() {
            m_tweenType = (int)JTweenAudioSource.Fade;
            m_tweenElement = JTweenElement.AudioSource;
        }
        public float BeginVolume {
            get {
                return m_beginVolume;
            }
            set {
                m_beginVolume = value;
                if (m_beginVolume < 0) {
                    m_beginVolume = 0;
                } else if (m_beginVolume > 1) {
                    m_beginVolume = 1;
                } // end if
            }
        }
        public float ToVolume {
            get {
                return m_toVolume;
            }
            set {
                m_toVolume = value;
                if (m_toVolume < 0) {
                    m_toVolume = 0;
                } else if (m_toVolume > 1) {
                    m_toVolume = 1;
                } // end if
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_AudioSource = m_target.GetComponent<UnityEngine.AudioSource>();
            if (null == m_AudioSource) return;
            // end if
            m_beginVolume = m_AudioSource.volume;
        }

        protected override Tween DOPlay() {
            if (null == m_AudioSource) return null;
            // end if
            return m_AudioSource.DOFade(m_toVolume, m_duration);
        }

        public override void Restore() {
            if (null == m_AudioSource) return;
            // end if
            m_AudioSource.volume = m_beginVolume;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginVolume")) BeginVolume = json.GetFloat("beginVolume");
            // end if
            if (json.Contains("volume")) m_toVolume = json.GetFloat("volume");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetFloat("beginVolume", m_beginVolume);
            json.SetFloat("volume", m_toVolume);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_AudioSource) {
                errorInfo = "JTweenAudioSourceFade GetComponent<AudioSource> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
