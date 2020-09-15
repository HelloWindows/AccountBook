using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Light {
    public class JTweenLightColor : JTweenBase {
        private Color m_beginColor = Color.white;
        private Color m_toColor = Color.white;
        private UnityEngine.Light m_Light;

        public JTweenLightColor() {
            m_tweenType = (int)JTweenLight.Color;
            m_tweenElement = JTweenElement.Light;
        }

        public Color BeginColor {
            get {
                return m_beginColor;
            }
            set {
                m_beginColor = value;
                if (m_Light != null) {
                    m_Light.color = m_beginColor;
                } // end if
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
            m_Light = m_target.GetComponent<UnityEngine.Light>();
            if (null == m_Light) return;
            // end if
            m_beginColor = m_Light.color;
        }

        protected override Tween DOPlay() {
            if (null == m_Light) return null;
            // end if
            return m_Light.DOColor(m_toColor, m_duration);
        }

        public override void Restore() {
            if (null == m_Light) return;
            // end if
            m_Light.color = m_beginColor;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginColor")) BeginColor = JTweenUtils.JsonToColor(json["beginColor"]);
            // end if
            if (json.Contains("color")) m_toColor = JTweenUtils.JsonToColor(json["color"]);
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["beginColor"] = JTweenUtils.ColorJson(m_beginColor);
            json["color"] = JTweenUtils.ColorJson(m_toColor);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Light) {
                errorInfo = GetType().FullName + " GetComponent<Light> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
