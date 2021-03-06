﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Rigidbody {
    public class JTweenRigidbodyJump : JTweenBase {
        private Vector3 m_beginPosition = Vector3.zero;
        private Vector3 m_toPosition = Vector3.zero;
        private int m_numJumps = 0;
        private float m_jumpPower = 0;
        private UnityEngine.Rigidbody m_Rigidbody;

        public JTweenRigidbodyJump() {
            m_tweenType = (int)JTweenRigidbody.Jump;
            m_tweenElement = JTweenElement.Rigidbody;
        }

        public Vector3 BeginPosition {
            get {
                return m_beginPosition;
            }
            set {
                m_beginPosition = value;
            }
        }

        public Vector3 ToPosition {
            get {
                return m_toPosition;
            }
            set {
                m_toPosition = value;
            }
        }

        public float JumpPower {
            get {
                return m_jumpPower;
            }
            set {
                m_jumpPower = value;
            }
        }

        public int NumJumps {
            get {
                return m_numJumps;
            }
            set {
                m_numJumps = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Rigidbody = m_target.GetComponent<UnityEngine.Rigidbody>();
            if (null == m_Rigidbody) return;
            // end if
            m_beginPosition = m_Rigidbody.position;
        }

        protected override Tween DOPlay() {
            if (null == m_Rigidbody) return null;
            // end if
            return m_Rigidbody.DOJump(m_toPosition, m_jumpPower, m_numJumps, m_duration, m_isSnapping);
        }

        public override void Restore() {
            if (null == m_Rigidbody) return;
            // end if
            m_Rigidbody.position = m_beginPosition;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginPosition")) BeginPosition = JTweenUtils.JsonToVector3(json["beginPosition"]);
            // end if
            if (json.Contains("endValue")) m_toPosition = JTweenUtils.JsonToVector3(json["endValue"]);
            // end if
            if (json.Contains("jumpPower")) m_jumpPower = (float)json["jumpPower"];
            // end if
            if (json.Contains("numJumps")) m_numJumps = (int)json["numJumps"];
            // end if
            Restore();
        }

        protected override void ToJson(ref JsonData json) {
            json["beginPosition"] = JTweenUtils.Vector3Json(m_beginPosition);
            json["endValue"] = JTweenUtils.Vector3Json(m_toPosition);
            json["jumpPower"] = m_jumpPower;
            json["numJumps"] = m_numJumps;
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
