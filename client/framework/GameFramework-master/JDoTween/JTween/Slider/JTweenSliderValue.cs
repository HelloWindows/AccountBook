using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Slider {
    public class JTweenSliderValue : JTweenBase {
        private float m_beginValue = 0;
        private float m_toValue = 0;
        private UnityEngine.UI.Slider m_slider;

        public JTweenSliderValue() {
            m_tweenType = (int)JTweenSlider.Value;
            m_tweenElement = JTweenElement.Slider;
        }

        public float ToValue {
            get {
                return m_toValue;
            }
            set {
                m_toValue = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_slider = m_target.GetComponent<UnityEngine.UI.Slider>();
            if (null == m_slider) return;
            // end if
            m_beginValue = m_slider.value;
        }

        protected override Tween DOPlay() {
            if (null == m_slider) return null;
            // end if
            return m_slider.DOValue(m_toValue, m_duration, m_isSnapping);
        }

        public override void Restore() {
            if (null == m_slider) return;
            // end if
            m_slider.value = m_beginValue;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("value")) m_toValue = json["value"].ToFloat();
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["value"] = m_toValue;
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_slider) {
                errorInfo = GetType().FullName + " GetComponent<Slider> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
