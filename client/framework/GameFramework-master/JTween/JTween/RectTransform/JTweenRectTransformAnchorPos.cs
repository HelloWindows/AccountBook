using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.RectTransform {
    public class JTweenRectTransformAnchorPos : JTweenBase {
        public enum PosTypeEnum {
            Pos = 0,
            PosX = 1,
            PosY = 2,
        }
        private Vector2 m_beginAnchorPos = Vector2.zero;
        private Vector2 m_toAnchorPos = Vector2.zero;
        private PosTypeEnum m_posType = PosTypeEnum.Pos;
        private float m_toAnchorPosX = 0;
        private float m_toAnchorPosY = 0;
        private UnityEngine.RectTransform m_RectTransform;

        public JTweenRectTransformAnchorPos() {
            m_tweenType = (int)JTweenRectTransform.AnchorPos;
            m_tweenElement = JTweenElement.RectTransform;
        }

        public PosTypeEnum PosType {
            get {
                return m_posType;
            }
            set {
                m_posType = value;
            }
        }

        public Vector2 BeginAnchorPos {
            get {
                return m_beginAnchorPos;
            }
            set {
                m_beginAnchorPos = value;
            }
        }

        public Vector2 ToAnchorPos {
            get {
                return m_toAnchorPos;
            }
            set {
                m_toAnchorPos = value;
            }
        }

        public float ToAnchorPosX {
            get {
                return m_toAnchorPosX;
            }
            set {
                m_toAnchorPosX = value;
            }
        }

        public float ToAnchorPosY {
            get {
                return m_toAnchorPosY;
            }
            set {
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
                case PosTypeEnum.Pos:
                    return m_RectTransform.DOAnchorPos(m_toAnchorPos, m_duration, m_isSnapping);
                case PosTypeEnum.PosX:
                    return m_RectTransform.DOAnchorPosX(m_toAnchorPosX, m_duration, m_isSnapping);
                case PosTypeEnum.PosY:
                    return m_RectTransform.DOAnchorPosY(m_toAnchorPosY, m_duration, m_isSnapping);
                default: return null;
            } // end switch
        }

        public override void Restore() {
            if (null == m_RectTransform) return;
            // end if
            m_RectTransform.anchoredPosition = m_beginAnchorPos;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginAnchorPos")) BeginAnchorPos = JTweenUtils.JsonToVector2(json.GetNode("beginAnchorPos"));
            // end if
            if (json.Contains("pos")) {
                m_posType = PosTypeEnum.Pos;
                m_toAnchorPos = JTweenUtils.JsonToVector2(json.GetNode("pos"));
            } else if (json.Contains("posX")) {
                m_posType = PosTypeEnum.PosX;
                m_toAnchorPosX = json.GetFloat("posX");
            } else if (json.Contains("posY")) {
                m_posType = PosTypeEnum.PosY;
                m_toAnchorPosY = json.GetFloat("posY");
            } else {
                Debug.LogError(GetType().FullName + " JsonTo PosType is null");
            } // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginAnchorPos", JTweenUtils.Vector2Json(m_beginAnchorPos));
            switch (m_posType) {
                case PosTypeEnum.Pos:
                    json.SetNode("pos", JTweenUtils.Vector2Json(m_toAnchorPos));
                    break;
                case PosTypeEnum.PosX:
                    json.SetFloat("posX", m_toAnchorPosX);
                    break;
                case PosTypeEnum.PosY:
                    json.SetFloat("posY", m_toAnchorPosY);
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
