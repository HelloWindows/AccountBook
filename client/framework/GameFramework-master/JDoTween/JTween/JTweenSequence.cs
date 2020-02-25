using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween {
    public class JTweenSequence : MonoBehaviour {

        private JTweenBase[] m_tweens;
        private TweenCallback m_onComplete;

        public void Init(bool complete = false) {
            KillAll(complete);

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

        public JsonData ToJson() {
            JsonData json = new JsonData();
            if (m_tweens != null && m_tweens.Length > 0) {
                JsonData node;
                foreach (var tween in m_tweens) {
                    node = tween.DoJson();
                    string curPath = Utility.Utils.GetTranPath(transform);
                    if (tween.Target != transform) {
                        node["_PATH"] = Utility.Utils.GetTranPath(tween.Target).Replace(curPath, "");
                    } // end if
                    json.Add(node);
                }
            }
            return json;
        }

        public void JsonTo(JsonData json) {
            Clear();
            KillAll();
            if (json == null || json.Count <= 0) return;
            // end if
            int count = json.Count;
            m_tweens = new JTweenBase[count];
        }
    }
}
