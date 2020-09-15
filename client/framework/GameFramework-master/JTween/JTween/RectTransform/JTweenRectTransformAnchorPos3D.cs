using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.RectTransform {
    public class JTweenRectTransformAnchorPos3D : JTweenBase {
        private enum PosType {
            Pos = 0,
            PosX = 1,
            PosY = 2,
            PosZ = 3,
        }
        private Vector3 m_beginAnchorPos = Vector3.zero;
        private Vector3 m_toAnchorPos = Vector3.zero;
        private PosType m_posType = PosType.Pos;
        private float m_toAnchorPosX = 0;
        private float m_toAnchorPosY = 0;
        private float m_toAnchorPosZ = 0;
        private UnityEngine.RectTransform m_RectTransform;

        public JTweenRectTransformAnchorPos3D() {
            m_tweenType = (int)JTweenRectTransform.AnchorPos3D;
            m_tweenElement = JTweenElement.RectTransform;
        }

        public Vector3 BeginAnchorPos {
            get {
                return m_beginAnchorPos;
            }
            set {
                m_beginAnchorPos = value;
                if (m_RectTransform != null) {
                    m_RectTransform.anchoredPosition3D = m_beginAnchorPos;
                } // end if
            }
        }

        public Vector3 ToAnchorPos {
            get {
                return m_toAnchorPos;
            }
            set {
                m_posType = PosType.Pos;
                m_toAnchorPos = value;
            }
        }

        public float ToAnchorPosX {
            get {
                return m_toAnchorPosX;
            }
            set {
                m_posType = PosType.PosX;
                m_toAnchorPosX = value;
            }
        }

        public float ToAnchorPosY {
            get {
                return m_toAnchorPosY;
            }
            set {
                m_posType = PosType.PosY;
                m_toAnchorPosY = value;
            }
        }

        public float ToAnchorPosZ {
            get {
                return m_toAnchorPosZ;
            }
            set {
                m_posType = PosType.PosZ;
                m_toAnchorPosZ = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_RectTransform = m_target.GetComponent<UnityEngine.RectTransform>();
            if (null == m_RectTransform) return;
            // end if
            m_beginAnchorPos = m_RectTransform.anchoredPosition3D;
        }

        protected override Tween DOPlay() {
            if (null == m_RectTransform) return null;
            // end if
            switch (m_posType) {
                case PosType.Pos:
                    return m_RectTransform.DOAnchorPos3D(m_toAnchorPos, m_duration, m_isSnapping);
                case PosType.PosX:
                    return m_RectTransform.DOAnchorPos3DX(m_toAnchorPosX, m_duration, m_isSnapping);
                case PosType.PosY:
                    return m_RectTransform.DOAnchorPos3DY(m_toAnchorPosY, m_duration, m_isSnapping);
                case PosType.PosZ:
                    return m_RectTransform.DOAnchorPos3DZ(m_toAnchorPosZ, m_duration, m_isSnapping);
                default: return null;
            } // end switch
        }

        public override void Restore() {
            if (null == m_RectTransform) return;
            // end if
            m_RectTransform.anchoredPosition3D = m_beginAnchorPos;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginAnchorPos")) BeginAnchorPos = JTweenUtils.JsonToVector3(json["beginAnchorPos"]);
            // end if
            if (json.Contains("pos")) {
                m_posType = PosType.Pos;
                m_toAnchorPos = JTweenUtils.JsonToVector3(json["pos"]);
            } else if (json.Contains("posX")) {
                m_posType = PosType.PosX;
                m_toAnchorPosX = (float)json["posX"];
            } else if (json.Contains("posY")) {
                m_posType = PosType.PosY;
                m_toAnchorPosY = (float)json["posY"];
            } else if (json.Contains("posZ")) {
                m_posType = PosType.PosZ;
                m_toAnchorPosZ = (float)json["posZ"];
            } else {
                Debug.LogError(GetType().FullName + " JsonTo PosType is null");
            } // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["beginAnchorPos"] = JTweenUtils.Vector3Json(m_beginAnchorPos);
            switch (m_posType) {
                case PosType.Pos:
                    json["pos"] = JTweenUtils.Vector3Json(m_toAnchorPos);
                    break;
                case PosType.PosX:
                    json["posX"] = m_toAnchorPosX;
                    break;
                case PosType.PosY:
                    json["posY"] = m_toAnchorPosY;
                    break;
                case PosType.PosZ:
                    json["posZ"] = m_toAnchorPosZ;
                    break;
                default:
                    Debug.LogError(GetType().FullName + " ToJson PosType is null");
                    break;
            } // end swtich
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_RectTransform) {
                errorInfo = GetType().FullName + " GetComponent<RectTransform> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
