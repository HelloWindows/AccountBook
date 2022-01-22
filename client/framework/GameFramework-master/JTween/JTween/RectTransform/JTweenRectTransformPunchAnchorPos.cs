using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.RectTransform {
    public class JTweenRectTransformPunchAnchorPos : JTweenBase {
        private Vector2 m_beginAnchorPos = Vector2.zero;
        private Vector2 m_punch = Vector2.zero;
        private int m_vibrato = 0;
        private float m_elasticity = 0;
        private UnityEngine.RectTransform m_rectTransform;

        public JTweenRectTransformPunchAnchorPos() {
            m_tweenType = (int)JTweenRectTransform.PunchAnchorPos;
            m_tweenElement = JTweenElement.RectTransform;
        }
        public Vector2 BeginAnchorPos {
            get {
                return m_beginAnchorPos;
            }
            set {
                m_beginAnchorPos = value;
            }
        }
        /// <summary>
        /// The direction and strength of the punch (added to the RectTransform's current position). 
        /// </summary>
        public Vector2 Punch {
            get {
                return m_punch;
            }
            set {
                m_punch = value;
            }
        }
        /// <summary>
        ///  Indicates how much the punch will vibrate. 
        /// </summary>
        public int Vibrato {
            get {
                return m_vibrato;
            }
            set {
                m_vibrato = value;
            }
        }
        /// <summary>
        ///  Represents how much (0 to 1) the vector will go beyond the starting position when bouncing backwards. 1 creates a full      
        ///  oscillation between the punch direction and the opposite direction, while 0 oscillates only between the punch and the start 
        ///  position. 
        /// </summary>
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
            m_rectTransform = m_target.GetComponent<UnityEngine.RectTransform>();
            if (null == m_rectTransform) return;
            // end if
            m_beginAnchorPos = m_rectTransform.anchoredPosition;
        }

        protected override Tween DOPlay() {
            if (null == m_rectTransform) return null;
            // end if
            return m_rectTransform.DOPunchAnchorPos(m_punch, m_duration, m_vibrato, m_elasticity, m_isSnapping);
        }

        public override void Restore() {
            if (null == m_rectTransform) return;
            // end if
            m_rectTransform.anchoredPosition = m_beginAnchorPos;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginAnchorPos")) BeginAnchorPos = JTweenUtils.JsonToVector2(json.GetNode("beginAnchorPos"));
            // end if
            if (json.Contains("punch")) m_punch = JTweenUtils.JsonToVector2(json.GetNode("punch")); 
            // end if
            if (json.Contains("vibrato")) m_vibrato = json.GetInt("vibrato"); 
            // end if
            if (json.Contains("elasticity")) m_elasticity = json.GetFloat("elasticity");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginAnchorPos", JTweenUtils.Vector2Json(m_beginAnchorPos));
            json.SetNode("punch", JTweenUtils.Vector2Json(m_punch));
            json.SetInt("vibrato", m_vibrato);
            json.SetFloat("elasticity", m_elasticity);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_rectTransform) {
                errorInfo = GetType().FullName + " GetComponent<RectTransform> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
