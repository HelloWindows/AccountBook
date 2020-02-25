using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Text {
    public class JTweenTextFade : JTweenBase {
        private Color m_beginColor = Color.white;
        private float m_toAlpha = 0;
        private UnityEngine.UI.Text m_text;

        public float ToAlpha {
            get {
                return m_toAlpha;
            }
            set {
                m_toAlpha = value;
            }
        }

        public override void Init() {
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
            return m_text.DOFade(m_toAlpha, m_duration);
        }

        protected override void Restore() {
            if (null == m_text) return;
            // end if
            m_text.color = m_beginColor;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("alpha")) m_toAlpha = (float)json["alpha"];
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["alpha"] = m_toAlpha;
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
