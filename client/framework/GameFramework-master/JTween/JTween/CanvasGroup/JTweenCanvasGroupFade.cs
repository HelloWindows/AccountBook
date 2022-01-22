using Json;
using DG.Tweening;

namespace JTween.CanvasGroup {
    public class JTweenCanvasGroupFade : JTweenBase {
        private float m_beginAlpha = 0;
        private float m_toAlpha = 0;
        private UnityEngine.CanvasGroup m_CanvasGroup;

        public JTweenCanvasGroupFade() {
            m_tweenType = (int)JTweenCanvasGroup.Fade;
            m_tweenElement = JTweenElement.CanvasGroup;
        }

        public float BeginAlpha {
            get {
                return m_beginAlpha;
            }
            set {
                m_beginAlpha = value;
            }
        }

        public float ToAlpha {
            get {
                return m_toAlpha;
            }
            set {
                m_toAlpha = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_CanvasGroup = m_target.GetComponent<UnityEngine.CanvasGroup>();
            if (null == m_CanvasGroup) return;
            // end if
            m_beginAlpha = m_CanvasGroup.alpha;
        }

        protected override Tween DOPlay() {
            if (null == m_CanvasGroup) return null;
            // end if
            return m_CanvasGroup.DOFade(m_toAlpha, m_duration);
        }

        public override void Restore() {
            if (null == m_CanvasGroup) return;
            // end if
            m_CanvasGroup.alpha = m_beginAlpha;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginAlpha")) BeginAlpha = json.GetFloat("beginAlpha");
            // end if
            if (json.Contains("alpha")) m_toAlpha = json.GetFloat("alpha");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetFloat("beginAlpha", m_beginAlpha);
            json.SetFloat("alpha", m_toAlpha);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_CanvasGroup) {
                errorInfo = GetType().FullName + " GetComponent<CanvasGroup> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
