using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.LineRenderer {
    public class JTweenLineRendererColor : JTweenBase {
        private Color m_beginStartColor = Color.white;
        private Color m_beginEndColor = Color.white;
        private Color m_startColor = Color.white;
        private Color m_toStartColor = Color.white;
        private Color m_endColor = Color.white;
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
                if (m_LineRenderer != null) {
                    m_LineRenderer.startColor = m_beginStartColor;
                } // end if
            }
        }

        public Color BeginEndColor {
            get {
                return m_beginEndColor;
            }
            set {
                m_beginEndColor = value;
                if (m_LineRenderer != null) {
                    m_LineRenderer.endColor = m_beginEndColor;
                } // end if
            }
        }

        public Color StartColor {
            get {
                return m_startColor;
            }
            set {
                m_startColor = value;
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

        public Color EndColor {
            get {
                return m_endColor;
            }
            set {
                m_endColor = value;
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
            return m_LineRenderer.DOColor(new Color2(m_startColor, m_toStartColor), new Color2(m_endColor, m_toEndColor), m_duration);
        }

        public override void Restore() {
            if (null == m_LineRenderer) return;
            // end if
            m_LineRenderer.startColor = m_beginStartColor;
            m_LineRenderer.endColor = m_beginEndColor;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginStartColor")) BeginStartColor = JTweenUtils.JsonToColor(json["beginStartColor"]);
            // end if
            if (json.Contains("beginEndColor")) BeginEndColor = JTweenUtils.JsonToColor(json["beginEndColor"]);
            // end if
            if (json.Contains("startColor")) m_startColor = JTweenUtils.JsonToColor(json["startColor"]);
            // end if
            if (json.Contains("toStartColor")) m_toStartColor = JTweenUtils.JsonToColor(json["toStartColor"]);
            // end if
            if (json.Contains("endColor")) m_endColor = JTweenUtils.JsonToColor(json["endColor"]);
            // end if
            if (json.Contains("toEndColor")) m_toEndColor = JTweenUtils.JsonToColor(json["toEndColor"]);
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["beginStartColor"] = JTweenUtils.ColorJson(m_beginStartColor);
            json["beginEndColor"] = JTweenUtils.ColorJson(m_beginEndColor);
            json["startColor"] = JTweenUtils.ColorJson(m_startColor);
            json["toStartColor"] = JTweenUtils.ColorJson(m_toStartColor);
            json["endColor"] = JTweenUtils.ColorJson(m_endColor);
            json["toEndColor"] = JTweenUtils.ColorJson(m_toEndColor);
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
