using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JTween;
using JTween.AudioSource;
using JTween.Camera;
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
        private JTweenElement m_TweenElement;
        private string m_CurrentTweenName = string.Empty;
        private string m_TweenName;
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
                                    tween.Tweens = tweenList.ToArray();
                                } // end if
                            });
                        } // end for
                    } // end if
                    m_TweenElement = (JTweenElement)EditorGUILayout.EnumPopup("增加新的动画元素:", m_TweenElement);
                    if (m_TweenElement != JTweenElement.None) {
                        int tweenType = 0;
                        switch (m_TweenElement) {
                            case JTweenElement.AudioSource:
                                tweenType = (int)(JTweenAudioSource)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenAudioSource.None);
                                break;
                            case JTweenElement.Camera:
                                tweenType = (int)(JTweenCamera)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenCamera.None);
                                break;
                            case JTweenElement.Light:
                                tweenType = (int)(JTweenLight)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenLight.None);
                                break;
                            case JTweenElement.LineRenderer:
                                tweenType = (int)(JTweenLineRenderer)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenLineRenderer.None);
                                break;
                            case JTweenElement.Material:
                                tweenType = (int)(JTweenMaterial)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenMaterial.None);
                                break;
                            case JTweenElement.Rigidbody:
                                tweenType = (int)(JTweenRigidbody)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenRigidbody.None);
                                break;
                            case JTweenElement.Rigidbody2D:
                                tweenType = (int)(JTweenRigidbody2D)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenRigidbody2D.None);
                                break;
                            case JTweenElement.SpriteRenderer:
                                tweenType = (int)(JTweenSpriteRenderer)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenSpriteRenderer.None);
                                break;
                            case JTweenElement.TrailRenderer:
                                tweenType = (int)(JTweenTrailRenderer)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenTrailRenderer.None);
                                break;
                            case JTweenElement.Transform:
                                tweenType = (int)(JTweenTransform)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenTransform.None);
                                break;
                            case JTweenElement.CanvasGroup:
                                tweenType = (int)(JTweenCanvasGroup)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenCanvasGroup.None);
                                break;
                            case JTweenElement.Graphic:
                                tweenType = (int)(JTweenGraphic)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenGraphic.None);
                                break;
                            case JTweenElement.Image:
                                tweenType = (int)(JTweenImage)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenImage.None);
                                break;
                            case JTweenElement.LayoutElement:
                                tweenType = (int)(JTweenLayoutElement)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenLayoutElement.None);
                                break;
                            case JTweenElement.Outline:
                                tweenType = (int)(JTweenOutline)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenOutline.None);
                                break;
                            case JTweenElement.RectTransform:
                                tweenType = (int)(JTweenRectTransform)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenRectTransform.None);
                                break;
                            case JTweenElement.ScrollRect:
                                tweenType = (int)(JTweenScrollRect)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenScrollRect.None);
                                break;
                            case JTweenElement.Slider:
                                tweenType = (int)(JTweenSlider)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenSlider.None);
                                break;
                            case JTweenElement.Text:
                                tweenType = (int)(JTweenText)EditorGUILayout.EnumPopup("增加新的动画类型:", JTweenText.None);
                                break;
                        } // end swtich
                        if (tweenType != 0) {
                            JTweenBase tweenBase = JTweenFactory.CreateTween((int)m_TweenElement, tweenType);
                            if (CheckBindTarget(tween, tweenBase, tween.gameObject)) {
                                tweenBase.Bind(tween.transform);
                            } // end if
                            tweenList.Add(tweenBase);
                            m_TweenElement = JTweenElement.None;
                        } // end if
                        tween.Tweens = tweenList.ToArray();
                    } // end if
                    string text = "";
                    bool isVaild = tween.IsValid(out text);
                    if (!isVaild) {
                        GUI.color = Color.red;
                        EditorGUILayout.LabelField("ErrorInfo::" + text);
                        GUI.color = Color.white;
                    } // end if
                    if (tweenList.Count > 0 && isVaild) {
                        GUILayout.BeginVertical("Box"); {
                            GUILayout.BeginHorizontal(); {
                                if (GUILayout.Button("播放", GUILayout.Width(83f))) {
                                    tween.Init();
                                    tween.Play();
                                } // end if
                                if (GUILayout.Button("暂停", GUILayout.Width(83f))) {
                                    tween.KillAll();
                                } // end if
                                if (GUILayout.Button("还原", GUILayout.Width(83f))) {
                                    tween.Init();
                                } // end if
                            } GUILayout.EndHorizontal();
                            m_CurrentTweenName = EditorGUILayout.TextField("保存动画名:", m_CurrentTweenName);
                            m_CurrentTweenName = m_CurrentTweenName.Replace(" ", "_");
                            if (GUILayout.Button("保存动画信息", GUILayout.Height(28))) {
                                if (string.IsNullOrEmpty(m_CurrentTweenName) || m_CurrentTweenName.Length < 4) {
                                    EditorUtility.DisplayDialog("动画名错误", "请输入正确的动画名(>4个字符), 或选择要修改的动画!!!", "确定");
                                } else {
                                    string assetPath = Path.Combine(TWEENER_PATH, m_CurrentTweenName + ".json").Replace("\\", "/");
                                    if ((!File.Exists(assetPath) &&
                                        EditorUtility.DisplayDialog("提示", "确定要保存此动画吗?", "确定", "取消")) ||
                                        EditorUtility.DisplayDialog("警告", "此动画名已经存在, 继续操作将会进行覆盖!!", "继续", "取消")) {
                                        using (StreamWriter sw = new StreamWriter(assetPath, false, new System.Text.UTF8Encoding(false))) {
                                            JsonWriter jw = new JsonWriter(sw);
                                            JsonMapper.ToJson(tween.DoJson(), jw);
                                        } // end using
                                        AssetDatabase.Refresh();
                                    } // end if
                                } // end if
                            } // end if
                        } GUILayout.EndVertical();
                    } // end if
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
                GUILayout.BeginHorizontal();{
                    tweenBase.LoopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup("循环类型:", tweenBase.LoopType);
                    tweenBase.LoopCount = EditorGUILayout.IntField("循环次数(-1为无限):", tweenBase.LoopCount);
                } GUILayout.EndHorizontal();
                tweenBase.IsSnapping = EditorGUILayout.ToggleLeft("使用平稳差值处理", tweenBase.IsSnapping);
                #endregion
                switch (tweenBase.TweenElement) {
                    case JTweenElement.AudioSource:
                        DrawAudioSourceTween(tweenBase);
                        break;
                    case JTweenElement.Camera:
                        DrawCameraTween(tweenBase);
                        break;
                    case JTweenElement.Light:
                    case JTweenElement.LineRenderer:
                    case JTweenElement.Material:
                    case JTweenElement.Rigidbody:
                    case JTweenElement.Rigidbody2D:
                    case JTweenElement.SpriteRenderer:
                    case JTweenElement.TrailRenderer:
                    case JTweenElement.Transform:
                    case JTweenElement.CanvasGroup:
                    case JTweenElement.Graphic:
                    case JTweenElement.Image:
                    case JTweenElement.LayoutElement:
                    case JTweenElement.Outline:
                    case JTweenElement.RectTransform:
                    case JTweenElement.ScrollRect:
                    case JTweenElement.Slider:
                    case JTweenElement.Text:
                        break;
                } // end switch
            } GUILayout.EndVertical();
        }
        private void DrawAudioSourceTween(JTweenBase tweenBase) {
            switch ((JTweenAudioSource)tweenBase.TweenType) {
                case JTweenAudioSource.Fade:
                    JTweenAudioSourceFade fadeTween = tweenBase as JTweenAudioSourceFade;
                    fadeTween.BeginVolume = EditorGUILayout.FloatField("初始音量:", fadeTween.BeginVolume);
                    fadeTween.ToVolume = EditorGUILayout.FloatField("目标音量:", fadeTween.ToVolume);
                    break;
                case JTweenAudioSource.Pitch:
                    JTweenAudioSourcePitch pitchTween = tweenBase as JTweenAudioSourcePitch;
                    pitchTween.BeginPitch = EditorGUILayout.FloatField("初始音量:", pitchTween.BeginPitch);
                    pitchTween.ToPitch = EditorGUILayout.FloatField("目标音量:", pitchTween.ToPitch);
                    break;
            } // end switch
        }
        private void DrawCameraTween(JTweenBase tweenBase) {
            switch ((JTweenCamera)tweenBase.TweenType) {
                case JTweenCamera.Aspect:
                    JTweenCameraAspect aspectTween = tweenBase as JTweenCameraAspect;
                    aspectTween.BeginAspect = EditorGUILayout.FloatField("初始Aspect:", aspectTween.BeginAspect);
                    aspectTween.ToAspect = EditorGUILayout.FloatField("目标Aspect:", aspectTween.ToAspect);
                    break;
                case JTweenCamera.Color:
                    JTweenCameraColor colorTween = tweenBase as JTweenCameraColor;
                    colorTween.BeginColor = EditorGUILayout.ColorField("初始颜色:", colorTween.BeginColor);
                    colorTween.ToColor = EditorGUILayout.ColorField("目标颜色:", colorTween.ToColor);
                    break;
                case JTweenCamera.FCP:
                    JTweenCameraFCP fcpTween = tweenBase as JTweenCameraFCP;
                    fcpTween.BeginFCP = EditorGUILayout.FloatField("初始FCP:", fcpTween.BeginFCP);
                    fcpTween.ToFCP = EditorGUILayout.FloatField("目标FCP:", fcpTween.ToFCP);
                    break;
                case JTweenCamera.FOV:
                    JTweenCameraFOV fovTween = tweenBase as JTweenCameraFOV;
                    fovTween.BeginFOV = EditorGUILayout.FloatField("初始FOV:", fovTween.BeginFOV);
                    fovTween.ToFOV = EditorGUILayout.FloatField("目标FOV:", fovTween.ToFOV);
                    break;
                case JTweenCamera.NCP:
                    JTweenCameraNCP ncpTween = tweenBase as JTweenCameraNCP;
                    ncpTween.BeginNCP = EditorGUILayout.FloatField("初始NCP:", ncpTween.BeginNCP);
                    ncpTween.ToNCP = EditorGUILayout.FloatField("目标NCP:", ncpTween.ToNCP);
                    break;
                case JTweenCamera.OrthoSize:
                    JTweenCameraOrthoSize orthoSizeTween = tweenBase as JTweenCameraOrthoSize;
                    orthoSizeTween.BeginOrthoSize = EditorGUILayout.FloatField("初始OrthoSize:", orthoSizeTween.BeginOrthoSize);
                    orthoSizeTween.ToOrthoSize = EditorGUILayout.FloatField("目标OrthoSize:", orthoSizeTween.ToOrthoSize);
                    break;
                case JTweenCamera.PixelRect:
                    JTweenCameraPixelRect pixelRectTween = tweenBase as JTweenCameraPixelRect;
                    pixelRectTween.BeginPixelRect = EditorGUILayout.RectField("初始PixelRect:", pixelRectTween.BeginPixelRect);
                    pixelRectTween.ToPixelRect = EditorGUILayout.RectField("目标PixelRect:", pixelRectTween.ToPixelRect);
                    break;
                case JTweenCamera.Rect:
                    JTweenCameraRect rectTween = tweenBase as JTweenCameraRect;
                    rectTween.BeginRect = EditorGUILayout.RectField("初始Rect:", rectTween.BeginRect);
                    rectTween.ToRect = EditorGUILayout.RectField("目标Rect:", rectTween.ToRect);
                    break;
                case JTweenCamera.ShakePosition:
                    JTweenCameraShakePosition shakePositionTween = tweenBase as JTweenCameraShakePosition;
                    shakePositionTween.BegainPosition = EditorGUILayout.Vector3Field("初始位置:", shakePositionTween.BegainPosition);
                    if (shakePositionTween.StrengthVec == Vector3.zero) {
                        shakePositionTween.Strength = EditorGUILayout.FloatField("晃动强度:", shakePositionTween.Strength);
                    } // end if
                    if (shakePositionTween.Strength == 0) {
                        shakePositionTween.StrengthVec = EditorGUILayout.Vector3Field("晃动强度:", shakePositionTween.StrengthVec);
                    } // end if
                    shakePositionTween.Vibrato = EditorGUILayout.IntField("Vibrato:", shakePositionTween.Vibrato);
                    shakePositionTween.Randomness = EditorGUILayout.FloatField("Randomness:", shakePositionTween.Randomness);
                    shakePositionTween.FadeOut = EditorGUILayout.ToggleLeft("FadeOut:", shakePositionTween.FadeOut);
                    break;
                case JTweenCamera.ShakeRotation:
                    JTweenCameraShakeRotation shakeRotationTween = tweenBase as JTweenCameraShakeRotation;
                    shakeRotationTween.BegainRotation = EditorGUILayout.Vector3Field("初始角度:", shakeRotationTween.BegainRotation);
                    if (shakeRotationTween.StrengthVec == Vector3.zero) {
                        shakeRotationTween.Strength = EditorGUILayout.FloatField("晃动强度:", shakeRotationTween.Strength);
                    } // end if
                    if (shakeRotationTween.Strength == 0) {
                        shakeRotationTween.StrengthVec = EditorGUILayout.Vector3Field("晃动强度:", shakeRotationTween.StrengthVec);
                    } // end if
                    shakeRotationTween.Vibrato = EditorGUILayout.IntField("Vibrato:", shakeRotationTween.Vibrato);
                    shakeRotationTween.Randomness = EditorGUILayout.FloatField("Randomness:", shakeRotationTween.Randomness);
                    shakeRotationTween.FadeOut = EditorGUILayout.ToggleLeft("FadeOut:", shakeRotationTween.FadeOut);
                    break;
            } // end switch
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
