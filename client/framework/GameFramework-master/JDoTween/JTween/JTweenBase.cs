using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using LitJson;

namespace JTween {
    [Serializable]
    [SerializeField]
    public abstract class JTweenBase {
        protected class EaseInfo {
            private Ease m_Ease = Ease.Linear;
            private AnimationCurve m_Curve;

            /// <summary>
            /// 动效类型
            /// </summary>
            public Ease Ease {
                get { return m_Ease; }
                set { m_Ease = value; m_Curve = null; }
            }
            /// <summary>
            /// 动效曲线
            /// </summary>
            public AnimationCurve Cure {
                get { return m_Curve; }
                set { m_Ease = Ease.Linear; m_Curve = value; }
            }
            public void SetToTweener(Tween tweener) {
                if (m_Curve != null && m_Curve.keys != null && m_Curve.keys.Length > 0) {
                    tweener.SetEase(m_Curve);
                } else {
                    tweener.SetEase(m_Ease);
                } // end if
            }
        }
    } // end class JTweenBase 
} // end namespace JTween 
