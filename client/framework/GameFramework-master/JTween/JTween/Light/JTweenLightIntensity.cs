﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Light {
    public class JTweenLightIntensity : JTweenBase {
        private float m_beginIntensity = 0;
        private float m_toIntensity = 0;
        private UnityEngine.Light m_Light;

        public JTweenLightIntensity() {
            m_tweenType = (int)JTweenLight.Intensity;
            m_tweenElement = JTweenElement.Light;
        }

        public float BeginIntensity {
            get {
                return m_beginIntensity;
            }
            set {
                m_beginIntensity = value;
            }
        } 

        public float ToIntensity {
            get {
                return m_toIntensity;
            }
            set {
                m_toIntensity = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Light = m_target.GetComponent<UnityEngine.Light>();
            if (null == m_Light) return;
            // end if
            m_beginIntensity = m_Light.intensity;
        }

        protected override Tween DOPlay() {
            if (null == m_Light) return null;
            // end if
            return m_Light.DOIntensity(m_toIntensity, m_duration);
        }

        public override void Restore() {
            if (null == m_Light) return;
            // end if
            m_Light.intensity = m_beginIntensity;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginIntensity")) BeginIntensity = (float)json["beginIntensity"];
            // end if
            if (json.Contains("intensity")) m_toIntensity = (float)json["intensity"];
            // end if
            Restore();
        }

        protected override void ToJson(ref JsonData json) {
            json["beginIntensity"] = m_beginIntensity;
            json["intensity"] = m_toIntensity;
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Light) {
                errorInfo = GetType().FullName + " GetComponent<Light> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
