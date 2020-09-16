using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Transform {
    public class JTweenTransformPunchPosition : JTweenBase {
        private Vector3 m_beginPosition = Vector3.zero;
        private Vector3 m_toPunch = Vector3.zero;
        private int m_vibrate = 0;
        private float m_elasticity = 0; // [0 - 1]
        private UnityEngine.Transform m_Transform;

        public JTweenTransformPunchPosition() {
            m_tweenType = (int)JTweenTransform.PunchPosition;
            m_tweenElement = JTweenElement.Transform;
        }

        public Vector3 BeginPosition {
            get {
                return m_beginPosition;
            }
            set {
                m_beginPosition = value;
            }
        }

        public Vector3 ToPunch {
            get {
                return m_toPunch;
            }
            set {
                m_toPunch = value;
            }
        }

        public int Vibrate {
            get {
                return m_vibrate;
            }
            set {
                m_vibrate = value;
            }
        }

        public float Elasticity {
            get {
                return m_elasticity;
            }
            set {
                m_elasticity = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Transform = m_target.GetComponent<UnityEngine.Transform>();
            if (null == m_Transform) return;
            // end if
            m_beginPosition = m_Transform.position;
        }

        protected override Tween DOPlay() {
            if (null == m_Transform) return null;
            // end if
            return m_Transform.DOPunchPosition(m_toPunch, m_duration, m_vibrate, m_elasticity, m_isSnapping);
        }

        public override void Restore() {
            if (null == m_Transform) return;
            // end if
            m_Transform.position = m_beginPosition;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginPosition")) BeginPosition = JTweenUtils.JsonToVector3(json["beginPosition"]);
            // end if
            if (json.Contains("punch")) m_toPunch = JTweenUtils.JsonToVector3(json["punch"]);
            // end if
            if (json.Contains("vibrate")) m_vibrate = (int)json["vibrate"];
            // end if
            if (json.Contains("elasticity")) m_elasticity = (float)json["elasticity"];
            // end if
            Restore();
        }

        protected override void ToJson(ref JsonData json) {
            json["beginPosition"] = JTweenUtils.Vector3Json(m_beginPosition);
            json["punch"] = JTweenUtils.Vector3Json(m_toPunch);
            json["vibrate"] = m_vibrate;
            json["elasticity"] = m_elasticity;
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
