using Json;
using DG.Tweening;
using UnityEngine;

namespace JTween.LineRenderer {
    public class JTweenLineRendererColor : JTweenBase {
        private Color m_beginStartColor = Color.white;
        private Color m_beginEndColor = Color.white;
        private Color m_toStartColor = Color.white;
        private Color m_toEndColor = Color.white;
        private UnityEngine.LineRenderer m_LineRenderer;

        public JTweenLineRendererColor() {
            m_tweenType = (int)JTweenLineRenderer.Color;
            m_tweenElement = JTweenElement.LineRenderer;
        }

        public Color BeginStartColor {
            get {
                return m_beginStartColor;
            }
            set {
                m_beginStartColor = value;
            }
        }

        public Color BeginEndColor {
            get {
                return m_beginEndColor;
            }
            set {
                m_beginEndColor = value;
            }
        }

        public Color ToStartColor {
            get {
                return m_toStartColor;
            }
            set {
                m_toStartColor = value;
            }
        }

        public Color ToEndColor {
            get {
                return m_toEndColor;
            }
            set {
                m_toEndColor = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_LineRenderer = m_target.GetComponent<UnityEngine.LineRenderer>();
            if (null == m_LineRenderer) return;
            // end if
            m_beginStartColor = m_LineRenderer.startColor;
            m_beginEndColor = m_LineRenderer.endColor;
        }

        protected override Tween DOPlay() {
            if (null == m_LineRenderer) return null;
            // end if
            return m_LineRenderer.DOColor(new Color2(m_beginStartColor, m_toStartColor), new Color2(m_beginEndColor, m_toEndColor), m_duration);
        }

        public override void Restore() {
            if (null == m_LineRenderer) return;
            // end if
            m_LineRenderer.startColor = m_beginStartColor;
            m_LineRenderer.endColor = m_beginEndColor;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginStartColor")) BeginStartColor = JTweenUtils.JsonToColor(json.GetNode("beginStartColor"));
            // end if
            if (json.Contains("beginEndColor")) BeginEndColor = JTweenUtils.JsonToColor(json.GetNode("beginEndColor"));
            // end if
            if (json.Contains("toStartColor")) m_toStartColor = JTweenUtils.JsonToColor(json.GetNode("toStartColor"));
            // end if
            if (json.Contains("toEndColor")) m_toEndColor = JTweenUtils.JsonToColor(json.GetNode("toEndColor"));
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginStartColor", JTweenUtils.ColorJson(m_beginStartColor));
            json.SetNode("beginEndColor", JTweenUtils.ColorJson(m_beginEndColor));
            json.SetNode("toStartColor", JTweenUtils.ColorJson(m_toStartColor));
            json.SetNode("toEndColor", JTweenUtils.ColorJson(m_toEndColor));
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_LineRenderer) {
                errorInfo = GetType().FullName + " GetComponent<LineRenderer> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
