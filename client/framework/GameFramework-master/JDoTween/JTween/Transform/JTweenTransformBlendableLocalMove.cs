using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Transform {
    public class JTweenTransformBlendableLocalMove : JTweenBase {
        private Vector3 m_beginPosition = Vector3.zero;
        private Vector3 m_toPosition = Vector3.zero;
        private UnityEngine.Transform m_Transform;

        public Vector3 ToPosition {
            get {
                return m_toPosition;
            }
            set {
                m_toPosition = value;
            }
        }

        public override void Init() {
            if (null == m_Target) return;
            // end if
            m_Transform = m_Target.GetComponent<UnityEngine.Transform>();
            if (null == m_Transform) return;
            // end if
            m_beginPosition = m_Transform.localPosition;
        }

        protected override Tween DOPlay() {
            if (null == m_Transform) return null;
            // end if
            return m_Transform.DOBlendableLocalMoveBy(m_toPosition, m_Duration, m_IsSnapping);
        }

        protected override void Restore() {
            if (null == m_Transform) return;
            // end if
            m_Transform.localPosition = m_beginPosition;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("move")) m_toPosition = Utility.Utils.JsonToVector3(json["move"]);

        }

        protected override void ToJson(ref JsonData json) {
            json["move"] = Utility.Utils.Vector3Json(m_toPosition);
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
