using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Camera {
    public class JTweenCameraColor : JTweenBase {
        private Color m_beginColor = Color.white;
        private Color m_toColor = Color.white;
        private UnityEngine.Camera m_Camera;

        public JTweenCameraColor() {
            m_tweenType = (int)JTweenCamera.Color;
            m_tweenElement = JTweenElement.Camera;
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
            m_Camera = m_target.GetComponent<UnityEngine.Camera>();
            if (null == m_Camera) return;
            // end if
            m_beginColor = m_Camera.backgroundColor;
        }

        protected override Tween DOPlay() {
            if (null == m_Camera) return null;
            // end if
            return m_Camera.DOColor(m_toColor, m_duration);
        }

        public override void Restore() {
            if (null == m_Camera) return;
            // end if
            m_Camera.backgroundColor = m_beginColor;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("color")) m_toColor = JTweenUtils.JsonToColor(json["color"]);
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["color"] = JTweenUtils.ColorJson(m_toColor);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Camera) {
                errorInfo = GetType().FullName + " GetComponent<Camera> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
