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

        public override void Init() {
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

        protected override void Restore() {
            if (null == m_LineRenderer) return;
            // end if
            m_LineRenderer.startColor = m_beginStartColor;
            m_LineRenderer.endColor = m_beginEndColor;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("startColor")) m_startColor = Utility.Utils.JsonToColor(json["startColor"]);
            // end if
            if (json.Contains("toStartColor")) m_toStartColor = Utility.Utils.JsonToColor(json["toStartColor"]);
            // end if
            if (json.Contains("endColor")) m_endColor = Utility.Utils.JsonToColor(json["endColor"]);
            // end if
            if (json.Contains("toEndColor")) m_toEndColor = Utility.Utils.JsonToColor(json["toEndColor"]);
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["startColor"] = Utility.Utils.ColorJson(m_startColor);
            json["toStartColor"] = Utility.Utils.ColorJson(m_toStartColor);
            json["endColor"] = Utility.Utils.ColorJson(m_endColor);
            json["toEndColor"] = Utility.Utils.ColorJson(m_toEndColor);
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
