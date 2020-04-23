using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JTween;
using JTween.AudioSource;
using LitJson;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor;

namespace JTween.Editor {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(JTweenSequence), true)]
    public class JTweenSequenceInspector : GameFrameworkInspector {
        public static readonly string TWEENER_PATH = "Assets/Resources/Dev/Tween";
        public static string GetText(string path) {
            TextAsset textAsset = Resources.Load<TextAsset>(path);
            if (textAsset == null) {
                Debug.LogErrorFormat("GetText Resources.Load<TextAsset> is NULL, path={0}" + path);
                return null;
            } // end if
            return textAsset.text;
        }
        /// <summary>
        /// 检测绑定的物品
        /// </summary>
        /// <returns></returns>
        public static bool CheckBindTarget(JTweenSequence tween, JTweenBase tweenBase, GameObject target) {
            if (!JTweenUtils.GetTranPath(target.transform).StartsWith(JTweenUtils.GetTranPath(tween.transform))) {
                Debug.LogError("必须是自身节点或者子节点!!!");
                return false;
            } // end if
            switch (tweenBase.TweenElement) {
                case JTweenElement.AudioSource:
                    return target.GetComponent<UnityEngine.AudioSource>() != null;
                case JTweenElement.Camera:
                    return target.GetComponent<UnityEngine.Camera>() != null;
                case JTweenElement.Light:
                    return target.GetComponent<UnityEngine.Light>() != null;
                case JTweenElement.LineRenderer:
                    return target.GetComponent<UnityEngine.LineRenderer>() != null;
                case JTweenElement.Material:
                    var renderer = target.GetComponent<UnityEngine.Renderer>();
                    return renderer != null && renderer.material != null;
                case JTweenElement.Rigidbody:
                    return target.GetComponent<UnityEngine.Rigidbody>() != null;
                case JTweenElement.Rigidbody2D:
                    return target.GetComponent<UnityEngine.Rigidbody2D>() != null;
                case JTweenElement.SpriteRenderer:
                    return target.GetComponent<UnityEngine.SpriteRenderer>() != null;
                case JTweenElement.TrailRenderer:
                    return target.GetComponent<UnityEngine.TrailRenderer>() != null;
                case JTweenElement.Transform:
                    return target.GetComponent<UnityEngine.Transform>() != null;
                case JTweenElement.CanvasGroup:
                    return target.GetComponent<UnityEngine.CanvasGroup>() != null;
                case JTweenElement.Graphic:
                    return target.GetComponent<UnityEngine.UI.Graphic>() != null;
                case JTweenElement.Image:
                    return target.GetComponent<UnityEngine.UI.Image>() != null;
                case JTweenElement.LayoutElement:
                    return target.GetComponent<UnityEngine.UI.LayoutElement>() != null;
                case JTweenElement.Outline:
                    return target.GetComponent<UnityEngine.UI.Outline>() != null;
                case JTweenElement.RectTransform:
                    return target.GetComponent<UnityEngine.RectTransform>() != null;
                case JTweenElement.ScrollRect:
                    return target.GetComponent<UnityEngine.UI.ScrollRect>() != null;
                case JTweenElement.Slider:
                    return target.GetComponent<UnityEngine.UI.Slider>() != null;
                case JTweenElement.Text:
                    return target.GetComponent<UnityEngine.UI.Text>() != null;
            } // end switch
            return false;
        }
        private string m_CurrentTweenName;
        /// <summary>
        /// 绘制Inspector
        /// </summary>
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            serializedObject.Update();
            JTweenSequence tween = target as JTweenSequence;
            GUILayout.BeginVertical("box"); {
                EditorGUILayout.LabelField("动画编辑", EditorStyles.boldLabel);
                DirectoryInfo info = new DirectoryInfo(TWEENER_PATH);
                if (info != null) {
                    FileInfo[] files = info.GetFiles("*.json");
                    List<string> fileNameList = new List<string>();
                    if (files != null && files.Length > 0) {
                        foreach (FileInfo f in files) {
                            string fileName = Path.GetFileNameWithoutExtension(f.Name);
                            fileNameList.Add(fileName);
                        } // end foreach
                    } // end if
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("加载动画")) {
                        PopupWindow.Show(GUILayoutUtility.GetLastRect(), new PopupTip(fileNameList.ToArray(), (index) => {
                            if (index < 0 || index >= fileNameList.Count) return;
                            // end if
                            string fileName = fileNameList[index];
                            if (!string.IsNullOrEmpty(fileName)) {
                                if (EditorUtility.DisplayDialog("警告", "确定加载名字为 " + fileName + " 的动画?", "确定", "取消")) {
                                    string jsonStr = GetText("Dev/Tween/" + fileName);
                                    if (jsonStr != null) {
                                        tween.JsonDo(JsonMapper.ToObject(jsonStr));
                                    } // end if
                                } // end if
                            } // end if
                            m_CurrentTweenName = fileName;
                        }));
                    } // end if
                    GUILayout.EndHorizontal();
                    List<JTweenBase> tweenList = new List<JTweenBase>();
                    if (tween.Tweens != null && tween.Tweens.Length > 0) {
                        tweenList.AddRange(tween.Tweens);
                        for (int i = 0; i < tweenList.Count; ++i) {
                            int index = i;
                            DrawTweenBase(tween, tweenList[i], () => {
                                if (EditorUtility.DisplayDialog("警告", "确定要删除此动画吗?", "确定", "取消")) {
                                    tweenList.RemoveAt(index);
                                } // end if
                            });
                        }
                    }
                } // end if
            } GUILayout.EndVertical();
        }
        /// <summary>
        /// 绘制动画
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="tweenBase"></param>
        /// <param name="OnDeleteTween"></param>
        private void DrawTweenBase(JTweenSequence tween, JTweenBase tweenBase, Action OnDeleteTween) {
            GUILayout.BeginVertical("Box"); {
                #region Base
                GUILayout.BeginHorizontal(); {
                    Color lastColor = GUI.color;
                    GUI.color = Color.yellow;
                    EditorGUILayout.LabelField("动画类型:" + tweenBase.TweenElement.ToString(), GUILayout.Width(130));
                    GUI.color = lastColor;
                    if (GUILayout.Button("删除此动画")) {
                        if (OnDeleteTween != null) {
                            OnDeleteTween();
                        } // end if
                    } // end if
                } GUILayout.EndHorizontal();
                GameObject target = EditorGUILayout.ObjectField("绑定对象:", tweenBase.Target, typeof(GameObject), true) as GameObject;
                if (target != null && target != tweenBase.Target) {
                    if (CheckBindTarget(tween, tweenBase, target)) {
                        tweenBase.Bind(target.transform);
                    } else {
                        EditorUtility.DisplayDialog("提示", "此对象不支持绑定该动画!!", "知道了");
                    } // end if
                } // end if
                tweenBase.Duration = EditorGUILayout.FloatField("持续时间:", tweenBase.Duration);
                tweenBase.Delay = EditorGUILayout.FloatField("延迟时间:", tweenBase.Delay);
                tweenBase.AnimEase = (DG.Tweening.Ease)EditorGUILayout.EnumPopup("运动类型:", tweenBase.AnimEase);
                AnimationCurve cure = EditorGUILayout.CurveField("运动帧:", tweenBase.AnimCure == null ? new AnimationCurve() : tweenBase.AnimCure);
                if (cure != tweenBase.AnimCure) {
                    if (tweenBase.AnimCure == null) {
                        if (cure != null && cure.keys != null && cure.keys.Length > 0) {
                            tweenBase.AnimCure = cure;
                        } // end if
                    } else {
                        if (cure == null || cure.keys == null || cure.keys.Length == 0) {
                            tweenBase.AnimCure = null;
                        } else {
                            tweenBase.AnimCure = cure;
                        } // end if
                    } // end if
                } // end if
                tweenBase.LoopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup("循环类型:", tweenBase.LoopType);
                tweenBase.LoopCount = EditorGUILayout.IntField("循环次数(-1为无限):", tweenBase.LoopCount);
                tweenBase.IsSnapping = EditorGUILayout.ToggleLeft("使用平稳差值处理", tweenBase.IsSnapping);
                #endregion
                switch (tweenBase.TweenElement) {
                    case JTweenElement.AudioSource:
                        switch ((JTweenAudioSource)tweenBase.TweenType) {
                            case JTweenAudioSource.Fade:
                                JTweenAudioSourceFade fadeTween = tweenBase as JTweenAudioSourceFade;
                                fadeTween.BeginVolume = EditorGUILayout.Vector3Field("初始音量:", fadeTween.BeginVolume);
                                fadeTween.ToVolume = EditorGUILayout.FloatField("目标音量:", fadeTween.ToVolume);
                                break;
                        } // end switch
                    case JTweenElement.Camera:
                        return target.GetComponent<UnityEngine.Camera>() != null;
                    case JTweenElement.Light:
                        return target.GetComponent<UnityEngine.Light>() != null;
                    case JTweenElement.LineRenderer:
                        return target.GetComponent<UnityEngine.LineRenderer>() != null;
                    case JTweenElement.Material:
                        var renderer = target.GetComponent<UnityEngine.Renderer>();
                        return renderer != null && renderer.material != null;
                    case JTweenElement.Rigidbody:
                        return target.GetComponent<UnityEngine.Rigidbody>() != null;
                    case JTweenElement.Rigidbody2D:
                        return target.GetComponent<UnityEngine.Rigidbody2D>() != null;
                    case JTweenElement.SpriteRenderer:
                        return target.GetComponent<UnityEngine.SpriteRenderer>() != null;
                    case JTweenElement.TrailRenderer:
                        return target.GetComponent<UnityEngine.TrailRenderer>() != null;
                    case JTweenElement.Transform:
                        return target.GetComponent<UnityEngine.Transform>() != null;
                    case JTweenElement.CanvasGroup:
                        return target.GetComponent<UnityEngine.CanvasGroup>() != null;
                    case JTweenElement.Graphic:
                        return target.GetComponent<UnityEngine.UI.Graphic>() != null;
                    case JTweenElement.Image:
                        return target.GetComponent<UnityEngine.UI.Image>() != null;
                    case JTweenElement.LayoutElement:
                        return target.GetComponent<UnityEngine.UI.LayoutElement>() != null;
                    case JTweenElement.Outline:
                        return target.GetComponent<UnityEngine.UI.Outline>() != null;
                    case JTweenElement.RectTransform:
                        return target.GetComponent<UnityEngine.RectTransform>() != null;
                    case JTweenElement.ScrollRect:
                        return target.GetComponent<UnityEngine.UI.ScrollRect>() != null;
                    case JTweenElement.Slider:
                        return target.GetComponent<UnityEngine.UI.Slider>() != null;
                    case JTweenElement.Text:
                        return target.GetComponent<UnityEngine.UI.Text>() != null;
                } // end switch
            } GUILayout.EndVertical();
        }
        public class PopupTip : PopupWindowContent {
            int m_index = -1;
            Vector2 m_scrollPos;
            string[] m_selStrings;
            System.Action<int> m_backCall;

            public PopupTip(string[] selectArr, System.Action<int> backCall, int index = -1) {
                m_index = index;
                m_selStrings = selectArr;
                m_backCall = backCall;
            }

            public override Vector2 GetWindowSize() {
                return new Vector2(400, 300);
            }

            public override void OnGUI(Rect rect) {
                EditorGUILayout.BeginVertical();
                GUILayout.Label("选择动画", EditorStyles.boldLabel);
                m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
                m_index = GUILayout.SelectionGrid(m_index, m_selStrings, 1);
                if (m_index >= 0 && m_backCall != null) {
                    m_backCall(m_index);
                } // end if
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }
        }
    }
}
