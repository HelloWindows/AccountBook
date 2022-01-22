using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.Rigidbody {
    public class JTweenRigidbodyMove : JTweenBase {
        public enum MoveTypeEnum {
            Move = 0,
            MoveX = 1,
            MoveY = 2,
            MoveZ = 3,
        }
        private MoveTypeEnum m_MoveType = MoveTypeEnum.Move;
        private Vector3 m_beginPosition = Vector3.zero;
        private Vector3 m_toPosition = Vector3.zero;
        private float m_toMoveX = 0;
        private float m_toMoveY = 0;
        private float m_toMoveZ = 0;
        private UnityEngine.Rigidbody m_Rigidbody;

        public JTweenRigidbodyMove() {
            m_tweenType = (int)JTweenRigidbody.Move;
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

        public MoveTypeEnum MoveType {
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

        public float ToMoveZ {
            get {
                return m_toMoveZ;
            }
            set {
                m_toMoveZ = value;
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
            switch (m_MoveType) {
                case MoveTypeEnum.Move:
                    return m_Rigidbody.DOMove(m_toPosition, m_duration, m_isSnapping);
                case MoveTypeEnum.MoveX:
                    return m_Rigidbody.DOMoveX(m_toMoveX, m_duration, m_isSnapping);
                case MoveTypeEnum.MoveY:
                    return m_Rigidbody.DOMoveY(m_toMoveY, m_duration, m_isSnapping);
                case MoveTypeEnum.MoveZ:
                    return m_Rigidbody.DOMoveZ(m_toMoveZ, m_duration, m_isSnapping);
                default: return null;
            } // end switch
        }

        public override void Restore() {
            if (null == m_Rigidbody) return;
            // end if
            m_Rigidbody.position = m_beginPosition;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginPosition")) BeginPosition = JTweenUtils.JsonToVector3(json.GetNode("beginPosition"));
            // end if
            if (json.Contains("move")) {
                m_MoveType = MoveTypeEnum.Move;
                m_toPosition = JTweenUtils.JsonToVector3(json.GetNode("move"));
            } else if (json.Contains("moveX")) {
                m_MoveType = MoveTypeEnum.MoveX;
                m_toMoveX = json.GetFloat("moveX");
            } else if (json.Contains("moveY")) {
                m_MoveType = MoveTypeEnum.MoveY;
                m_toMoveY = json.GetFloat("moveY");
            } else if (json.Contains("moveZ")) {
                m_MoveType = MoveTypeEnum.MoveZ;
                m_toMoveZ = json.GetFloat("moveZ");
            } else {
                Debug.LogError(GetType().FullName + " JsonTo MoveType is null");
            } // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginPosition", JTweenUtils.Vector3Json(m_beginPosition));
            switch (m_MoveType) {
                case MoveTypeEnum.Move:
                    json.SetNode("move", JTweenUtils.Vector3Json(m_toPosition));
                    break;
                case MoveTypeEnum.MoveX:
                    json.SetFloat("moveX", m_toMoveX);
                    break;
                case MoveTypeEnum.MoveY:
                    json.SetFloat("moveY", m_toMoveY);
                    break;
                case MoveTypeEnum.MoveZ:
                    json.SetFloat("moveZ", m_toMoveZ);
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
