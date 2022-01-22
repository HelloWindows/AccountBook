using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.SpriteRenderer {
    public class JTweenSpriteRendererBlendableColor : JTweenBase {
        private Color m_beginColor = Color.white;
        private Color m_toColor = Color.white;
        private UnityEngine.SpriteRenderer m_SpriteRenderer;

        public JTweenSpriteRendererBlendableColor() {
            m_tweenType = (int)JTweenSpriteRenderer.BlendableColor;
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

        public Color ToColor {
            get {
                return m_toColor;
            }
            set {
                m_toColor = value;
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
            return m_SpriteRenderer.DOBlendableColor(m_toColor, m_duration);
        }

        public override void Restore() {
            if (null == m_SpriteRenderer) return;
            // end if
            m_SpriteRenderer.color = m_beginColor;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginColor")) BeginColor = JTweenUtils.JsonToColor(json.GetNode("beginColor"));
            // end if
            if (json.Contains("color")) m_toColor = JTweenUtils.JsonToColor(json.GetNode("color"));
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginColor", JTweenUtils.ColorJson(m_beginColor));
            json.SetNode("color", JTweenUtils.ColorJson(m_toColor));
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
