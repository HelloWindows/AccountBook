using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.RectTransform {
    public class JTweenRectTransformAnchorPos : JTweenBase {
        private enum PosType {
            Pos = 0,
            PosX = 1,
            PosY = 2,
        }
        private Vector2 m_beginAnchorPos = Vector2.zero;
        private Vector2 m_toAnchorPos = Vector2.zero;
        private PosType m_posType = PosType.Pos;
        private float m_toAnchorPosX = 0;
        private float m_toAnchorPosY = 0;
        private UnityEngine.RectTransform m_RectTransform;

        public JTweenRectTransformAnchorPos() {
            m_tweenType = (int)JTweenRectTransform.AnchorPos;
            m_tweenElement = JTweenElement.RectTransform;
        }

        public Vector2 ToAnchorPos {
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

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_RectTransform = m_target.GetComponent<UnityEngine.RectTransform>();
            if (null == m_RectTransform) return;
            // end if
            m_beginAnchorPos = m_RectTransform.anchoredPosition;
        }

        protected override Tween DOPlay() {
            if (null == m_RectTransform) return null;
            // end if
            switch (m_posType) {
                case PosType.Pos:
                    return m_RectTransform.DOAnchorPos(m_toAnchorPos, m_duration, m_isSnapping);
                case PosType.PosX:
                    return m_RectTransform.DOAnchorPosX(m_toAnchorPosX, m_duration, m_isSnapping);
                case PosType.PosY:
                    return m_RectTransform.DOAnchorPosY(m_toAnchorPosY, m_duration, m_isSnapping);
                default: return null;
            } // end switch
        }

        public override void Restore() {
            if (null == m_RectTransform) return;
            // end if
            m_RectTransform.anchoredPosition = m_beginAnchorPos;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("pos")) {
                m_posType = PosType.Pos;
                m_toAnchorPos = JTweenUtils.JsonToVector2(json["pos"]);
            } else if (json.Contains("posX")) {
                m_posType = PosType.PosX;
                m_toAnchorPosX = (float)json["posX"];
            } else if (json.Contains("posY")) {
                m_posType = PosType.PosY;
                m_toAnchorPosY = (float)json["posY"];
            } else {
                Debug.LogError(GetType().FullName + " JsonTo PosType is null");
            } // end if
        }

        protected override void ToJson(ref JsonData json) {
            switch (m_posType) {
                case PosType.Pos:
                    json["pos"] = JTweenUtils.Vector2Json(m_toAnchorPos);
                    break;
                case PosType.PosX:
                    json["posX"] = m_toAnchorPosX;
                    break;
                case PosType.PosY:
                    json["posY"] = m_toAnchorPosY;
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
