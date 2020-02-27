using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Outline {
    public class JTweenOutlineColor : JTweenBase {
        private Color m_beginColor = Color.white;
        private Color m_toColor = Color.white;
        private UnityEngine.UI.Outline m_Outline;

        public JTweenOutlineColor() {
            m_tweenType = (int)JTweenOutline.Color;
            m_tweenElement = JTweenElement.Outline;
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

        protected override void JsonTo(JsonData json) {
            if (json.Contains("color")) m_toColor = Utility.Utils.JsonToColor(json["color"]);
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["color"] = Utility.Utils.ColorJson(m_toColor);
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
