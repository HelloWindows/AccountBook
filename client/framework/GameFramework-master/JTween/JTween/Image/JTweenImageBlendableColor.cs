using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Image {
    public class JTweenImageBlendableColor : JTweenBase {
        private Color m_beginColor = Color.white;
        private Color m_toColor = Color.white;
        private UnityEngine.UI.Image m_Image;

        public JTweenImageBlendableColor() {
            m_tweenType = (int)JTweenImage.BlendableColor;
            m_tweenElement = JTweenElement.Image;
        }

        public Color BeginColor {
            get {
                return m_beginColor;
            }
            set {
                m_beginColor = value;
                if (m_Image != null) {
                    m_Image.color = m_beginColor;
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
            m_Image = m_target.GetComponent<UnityEngine.UI.Image>();
            if (null == m_Image) return;
            // end if
            m_beginColor = m_Image.color;
        }

        protected override Tween DOPlay() {
            if (null == m_Image) return null;
            // end if
            return m_Image.DOBlendableColor(m_toColor, m_duration);
        }

        public override void Restore() {
            if (null == m_Image) return;
            // end if
            m_Image.color = m_beginColor;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("color")) m_toColor = JTweenUtils.JsonToColor(json["color"]);
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["color"] = JTweenUtils.ColorJson(m_toColor);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Image) {
                errorInfo = GetType().FullName + " GetComponent<Image> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
