using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using UnityGameFramework.Editor;
using JTween;
using JTween.AudioSource;
using JTween.Camera;
using JTween.Light;
using JTween.LineRenderer;
using JTween.Material;
using JTween.Rigidbody;
using JTween.Rigidbody2D;
using JTween.SpriteRenderer;
using JTween.TrailRenderer;
using JTween.Transform;

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
        /// <param name="tween"> 动画序列 </param>
        /// <param name="tweenBase"> 动画 </param>
        /// <param name="target"> 目标物体 </param>
        /// <returns> 是否符合绑定要求 </returns>
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
        /// <param name="tween"> 动画序列 </param>
        /// <param name="tweenBase"> 基础动画 </param>
        /// <param name="OnDeleteTween"> 删除动画回调 </param>
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
                        DrawLightTween(tweenBase);
                        break;
                    case JTweenElement.LineRenderer:
                        DrawLineRendererTween(tweenBase);
                        break;
                    case JTweenElement.Material:
                        DrawMaterialTween(tweenBase);
                        break;
                    case JTweenElement.Rigidbody:
                        DrawRigidbodyTween(tweenBase);
                        break;
                    case JTweenElement.Rigidbody2D:
                        DrawRigidbody2DTween(tweenBase);
                        break;
                    case JTweenElement.SpriteRenderer:
                        DrawSpriteRendererTween(tweenBase);
                        break;
                    case JTweenElement.TrailRenderer:
                        DrawTrailRendererTween(tweenBase);
                        break;
                    case JTweenElement.Transform:
                        DrawTransformTween(tweenBase);
                        break;
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
        /// <summary>
        /// 绘制音源动画
        /// </summary>
        /// <param name="tweenBase"> 音源动画 </param>
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
        /// <summary>
        /// 绘制相机动画
        /// </summary>
        /// <param name="tweenBase"> 相机动画 </param>
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
        /// <summary>
        /// 绘制灯光动画
        /// </summary>
        /// <param name="tweenBase"> 灯光动画 </param>
        private void DrawLightTween(JTweenBase tweenBase) {
            switch ((JTweenLight)tweenBase.TweenType) {
                case JTweenLight.Color:
                    JTweenLightColor colorTween = tweenBase as JTweenLightColor;
                    colorTween.BeginColor = EditorGUILayout.ColorField("初始光照颜色:", colorTween.BeginColor);
                    colorTween.ToColor = EditorGUILayout.ColorField("目标光照颜色:", colorTween.ToColor);
                    break;
                case JTweenLight.Intensity:
                    JTweenLightIntensity intensityTween = tweenBase as JTweenLightIntensity;
                    intensityTween.BeginIntensity = EditorGUILayout.FloatField("初始光照强度:", intensityTween.BeginIntensity);
                    intensityTween.ToIntensity = EditorGUILayout.FloatField("目标光照强度:", intensityTween.ToIntensity);
                    break;
                case JTweenLight.ShadowStrength:
                    JTweenLightShadowStrength shadowTween = tweenBase as JTweenLightShadowStrength;
                    shadowTween.BeginStrength = EditorGUILayout.FloatField("初始阴影强度:", shadowTween.BeginStrength);
                    shadowTween.ToStrength = EditorGUILayout.FloatField("目标阴影强度:", shadowTween.ToStrength);
                    break;
                case JTweenLight.BlendableColor:
                    JTweenLightBlendableColor blendableColorTween = tweenBase as JTweenLightBlendableColor;
                    blendableColorTween.BeginColor = EditorGUILayout.ColorField("初始光照颜色:", blendableColorTween.BeginColor);
                    blendableColorTween.ToColor = EditorGUILayout.ColorField("目标光照颜色:", blendableColorTween.ToColor);
                    break;
            } // end switch
        }
        /// <summary>
        /// 绘制线条渲染动画
        /// </summary>
        /// <param name="tweenBase"> 线条渲染动画 </param>
        private void DrawLineRendererTween(JTweenBase tweenBase) {
            switch ((JTweenLineRenderer)tweenBase.TweenType) {
                case JTweenLineRenderer.Color:
                    JTweenLineRendererColor colorTween = tweenBase as JTweenLineRendererColor;
                    colorTween.BeginStartColor = EditorGUILayout.ColorField("初始起点颜色:", colorTween.BeginStartColor);
                    colorTween.ToStartColor = EditorGUILayout.ColorField("目标起点颜色:", colorTween.ToStartColor);
                    colorTween.BeginEndColor = EditorGUILayout.ColorField("初始终点颜色:", colorTween.BeginEndColor);
                    colorTween.ToEndColor = EditorGUILayout.ColorField("目标终点颜色:", colorTween.ToEndColor);
                    break;
            } // end switch
        }
        /// <summary>
        /// 绘制材质动画
        /// </summary>
        /// <param name="tweenBase"> 材质动画 </param>
        private void DrawMaterialTween(JTweenBase tweenBase) {
            switch ((JTweenMaterial)tweenBase.TweenType) {
                case JTweenMaterial.Color:
                    JTweenMaterialColor colorTween = tweenBase as JTweenMaterialColor;
                    colorTween.BeginColor = EditorGUILayout.ColorField("初始颜色:", colorTween.BeginColor);
                    colorTween.ToColor = EditorGUILayout.ColorField("目标颜色:", colorTween.ToColor);
                    colorTween.Property = EditorGUILayout.TextField("属性名:", colorTween.Property);
                    colorTween.PropertyID = EditorGUILayout.IntField("属性ID:", colorTween.PropertyID);
                    break;
                case JTweenMaterial.Fade:
                    JTweenMaterialFade fadeTween = tweenBase as JTweenMaterialFade;
                    fadeTween.BeginColor = EditorGUILayout.ColorField("初始颜色:", fadeTween.BeginColor);
                    fadeTween.ToAlpha = EditorGUILayout.FloatField("目标Alpha:", fadeTween.ToAlpha);
                    fadeTween.Property = EditorGUILayout.TextField("属性名:", fadeTween.Property);
                    fadeTween.PropertyID = EditorGUILayout.IntField("属性ID:", fadeTween.PropertyID);
                    break;
                case JTweenMaterial.Float:
                    JTweenMaterialFloat floatTween = tweenBase as JTweenMaterialFloat;
                    floatTween.BeginFloat = EditorGUILayout.FloatField("初始数值:", floatTween.BeginFloat);
                    floatTween.ToFloat = EditorGUILayout.FloatField("目标数值:", floatTween.ToFloat);
                    floatTween.Property = EditorGUILayout.TextField("属性名:", floatTween.Property);
                    floatTween.PropertyID = EditorGUILayout.IntField("属性ID:", floatTween.PropertyID);
                    break;
                case JTweenMaterial.GradientColor:
                    break;
                case JTweenMaterial.Offset:
                    JTweenMaterialOffset offsetTween = tweenBase as JTweenMaterialOffset;
                    offsetTween.BeginOffset = EditorGUILayout.Vector2Field("初始偏移:", offsetTween.BeginOffset);
                    offsetTween.ToOffset = EditorGUILayout.Vector2Field("目标偏移:", offsetTween.ToOffset);
                    offsetTween.Property = EditorGUILayout.TextField("属性名:", offsetTween.Property);
                    offsetTween.PropertyID = EditorGUILayout.IntField("属性ID:", offsetTween.PropertyID);
                    break;
                case JTweenMaterial.Tiling:
                    JTweenMaterialTiling tilingTween = tweenBase as JTweenMaterialTiling;
                    tilingTween.BeginTiling = EditorGUILayout.Vector2Field("初始平铺:", tilingTween.BeginTiling);
                    tilingTween.ToTiling = EditorGUILayout.Vector2Field("目标平铺:", tilingTween.ToTiling);
                    tilingTween.Property = EditorGUILayout.TextField("属性名:", tilingTween.Property);
                    tilingTween.PropertyID = EditorGUILayout.IntField("属性ID:", tilingTween.PropertyID);
                    break;
                case JTweenMaterial.Vector:
                    JTweenMaterialVector vectorTween = tweenBase as JTweenMaterialVector;
                    vectorTween.BeginVector = EditorGUILayout.Vector4Field("初始值:", vectorTween.BeginVector);
                    vectorTween.ToVector = EditorGUILayout.Vector4Field("目标值:", vectorTween.ToVector);
                    vectorTween.Property = EditorGUILayout.TextField("属性名:", vectorTween.Property);
                    vectorTween.PropertyID = EditorGUILayout.IntField("属性ID:", vectorTween.PropertyID);
                    break;
            } // end switch
        }
        /// <summary>
        /// 绘制刚体动画
        /// </summary>
        /// <param name="tweenBase"> 刚体动画 </param>
        private void DrawRigidbodyTween(JTweenBase tweenBase) {
            switch ((JTweenRigidbody)tweenBase.TweenType) {
                case JTweenRigidbody.Move:
                    break;
                case JTweenRigidbody.Jump:
                    JTweenRigidbodyJump jumpTween = tweenBase as JTweenRigidbodyJump;
                    jumpTween.BeginPosition = EditorGUILayout.Vector3Field("初始位置:", jumpTween.BeginPosition);
                    jumpTween.ToPosition = EditorGUILayout.Vector3Field("目标位置:", jumpTween.ToPosition);
                    jumpTween.NumJumps = EditorGUILayout.IntField("跳跃次数:", jumpTween.NumJumps);
                    jumpTween.JumpPower = EditorGUILayout.FloatField("跳跃力度（最大高度）:", jumpTween.JumpPower);
                    break;
                case JTweenRigidbody.Rotate:
                    JTweenRigidbodyRotate rotateTween = tweenBase as JTweenRigidbodyRotate;
                    rotateTween.BeginRotate = EditorGUILayout.Vector3Field("初始角度:", rotateTween.BeginRotate);
                    rotateTween.ToRotate = EditorGUILayout.Vector3Field("目标角度:", rotateTween.ToRotate);
                    rotateTween.RotateMode = (RotateMode)EditorGUILayout.EnumPopup("跳跃次数:", RotateMode.Fast);
                    break;
                case JTweenRigidbody.LookAt:
                    JTweenRigidbodyLookAt lookAtTween = tweenBase as JTweenRigidbodyLookAt;
                    lookAtTween.BeginRotate = EditorGUILayout.Vector3Field("初始角度:", lookAtTween.BeginRotate);
                    lookAtTween.Towards = EditorGUILayout.Vector3Field("朝向角度:", lookAtTween.Towards);
                    lookAtTween.AxisConstraint = (AxisConstraint)EditorGUILayout.EnumPopup("约束轴:", AxisConstraint.None);
                    lookAtTween.Up = EditorGUILayout.Vector3Field("向上方向:", lookAtTween.Up);
                    break;
            } // end switch
        }
        /// <summary>
        /// 绘制2D刚体动画
        /// </summary>
        /// <param name="tweenBase"> 2D刚体动画 </param>
        private void DrawRigidbody2DTween(JTweenBase tweenBase) {
            switch ((JTweenRigidbody2D)tweenBase.TweenType) {
                case JTweenRigidbody2D.Move:
                    break;
                case JTweenRigidbody2D.Jump:
                    JTweenRigidbody2DJump jumpTween = tweenBase as JTweenRigidbody2DJump;
                    jumpTween.BeginPosition = EditorGUILayout.Vector3Field("初始位置:", jumpTween.BeginPosition);
                    jumpTween.ToPosition = EditorGUILayout.Vector3Field("目标位置:", jumpTween.ToPosition);
                    jumpTween.NumJumps = EditorGUILayout.IntField("跳跃次数:", jumpTween.NumJumps);
                    jumpTween.JumpPower = EditorGUILayout.FloatField("跳跃力度（最大高度）:", jumpTween.JumpPower);
                    break;
                case JTweenRigidbody2D.Rotate:
                    JTweenRigidbody2DRotate rotateTween = tweenBase as JTweenRigidbody2DRotate;
                    rotateTween.BeginRotation = EditorGUILayout.FloatField("初始角度:", rotateTween.BeginRotation);
                    rotateTween.ToAngle = EditorGUILayout.FloatField("目标角度:", rotateTween.ToAngle);
                    break;
            } // end switch
        }
        /// <summary>
        /// 绘制精灵渲染动画
        /// </summary>
        /// <param name="tweenBase"> 精灵渲染动画 </param>
        private void DrawSpriteRendererTween(JTweenBase tweenBase) {
            switch ((JTweenSpriteRenderer)tweenBase.TweenType) {
                case JTweenSpriteRenderer.Color:
                    JTweenSpriteRendererColor colorTween = tweenBase as JTweenSpriteRendererColor;
                    colorTween.BeginColor = EditorGUILayout.ColorField("初始颜色:", colorTween.BeginColor);
                    colorTween.ToColor = EditorGUILayout.ColorField("目标颜色:", colorTween.ToColor);
                    break;
                case JTweenSpriteRenderer.Fade:
                    JTweenSpriteRendererFade fadeTween = tweenBase as JTweenSpriteRendererFade;
                    fadeTween.BeginColor = EditorGUILayout.ColorField("初始颜色:", fadeTween.BeginColor);
                    fadeTween.ToAlpha = EditorGUILayout.FloatField("目标Alpha:", fadeTween.ToAlpha);
                    break;
                case JTweenSpriteRenderer.BlendableColor:
                    JTweenSpriteRendererBlendableColor blendableColorTween = tweenBase as JTweenSpriteRendererBlendableColor;
                    blendableColorTween.BeginColor = EditorGUILayout.ColorField("初始颜色:", blendableColorTween.BeginColor);
                    blendableColorTween.ToColor = EditorGUILayout.ColorField("目标颜色:", blendableColorTween.ToColor);
                    break;
            } // end switch
        }
        /// <summary>
        /// 绘制拖尾渲染动画
        /// </summary>
        /// <param name="tweenBase"> 拖尾渲染动画 </param>
        private void DrawTrailRendererTween(JTweenBase tweenBase) {
            switch ((JTweenTrailRenderer)tweenBase.TweenType) {
                case JTweenTrailRenderer.Resize:
                    JTweenTrailRendererResize resizeTween = tweenBase as JTweenTrailRendererResize;
                    resizeTween.BeginStartWidth = EditorGUILayout.FloatField("初始头部宽度:", resizeTween.BeginStartWidth);
                    resizeTween.StartWidth = EditorGUILayout.FloatField("目标头部宽度:", resizeTween.StartWidth);
                    resizeTween.BeginEndWidth = EditorGUILayout.FloatField("初始尾部宽度:", resizeTween.BeginEndWidth);
                    resizeTween.EndWidth = EditorGUILayout.FloatField("目标尾部宽度:", resizeTween.EndWidth);
                    break;
                case JTweenTrailRenderer.Time:
                    JTweenTrailRendererTime timeTween = tweenBase as JTweenTrailRendererTime;
                    timeTween.BeginTime = EditorGUILayout.FloatField("初始时间:", timeTween.BeginTime);
                    timeTween.ToTime = EditorGUILayout.FloatField("目标时间:", timeTween.ToTime);
                    break;
            } // end switch
        }
        /// <summary>
        /// 绘制3D基础组件动画
        /// </summary>
        /// <param name="tweenBase"> 3D基础组件动画 </param>
        private void DrawTransformTween(JTweenBase tweenBase) {
            switch ((JTweenTransform)tweenBase.TweenType) {
                case JTweenTransform.Move:
                    break;
                case JTweenTransform.LocalMove:
                    break;
                case JTweenTransform.Jump:
                    JTweenTransformJump jumpTween = tweenBase as JTweenTransformJump;
                    jumpTween.BeginPosition = EditorGUILayout.Vector3Field("初始位置:", jumpTween.BeginPosition);
                    jumpTween.ToPosition = EditorGUILayout.Vector3Field("目标位置:", jumpTween.ToPosition);
                    jumpTween.NumJumps = EditorGUILayout.IntField("跳跃次数:", jumpTween.NumJumps);
                    jumpTween.JumpPower = EditorGUILayout.FloatField("跳跃力度（最大高度）:", jumpTween.JumpPower);
                    break;
                case JTweenTransform.LocalJump:
                    JTweenTransformJump localJumpTween = tweenBase as JTweenTransformJump;
                    localJumpTween.BeginPosition = EditorGUILayout.Vector3Field("初始位置:", localJumpTween.BeginPosition);
                    localJumpTween.ToPosition = EditorGUILayout.Vector3Field("目标位置:", localJumpTween.ToPosition);
                    localJumpTween.NumJumps = EditorGUILayout.IntField("跳跃次数:", localJumpTween.NumJumps);
                    localJumpTween.JumpPower = EditorGUILayout.FloatField("跳跃力度（最大高度）:", localJumpTween.JumpPower);
                    break;
                case JTweenTransform.Rotate:
                    JTweenTransformRotate rotateTween = tweenBase as JTweenTransformRotate;
                    rotateTween.BeginRotation = EditorGUILayout.Vector3Field("初始角度:", rotateTween.BeginRotation);
                    rotateTween.ToRotate = EditorGUILayout.Vector3Field("目标角度:", rotateTween.ToRotate);
                    rotateTween.RotateMode = (RotateMode)EditorGUILayout.EnumPopup("跳跃次数:", RotateMode.Fast);
                    break;
                case JTweenTransform.Quaternion:
                    JTweenTransformQuaternion quaternionTween = tweenBase as JTweenTransformQuaternion;
                    var ratation = quaternionTween.BeginRotation;
                    Vector4 vector = new Vector4(ratation.x, ratation.y, ratation.z, ratation.w);
                    vector = EditorGUILayout.Vector4Field("初始四元数:", vector);
                    quaternionTween.BeginRotation = new Quaternion(vector.x, vector.y, vector.z, vector.w);
                    ratation = quaternionTween.ToRotate;
                    vector = new Vector4(ratation.x, ratation.y, ratation.z, ratation.w);
                    vector = EditorGUILayout.Vector4Field("目标四元数:", vector);
                    quaternionTween.ToRotate = new Quaternion(vector.x, vector.y, vector.z, vector.w);
                    break;
                case JTweenTransform.LocalRotate:
                    JTweenTransformRotate localRotateTween = tweenBase as JTweenTransformRotate;
                    localRotateTween.BeginRotation = EditorGUILayout.Vector3Field("初始角度:", localRotateTween.BeginRotation);
                    localRotateTween.ToRotate = EditorGUILayout.Vector3Field("目标角度:", localRotateTween.ToRotate);
                    localRotateTween.RotateMode = (RotateMode)EditorGUILayout.EnumPopup("跳跃次数:", RotateMode.Fast);
                    break;
                case JTweenTransform.LocalQuaternion:
                    JTweenTransformQuaternion localQuaternionTween = tweenBase as JTweenTransformQuaternion;
                    ratation = localQuaternionTween.BeginRotation;
                    vector = new Vector4(ratation.x, ratation.y, ratation.z, ratation.w);
                    vector = EditorGUILayout.Vector4Field("初始四元数:", vector);
                    localQuaternionTween.BeginRotation = new Quaternion(vector.x, vector.y, vector.z, vector.w);
                    ratation = localQuaternionTween.ToRotate;
                    vector = new Vector4(ratation.x, ratation.y, ratation.z, ratation.w);
                    vector = EditorGUILayout.Vector4Field("目标四元数:", vector);
                    localQuaternionTween.ToRotate = new Quaternion(vector.x, vector.y, vector.z, vector.w);
                    break;
                case JTweenTransform.LookAt:
                    JTweenTransformLookAt lookAtTween = tweenBase as JTweenTransformLookAt;
                    lookAtTween.BeginRotate = EditorGUILayout.Vector3Field("初始角度:", lookAtTween.BeginRotate);
                    lookAtTween.Towards = EditorGUILayout.Vector3Field("朝向角度:", lookAtTween.Towards);
                    lookAtTween.AxisConstraint = (AxisConstraint)EditorGUILayout.EnumPopup("约束轴:", AxisConstraint.None);
                    lookAtTween.Up = EditorGUILayout.Vector3Field("向上方向:", lookAtTween.Up);
                    break;
                case JTweenTransform.Scale:
                    break;
                case JTweenTransform.PunchPosition:
                    JTweenTransformPunchPosition punchPosTween = tweenBase as JTweenTransformPunchPosition;
                    punchPosTween.BeginPosition = EditorGUILayout.Vector3Field("初始位置:", punchPosTween.BeginPosition);
                    punchPosTween.ToPunch = EditorGUILayout.Vector3Field("抖动方向强度:", punchPosTween.ToPunch);
                    punchPosTween.Vibrate = EditorGUILayout.IntField("抖动次数:", punchPosTween.Vibrate);
                    punchPosTween.Elasticity = EditorGUILayout.FloatField("弹力:", punchPosTween.Elasticity);
                    break;
                case JTweenTransform.PunchRatation:
                    JTweenTransformPunchRotation punchRotTween = tweenBase as JTweenTransformPunchRotation;
                    punchRotTween.BeginRotation = EditorGUILayout.Vector3Field("初始角度:", punchRotTween.BeginRotation);
                    punchRotTween.ToPunch = EditorGUILayout.Vector3Field("抖动强度:", punchRotTween.ToPunch);
                    punchRotTween.Vibrate = EditorGUILayout.IntField("抖动次数:", punchRotTween.Vibrate);
                    punchRotTween.Elasticity = EditorGUILayout.FloatField("弹力:", punchRotTween.Elasticity);
                    break;
                case JTweenTransform.PunchScale:
                    JTweenTransformPunchScale punchScaTween = tweenBase as JTweenTransformPunchScale;
                    punchScaTween.BeginScale = EditorGUILayout.Vector3Field("初始尺寸:", punchScaTween.BeginScale);
                    punchScaTween.ToPunch = EditorGUILayout.Vector3Field("抖动强度:", punchScaTween.ToPunch);
                    punchScaTween.Vibrate = EditorGUILayout.IntField("抖动次数:", punchScaTween.Vibrate);
                    punchScaTween.Elasticity = EditorGUILayout.FloatField("弹力:", punchScaTween.Elasticity);
                    break;
                case JTweenTransform.ShakePosition:
                    JTweenTransformShakePosition shakePosTween = tweenBase as JTweenTransformShakePosition;
                    shakePosTween.BeginPosition = EditorGUILayout.Vector3Field("初始位置:", shakePosTween.BeginPosition);
                    shakePosTween.StrengthVec = EditorGUILayout.Vector3Field("晃动次数:", shakePosTween.StrengthVec);
                    if (shakePosTween.StrengthVec == Vector3.zero) {
                        shakePosTween.Strength = EditorGUILayout.FloatField("晃动强度:", shakePosTween.Strength);
                    } // end if
                    shakePosTween.Randomness = EditorGUILayout.FloatField("随机性:", shakePosTween.Randomness);
                    shakePosTween.FadeOut = EditorGUILayout.ToggleLeft("淡出:", shakePosTween.FadeOut);
                    break;
                case JTweenTransform.ShakeRotation:
                    JTweenTransformShakeRotation shakeRotTween = tweenBase as JTweenTransformShakeRotation;
                    shakeRotTween.BeginRotation = EditorGUILayout.Vector3Field("初始角度:", shakeRotTween.BeginRotation);
                    shakeRotTween.StrengthVec = EditorGUILayout.Vector3Field("晃动次数:", shakeRotTween.StrengthVec);
                    if (shakeRotTween.StrengthVec == Vector3.zero) {
                        shakeRotTween.Strength = EditorGUILayout.FloatField("晃动强度:", shakeRotTween.Strength);
                    } // end if
                    shakeRotTween.Randomness = EditorGUILayout.FloatField("随机性:", shakeRotTween.Randomness);
                    shakeRotTween.FadeOut = EditorGUILayout.ToggleLeft("淡出:", shakeRotTween.FadeOut);
                    break;
                case JTweenTransform.ShakeScale:
                    JTweenTransformShakeScale shakeScaTween = tweenBase as JTweenTransformShakeScale;
                    shakeScaTween.BeginScale = EditorGUILayout.Vector3Field("初始尺寸:", shakeScaTween.BeginScale);
                    shakeScaTween.StrengthVec = EditorGUILayout.Vector3Field("晃动次数:", shakeScaTween.StrengthVec);
                    if (shakeScaTween.StrengthVec == Vector3.zero) {
                        shakeScaTween.Strength = EditorGUILayout.FloatField("晃动强度:", shakeScaTween.Strength);
                    } // end if
                    shakeScaTween.Randomness = EditorGUILayout.FloatField("随机性:", shakeScaTween.Randomness);
                    shakeScaTween.FadeOut = EditorGUILayout.ToggleLeft("淡出:", shakeScaTween.FadeOut);
                    break;
                case JTweenTransform.Path:
                    JTweenTransformPath pathTween = tweenBase as JTweenTransformPath;
                    pathTween.BeginPosition = EditorGUILayout.Vector3Field("初始位置:", pathTween.BeginPosition);
                    pathTween.PathType = (PathType)EditorGUILayout.EnumPopup("曲线类型:", PathType.Linear);
                    pathTween.PathMode = (PathMode)EditorGUILayout.EnumPopup("路线模式:", PathMode.Full3D);
                    pathTween.Resolution = EditorGUILayout.IntField("辨析率:", pathTween.Resolution);
                    GUILayout.BeginVertical("Box"); {
                        GUILayout.Label("路径点：");
                        List<Vector3> posList = new List<Vector3>();
                        if (pathTween.ToPath != null && pathTween.ToPath.Length >= 0) {
                            posList.AddRange(pathTween.ToPath);
                        } // end if
                        for (int i = 0; i < posList.Count; ++i) {
                            GUILayout.BeginHorizontal(); {
                                posList[i] = EditorGUILayout.Vector3Field("pos_" + (i + 1), posList[i]);
                                if (GUILayout.Button("+", GUILayout.Width(40f), GUILayout.Height(15))) {
                                    posList.Insert(i + 1, Vector3.zero);
                                    break;
                                } // end if
                                if (GUILayout.Button("-", GUILayout.Width(40f), GUILayout.Height(15))) {
                                    posList.RemoveAt(i);
                                    break;
                                } // end if
                            } GUILayout.EndHorizontal();
                        } // end for
                        if (GUILayout.Button("Add", GUILayout.Width(83f))) {
                            posList.Add(Vector3.zero);
                        } // end if
                        if (posList.Count > 0) {
                            pathTween.ToPath = posList.ToArray();
                        } // end if
                    } GUILayout.EndVertical();
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
