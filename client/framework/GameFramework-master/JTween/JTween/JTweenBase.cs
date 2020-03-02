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
        protected int m_tweenType = 0;
        protected JTweenElement m_tweenElement = JTweenElement.None;
        protected string m_name = string.Empty;
        protected float m_duration = 0;
        protected float m_delay = 0;
        protected bool m_isSnapping = false;
        protected Ease m_animEase = Ease.Linear;
        protected AnimationCurve m_animCurve = null;
        protected int m_loopCount = 0;
        protected LoopType m_loopType = LoopType.Restart;
        protected UnityEngine.Transform m_target = null;
        private Tween m_lastPlayTween = null;

        /// <summary>
        /// 事件名
        /// </summary>
        public string Name {
            get {
                return m_name;
            }
            set {
                m_name = value;
            }
        }

        /// <summary>
        /// 持续时间
        /// </summary>
        public float Duration {
            get {
                return m_duration;
            }
            set {
                m_duration = value;
            }
        }
        /// <summary>
        /// 延迟时间
        /// </summary>
        public float Delay {
            get {
                return m_delay;
            }
            set {
                m_delay = value;
            }
        }
        /// <summary>
        /// 结果取整
        /// </summary>
        public bool IsSnapping {
            get {
                return m_isSnapping;
            }
            set {
                m_isSnapping = value;
            }
        }
        /// <summary>
        /// 动效方式
        /// </summary>
        public Ease AnimEase {
            get {
                return m_animEase;
            }
            set {
                m_animEase = value;
            }
        }
        /// <summary>
        /// 动效曲线
        /// </summary>
        public AnimationCurve AnimCure {
            get {
                return m_animCurve;
            }
            set {
                m_animCurve = value;
            }
        }
        /// <summary>
        /// 循环次数
        /// </summary>
        public int LoopCount {
            get {
                return m_loopCount;
            }
            set {
                m_loopCount = value;
            }
        }
        /// <summary>
        /// 循环类型
        /// </summary>
        public LoopType LoopType {
            get {
                return m_loopType;
            }
            set {
                m_loopType = value;
            }
        }
        /// <summary>
        /// 上一个动效
        /// </summary>
        public Tween LastTween {
            get {
                return m_lastPlayTween;
            }
        }
        /// <summary>
        /// 动效实体
        /// </summary>
        public UnityEngine.Transform Target {
            get {
                return m_target;
            }
        }
        /// <summary>
        /// 动效元素
        /// </summary>
        public JTweenElement TweenElement {
            get {
                return m_tweenElement;
            }
        }
        /// <summary>
        /// 动效类型
        /// </summary>
        public int TweenType {
            get {
                return m_tweenType;
            }
            set {
                m_tweenType = value;
            }
        }
        /// <summary>
        /// 绑定实体
        /// </summary>
        /// <param name="tran"></param>
        public void Bind(UnityEngine.Transform tran) {
            m_target = tran;
            Init();
        }
        /// <summary>
        /// 播放动效
        /// </summary>
        /// <param name="_onComplete"> 动效完成回调 </param>
        /// <returns></returns>
        public Tween Play(TweenCallback _onComplete = null) {
            if (m_target == null) {
                Debug.LogError("must Binding tran first!!!");
                return null;
            } // end if
            m_lastPlayTween = DOPlay();
            if (m_lastPlayTween != null) {
                if (m_delay > 0) m_lastPlayTween.SetDelay(m_delay);
                // end if
                if (m_animCurve == null) {
                    m_lastPlayTween.SetEase(m_animEase);
                } else {
                    m_lastPlayTween.SetEase(m_animCurve);
                } // end if
                if (m_loopCount != 0) {
                    m_lastPlayTween.SetLoops(m_loopCount, m_loopType);
                } // end if
                if (_onComplete != null) m_lastPlayTween.OnComplete(_onComplete);
                // end if
            }
            return m_lastPlayTween;
        }
        /// <summary>
        /// 动效参数是否有效
        /// </summary>
        /// <param name="errorInfo"> 错误信息 </param>
        /// <returns></returns>
        public bool IsValid(out string errorInfo) {
            if (m_target == null) {
                errorInfo = "target is Null!!";
                return false;
            } // end if
            if (JTweenUtils.IsEqual(m_duration, 0)) {
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
            if (m_lastPlayTween != null) m_lastPlayTween.Kill(complete);
            // end if
            OnKill();
        }
        /// <summary>
        /// 转成Json
        /// </summary>
        public JsonData DoJson() {
            JsonData json = new JsonData();
            if (m_tweenType != 0) json["tweenType"] = m_tweenType;
            // end if
            json["tweenElement"] = (int)m_tweenElement;
            if (!string.IsNullOrEmpty(m_name)) json["name"] = m_name;
            // end if
            json["duration"] = Math.Round(m_duration, 4);
            if (m_delay > 0.01f) json["delay"] = Math.Round(m_delay, 4);
            // end if
            json["snapping"] = m_isSnapping;
            if (m_animCurve != null && m_animCurve.keys != null && m_animCurve.keys.Length > 0) {
                json["animCurve"] = JTweenUtils.AnimationCurveJson(m_animCurve);
            } else {
                json["animEase"] = (int)m_animEase;
            } // end if
            if (m_loopCount > 0) {
                json["loopCount"] = m_loopCount;
                json["loopType"] = (int)m_loopType;
            } // end if
            ToJson(ref json);
            return json;
        }
        /// <summary>
        /// 加载Json
        /// </summary>
        /// <param name="json"></param>
        public void JsonDo(JsonData json) {
            if (json.Contains("tweenType")) m_tweenType = json["tweenType"].ToInt32();
            // end if
            if (json.Contains("tweenElement")) m_tweenElement = (JTweenElement)json["tweenElement"].ToInt32();
            // end if
            if (json.Contains("name")) m_name = json["name"].ToString();
            // end if
            if (json.Contains("duration")) m_duration = json["duration"].ToFloat();
            // end if
            if (json.Contains("snapping")) m_isSnapping = json["snapping"].ToBool();
            // end if
            if (json.Contains("animCurve")) m_animCurve = JTweenUtils.JsonAnimationCurve(json["animCurve"]);
            // end if
            if (json.Contains("animEase")) m_animEase = (Ease)json["animEase"].ToInt32();
            // end if
            if (json.Contains("loopCount")) m_loopCount = json["loopCount"].ToInt32();
            // end if
            if (json.Contains("loopType")) m_loopType = (LoopType)json["loopType"].ToInt32();
            // end if
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
        protected abstract void Init();
        /// <summary>
        /// 还原
        /// </summary>
        public abstract void Restore();
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
