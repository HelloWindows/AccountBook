using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Transform {
    public class JTweenTransformMove : JTweenBase {
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
        private UnityEngine.Transform m_Transform;

        public JTweenTransformMove() {
            m_tweenType = (int)JTweenTransform.Move;
            m_tweenElement = JTweenElement.Transform;
        }

        public Vector3 BeginPosition {
            get {
                return m_beginPosition;
            }
            set {
                m_beginPosition = value;
                if (m_Transform != null) {
                    m_Transform.position = m_beginPosition;
                } // end if
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
            m_Transform = m_target.GetComponent<UnityEngine.Transform>();
            if (null == m_Transform) return;
            // end if
            m_beginPosition = m_Transform.position;
        }

        protected override Tween DOPlay() {
            if (null == m_Transform) return null;
            // end if
            switch (m_MoveType) {
                case MoveTypeEnum.Move:
                    return m_Transform.DOMove(m_toPosition, m_duration, m_isSnapping);
                case MoveTypeEnum.MoveX:
                    return m_Transform.DOMoveX(m_toMoveX, m_duration, m_isSnapping);
                case MoveTypeEnum.MoveY:
                    return m_Transform.DOMoveY(m_toMoveY, m_duration, m_isSnapping);
                case MoveTypeEnum.MoveZ:
                    return m_Transform.DOMoveZ(m_toMoveZ, m_duration, m_isSnapping);
                default: return null;
            } // end switch
        }

        public override void Restore() {
            if (null == m_Transform) return;
            // end if
            m_Transform.position = m_beginPosition;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginPosition")) BeginPosition = JTweenUtils.JsonToVector3(json["beginPosition"]);
            // end if
            if (json.Contains("move")) {
                m_MoveType = MoveTypeEnum.Move;
                m_toPosition = JTweenUtils.JsonToVector3(json["move"]);
            } else if (json.Contains("moveX")) {
                m_MoveType = MoveTypeEnum.MoveX;
                m_toMoveX = (float)json["moveX"];
            } else if (json.Contains("moveY")) {
                m_MoveType = MoveTypeEnum.MoveY;
                m_toMoveY = (float)json["moveY"];
            } else if (json.Contains("moveZ")) {
                m_MoveType = MoveTypeEnum.MoveZ;
                m_toMoveZ = (float)json["moveZ"];
            } else {
                Debug.LogError(GetType().FullName + " JsonTo MoveType is null");
            } // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["beginPosition"] = JTweenUtils.Vector3Json(m_beginPosition);
            switch (m_MoveType) {
                case MoveTypeEnum.Move:
                    json["move"] = JTweenUtils.Vector3Json(m_toPosition);
                    break;
                case MoveTypeEnum.MoveX:
                    json["moveX"] = m_toMoveX;
                    break;
                case MoveTypeEnum.MoveY:
                    json["moveY"] = m_toMoveY;
                    break;
                case MoveTypeEnum.MoveZ:
                    json["moveZ"] = m_toMoveZ;
                    break;
                default:
                    Debug.LogError(GetType().FullName + " ToJson MoveType is null");
                    break;
            } // end swtich
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
