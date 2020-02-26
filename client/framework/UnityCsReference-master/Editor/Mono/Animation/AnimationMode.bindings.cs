// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Playables;
using UnityEngine;

using Object = UnityEngine.Object;

namespace UnityEditor
{
    public class AnimationModeDriver : ScriptableObject
    {
        internal delegate bool IsKeyCallback(Object target, string propertyPath);

        internal IsKeyCallback isKeyCallback;

        [UsedByNativeCode]
        internal bool InvokeIsKeyCallback_Internal(Object target, string propertyPath)
        {
            if (isKeyCallback == null)
                return false;

            return isKeyCallback(target, propertyPath);
        }
    }

    [NativeHeader("Editor/Src/Animation/AnimationMode.bindings.h")]
    [NativeHeader("Editor/Src/Animation/EditorCurveBinding.bindings.h")]
    [NativeHeader("Editor/Src/Prefabs/PropertyModification.h")]
    public class AnimationMode
    {
        static private bool s_InAnimationPlaybackMode = false;
        static private bool s_InAnimationRecordMode = false;

        static internal event Action onAnimationRecordingStart;
        static internal event Action onAnimationRecordingStop;

        static private PrefColor s_AnimatedPropertyColor = new PrefColor("Animation/Property Animated", 0.82f, 0.97f, 1.00f, 1.00f, 0.54f, 0.85f, 1.00f, 1.00f);
        static private PrefColor s_RecordedPropertyColor = new PrefColor("Animation/Property Recorded", 1.00f, 0.60f, 0.60f, 1.00f, 1.00f, 0.50f, 0.50f, 1.00f);
        static private PrefColor s_CandidatePropertyColor = new PrefColor("Animation/Property Candidate", 1.00f, 0.70f, 0.60f, 1.00f, 1.00f, 0.67f, 0.43f, 1.00f);

        static public Color animatedPropertyColor { get { return s_AnimatedPropertyColor; } }
        static public Color recordedPropertyColor { get { return s_RecordedPropertyColor; } }
        static public Color candidatePropertyColor { get { return s_CandidatePropertyColor; } }

        static private AnimationModeDriver s_DummyDriver;
        static private AnimationModeDriver DummyDriver()
        {
            if (s_DummyDriver == null)
            {
                s_DummyDriver = ScriptableObject.CreateInstance<AnimationModeDriver>();
                s_DummyDriver.name = "DummyDriver";
            }
            return s_DummyDriver;
        }

        extern public static bool IsPropertyAnimated(Object target, string propertyPath);
        extern internal static bool IsPropertyCandidate(Object target, string propertyPath);

        // Stops animation mode, as used by the animation editor.
        public static void StopAnimationMode()
        {
            Internal_StopAnimationMode(DummyDriver());
        }

        // Stops animation mode, as used by the animation editor.
        public static void StopAnimationMode(AnimationModeDriver driver)
        {
            Internal_StopAnimationMode(driver);
        }

        // Returns true if the editor is currently in animation mode.
        public static bool InAnimationMode()
        {
            return Internal_InAnimationModeNoDriver();
        }

        // Returns true if the editor is currently in animation mode.
        public static bool InAnimationMode(AnimationModeDriver driver)
        {
            return Internal_InAnimationMode(driver);
        }

        // Starts animation mode, as used by the animation editor.
        public static void StartAnimationMode()
        {
            Internal_StartAnimationMode(DummyDriver());
        }

        // Starts animation mode, as used by the animation editor.
        public static void StartAnimationMode(AnimationModeDriver driver)
        {
            Internal_StartAnimationMode(driver);
        }

        // Stops animation playback mode, as used by the animation editor.
        internal static void StopAnimationPlaybackMode()
        {
            s_InAnimationPlaybackMode = false;
        }

        // Returns true if the editor is currently in animation playback mode.
        internal static bool InAnimationPlaybackMode()
        {
            return s_InAnimationPlaybackMode;
        }

        // Starts animation mode, as used by the animation editor playback mode.
        internal static void StartAnimationPlaybackMode()
        {
            s_InAnimationPlaybackMode = true;
        }

        internal static void StopAnimationRecording()
        {
            s_InAnimationRecordMode = false;

            onAnimationRecordingStop?.Invoke();
        }

        internal static bool InAnimationRecording()
        {
            return s_InAnimationRecordMode;
        }

        internal static void StartAnimationRecording()
        {
            s_InAnimationRecordMode = true;

            onAnimationRecordingStart?.Invoke();
        }

        internal static void StartCandidateRecording(AnimationModeDriver driver)
        {
            Internal_StartCandidateRecording(driver);
        }

        [NativeThrows]
        extern internal static void AddCandidate(EditorCurveBinding binding, PropertyModification modification, bool keepPrefabOverride);

        [NativeThrows]
        extern internal static void AddCandidates([NotNull] GameObject gameObject, [NotNull] AnimationClip clip);

        extern internal static void StopCandidateRecording();

        extern internal static bool IsRecordingCandidates();

        [NativeThrows]
        extern public static void BeginSampling();

        [NativeThrows]
        extern public static void EndSampling();

        [NativeThrows]
        extern public static void SampleAnimationClip([NotNull] GameObject gameObject, [NotNull] AnimationClip clip, float time);

        [NativeThrows]
        extern internal static void SampleCandidateClip([NotNull] GameObject gameObject, [NotNull] AnimationClip clip, float time);

        [NativeThrows]
        extern public static void SamplePlayableGraph(PlayableGraph graph, int index, float time);

        [NativeThrows]
        extern public static void AddPropertyModification(EditorCurveBinding binding, PropertyModification modification, bool keepPrefabOverride);

        [NativeThrows]
        extern public static void AddEditorCurveBinding([NotNull] GameObject gameObject, EditorCurveBinding binding);

        [NativeThrows]
        extern internal static void AddTransformTR([NotNull] GameObject root, string path);

        [NativeThrows]
        extern internal static void AddTransformTRS([NotNull] GameObject root, string path);

        [NativeThrows]
        extern internal static void InitializePropertyModificationForGameObject([NotNull] GameObject gameObject, [NotNull] AnimationClip clip);

        [NativeThrows]
        extern internal static void InitializePropertyModificationForObject([NotNull] Object target, [NotNull] AnimationClip clip);

        [NativeThrows]
        extern internal static void RevertPropertyModificationsForGameObject([NotNull] GameObject gameObject);

        [NativeThrows]
        extern internal static void RevertPropertyModificationsForObject([NotNull] Object target);

        // Returns editor curve bindings for animation clip and animator hierarchy that need to be snapshot for animation mode.
        extern internal static EditorCurveBinding[] GetAllBindings([NotNull] GameObject root, [NotNull] AnimationClip clip);

        // Returns editor curve bindings for animation clip that need to be snapshot for animation mode.
        extern internal static EditorCurveBinding[] GetCurveBindings([NotNull] AnimationClip clip);

        // Return editor curve bindings for animator hierarhcy that need to be snapshot for animation mode.
        extern internal static EditorCurveBinding[] GetAnimatorBindings([NotNull] GameObject root);

        extern private static void Internal_StartAnimationMode(Object driver);

        extern private static void Internal_StopAnimationMode(Object driver);

        extern private static bool Internal_InAnimationMode(Object driver);

        extern private static bool Internal_InAnimationModeNoDriver();

        [NativeThrows]
        extern private static void Internal_StartCandidateRecording(Object driver);
    }
}
