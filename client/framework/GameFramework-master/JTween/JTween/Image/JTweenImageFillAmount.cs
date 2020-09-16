using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Image {
    public class JTweenImageFillAmount : JTweenBase {
        private float m_beginAmount = 0;
        private float m_toAmount = 0;
        private UnityEngine.UI.Image m_Image;

        public JTweenImageFillAmount() {
            m_tweenType = (int)JTweenImage.FillAmount;
            m_tweenElement = JTweenElement.Image;
        }

        public float BeginAmount {
            get {
                return m_beginAmount;
            }
            set {
                m_beginAmount = value;
            }
        }

        public float ToAmount {
            get {
                return m_toAmount;
            }
            set {
                m_toAmount = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Image = m_target.GetComponent<UnityEngine.UI.Image>();
            if (null == m_Image) return;
            // end if
            m_beginAmount = m_Image.fillAmount;
        }

        protected override Tween DOPlay() {
            if (null == m_Image) return null;
            // end if
            return m_Image.DOFillAmount(m_toAmount, m_duration);
        }

        public override void Restore() {
            if (null == m_Image) return;
            // end if
            m_Image.fillAmount = m_beginAmount;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginAmount")) BeginAmount = (float)json["beginAmount"];
            // end if
            if (json.Contains("amount")) m_toAmount = (float)json["amount"];
            // end if
            Restore();
        }

        protected override void ToJson(ref JsonData json) {
            json["beginAmount"] = m_beginAmount;
            json["amount"] = m_toAmount;
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
