using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.RectTransform {
    public class JTweenRectTransformPivot : JTweenBase {
        public enum PivotTypeEnum {
            Pivot = 0,
            PivotX = 1,
            PivotY = 2,
        }
        private Vector2 m_beginPivot = Vector2.zero;
        private Vector2 m_toPivot = Vector2.zero;
        private PivotTypeEnum m_pivotType = PivotTypeEnum.Pivot;
        private float m_toPivotX = 0;
        private float m_toPivotY = 0;
        private UnityEngine.RectTransform m_RectTransform;

        public JTweenRectTransformPivot() {
            m_tweenType = (int)JTweenRectTransform.Pivot;
            m_tweenElement = JTweenElement.RectTransform;
        }

        public PivotTypeEnum PivotType {
            get {
                return m_pivotType;
            }
            set {
                m_pivotType = value;
            }
        }

        public Vector2 BeginPivot {
            get {
                return m_beginPivot;
            }
            set {
                m_beginPivot = value;
            }
        }

        public Vector2 ToPivot {
            get {
                return m_toPivot;
            }
            set {
                m_toPivot = value;
            }
        }

        public float ToPivotX {
            get {
                return m_toPivotX;
            }
            set {
                m_toPivotX = value;
            }
        }

        public float ToPivotY {
            get {
                return m_toPivotY;
            }
            set {
                m_toPivotY = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_RectTransform = m_target.GetComponent<UnityEngine.RectTransform>();
            if (null == m_RectTransform) return;
            // end if
            m_beginPivot = m_RectTransform.pivot;
        }

        protected override Tween DOPlay() {
            if (null == m_RectTransform) return null;
            // end if
            switch (m_pivotType) {
                case PivotTypeEnum.Pivot:
                    return m_RectTransform.DOPivot(m_toPivot, m_duration);
                case PivotTypeEnum.PivotX:
                    return m_RectTransform.DOPivotX(m_toPivotX, m_duration);
                case PivotTypeEnum.PivotY:
                    return m_RectTransform.DOPivotY(m_toPivotY, m_duration);
                default: return null;
            } // end switch
        }

        public override void Restore() {
            if (null == m_RectTransform) return;
            // end if
            m_RectTransform.pivot = m_beginPivot;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginPivot")) BeginPivot = JTweenUtils.JsonToVector2(json["beginPivot"]);
            // end if
            if (json.Contains("pivot")) {
                m_pivotType = PivotTypeEnum.Pivot;
                m_toPivot = JTweenUtils.JsonToVector2(json["pivot"]);
            } else if (json.Contains("pivotX")) {
                m_pivotType = PivotTypeEnum.PivotX;
                m_toPivotX = (float)json["pivotX"];
            } else if (json.Contains("pivotY")) {
                m_pivotType = PivotTypeEnum.PivotY;
                m_toPivotY = (float)json["pivotY"];
            } else {
                Debug.LogError(GetType().FullName + " JsonTo PivotType is null");
            } // end if
            Restore();
        }

        protected override void ToJson(ref JsonData json) {
            json["beginPivot"] = JTweenUtils.Vector2Json(m_beginPivot);
            switch (m_pivotType) {
                case PivotTypeEnum.Pivot:
                    json["pivot"] = JTweenUtils.Vector2Json(m_toPivot);
                    break;
                case PivotTypeEnum.PivotX:
                    json["pivotX"] = m_toPivotX;
                    break;
                case PivotTypeEnum.PivotY:
                    json["pivotY"] = m_toPivotY;
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
