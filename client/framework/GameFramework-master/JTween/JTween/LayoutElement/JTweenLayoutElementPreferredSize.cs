using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.LayoutElement {
    public class JTweenLayoutElementPreferredSize : JTweenBase {
        private float m_width = 0;
        private float m_height = 0;
        private float m_beginWidth = 0;
        private float m_beginHeight = 0;
        private UnityEngine.UI.LayoutElement m_LayoutElement;

        public JTweenLayoutElementPreferredSize() {
            m_tweenType = (int)JTweenLayoutElement.PreferredSize;
            m_tweenElement = JTweenElement.LayoutElement;
        }

        public float Width {
            get {
                return m_width;
            }
            set {
                m_width = value;
            }
        }

        public float Height {
            get {
                return m_height;
            }
            set {
                m_height = value;
            }
        }

        public float BeginWidth {
            get {
                return m_beginWidth;
            }
            set {
                m_beginWidth = value;
                if (m_LayoutElement != null) {
                    m_LayoutElement.minWidth = m_beginWidth;
                } // end if
            }
        }

        public float BeginHeight {
            get {
                return m_beginHeight;
            }
            set {
                m_beginHeight = value;
                if (m_LayoutElement != null) {
                    m_LayoutElement.minHeight = m_beginHeight;
                } // end if
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_LayoutElement = m_target.GetComponent<UnityEngine.UI.LayoutElement>();
            if (null == m_LayoutElement) return;
            // end if
            m_beginWidth = m_LayoutElement.preferredWidth;
            m_beginHeight = m_LayoutElement.preferredHeight;
        }

        protected override Tween DOPlay() {
            if (null == m_LayoutElement) return null;
            // end if
            return m_LayoutElement.DOPreferredSize(new Vector2(m_width, m_height), m_duration, m_isSnapping);
        }

        public override void Restore() {
            if (null == m_LayoutElement) return;
            // end if
            m_LayoutElement.minWidth = m_beginWidth;
            m_LayoutElement.minHeight = m_beginHeight;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("width")) m_width = json["width"].ToFloat();
            // end if
            if (json.Contains("height")) m_height = json["height"].ToFloat();
            // end if
            if (json.Contains("beginWidth")) BeginWidth = json["beginWidth"].ToFloat();
            // end if
            if (json.Contains("beginHeight")) BeginHeight = json["beginHeight"].ToFloat();
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["width"] = m_width;
            json["height"] = m_height;
            json["beginWidth"] = m_beginWidth;
            json["beginHeight"] = m_beginHeight;
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_LayoutElement) {
                errorInfo = GetType().FullName + " GetComponent<LayoutElement> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
