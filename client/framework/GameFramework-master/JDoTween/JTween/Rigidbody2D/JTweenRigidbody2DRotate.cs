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

        public float ToAngle {
            get {
                return m_toAngle;
            }
            set {
                m_toAngle = value;
            }
        }

        public override void Init() {
            if (null == m_Target) return;
            // end if
            m_Rigidbody = m_Target.GetComponent<UnityEngine.Rigidbody2D>();
            if (null == m_Rigidbody) return;
            // end if
            m_beginRotation = m_Rigidbody.rotation;
        }

        protected override Tween DOPlay() {
            if (null == m_Rigidbody) return null;
            // end if
            return m_Rigidbody.DORotate(m_toAngle, m_Duration);
        }

        protected override void Restore() {
            if (null == m_Rigidbody) return;
            // end if
            m_Rigidbody.rotation = m_beginRotation;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("angle")) m_toAngle = (float)json["angle"];
        }

        protected override void ToJson(ref JsonData json) {
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
