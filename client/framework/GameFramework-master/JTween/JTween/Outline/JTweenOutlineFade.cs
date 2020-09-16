using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Outline {
    public class JTweenOutlineFade : JTweenBase {
        private Color m_beginColor = Color.white;
        private float m_toAlpha = 0;
        private UnityEngine.UI.Outline m_Outline;

        public JTweenOutlineFade() {
            m_tweenType = (int)JTweenOutline.Fade;
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
            m_Outline = m_target.GetComponent<UnityEngine.UI.Outline>();
            if (null == m_Outline) return;
            // end if
            m_beginColor = m_Outline.effectColor;
        }

        protected override Tween DOPlay() {
            if (null == m_Outline) return null;
            // end if
            return m_Outline.DOFade(m_toAlpha, m_duration);
        }

        public override void Restore() {
            if (null == m_Outline) return;
            // end if
            m_Outline.effectColor = m_beginColor;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginColor")) BeginColor = JTweenUtils.JsonToColor(json["beginColor"]);
            // end if
            if (json.Contains("alpha")) m_toAlpha = (float)json["alpha"];
            // end if
            Restore();
        }

        protected override void ToJson(ref JsonData json) {
            json["beginColor"] = JTweenUtils.ColorJson(m_beginColor);
            json["alpha"] = m_toAlpha;
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
