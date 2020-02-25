using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Image {
    public class JTweenImageColor : JTweenBase {
        private Color m_beginColor = Color.white;
        private Color m_toColor = Color.white;
        private UnityEngine.UI.Image m_Image;

        public Color ToColor {
            get {
                return m_toColor;
            }
            set {
                m_toColor = value;
            }
        }

        public override void Init() {
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
            return m_Image.DOColor(m_toColor, m_duration);
        }

        protected override void Restore() {
            if (null == m_Image) return;
            // end if
            m_Image.color = m_beginColor;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("color")) m_toColor = Utility.Utils.JsonToColor(json["color"]);
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["color"] = Utility.Utils.ColorJson(m_toColor);
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
