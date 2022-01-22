using Json;
using DG.Tweening;
using UnityEngine;

namespace JTween.Camera {
    public class JTweenCameraShakePosition : JTweenBase {
        public enum ShakeTypeEnum {
            Value = 0,
            Axis = 1,
        }
        private float m_strength = 0;
        private Vector3 m_strengthVec = Vector3.zero;
        private int m_vibrato = 0;
        private float m_randomness = 0;
        private bool m_fadeOut = false;
        private Vector3 m_begainPosition = Vector3.zero;
        private ShakeTypeEnum m_shakeType = ShakeTypeEnum.Value;
        private UnityEngine.Camera m_Camera;

        public JTweenCameraShakePosition() {
            m_tweenType = (int)JTweenCamera.ShakePosition;
            m_tweenElement = JTweenElement.Camera;
        }

        public ShakeTypeEnum ShakeType {
            get {
                return m_shakeType;
            }
            set {
                m_shakeType = value;
            }
        }

        public float Strength {
            get {
                return m_strength;
            }
            set {
                m_strength = value;
            }
        }

        public Vector3 StrengthVec {
            get {
                return m_strengthVec;
            }
            set {
                m_strengthVec = value;
            }
        }

        public float Randomness {
            get {
                return m_randomness;
            }
            set {
                m_randomness = value;
            }
        }

        public int Vibrato {
            get {
                return m_vibrato;
            }
            set {
                m_vibrato = value;
            }
        }

        public bool FadeOut {
            get {
                return m_fadeOut;
            }
            set {
                m_fadeOut = value;
            }
        }

        public Vector3 BegainPosition {
            get {
                return m_begainPosition;
            }
            set {
                m_begainPosition = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Camera = m_target.GetComponent<UnityEngine.Camera>();
            if (null == m_Camera) return;
            // end if
            m_begainPosition = m_target.position;
        }

        protected override Tween DOPlay() {
            if (null == m_Camera) return null;
            // end if
            if (m_strengthVec == null || m_strengthVec == Vector3.zero) {
                return m_Camera.DOShakePosition(m_duration, m_strength, m_vibrato, m_randomness, m_fadeOut);
            } // end if
            return m_Camera.DOShakePosition(m_duration, m_strengthVec, m_vibrato, m_randomness, m_fadeOut);
        }

        public override void Restore() {
            if (null == m_Camera) return;
            // end if
            m_target.position = m_begainPosition;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("strength")) {
                m_shakeType = ShakeTypeEnum.Value;
                m_strength = json.GetFloat("strength");
            } // end if
            if (json.Contains("strengthVec")) {
                m_shakeType = ShakeTypeEnum.Axis;
                m_strengthVec = JTweenUtils.JsonToVector3(json.GetNode("strengthVec"));
            } // end if
            if (json.Contains("vibrato")) m_vibrato = json.GetInt("vibrato");
            // end if
            if (json.Contains("randomness")) m_randomness = json.GetFloat("randomness");
            // end if
            if (json.Contains("fadeOut")) m_fadeOut = json.GetBool("fadeOut"); 
            // end if
            if (json.Contains("begainPosition")) BegainPosition = JTweenUtils.JsonToVector3(json.GetNode("begainPosition"));
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            switch (m_shakeType) {
                case ShakeTypeEnum.Value:
                    json.SetFloat("strength", m_strength);
                    break;
                case ShakeTypeEnum.Axis:
                    json.SetNode("strengthVec", JTweenUtils.Vector3Json(m_strengthVec));
                    break;
                default:
                    Debug.LogError(GetType().FullName + " ToJson ShakeType is null");
                    break;
            } // end swtich
            json.SetInt("vibrato", m_vibrato);
            json.SetFloat("randomness", m_randomness);
            json.SetBool("fadeOut", m_fadeOut);
            if (m_begainPosition != null) {
                json.SetNode("begainPosition", JTweenUtils.Vector3Json(m_begainPosition));
            } // end if
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Camera) {
                errorInfo = GetType().FullName + " GetComponent<Camera> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}