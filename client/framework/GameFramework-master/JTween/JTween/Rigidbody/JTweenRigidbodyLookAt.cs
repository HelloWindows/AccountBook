﻿using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.Rigidbody {
    public class JTweenRigidbodyLookAt : JTweenBase {
        private Vector3 m_beginRotate = Vector3.zero;
        private Vector3 m_towards = Vector3.zero;
        private AxisConstraint m_axisConstraint = AxisConstraint.None;
        private Vector3 m_up = Vector3.up;
        private UnityEngine.Rigidbody m_Rigidbody;

        public JTweenRigidbodyLookAt() {
            m_tweenType = (int)JTweenRigidbody.LookAt;
            m_tweenElement = JTweenElement.Rigidbody;
        }

        public Vector3 BeginRotate {
            get {
                return m_beginRotate;
            }
            set {
                m_beginRotate = value;
            }
        }

        public Vector3 Towards {
            get {
                return m_towards;
            }
            set {
                m_towards = value;
            }
        }

        public AxisConstraint AxisConstraint {
            get {
                return m_axisConstraint;
            }
            set {
                m_axisConstraint = value;
            }
        }
        public Vector3 Up {
            get {
                return m_up;
            }
            set {
                m_up = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Rigidbody = m_target.GetComponent<UnityEngine.Rigidbody>();
            if (null == m_Rigidbody) return;
            // end if
            m_beginRotate = m_Rigidbody.rotation.eulerAngles;
        }

        protected override Tween DOPlay() {
            if (null == m_Rigidbody) return null;
            // end if
            return m_Rigidbody.DOLookAt(m_towards, m_duration, m_axisConstraint, m_up);
        }

        public override void Restore() {
            if (null == m_Rigidbody) return;
            // end if
            m_Rigidbody.rotation = Quaternion.Euler(m_beginRotate);
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginRotate")) BeginRotate = JTweenUtils.JsonToVector3(json.GetNode("beginRotate"));
            // end if
            if (json.Contains("towards")) m_towards = JTweenUtils.JsonToVector3(json.GetNode("towards"));
            // end if
            if (json.Contains("axis")) m_axisConstraint = (AxisConstraint)(int)json.GetInt("axis");
            // end if
            if (json.Contains("up")) m_up = JTweenUtils.JsonToVector3(json.GetNode("up")); 
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginRotate", JTweenUtils.Vector3Json(m_beginRotate));
            json.SetNode("towards", JTweenUtils.Vector3Json(m_towards));
            json.SetInt("axis", (int)m_axisConstraint);
            json.SetNode("up", JTweenUtils.Vector3Json(m_up));
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
