// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
    internal class AppStatusBar : GUIView
    {
        static readonly NumberFormatInfo percentageFormat = new CultureInfo("en-US", false).NumberFormat;

        static class Styles
        {
            public const int spacing = 4;

            public static readonly GUILayoutOption progressBarWidth = GUILayout.Width(180);
            public static readonly GUILayoutOption progressLabelMaxWidth = GUILayout.MaxWidth(230);
            public static readonly GUIStyle background = "AppToolbar";
            public static readonly GUIStyle statusLabel = EditorStyles.FromUSS(EditorStyles.label, "status-bar-label");
            public static readonly GUIStyle statusIcon = "StatusBarIcon";

            public static readonly GUIStyle progressBar = EditorStyles.FromUSS(EditorStyles.progressBarBar, "status-bar-progress");
            public static readonly GUIStyle progressBarAlmostDone = EditorStyles.FromUSS(progressBar, "status-bar-progress-almost-done");
            public static readonly GUIStyle progressBarUnresponsive = EditorStyles.FromUSS(progressBar, "status-bar-progress-unresponsive");
            public static readonly GUIStyle progressBarIndefinite = EditorStyles.FromUSS(progressBar, "status-bar-progress-indefinite");
            public static readonly GUIStyle progressBarBack = EditorStyles.FromUSS(EditorStyles.progressBarBack, "status-bar-progress-background");
            public static readonly GUIStyle progressBarText = EditorStyles.FromUSS(statusLabel, "status-bar-progress-text");

            public static readonly GUIContent[] statusWheel;
            public static readonly GUIContent assemblyLock = EditorGUIUtility.IconContent("AssemblyLock", "|Assemblies are currently locked. Compilation will resume once they are unlocked");
            public static readonly GUIContent progressIcon = EditorGUIUtility.IconContent("Progress", "Open progress details...");
            public static readonly GUIContent autoGenerateLightOn = EditorGUIUtility.IconContent("AutoLightbakingOn", "Auto Generate Lighting On");
            public static readonly GUIContent autoGenerateLightOff = EditorGUIUtility.IconContent("AutoLightbakingOff", "Auto Generate Lighting Off");

            static Styles()
            {
                percentageFormat.PercentDecimalDigits = 0;
                statusWheel = new GUIContent[12];
                for (int i = 0; i < 12; i++)
                    statusWheel[i] = EditorGUIUtility.IconContent("WaitSpin" + i.ToString("00"));
            }
        }

        private static AppStatusBar s_AppStatusBar;

        private string m_MiniMemoryOverview = "";
        private bool? m_autoLightBakingOn;
        private GUIContent m_ProgressStatus = new GUIContent();
        private GUIContent m_ProgressPercentageStatus = new GUIContent();
        private bool m_CurrentProgressNotResponding = false;
        private int m_LastProgressId = -1;
        private float m_LastElapsedTime = 0f;

        private ManagedDebuggerToggle m_ManagedDebuggerToggle = null;
        private CacheServerToggle m_CacheServerToggle = null;

        private bool showBakeMode
        {
            get
            {
                var settings = Lightmapping.GetLightingSettingsOrDefaultsFallback();
                return settings.bakedGI || settings.realtimeGI;
            }
        }

        private bool showProgress
        {
            get
            {
                return Progress.running && m_LastElapsedTime > 0.5f;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            s_AppStatusBar = this;
            m_autoLightBakingOn = GetBakeMode();
            m_ManagedDebuggerToggle = new ManagedDebuggerToggle();
            m_CacheServerToggle = new CacheServerToggle();

            Progress.added += RefreshProgressBar;
            Progress.removed += RefreshProgressBar;
            Progress.updated += RefreshProgressBar;
        }

        protected override void OnDisable()
        {
            Progress.added -= RefreshProgressBar;
            Progress.removed -= RefreshProgressBar;
            Progress.updated -= RefreshProgressBar;
            base.OnDisable();
        }

        protected void OnInspectorUpdate()
        {
            string miniOverview = UnityEditorInternal.ProfilerDriver.miniMemoryOverview;
            bool? autoLightBakingOn = GetBakeMode();

            if ((Unsupported.IsDeveloperMode() && m_MiniMemoryOverview != miniOverview) || (m_autoLightBakingOn != autoLightBakingOn))
            {
                m_MiniMemoryOverview = miniOverview;
                m_autoLightBakingOn = autoLightBakingOn;

                Repaint();
            }
        }

        private void DelayCheckProgressUnresponsive()
        {
            EditorApplication.delayCall -= CheckProgressUnresponsive;
            EditorApplication.delayCall += CheckProgressUnresponsive;
        }

        private void CheckProgressUnresponsive()
        {
            if (Progress.running)
            {
                var progressItem = Progress.GetProgressById(m_LastProgressId);
                if (progressItem != null && !progressItem.responding)
                    RefreshProgressBar(new[] {progressItem});

                DelayCheckProgressUnresponsive();
            }
            else
            {
                m_LastProgressId = -1;
            }
        }

        protected override void OldOnGUI()
        {
            ConsoleWindow.LoadIcons();

            GUI.color = EditorApplication.isPlayingOrWillChangePlaymode ? HostView.kPlayModeDarken : Color.white;

            if (Event.current.type == EventType.Repaint)
                Styles.background.Draw(new Rect(0, 0, position.width, position.height), false, false, false, false);

            GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
            {
                GUILayout.Space(2);
                DrawStatusText();
                GUILayout.FlexibleSpace();
                DrawSpecialModeLabel();
                DrawProgressBar();
                DrawDebuggerToggle();
                DrawCacheServerToggle();
                DrawBakeMode();
                DrawRefreshStatus();
            }
            GUILayout.EndHorizontal();

            DoWindowDecorationEnd();
            EditorGUI.ShowRepaints();
        }

        private void DrawRefreshStatus()
        {
            bool compiling = EditorApplication.isCompiling;
            bool assembliesLocked = !EditorApplication.CanReloadAssemblies();

            if (compiling || showProgress)
            {
                if (assembliesLocked)
                {
                    GUILayout.Button(Styles.assemblyLock, Styles.statusIcon);
                }
                else
                {
                    int frame = (int)Mathf.Repeat(Time.realtimeSinceStartup * 10, 11.99f);
                    GUILayout.Button(Styles.statusWheel[frame], Styles.statusIcon);
                }
            }
            else
            {
                if (GUILayout.Button(Styles.progressIcon, Styles.statusIcon))
                    Progress.ShowDetails();

                var buttonRect = GUILayoutUtility.GetLastRect();
                EditorGUIUtility.AddCursorRect(buttonRect, MouseCursor.Link);
            }
        }

        private string m_SpecialModeLabel = "";
        private void DrawSpecialModeLabel()
        {
            if (Unsupported.IsBleedingEdgeBuild())
            {
                GUILayout.Space(k_SpaceBeforeProgress);
                m_SpecialModeLabel = "THIS IS AN UNTESTED BLEEDINGEDGE UNITY BUILD";
                var backup = GUI.color;
                GUI.color = Color.yellow;
                GUILayout.Label(m_SpecialModeLabel);
                GUI.color = backup;
            }
            else if (Unsupported.IsDeveloperMode())
            {
                GUILayout.Space(k_SpaceBeforeProgress);
                m_SpecialModeLabel = m_MiniMemoryOverview;
                GUILayout.Label(m_SpecialModeLabel, Styles.statusLabel);
                EditorGUIUtility.CleanCache(m_MiniMemoryOverview);
            }
            else
                m_SpecialModeLabel = "";
        }

        float k_SpaceBeforeProgress = 15;
        float k_SpaceAfterProgress = 4;
        private void DrawProgressBar()
        {
            if (!showProgress)
                return;

            GUILayout.Space(k_SpaceBeforeProgress);
            if (GUILayout.Button(m_ProgressStatus, Styles.statusLabel))
                Progress.ShowDetails();

            var buttonRect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(buttonRect, MouseCursor.Link);

            // Define the progress styles to be used based on the current progress state.
            Color oldColor  = GUI.color;
            if (m_CurrentProgressNotResponding)
            {
                var fade = Mathf.Min(Mathf.Max(0.25f, Mathf.Cos((float)EditorApplication.timeSinceStartup * 2.0f) / 2.0f + 0.75f), 0.85f);
                GUI.color = new Color(fade, fade, fade);
            }
            var globalProgress = Progress.globalProgress;
            var progressBarStyle = GetProgressBarStyle(globalProgress);
            var progressBarContent = GetProgressBarContent(globalProgress, m_ProgressPercentageStatus);
            var progressRect = EditorGUILayout.GetControlRect(false, position.height, Styles.progressBarBack, Styles.progressBarWidth);
            if (EditorGUI.ProgressBar(progressRect, globalProgress, progressBarContent, true,
                Styles.progressBarBack, progressBarStyle, Styles.progressBarText))
                Progress.ShowDetails();

            GUI.color = oldColor;

            if (globalProgress == -1.0f)
                EditorApplication.delayCall += () => Repaint();

            EditorGUIUtility.AddCursorRect(progressRect, MouseCursor.Link);
            GUILayout.Space(k_SpaceAfterProgress);
        }

        private GUIContent GetProgressBarContent(float globalProgress, GUIContent defaultContent)
        {
            GUIContent progressBarContent = defaultContent;
            if (m_CurrentProgressNotResponding || globalProgress < 0.15f)
                progressBarContent = GUIContent.none;

            return progressBarContent;
        }

        private GUIStyle GetProgressBarStyle(float globalProgress)
        {
            var progressBarStyle = Styles.progressBar;

            if (globalProgress == -1.0f)
                progressBarStyle = Styles.progressBarIndefinite;
            else if (m_CurrentProgressNotResponding)
                progressBarStyle = Styles.progressBarUnresponsive;
            else if (globalProgress >= 0.99f)
                progressBarStyle = Styles.progressBarAlmostDone;

            return progressBarStyle;
        }

        private void DrawBakeMode()
        {
            if (!showBakeMode)
                return;

            if (GUILayout.Button(GetBakeModeIcon(m_autoLightBakingOn), Styles.statusIcon))
            {
                Event.current.Use();
                LightingWindow.CreateLightingWindow();
            }

            var buttonRect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(buttonRect, MouseCursor.Link);
        }

        private void DrawDebuggerToggle()
        {
            var rect = EditorGUILayout.GetControlRect(GUILayout.Width(m_ManagedDebuggerToggle.GetWidth() - 3));
            m_ManagedDebuggerToggle.OnGUI(rect.x, 0);
        }

        private void DrawCacheServerToggle()
        {
            var rect = EditorGUILayout.GetControlRect(GUILayout.Width(m_CacheServerToggle.GetWidth()));
            m_CacheServerToggle.OnGUI(rect.x, 0);
        }

        private void DrawStatusText()
        {
            string statusText = LogEntries.GetStatusText();
            if (String.IsNullOrEmpty(statusText))
                return;

            int mask = LogEntries.GetStatusMask();
            GUIStyle errorStyle = ConsoleWindow.GetStatusStyleForErrorMode(mask);

            Texture2D icon = ConsoleWindow.GetIconForErrorMode(mask, false);
            GUILayout.Label(icon, errorStyle);

            var iconRect = GUILayoutUtility.GetLastRect();

            GUILayout.Space(2);
            GUILayout.BeginVertical();
            GUILayout.Space(1);

            GUILayout.Label(statusText, errorStyle, GetStatusTextLayoutOption(m_ProgressStatus, (icon != null ? icon.width : 0) + 6));

            // Handle status bar click
            if (Event.current.type == EventType.MouseDown)
            {
                var labelRect = GUILayoutUtility.GetLastRect();
                if (iconRect.Contains(Event.current.mousePosition) || labelRect.Contains(Event.current.mousePosition))
                {
                    Event.current.Use();
                    LogEntries.ClickStatusBar(Event.current.clickCount);
                    GUIUtility.ExitGUI();
                }
            }

            GUILayout.EndVertical();
        }

        private void RefreshProgressBar(Progress.Item[] progressItems)
        {
            var taskCount = Progress.GetRunningProgressCount();
            if (taskCount == 0)
            {
                m_LastProgressId = -1;
                m_LastElapsedTime = 0f;
                m_CurrentProgressNotResponding = false;
                m_ProgressStatus.text = String.Empty;
                m_ProgressPercentageStatus.text = String.Empty;
            }
            else
            {
                var currentItem = progressItems[0];
                if (!String.IsNullOrEmpty(currentItem.description))
                    m_ProgressStatus.tooltip = currentItem.name + "\r\n" + currentItem.description;
                m_ProgressPercentageStatus.text = Progress.globalProgress.ToString("P", percentageFormat);

                var remainingTimeText = "";
                if (Progress.EnumerateItems().Any(item => item.timeDisplayMode == Progress.TimeDisplayMode.ShowRemainingTime) && Progress.EnumerateItems().All(item => !item.indefinite) && Progress.globalRemainingTime.TotalSeconds > 0)
                {
                    remainingTimeText = $" [{Progress.globalRemainingTime:g}]";
                }

                if (taskCount > 1)
                    m_ProgressStatus.text = $"Multiple tasks ({taskCount}){remainingTimeText}";
                else
                    m_ProgressStatus.text = $"{currentItem.name}{remainingTimeText}";


                m_LastProgressId = currentItem.id;
                m_CurrentProgressNotResponding = true;
                for (int i = 0; i < progressItems.Length; ++i)
                {
                    if (!progressItems[i].responding)
                    {
                        m_LastProgressId = progressItems[i].id;
                        continue;
                    }
                    m_CurrentProgressNotResponding = false;
                    break;
                }

                m_LastElapsedTime = Mathf.Max(m_LastElapsedTime, currentItem.elapsedTime);
                if (m_CurrentProgressNotResponding)
                    m_LastElapsedTime = float.MaxValue;
            }
            RepaintProgress(progressItems);
        }

        private void RepaintProgress(Progress.Item[] progressItems)
        {
            bool hasSynchronous = false;
            bool hasIndefinite = false;
            foreach (var item in progressItems)
            {
                if ((item.options & Progress.Options.Synchronous) == Progress.Options.Synchronous)
                {
                    hasSynchronous = true;
                    break;
                }
                if (item.indefinite)
                    hasIndefinite = true;
            }
            if (hasSynchronous)
            {
                RepaintImmediately();
            }
            else if (hasIndefinite)
            {
                Repaint();
            }
            else
            {
                Repaint();
                DelayCheckProgressUnresponsive();
            }
        }

        private GUILayoutOption GetStatusTextLayoutOption(GUIContent progressContent, float consoleIconWidth)
        {
            int iconWidth = 25;
            float specialModeLabelWidth = Styles.statusLabel.CalcSize(new GUIContent(m_SpecialModeLabel)).x + k_SpaceBeforeProgress + 8;
            float debugToggleWidth = m_ManagedDebuggerToggle.GetWidth() + 1;
            float statusRightReservedSpace = specialModeLabelWidth + debugToggleWidth + (showBakeMode ? (iconWidth) : 0) + iconWidth;
            if (!showProgress)
                return GUILayout.MaxWidth(position.width - statusRightReservedSpace - consoleIconWidth);

            var progressStatusContentWidth = Styles.statusLabel.CalcSize(progressContent).x;
            return GUILayout.MaxWidth(position.width - statusRightReservedSpace - k_SpaceBeforeProgress - 8 - progressStatusContentWidth - (float)Styles.progressBarWidth.value - k_SpaceAfterProgress - consoleIconWidth);
        }

        private GUIContent GetBakeModeIcon(bool? bakeMode)
        {
            if (!bakeMode.HasValue)
                return new GUIContent();
            else if (bakeMode.Value)
                return Styles.autoGenerateLightOn;
            else
                return Styles.autoGenerateLightOff;
        }

        private bool? GetBakeMode()
        {
            if (!showBakeMode)
                return null;

            return Lightmapping.GetLightingSettingsOrDefaultsFallback().autoGenerate;
        }

        [RequiredByNativeCode]
        public static void StatusChanged()
        {
            if (s_AppStatusBar)
                s_AppStatusBar.Repaint();
        }

        [RequiredByNativeCode]
        internal static void StatusChangedImmediate()
        {
            if (s_AppStatusBar)
                s_AppStatusBar.RepaintImmediately();
        }
    }
}
