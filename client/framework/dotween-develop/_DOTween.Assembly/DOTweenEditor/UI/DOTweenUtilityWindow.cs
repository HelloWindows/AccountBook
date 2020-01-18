﻿// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/12/24 13:37

using System.IO;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor.UI
{
    public class DOTweenUtilityWindow : EditorWindow
    {
        [MenuItem("Tools/Demigiant/" + _Title)]
        static void ShowWindow() { Open(); }
		
        const string _Title = "DOTween Utility Panel";
        static readonly Vector2 _WinSize = new Vector2(370,650);
        public const string Id = "DOTweenVersion";
        public const string IdPro = "DOTweenProVersion";
        static readonly float _HalfBtSize = _WinSize.x * 0.5f - 6;

        bool _initialized;
        DOTweenSettings _src;
        Texture2D _headerImg, _footerImg;
        Vector2 _headerSize, _footerSize;
        string _innerTitle;
        bool _setupRequired;
        Vector2 _scrollVal;

        int _selectedTab;
        string[] _tabLabels = new[] { "Setup", "Preferences" };
        string[] _settingsLocation = new[] {"Assets > Resources", "DOTween > Resources", "Demigiant > Resources"};

        // If force is FALSE opens the window only if DOTween's version has changed
        // (set to FALSE by OnPostprocessAllAssets).<para/>
        // NOTE: this is also called via Reflection by UpgradeWindow
        public static void Open()
        {
            DOTweenUtilityWindow window = EditorWindow.GetWindow<DOTweenUtilityWindow>(true, _Title, true);
            window.minSize = _WinSize;
            window.maxSize = _WinSize;
            window.ShowUtility();
            EditorPrefs.SetString(Id, DOTween.Version);
            EditorPrefs.SetString(IdPro, EditorUtils.proVersion);
        }

        // Returns TRUE if DOTween is initialized
        bool Init()
        {
            if (_initialized) return true;

            if (_headerImg == null) {
                _headerImg = AssetDatabase.LoadAssetAtPath("Assets/" + EditorUtils.editorADBDir + "Imgs/Header.jpg", typeof(Texture2D)) as Texture2D;
                if (_headerImg == null) return false; // DOTween imported for the first time and images not yet imported
                EditorUtils.SetEditorTexture(_headerImg, FilterMode.Bilinear, 512);
                _headerSize.x = _WinSize.x;
                _headerSize.y = (int)((_WinSize.x * _headerImg.height) / _headerImg.width);
                _footerImg = AssetDatabase.LoadAssetAtPath("Assets/" + EditorUtils.editorADBDir + (EditorGUIUtility.isProSkin ? "Imgs/Footer.png" : "Imgs/Footer_dark.png"), typeof(Texture2D)) as Texture2D;
                EditorUtils.SetEditorTexture(_footerImg, FilterMode.Bilinear, 256);
                _footerSize.x = _WinSize.x;
                _footerSize.y = (int)((_WinSize.x * _footerImg.height) / _footerImg.width);
            }
            _initialized = true;
            return true;
        }

        // ===================================================================================
        // UNITY METHODS ---------------------------------------------------------------------

        void OnHierarchyChange()
        { Repaint(); }

        void OnEnable()
        {
#if COMPATIBLE
            _innerTitle = "DOTween v" + DOTween.Version + " [Compatibility build]";
#else
            _innerTitle = "DOTween v" + DOTween.Version + (TweenManager.isDebugBuild ? " [Debug build]" : " [Release build]");
#endif
            if (EditorUtils.hasPro) _innerTitle += "\nDOTweenPro v" + EditorUtils.proVersion;
            else _innerTitle += "\nDOTweenPro not installed";

            Init();

            _setupRequired = EditorUtils.DOTweenSetupRequired();
        }

        void OnDestroy()
        {
            if (_src != null) {
                _src.modules.showPanel = false;
                EditorUtility.SetDirty(_src);
            }
        }

        void OnGUI()
        {
            if (!Init()) {
                GUILayout.Space(8);
                GUILayout.Label("Completing import process...");
                return;
            }
            Connect();
            EditorGUIUtils.SetGUIStyles(_footerSize);

            if (Application.isPlaying) {
                GUILayout.Space(40);
                GUILayout.BeginHorizontal();
                GUILayout.Space(40);
                GUILayout.Label("DOTween Utility Panel\nis disabled while in Play Mode", EditorGUIUtils.wrapCenterLabelStyle, GUILayout.ExpandWidth(true));
                GUILayout.Space(40);
                GUILayout.EndHorizontal();
            } else {
                _scrollVal = GUILayout.BeginScrollView(_scrollVal);
                if (_src.modules.showPanel) {
                    if (DOTweenUtilityWindowModules.Draw(this, _src)) {
                        _setupRequired = EditorUtils.DOTweenSetupRequired();
                        _src.modules.showPanel = false;
                        EditorUtility.SetDirty(_src);
                    }
                } else {
                    Rect areaRect = new Rect(0, 0, _headerSize.x, 30);
                    _selectedTab = GUI.Toolbar(areaRect, _selectedTab, _tabLabels);

                    switch (_selectedTab) {
                    case 1:
                        float labelW = EditorGUIUtility.labelWidth;
                        EditorGUIUtility.labelWidth = 160;
                        DrawPreferencesGUI();
                        EditorGUIUtility.labelWidth = labelW;
                        break;
                    default:
                        DrawSetupGUI();
                        break;
                    }
                }
                GUILayout.EndScrollView();
            }

            if (GUI.changed) EditorUtility.SetDirty(_src);
        }

        // ===================================================================================
        // GUI METHODS -----------------------------------------------------------------------

        void DrawSetupGUI()
        {
            Rect areaRect = new Rect(0, 30, _headerSize.x, _headerSize.y);
            GUI.DrawTexture(areaRect, _headerImg, ScaleMode.StretchToFill, false);
            GUILayout.Space(areaRect.y + _headerSize.y + 2);
            GUILayout.Label(_innerTitle, TweenManager.isDebugBuild ? EditorGUIUtils.redLabelStyle : EditorGUIUtils.boldLabelStyle);

            if (_setupRequired) {
                GUI.backgroundColor = Color.red;
                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label("DOTWEEN SETUP REQUIRED", EditorGUIUtils.setupLabelStyle);
                GUILayout.EndVertical();
                GUI.backgroundColor = Color.white;
            } else GUILayout.Space(8);
            GUI.color = Color.green;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("<b>Setup DOTween...</b>\n(add/remove Modules)", EditorGUIUtils.btSetup, GUILayout.Width(200))) {
//                DOTweenDefines.Setup();
//                _setupRequired = EditorUtils.DOTweenSetupRequired();
                DOTweenUtilityWindowModules.ApplyModulesSettings();
                _src.modules.showPanel = true;
                EditorUtility.SetDirty(_src);
                EditorUtils.DeleteLegacyNoModulesDOTweenFiles();
                DOTweenDefines.RemoveAllLegacyDefines();
                EditorUtils.DeleteDOTweenUpgradeManagerFiles();
                return;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUI.color = Color.white;
            GUILayout.Space(4);

            // ASMDEF
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.color = ASMDEFManager.hasModulesASMDEF ? Color.yellow : Color.cyan;
            if (GUILayout.Button(ASMDEFManager.hasModulesASMDEF ? "Remove ASMDEF..." : "Create ASMDEF...", EditorGUIUtils.btSetup, GUILayout.Width(200))) {
                if (ASMDEFManager.hasModulesASMDEF) {
                    if (EditorUtility.DisplayDialog("Remove ASMDEF",
                        string.Format("This will remove the \"DOTween/Modules/DOTween.Modules.asmdef\" file" +
                                      " (and if you have DOTween Pro also the \"DOTweenPro/DOTweenPro.Scripts.asmdef\"" +
                                      " and \"DOTweenPro/Editor/DOTweenPro.EditorScripts.asmdef\" files)"),
                        "Ok", "Cancel"
                    )) ASMDEFManager.RemoveAllASMDEF();
                } else {
                    if (EditorUtility.DisplayDialog("Create ASMDEF",
                        string.Format("This will create the \"DOTween/Modules/DOTween.Modules.asmdef\" file" +
                                      " (and if you have DOTween Pro also the \"DOTweenPro/DOTweenPro.Scripts.asmdef\"" +
                                      " and \"DOTweenPro/Editor/DOTweenPro.EditorScripts.asmdef\" files)"),
                        "Ok", "Cancel"
                    )) ASMDEFManager.CreateAllASMDEF();
                }
            }
            GUI.color = Color.white;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Label(
                "ASMDEFs are useful if you need to reference the extra DOTween modules API (like [<i>UIelement</i>].DOColor)" +
                " from other ASMDEFs/Libraries instead of loose scripts," +
                " but remember to have those <b>ASMDEFs/Libraries reference DOTween ones</b>.",
                EditorGUIUtils.wordWrapRichTextLabelStyle
            );
            GUILayout.Space(3);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Website", EditorGUIUtils.btBigStyle, GUILayout.Width(_HalfBtSize))) Application.OpenURL("http://dotween.demigiant.com/index.php");
            if (GUILayout.Button("Get Started", EditorGUIUtils.btBigStyle, GUILayout.Width(_HalfBtSize))) Application.OpenURL("http://dotween.demigiant.com/getstarted.php");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Documentation", EditorGUIUtils.btBigStyle, GUILayout.Width(_HalfBtSize))) Application.OpenURL("http://dotween.demigiant.com/documentation.php");
            if (GUILayout.Button("Support", EditorGUIUtils.btBigStyle, GUILayout.Width(_HalfBtSize))) Application.OpenURL("http://dotween.demigiant.com/support.php");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Changelog", EditorGUIUtils.btBigStyle, GUILayout.Width(_HalfBtSize))) Application.OpenURL("http://dotween.demigiant.com/download.php");
            if (GUILayout.Button("Check Updates", EditorGUIUtils.btBigStyle, GUILayout.Width(_HalfBtSize))) Application.OpenURL("http://dotween.demigiant.com/download.php?v=" + DOTween.Version);
            GUILayout.EndHorizontal();
            GUILayout.Space(4);
            if (GUILayout.Button(_footerImg, EditorGUIUtils.btImgStyle)) Application.OpenURL("http://www.demigiant.com/");
        }

        void DrawPreferencesGUI()
        {
            GUILayout.Space(40);
            if (GUILayout.Button("Reset", EditorGUIUtils.btBigStyle)) {
                // Reset to original defaults
                _src.useSafeMode = true;
                _src.safeModeOptions.nestedTweenFailureBehaviour = NestedTweenFailureBehaviour.TryToPreserveSequence;
                _src.showUnityEditorReport = false;
                _src.timeScale = 1;
                _src.useSmoothDeltaTime = false;
                _src.maxSmoothUnscaledTime = 0.15f;
                _src.rewindCallbackMode = RewindCallbackMode.FireIfPositionChanged;
                _src.logBehaviour = LogBehaviour.ErrorsOnly;
                _src.drawGizmos = true;
                _src.defaultRecyclable = false;
                _src.defaultAutoPlay = AutoPlay.All;
                _src.defaultUpdateType = UpdateType.Normal;
                _src.defaultTimeScaleIndependent = false;
                _src.defaultEaseType = Ease.OutQuad;
                _src.defaultEaseOvershootOrAmplitude = 1.70158f;
                _src.defaultEasePeriod = 0;
                _src.defaultAutoKill = true;
                _src.defaultLoopType = LoopType.Restart;
                _src.debugMode = false;
                _src.debugStoreTargetId = false;
                EditorUtility.SetDirty(_src);
            }
            GUILayout.Space(8);
            _src.useSafeMode = EditorGUILayout.Toggle("Safe Mode", _src.useSafeMode);
            if (_src.useSafeMode) {
                _src.safeModeOptions.nestedTweenFailureBehaviour = (NestedTweenFailureBehaviour)EditorGUILayout.EnumPopup(
                    new GUIContent("└ On Nested Tween Failure", "Behaviour in case a tween inside a Sequence fails"),
                    _src.safeModeOptions.nestedTweenFailureBehaviour
                );
            }
            _src.timeScale = EditorGUILayout.FloatField("DOTween's TimeScale", _src.timeScale);
            _src.useSmoothDeltaTime = EditorGUILayout.Toggle("Smooth DeltaTime", _src.useSmoothDeltaTime);
            _src.maxSmoothUnscaledTime = EditorGUILayout.Slider("Max SmoothUnscaledTime", _src.maxSmoothUnscaledTime, 0.01f, 1f);
            _src.rewindCallbackMode = (RewindCallbackMode)EditorGUILayout.EnumPopup("OnRewind Callback Mode", _src.rewindCallbackMode);
            GUILayout.Space(-5);
            GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUIUtility.labelWidth + 4);
                EditorGUILayout.HelpBox(
                    _src.rewindCallbackMode == RewindCallbackMode.FireIfPositionChanged
                        ? "When calling Rewind or PlayBackwards/SmoothRewind, OnRewind callbacks will be fired only if the tween isn't already rewinded"
                        : _src.rewindCallbackMode == RewindCallbackMode.FireAlwaysWithRewind
                            ? "When calling Rewind, OnRewind callbacks will always be fired, even if the tween is already rewinded."
                            : "When calling Rewind or PlayBackwards/SmoothRewind, OnRewind callbacks will always be fired, even if the tween is already rewinded",
                    MessageType.None
                );
            GUILayout.EndHorizontal();
            _src.showUnityEditorReport = EditorGUILayout.Toggle("Editor Report", _src.showUnityEditorReport);
            _src.logBehaviour = (LogBehaviour)EditorGUILayout.EnumPopup("Log Behaviour", _src.logBehaviour);
            _src.drawGizmos = EditorGUILayout.Toggle("Draw Path Gizmos", _src.drawGizmos);
            DOTweenSettings.SettingsLocation prevSettingsLocation = _src.storeSettingsLocation;
            _src.storeSettingsLocation = (DOTweenSettings.SettingsLocation)EditorGUILayout.Popup("Settings Location", (int)_src.storeSettingsLocation, _settingsLocation);
            if (_src.storeSettingsLocation != prevSettingsLocation) {
                if (_src.storeSettingsLocation == DOTweenSettings.SettingsLocation.DemigiantDirectory && EditorUtils.demigiantDir == null) {
                    EditorUtility.DisplayDialog("Change DOTween Settings Location", "Demigiant directory not present (must be the parent of DOTween's directory)", "Ok");
                    if (prevSettingsLocation == DOTweenSettings.SettingsLocation.DemigiantDirectory) {
                        _src.storeSettingsLocation = DOTweenSettings.SettingsLocation.AssetsDirectory;
                        Connect(true);
                    } else _src.storeSettingsLocation = prevSettingsLocation;
                } else Connect(true);
            }
            GUILayout.Space(8);
            GUILayout.Label("DEFAULTS ▼");
            _src.defaultRecyclable = EditorGUILayout.Toggle("Recycle Tweens", _src.defaultRecyclable);
            _src.defaultAutoPlay = (AutoPlay)EditorGUILayout.EnumPopup("AutoPlay", _src.defaultAutoPlay);
            _src.defaultUpdateType = (UpdateType)EditorGUILayout.EnumPopup("Update Type", _src.defaultUpdateType);
            _src.defaultTimeScaleIndependent = EditorGUILayout.Toggle("TimeScale Independent", _src.defaultTimeScaleIndependent);
            _src.defaultEaseType = (Ease)EditorGUILayout.EnumPopup("Ease", _src.defaultEaseType);
            _src.defaultEaseOvershootOrAmplitude = EditorGUILayout.FloatField("Ease Overshoot", _src.defaultEaseOvershootOrAmplitude);
            _src.defaultEasePeriod = EditorGUILayout.FloatField("Ease Period", _src.defaultEasePeriod);
            _src.defaultAutoKill = EditorGUILayout.Toggle("AutoKill", _src.defaultAutoKill);
            _src.defaultLoopType = (LoopType)EditorGUILayout.EnumPopup("Loop Type", _src.defaultLoopType);
            GUILayout.Space(8);
            _src.debugMode = EditorGUIUtils.ToggleButton(_src.debugMode, new GUIContent("DEBUG MODE", "Turns debug mode options on/off"), true);
            if (_src.debugMode) {
                GUILayout.BeginVertical(GUI.skin.box);
                EditorGUI.BeginDisabledGroup(!_src.useSafeMode && _src.logBehaviour != LogBehaviour.ErrorsOnly);
                _src.debugStoreTargetId = EditorGUILayout.Toggle("Store GameObject's ID", _src.debugStoreTargetId);
                GUILayout.Label(
                    "<b>Requires Safe Mode to be active + Default or Verbose LogBehaviour:</b>" +
                    " when using DO shortcuts stores the relative gameObject's name so it can be returned along the warning logs" +
                    " (helps with a clearer identification of the warning's target)",
                    EditorGUIUtils.wordWrapRichTextLabelStyle
                );
                EditorGUI.EndDisabledGroup();
                GUILayout.EndVertical();
            }
        }

        // ===================================================================================
        // METHODS ---------------------------------------------------------------------------

        public static DOTweenSettings GetDOTweenSettings()
        {
            return ConnectToSource(null, false, false);
        }

        static DOTweenSettings ConnectToSource(DOTweenSettings src, bool createIfMissing, bool fullSetup)
        {
            LocationData assetsLD = new LocationData(EditorUtils.assetsPath + EditorUtils.pathSlash + "Resources");
            LocationData dotweenLD = new LocationData(EditorUtils.dotweenDir + "Resources");
            bool hasDemigiantDir = EditorUtils.demigiantDir != null;
            LocationData demigiantLD = hasDemigiantDir ? new LocationData(EditorUtils.demigiantDir + "Resources") : new LocationData();

            if (src == null) {
                // Load eventual existing settings
                src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(assetsLD.adbFilePath, false);
                if (src == null) src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(dotweenLD.adbFilePath, false);
                if (src == null && hasDemigiantDir) src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(demigiantLD.adbFilePath, false);
            }
            if (src == null) {
                // Settings don't exist.
                if (!createIfMissing) return null; // Stop here
                // Create it in external folder
                if (!Directory.Exists(assetsLD.dir)) AssetDatabase.CreateFolder(assetsLD.adbParentDir, "Resources");
                src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(assetsLD.adbFilePath, true);
            }

            if (fullSetup) {
                // Move eventual settings from previous location and setup everything correctly
                DOTweenSettings.SettingsLocation settingsLoc = src.storeSettingsLocation;
                switch (settingsLoc) {
                case DOTweenSettings.SettingsLocation.AssetsDirectory:
                    src = MoveSrc(new[] { dotweenLD, demigiantLD }, assetsLD);
                    break;
                case DOTweenSettings.SettingsLocation.DOTweenDirectory:
                    src = MoveSrc(new[] { assetsLD, demigiantLD }, dotweenLD);
                    break;
                case DOTweenSettings.SettingsLocation.DemigiantDirectory:
                    src = MoveSrc(new[] { assetsLD, dotweenLD }, demigiantLD);
                    break;
                }
            }

            return src;
        }

        void Connect(bool forceReconnect = false)
        {
            if (_src != null && !forceReconnect) return;
            _src = ConnectToSource(_src, true, true);

//            LocationData assetsLD = new LocationData(EditorUtils.assetsPath + EditorUtils.pathSlash + "Resources");
//            LocationData dotweenLD = new LocationData(EditorUtils.dotweenDir + "Resources");
//            bool hasDemigiantDir = EditorUtils.demigiantDir != null;
//            LocationData demigiantLD = hasDemigiantDir ? new LocationData(EditorUtils.demigiantDir + "Resources") : new LocationData();
//
//            if (_src == null) {
//                // Load eventual existing settings
//                _src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(assetsLD.adbFilePath, false);
//                if (_src == null) _src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(dotweenLD.adbFilePath, false);
//                if (_src == null && hasDemigiantDir) _src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(demigiantLD.adbFilePath, false);
//            }
//            if (_src == null) {
//                // Settings don't exist.
//                if (!createSrcIfMissing) return; // Stop here
//                // Create it in external folder
//                if (!Directory.Exists(assetsLD.dir)) AssetDatabase.CreateFolder(assetsLD.adbParentDir, "Resources");
//                _src = EditorUtils.ConnectToSourceAsset<DOTweenSettings>(assetsLD.adbFilePath, true);
//            }
//
//            // Move eventual settings from previous location and setup everything correctly
//            DOTweenSettings.SettingsLocation settingsLoc = _src.storeSettingsLocation;
//            switch (settingsLoc) {
//            case DOTweenSettings.SettingsLocation.AssetsDirectory:
//                MoveSrc(new[] { dotweenLD, demigiantLD }, assetsLD);
//                break;
//            case DOTweenSettings.SettingsLocation.DOTweenDirectory:
//                MoveSrc(new[] { assetsLD, demigiantLD }, dotweenLD);
//                break;
//            case DOTweenSettings.SettingsLocation.DemigiantDirectory:
//                MoveSrc(new[] { assetsLD, dotweenLD }, demigiantLD);
//                break;
//            }
        }

        static DOTweenSettings MoveSrc(LocationData[] from, LocationData to)
        {
            if (!Directory.Exists(to.dir)) AssetDatabase.CreateFolder(to.adbParentDir, "Resources");
            foreach (LocationData ld in from) {
                if (File.Exists(ld.filePath)) {
                    // Move external src file to correct folder
                    AssetDatabase.MoveAsset(ld.adbFilePath, to.adbFilePath);
                    // Delete external settings
                    AssetDatabase.DeleteAsset(ld.adbFilePath);
                    // Check if external Resources folder is empty and in case delete it
                    if (Directory.GetDirectories(ld.dir).Length == 0 && Directory.GetFiles(ld.dir).Length == 0) {
                        AssetDatabase.DeleteAsset(EditorUtils.FullPathToADBPath(ld.dir));
                    }
                }
            }
            return EditorUtils.ConnectToSourceAsset<DOTweenSettings>(to.adbFilePath, true);
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // ||| INTERNAL CLASSES ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        struct LocationData
        {
            public string dir; // without final slash
            public string filePath;
            public string adbFilePath;
            public string adbParentDir; // without final slash

            public LocationData(string srcDir) : this()
            {
                dir = srcDir;
                filePath = dir + EditorUtils.pathSlash + DOTweenSettings.AssetFullFilename;
                adbFilePath = EditorUtils.FullPathToADBPath(filePath);
                adbParentDir = EditorUtils.FullPathToADBPath(dir.Substring(0, dir.LastIndexOf(EditorUtils.pathSlash)));
            }
        }
    }
}