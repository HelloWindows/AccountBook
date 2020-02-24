using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Transform {
    public class JTweenTransformBlendableLocalRotate : JTweenBase {
        private Vector3 m_beginRotation = Vector3.zero;
        private Vector3 m_toRotate = Vector3.zero;
        private UnityEngine.Transform m_Transform;

        public Vector3 ToRotate {
            get {
                return m_toRotate;
            }
            set {
                m_toRotate = value;
            }
        }

        public override void Init() {
            if (null == m_Target) return;
            // end if
            m_Transform = m_Target.GetComponent<UnityEngine.Transform>();
            if (null == m_Transform) return;
            // end if
            m_beginRotation = m_Transform.localEulerAngles;
        }

        protected override Tween DOPlay() {
            if (null == m_Transform) return null;
            // end if
            return m_Transform.DOBlendableLocalRotateBy(m_toRotate, m_Duration);
        }

        protected override void Restore() {
            if (null == m_Transform) return;
            // end if
            m_Transform.localEulerAngles = m_beginRotation;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("rotate")) m_toRotate = Utility.Utils.JsonToVector3(json["rotate"]);

        }

        protected override void ToJson(ref JsonData json) {
            json["rotate"] = Utility.Utils.Vector3Json(m_toRotate);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Transform) {
                errorInfo = GetType().FullName + " GetComponent<Transform> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
