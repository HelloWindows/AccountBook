using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Graphic {
    public class JTweenGraphicFade : JTweenBase {
        private Color m_beginColor = Color.white;
        private float m_toAlpha = 0;
        private UnityEngine.UI.Graphic m_Graphic;

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
            m_Graphic = m_target.GetComponent<UnityEngine.UI.Graphic>();
            if (null == m_Graphic) return;
            // end if
            m_beginColor = m_Graphic.color;
        }

        protected override Tween DOPlay() {
            if (null == m_Graphic) return null;
            // end if
            return m_Graphic.DOFade(m_toAlpha, m_duration);
        }

        protected override void Restore() {
            if (null == m_Graphic) return;
            // end if
            m_Graphic.color = m_beginColor;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("alpha")) m_toAlpha = (float)json["alpha"];
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["alpha"] = m_toAlpha;
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Graphic) {
                errorInfo = GetType().FullName + " GetComponent<Graphic> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
