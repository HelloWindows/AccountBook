using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.AudioSource {
    public class JTweenAudioSourceFade : JTweenBase {
        private float m_beginVolume = 0;
        private float m_toVolume = 0;
        private UnityEngine.AudioSource m_AudioSource;

        public JTweenAudioSourceFade() {
            m_tweenType = (int)JTweenAudioSource.Fade;
            m_tweenElement = JTweenElement.AudioSource;
        }

        public float ToVolume {
            get {
                return m_toVolume;
            }
            set {
                m_toVolume = value;
            }
        }

        public override void Init() {
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
            if (m_toVolume < 0) {
                m_toVolume = 0;
            } else if (m_toVolume > 1) {
                m_toVolume = 1;
            } // end if
            return m_AudioSource.DOFade(m_toVolume, m_duration);
        }

        protected override void Restore() {
            if (null == m_AudioSource) return;
            // end if
            m_AudioSource.volume = m_beginVolume;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("volume")) m_toVolume = (float)json["volume"];
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            if (m_toVolume < 0) {
                m_toVolume = 0;
            } else if (m_toVolume > 1) {
                m_toVolume = 1;
            } // end if
            json["volume"] = m_toVolume;
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
