using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Rigidbody2D {
    public class JTweenRigidbody2DRotate : JTweenBase {
        private float m_beginRotation = 0;
        private float m_toAngle = 0;
        private UnityEngine.Rigidbody2D m_Rigidbody;

        public JTweenRigidbody2DRotate() {
            m_tweenType = (int)JTweenRigidbody2D.Rotate;
            m_tweenElement = JTweenElement.Rigidbody2D;
        }

        public float BeginRotation {
            get {
                return m_beginRotation;
            }
            set {
                m_beginRotation = value;
                if (m_Rigidbody != null) {
                    m_Rigidbody.rotation = m_beginRotation;
                } // end if
            }
        }

        public float ToAngle {
            get {
                return m_toAngle;
            }
            set {
                m_toAngle = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Rigidbody = m_target.GetComponent<UnityEngine.Rigidbody2D>();
            if (null == m_Rigidbody) return;
            // end if
            m_beginRotation = m_Rigidbody.rotation;
        }

        protected override Tween DOPlay() {
            if (null == m_Rigidbody) return null;
            // end if
            return m_Rigidbody.DORotate(m_toAngle, m_duration);
        }

        public override void Restore() {
            if (null == m_Rigidbody) return;
            // end if
            m_Rigidbody.rotation = m_beginRotation;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginRotation")) BeginRotation = (float)json["beginRotation"];
            // end if
            if (json.Contains("angle")) m_toAngle = (float)json["angle"];
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["beginRotation"] = m_beginRotation;
            json["angle"] = m_toAngle;
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Rigidbody) {
                errorInfo = GetType().FullName + " GetComponent<Rigidbody> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
