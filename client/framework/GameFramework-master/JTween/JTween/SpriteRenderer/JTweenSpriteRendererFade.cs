using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.SpriteRenderer {
    public class JTweenSpriteRendererFade : JTweenBase {
        private Color m_beginColor = Color.white;
        private float m_toAlpha = 0;
        private UnityEngine.SpriteRenderer m_SpriteRenderer;

        public JTweenSpriteRendererFade() {
            m_tweenType = (int)JTweenSpriteRenderer.Fade;
            m_tweenElement = JTweenElement.SpriteRenderer;
        }

        public Color BeginColor {
            get {
                return m_beginColor;
            }
            set {
                m_beginColor = value;
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
            m_SpriteRenderer = m_target.GetComponent<UnityEngine.SpriteRenderer>();
            if (null == m_SpriteRenderer) return;
            // end if
            m_beginColor = m_SpriteRenderer.color;
        }

        protected override Tween DOPlay() {
            if (null == m_SpriteRenderer) return null;
            // end if
            return m_SpriteRenderer.DOFade(m_toAlpha, m_duration);
        }

        public override void Restore() {
            if (null == m_SpriteRenderer) return;
            // end if
            m_SpriteRenderer.color = m_beginColor;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginColor")) BeginColor = JTweenUtils.JsonToColor(json.GetNode("beginColor"));
            // end if
            if (json.Contains("alpha")) m_toAlpha = json.GetFloat("alpha");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginColor", JTweenUtils.ColorJson(m_beginColor));
            json.SetFloat("alpha", m_toAlpha);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_SpriteRenderer) {
                errorInfo = GetType().FullName + " GetComponent<SpriteRenderer> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
