using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Transform {
    public class JTweenTransformLocalPath : JTweenBase {
        private Vector3 m_beginPosition = Vector3.zero;
        private Vector3[] m_toPath;
        private PathType m_pathType = PathType.Linear;
        private PathMode m_pathMode = PathMode.Full3D;
        private int m_resolution = 10;
        private UnityEngine.Transform m_Transform;

        public JTweenTransformLocalPath() {
            m_tweenType = (int)JTweenTransform.LocalPath;
            m_tweenElement = JTweenElement.Transform;
        }

        public Vector3[] ToPath { get { return m_toPath; } set { m_toPath = value; } }
        public PathType PathType { get { return m_pathType; } set { m_pathType = value; } }
        public PathMode PathMode { get { return m_pathMode; } set { m_pathMode = value; } }
        public int Resolution { get { return m_resolution; } set { m_resolution = value; } }

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
            if (m_toPath == null || m_toPath.Length <= 0) return null;
            // end if
            return ShortcutExtensions.DOLocalPath(m_target, m_toPath, m_duration, m_pathType, m_pathMode, m_resolution);
        }

        public override void Restore() {
            if (null == m_Transform) return;
            // end if
            m_Transform.position = m_beginPosition;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("path")) {
                JsonData pathJson = json["path"];
                m_toPath = new Vector3[pathJson.Count];
                for (int i = 0, imax = pathJson.Count; i < imax; ++i) {
                    m_toPath[i] = JTweenUtils.JsonToVector3(pathJson[i]);
                } // end for
            } // end if
            if (json.Contains("type")) m_pathType = (PathType)json["type"].ToInt32();
            // end if
            if (json.Contains("mode")) m_pathMode = (PathMode)json["mode"].ToInt32();
            // end if
            if (json.Contains("resolution")) m_resolution = json["resolution"].ToInt32();
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            if (m_toPath == null || m_toPath.Length <= 0) return;
            JsonData pathJson = new JsonData();
            for (int i = 0; i < m_toPath.Length; ++i) {
                pathJson.Add(JTweenUtils.Vector3Json(m_toPath[i]));
            } // end for
            json["path"] = pathJson;
            json["type"] = (int)m_pathType;
            json["mode"] = (int)m_pathMode;
            json["resolution"] = (int)m_resolution;
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Transform) {
                errorInfo = GetType().FullName + " GetComponent<Transform> is null";
                return false;
            } // end if
            if (m_toPath == null || m_toPath.Length == 0) {
                errorInfo = GetType().FullName + " path point is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
