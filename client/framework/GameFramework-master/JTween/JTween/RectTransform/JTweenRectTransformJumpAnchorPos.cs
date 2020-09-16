using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.RectTransform {
    public class JTweenRectTransformJumpAnchorPos : JTweenBase {
        private Vector2 m_beginAnchorPos = Vector2.zero;
        private Vector2 m_toAnchorPos = Vector2.zero;
        private float m_jumpPower = 0;
        private int m_numJumps = 0;
        private UnityEngine.RectTransform m_RectTransform;

        public JTweenRectTransformJumpAnchorPos() {
            m_tweenType = (int)JTweenRectTransform.JumpAnchorPos;
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

        public Vector2 ToAnchorPos {
            get {
                return m_toAnchorPos;
            }
            set {
                m_toAnchorPos = value;
            }
        }

        public float JumpPower {
            get {
                return m_jumpPower;
            }
            set {
                m_jumpPower = value;
            }
        }

        public int NumJumps {
            get {
                return m_numJumps;
            }
            set {
                m_numJumps = value;
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
            return m_RectTransform.DOJumpAnchorPos(m_toAnchorPos, m_jumpPower, m_numJumps, m_duration, m_isSnapping);
        }

        public override void Restore() {
            if (null == m_RectTransform) return;
            // end if
            m_RectTransform.anchoredPosition = m_beginAnchorPos;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginAnchorPos")) BeginAnchorPos = JTweenUtils.JsonToVector2(json["beginAnchorPos"]);
            // end if
            if (json.Contains("anchorPos")) m_toAnchorPos = JTweenUtils.JsonToVector2(json["anchorPos"]);
            // end if
            if (json.Contains("jumpPower")) m_jumpPower = json["jumpPower"].ToFloat();
            // end if
            if (json.Contains("numJumps")) m_jumpPower = json["numJumps"].ToInt32();
            // end if
            Restore();
        }

        protected override void ToJson(ref JsonData json) {
            json["beginAnchorPos"] = JTweenUtils.Vector2Json(m_beginAnchorPos);
            json["anchorPos"] = JTweenUtils.Vector2Json(m_toAnchorPos);
            json["jumpPower"] = m_jumpPower;
            json["numJumps"] = m_jumpPower;
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
