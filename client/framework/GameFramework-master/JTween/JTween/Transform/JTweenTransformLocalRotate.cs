using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Transform {
    public class JTweenTransformLocalRotate : JTweenBase {
        private Vector3 m_beginRotation = Vector3.zero;
        private Vector3 m_toRotate = Vector3.zero;
        private RotateMode m_RotateMode = RotateMode.Fast;
        private UnityEngine.Transform m_Transform;

        public JTweenTransformLocalRotate() {
            m_tweenType = (int)JTweenTransform.LocalRotate;
            m_tweenElement = JTweenElement.Transform;
        }

        public Vector3 ToRotate {
            get {
                return m_toRotate;
            }
            set {
                m_toRotate = value;
            }
        }

        public RotateMode RotateMode {
            get {
                return m_RotateMode;
            }
            set {
                m_RotateMode = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Transform = m_target.GetComponent<UnityEngine.Transform>();
            if (null == m_Transform) return;
            // end if
            m_beginRotation = m_Transform.localEulerAngles;
        }

        protected override Tween DOPlay() {
            if (null == m_Transform) return null;
            // end if
            return m_Transform.DOLocalRotate(m_toRotate, m_duration, m_RotateMode);
        }

        public override void Restore() {
            if (null == m_Transform) return;
            // end if
            m_Transform.localEulerAngles = m_beginRotation;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("rotate")) m_toRotate = JTweenUtils.JsonToVector3(json["rotate"]);
            // end if
            if (json.Contains("mode")) m_RotateMode = (RotateMode)(int)json["mode"];
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["rotate"] = JTweenUtils.Vector3Json(m_toRotate);
            json["mode"] = (int)m_RotateMode;
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
