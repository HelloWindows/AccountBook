using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.AudioSource {
    public class JTweenAudioSourcePitch : JTweenBase {
        private float m_beginPitch = 0;
        private float m_toPitch = 0;
        private UnityEngine.AudioSource m_AudioSource;

        public float ToPitch {
            get {
                return m_toPitch;
            }
            set {
                m_toPitch = value;
            }
        }

        public override void Init() {
            if (null == m_Target) return;
            // end if
            m_AudioSource = m_Target.GetComponent<UnityEngine.AudioSource>();
            if (null == m_AudioSource) return;
            // end if
            m_beginPitch = m_AudioSource.volume;
        }

        protected override Tween DOPlay() {
            if (null == m_AudioSource) return null;
            // end if
            if (m_toPitch < 0) {
                m_toPitch = 0;
            } else if (m_toPitch > 1) {
                m_toPitch = 1;
            } // end if
            return m_AudioSource.DOPitch(m_toPitch, m_Duration);
        }

        protected override void Restore() {
            if (null == m_AudioSource) return;
            // end if
            m_AudioSource.volume = m_beginPitch;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("pitch")) m_toPitch = (float)json["pitch"];
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            if (m_toPitch < 0) {
                m_toPitch = 0;
            } else if (m_toPitch > 1) {
                m_toPitch = 1;
            } // end if
            json["pitch"] = m_toPitch;
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_AudioSource) {
                errorInfo = "JTweenAudioSourcePitch GetComponent<AudioSource> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
