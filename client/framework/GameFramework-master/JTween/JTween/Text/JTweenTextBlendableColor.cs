using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Text {
    public class JTweenTextBlendableColor : JTweenBase {
        private Color m_beginColor = Color.white;
        private Color m_toColor = Color.white;
        private UnityEngine.UI.Text m_text;

        public JTweenTextBlendableColor() {
            m_tweenType = (int)JTweenText.BlendableColor;
            m_tweenElement = JTweenElement.Text;
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
            m_text = m_target.GetComponent<UnityEngine.UI.Text>();
            if (null == m_text) return;
            // end if
            m_beginColor = m_text.color;
        }

        protected override Tween DOPlay() {
            if (null == m_text) return null;
            // end if
            return m_text.DOBlendableColor(m_toColor, m_duration);
        }

        public override void Restore() {
            if (null == m_text) return;
            // end if
            m_text.color = m_beginColor;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginColor")) BeginColor = JTweenUtils.JsonToColor(json["beginColor"]);
            // end if
            if (json.Contains("color")) m_toColor = JTweenUtils.JsonToColor(json["color"]);
            // end if
            Restore();
        }

        protected override void ToJson(ref JsonData json) {
            json["beginColor"] = JTweenUtils.ColorJson(m_beginColor);
            json["color"] = JTweenUtils.ColorJson(m_toColor);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_text) {
                errorInfo = GetType().FullName + " GetComponent<Text> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
