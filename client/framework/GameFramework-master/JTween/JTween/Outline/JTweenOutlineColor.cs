using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.Outline {
    public class JTweenOutlineColor : JTweenBase {
        private Color m_beginColor = Color.white;
        private Color m_toColor = Color.white;
        private UnityEngine.UI.Outline m_Outline;

        public JTweenOutlineColor() {
            m_tweenType = (int)JTweenOutline.Color;
            m_tweenElement = JTweenElement.Outline;
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
            m_Outline = m_target.GetComponent<UnityEngine.UI.Outline>();
            if (null == m_Outline) return;
            // end if
            m_beginColor = m_Outline.effectColor;
        }

        protected override Tween DOPlay() {
            if (null == m_Outline) return null;
            // end if
            return m_Outline.DOColor(m_toColor, m_duration);
        }

        public override void Restore() {
            if (null == m_Outline) return;
            // end if
            m_Outline.effectColor = m_beginColor;
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
            if (null == m_Outline) {
                errorInfo = GetType().FullName + " GetComponent<Outline> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
