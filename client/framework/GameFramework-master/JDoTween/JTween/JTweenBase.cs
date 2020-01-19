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
        protected float m_Duration = 0;
        protected float m_Delay = 0;
        protected bool m_IsSnapping = false;
        protected string m_WaitEvent = "";
        protected Ease m_AnimEase = Ease.Linear;
        protected AnimationCurve m_AnimCurve = null;
        protected int m_LoopCount = 0;
        protected LoopType m_LoopType = LoopType.Restart;
        protected Transform m_Target = null;
        protected int m_TweenType = 0;
        protected JTweenElement m_TweenElement = JTweenElement.None;
        private Tween m_LastPlayTween = null;

        /// <summary>
        /// 持续时间
        /// </summary>
        public float Duration {
            get {
                return m_Duration;
            }
            set {
                m_Duration = value;
            }
        }
        /// <summary>
        /// 延迟时间
        /// </summary>
        public float Delay {
            get {
                return m_Delay;
            }
            set {
                m_Delay = value;
            }
        }
        /// <summary>
        /// 结果取整
        /// </summary>
        public bool IsSnapping {
            get {
                return m_IsSnapping;
            }
            set {
                m_IsSnapping = value;
            }
        }
        /// <summary>
        /// 事件名
        /// </summary>
        public string WaitEvent {
            get {
                return m_WaitEvent;
            }
            set {
                m_WaitEvent = value;
            }
        }
        /// <summary>
        /// 动效方式
        /// </summary>
        public Ease AnimEase {
            get {
                return m_AnimEase;
            }
            set {
                m_AnimEase = value;
            }
        }
        /// <summary>
        /// 动效曲线
        /// </summary>
        public AnimationCurve AnimCure {
            get {
                return m_AnimCurve;
            }
            set {
                m_AnimCurve = value;
            }
        }
        /// <summary>
        /// 循环次数
        /// </summary>
        public int LoopCount {
            get {
                return m_LoopCount;
            }
            set {
                m_LoopCount = value;
            }
        }
        /// <summary>
        /// 循环类型
        /// </summary>
        public LoopType LoopType {
            get {
                return m_LoopType;
            }
            set {
                m_LoopType = value;
            }
        }
        /// <summary>
        /// 上一个动效
        /// </summary>
        public Tween LastTween {
            get {
                return m_LastPlayTween;
            }
        }
        /// <summary>
        /// 动效实体
        /// </summary>
        public Transform Target {
            get {
                return m_Target;
            }
        }
        /// <summary>
        /// 动效元素
        /// </summary>
        public JTweenElement TweenElement {
            get {
                return m_TweenElement;
            }
            set {
                m_TweenElement = value;
            }
        }
        /// <summary>
        /// 动效类型
        /// </summary>
        public int TweenType {
            get {
                return m_TweenType;
            }
            set {
                m_TweenType = value;
            }
        }
        /// <summary>
        /// 绑定实体
        /// </summary>
        /// <param name="tran"></param>
        public void Bind(Transform tran) {
            m_Target = tran;
            Init();
        }
        /// <summary>
        /// 播放动效
        /// </summary>
        /// <param name="_onComplete"> 动效完成回调 </param>
        /// <returns></returns>
        public Tween Play(TweenCallback _onComplete = null) {
            if (m_Target == null) {
                Debug.LogError("must Binding tran first!!!");
                return null;
            } // end if
            m_LastPlayTween = DOPlay();
            if (m_LastPlayTween != null) {
                if (m_Delay > 0) m_LastPlayTween.SetDelay(m_Delay);
                // end if
                if (m_AnimCurve == null) {
                    m_LastPlayTween.SetEase(m_AnimEase);
                } else {
                    m_LastPlayTween.SetEase(m_AnimCurve);
                } // end if
                if (m_LoopCount != 0) {
                    m_LastPlayTween.SetLoops(m_LoopCount, m_LoopType);
                } // end if
                if (_onComplete != null) m_LastPlayTween.OnComplete(_onComplete);
                // end if
            }
            return m_LastPlayTween;
        }
        /// <summary>
        /// 动效参数是否有效
        /// </summary>
        /// <param name="errorInfo"> 错误信息 </param>
        /// <returns></returns>
        public bool IsValid(out string errorInfo) {
            if (m_Target == null) {
                errorInfo = "target is Null!!";
                return false;
            } // end if
            if (Utility.Utils.IsEqual(m_Duration, 0)) {
                errorInfo = "duration is zero!!";
                return false;
            } // end if
            return CheckValid(out errorInfo);
        }
        /// <summary>
        /// 删除动效
        /// </summary>
        /// <param name="complete"> 是否设置为完成状态 </param>
        public void Kill(bool complete = false) {
            if (m_LastPlayTween != null)
                m_LastPlayTween.Kill(complete);
            OnKill();
        }
        /// <summary>
        /// 转成Json
        /// </summary>
        /// <param name="json"></param>
        protected abstract void ToJson(ref JsonData json);
        /// <summary>
        /// 加载Json
        /// </summary>
        /// <param name="json"></param>
        protected abstract void JsonTo(JsonData json);
        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void Init();
        /// <summary>
        /// 还原
        /// </summary>
        protected abstract void Restore();
        /// <summary>
        /// 播放
        /// </summary>
        /// <returns></returns>
        protected abstract Tween DOPlay();
        /// <summary>
        /// 检测参数是否有效
        /// </summary>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        protected virtual bool CheckValid(out string errorInfo) { errorInfo = null; return true; }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnKill() { }
    } // end class JTweenBase 
} // end namespace JTween 
