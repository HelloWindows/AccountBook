using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.RectTransform {
    public class JTweenRectTransformShakeAnchorPos : JTweenBase {
        public enum ShakeTypeEnum {
            Value = 0,
            Axis = 1,
        }
        private Vector2 m_beginAnchorPos = Vector2.zero;
        private Vector2 m_strengthAxis = Vector2.zero;
        private ShakeTypeEnum m_shakeType = ShakeTypeEnum.Value;
        private float m_strength = 100;
        private int m_vibrato = 10;
        private float m_randomness = 90;
        private bool m_fadeOut = true;
        private UnityEngine.RectTransform m_rectTransform;

        public JTweenRectTransformShakeAnchorPos() {
            m_tweenType = (int)JTweenRectTransform.ShakeAnchorPos;
            m_tweenElement = JTweenElement.RectTransform;
        }

        public ShakeTypeEnum ShakeType {
            get {
                return m_shakeType;
            }
            set {
                m_shakeType = value;
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

        /// <summary>
        /// The shake strength
        /// </summary>
        public float Strength {
            get {
                return m_strength;
            }
            set {
                m_strength = value;
            }
        }
        /// <summary>
        ///  Using a Vector3 instead of a float lets you choose the strength for each axis. 
        /// </summary>
        public Vector3 StrengthAxis {
            get {
                return m_strengthAxis;
            }
            set {
                m_strengthAxis = value;
            }
        }

        /// <summary>
        /// Indicates how much will the shake vibrate. 
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
        /// Indicates how much the shake will be random (0 to 180 - values higher than 90 kind of suck, so beware). Setting it to 0 wil
        /// shake along a single direction. 
        /// </summary>
        public float Randomness {
            get {
                return m_randomness;
            }
            set {
                m_randomness = value;
            }
        }
        /// <summary>
        ///  (default: true) If TRUE the shake will automatically fadeOut smoothly within the tween's duration, otherwise it will not. 
        /// </summary>
        public bool FadeOut {
            get {
                return m_fadeOut;
            }
            set {
                m_fadeOut = value;
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
            switch (m_shakeType) {
                case ShakeTypeEnum.Value:
                    return m_rectTransform.DOShakeAnchorPos(m_duration, m_strength, m_vibrato, m_randomness, m_isSnapping, m_fadeOut);
                case ShakeTypeEnum.Axis:
                    return m_rectTransform.DOShakeAnchorPos(m_duration, m_strengthAxis, m_vibrato, m_randomness, m_isSnapping, m_fadeOut);
                default: return null;
            } // end switch
        }

        public override void Restore() {
            if (null == m_rectTransform) return;
            // end if
            m_rectTransform.anchoredPosition = m_beginAnchorPos;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginAnchorPos")) BeginAnchorPos = JTweenUtils.JsonToVector2(json["beginAnchorPos"]);
            // end if
            if (json.Contains("strength")) {
                m_shakeType = ShakeTypeEnum.Value;
                m_strength = json["strength"].ToFloat();
            } else if (json.Contains("strengthAxis")) {
                m_shakeType = ShakeTypeEnum.Axis;
                m_strengthAxis = JTweenUtils.JsonToVector2(json["strengthAxis"]);
            } // end if
            if (json.Contains("vibrato")) m_vibrato = json["vibrato"].ToInt32();
            // end if
            if (json.Contains("randomness")) m_randomness = json["randomness"].ToFloat();
            // end if
            if (json.Contains("fadeOut")) m_fadeOut = json["fadeOut"].ToBool();
            // end if
            Restore();
        }

        protected override void ToJson(ref JsonData json) {
            json["beginAnchorPos"] = JTweenUtils.Vector2Json(m_beginAnchorPos);
            switch (m_shakeType) {
                case ShakeTypeEnum.Value:
                    json["strength"] = m_strength;
                    break;
                case ShakeTypeEnum.Axis:
                    json["strengthAxis"] = JTweenUtils.Vector2Json(m_strengthAxis);
                    break;
                default:
                    Debug.LogError(GetType().FullName + " ToJson ShakeType is null");
                    break;
            } // end swtich
            json["vibrato"] = m_vibrato;
            json["randomness"] = m_randomness;
            json["fadeOut"] = m_fadeOut;
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
