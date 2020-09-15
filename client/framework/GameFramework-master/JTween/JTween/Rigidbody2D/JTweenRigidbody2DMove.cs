using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Rigidbody2D {
    public class JTweenRigidbody2DMove : JTweenBase {
        public enum MoveTypeEnem {
            Move = 0,
            MoveX = 1,
            MoveY = 2,
        }
        private MoveTypeEnem m_MoveType = MoveTypeEnem.Move;
        private Vector3 m_beginPosition = Vector3.zero;
        private Vector3 m_toPosition = Vector3.zero;
        private float m_toMoveX = 0;
        private float m_toMoveY = 0;
        private UnityEngine.Rigidbody2D m_Rigidbody;

        public JTweenRigidbody2DMove() {
            m_tweenType = (int)JTweenRigidbody2D.Move;
            m_tweenElement = JTweenElement.Rigidbody2D;
        }

        public Vector3 BeginPosition {
            get {
                return m_beginPosition;
            }
            set {
                m_beginPosition = value;
                if (m_Rigidbody != null) {
                    m_Rigidbody.position = m_beginPosition;
                } // end if
            }
        }

        public MoveTypeEnem MoveType {
            get {
                return m_MoveType;
            }
            set {
                m_MoveType = value;
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

        public float ToMoveX {
            get {
                return m_toMoveX;
            }
            set {
                m_toMoveX = value;
            }
        }

        public float ToMoveY {
            get {
                return m_toMoveY;
            }
            set {
                m_toMoveY = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Rigidbody = m_target.GetComponent<UnityEngine.Rigidbody2D>();
            if (null == m_Rigidbody) return;
            // end if
            m_beginPosition = m_Rigidbody.position;
        }

        protected override Tween DOPlay() {
            if (null == m_Rigidbody) return null;
            // end if
            switch (m_MoveType) {
                case MoveTypeEnem.Move:
                    return m_Rigidbody.DOMove(m_toPosition, m_duration, m_isSnapping);
                case MoveTypeEnem.MoveX:
                    return m_Rigidbody.DOMoveX(m_toMoveX, m_duration, m_isSnapping);
                case MoveTypeEnem.MoveY:
                    return m_Rigidbody.DOMoveY(m_toMoveY, m_duration, m_isSnapping);
                default: return null;
            } // end switch
        }

        public override void Restore() {
            if (null == m_Rigidbody) return;
            // end if
            m_Rigidbody.position = m_beginPosition;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginPosition")) BeginPosition = JTweenUtils.JsonToVector3(json["beginPosition"]);
            // end if
            if (json.Contains("move")) {
                m_MoveType = MoveTypeEnem.Move;
                m_toPosition = JTweenUtils.JsonToVector3(json["move"]);
            } else if (json.Contains("moveX")) {
                m_MoveType = MoveTypeEnem.MoveX;
                m_toMoveX = (float)json["moveX"];
            } else if (json.Contains("moveY")) {
                m_MoveType = MoveTypeEnem.MoveY;
                m_toMoveY = (float)json["moveY"];
            } else {
                Debug.LogError(GetType().FullName + " JsonTo MoveType is null");
            } // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["beginPosition"] = JTweenUtils.Vector3Json(m_beginPosition);
            switch (m_MoveType) {
                case MoveTypeEnem.Move:
                    json["move"] = JTweenUtils.Vector3Json(m_toPosition);
                    break;
                case MoveTypeEnem.MoveX:
                    json["moveX"] = m_toMoveX;
                    break;
                case MoveTypeEnem.MoveY:
                    json["moveY"] = m_toMoveY;
                    break;
                default:
                    Debug.LogError(GetType().FullName + " ToJson MoveType is null");
                    break;
            } // end swtich
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
