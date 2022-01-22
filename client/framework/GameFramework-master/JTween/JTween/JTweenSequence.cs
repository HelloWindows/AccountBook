using System.Collections.Generic;
using DG.Tweening;
using Json;
using UnityEngine;

namespace JTween {
    [AddComponentMenu("JTween/JTweenSequence")]
    public class JTweenSequence : MonoBehaviour {

        private JTweenBase[] m_tweens;
        private TweenCallback m_onComplete;

        public JTweenBase[] Tweens {
            get { return m_tweens; }
            set { m_tweens = value; }
        }

        public void Init(bool complete = false) {
            if (m_tweens == null) {
                Debug.LogErrorFormat("JTweenSequence Init m_tweens is null, Name:{0}", gameObject.name);
                return;
            } // end if
            KillAll(complete);
            if (complete) return;
            // end if
            foreach (var tween in m_tweens) {
                tween.Restore();
            } // end foreach
        } // end Init

        public bool IsValid(out string errorInfo) {
            errorInfo = null;
            if (m_tweens == null || m_tweens.Length == 0) {
                errorInfo = "tweens is empty!!";
                return false;
            } // end if
            foreach (var tween in m_tweens) {
                if (!tween.IsValid(out errorInfo)) return false;
                // end if
            } // end foreach
            return true;
        }

        public JTweenBase[] GetTweensForName(string name) {
            if (m_tweens == null || m_tweens.Length == 0) return null;
            // end if
            List<JTweenBase> list = new List<JTweenBase>();
            foreach (var tween in m_tweens) {
                if (!name.Equals(tween.Name)) continue;
                // end if
                list.Add(tween);
            } // end foreach
            return list.ToArray();
        }

        public void SetOnComplete(TweenCallback onComplete) {
            m_onComplete = onComplete;
        }

        public void Play() {
            if (m_tweens == null || m_tweens.Length == 0) return;
            // end if
            if (m_onComplete != null) {
                float lastTime = 0;
                Tween lastTween = null;
                foreach (var tween in m_tweens) {
                    float time = tween.Duration + tween.Delay;
                    if (time > lastTime) {
                        lastTime = time;
                        lastTween = tween.Play().SetTarget(transform);
                    } else {
                        tween.Play().SetTarget(transform);
                    } // end if
                } // end foreach
                if (lastTween != null) lastTween.OnComplete(m_onComplete);
                // end if
            } else {
                foreach (var tween in m_tweens) {
                    tween.Play().SetTarget(transform);
                } // end foreach
            } // end if
        }

        public void KillAll(bool complete = false) {
            if (m_tweens == null || m_tweens.Length == 0) return;
            // end if
            foreach (var tween in m_tweens) {
                tween.Kill(complete);
            } // end foreach
        }

        public void Clear() {
            m_tweens = null;
            m_onComplete = null;
        }

        public IJsonNode DoJson() {
            IJsonNode json = JsonHelper.CreateNode();
            if (m_tweens != null && m_tweens.Length > 0) {
                IJsonNode node;
                for (int i = 0; i < m_tweens.Length; i++)
                {
                    JTweenBase tween = m_tweens[i];
                    node = tween.DoJson();
                    string curPath = JTweenUtils.GetTranPath(transform) + "/";
                    if (tween.Target != transform)
                    {
                        string targetPath = JTweenUtils.GetTranPath(tween.Target);
                        if (!targetPath.StartsWith(curPath))
                        {
                            Debug.LogErrorFormat("JTweenSequence DoJson target is not child! Path:{0}", targetPath);
                            continue;
                        } // end if
                        node.SetString("_PATH", JTweenUtils.GetTranPath(tween.Target).Replace(curPath, ""));
                    } // end if
                    json.Add(node);
                }
            }
            return json;
        }

        public void JsonDo(IJsonNode json) {
            Clear();
            KillAll();
            if (json == null || json.Count <= 0) return;
            // end if
            int count = json.Count;
            m_tweens = new JTweenBase[count];
            IJsonNode node;
            JTweenBase tween;
            string path;
            UnityEngine.Transform trans;
            Dictionary<string, UnityEngine.Transform> pathToTrans = new Dictionary<string, UnityEngine.Transform>();
            for (int i = 0; i < count; ++i) {
                node = json[i];
                tween = JTweenFactory.CreateTween(node);
                m_tweens[i] = tween;
                if (node.Contains("_PATH")) {
                    path = node.GetString("_PATH");
                    if (!pathToTrans.TryGetValue(path, out trans)) {
                        trans = transform.Find(path);
                        if (null != trans) {
                            pathToTrans.Add(path, trans);
                        } else {
                            Debug.LogErrorFormat("JTweenSequence con't find, Name:{0}, Path:{1}", gameObject.name, path);
                        } // end if
                    } // end if
                    tween.Bind(trans);
                } else {
                    tween.Bind(transform);
                } // end if
                tween.JsonDo(node);
            } // end for
        }
    }
}
