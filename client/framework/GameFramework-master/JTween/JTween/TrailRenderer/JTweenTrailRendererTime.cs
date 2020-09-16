using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.TrailRenderer {
    public class JTweenTrailRendererTime : JTweenBase {
        private float m_beginTime = 0;
        private float m_toTime = 0;
        private UnityEngine.TrailRenderer m_TrailRenderer;

        public JTweenTrailRendererTime() {
            m_tweenType = (int)JTweenTrailRenderer.Time;
            m_tweenElement = JTweenElement.TrailRenderer;
        }

        public float BeginTime {
            get {
                return m_beginTime;
            }
            set {
                m_beginTime = value;
            }
        }

        public float ToTime {
            get {
                return m_toTime;
            }
            set {
                m_toTime = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_TrailRenderer = m_target.GetComponent<UnityEngine.TrailRenderer>();
            if (null == m_TrailRenderer) return;
            // end if
            m_beginTime = m_TrailRenderer.time;
        }

        protected override Tween DOPlay() {
            if (null == m_TrailRenderer) return null;
            // end if
            return m_TrailRenderer.DOTime(m_toTime, m_duration);
        }

        public override void Restore() {
            if (null == m_TrailRenderer) return;
            // end if
            m_TrailRenderer.time = m_beginTime;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginTime")) BeginTime = (float)json["beginTime"];
            // end if
            if (json.Contains("time")) m_toTime = (float)json["time"];
            // end if
            Restore();
        }

        protected override void ToJson(ref JsonData json) {
            json["beginTime"] = m_beginTime;
            json["time"] = m_toTime;
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_TrailRenderer) {
                errorInfo = GetType().FullName + " GetComponent<TrailRenderer> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
