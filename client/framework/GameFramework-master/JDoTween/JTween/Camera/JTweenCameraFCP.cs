﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Camera {
    public class JTweenCameraFCP : JTweenBase {
        private float m_beginFCP = 0;
        private float m_toFCP = 0;
        private UnityEngine.Camera m_Camera;

        public float ToFCP {
            get {
                return m_toFCP;
            }
            set {
                m_toFCP = value;
            }
        }

        public override void Init() {
            if (null == m_target) return;
            // end if
            m_Camera = m_target.GetComponent<UnityEngine.Camera>();
            if (null == m_Camera) return;
            // end if
            m_beginFCP = m_Camera.farClipPlane;
        }

        protected override Tween DOPlay() {
            if (null == m_Camera) return null;
            // end if
            return m_Camera.DOFarClipPlane(m_toFCP, m_duration);
        }

        protected override void Restore() {
            if (null == m_Camera) return;
            // end if
            m_Camera.farClipPlane = m_beginFCP;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("FCP")) m_toFCP = (float)json["FCP"];
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["FCP"] = m_toFCP;
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