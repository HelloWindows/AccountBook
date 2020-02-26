using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.CanvasGroup {
    public class JTweenCanvasGroupFade : JTweenBase {
        private float m_beginAlpha = 0;
        private float m_toAlpha = 0;
        private UnityEngine.CanvasGroup m_CanvasGroup;

        public JTweenCanvasGroupFade() {
            m_tweenType = (int)JTweenCanvasGroup.Fade;
            m_tweenElement = JTweenElement.CanvasGroup;
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
            m_CanvasGroup = m_target.GetComponent<UnityEngine.CanvasGroup>();
            if (null == m_CanvasGroup) return;
            // end if
            m_beginAlpha = m_CanvasGroup.alpha;
        }

        protected override Tween DOPlay() {
            if (null == m_CanvasGroup) return null;
            // end if
            return m_CanvasGroup.DOFade(m_toAlpha, m_duration);
        }

        public override void Restore() {
            if (null == m_CanvasGroup) return;
            // end if
            m_CanvasGroup.alpha = m_beginAlpha;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("alpha")) m_toAlpha = (float)json["alpha"];
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["alpha"] = m_toAlpha;
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_CanvasGroup) {
                errorInfo = GetType().FullName + " GetComponent<CanvasGroup> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
