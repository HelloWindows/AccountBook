// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using UnityEditor;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditorInternal.VR
{
    [RequiredByNativeCode]
    [StructLayout(LayoutKind.Sequential)]
    [NativeType(CodegenOptions = CodegenOptions.Custom)]
    public partial struct VRDeviceInfoEditor
    {
        public string deviceNameKey;
        public string deviceNameUI;
        public string externalPluginName;
        public bool supportsEditorMode;
        public bool inListByDefault;
    }

    [NativeHeader("Modules/VREditor/VREditor.bindings.h")]
    public sealed partial class VREditor
    {
        extern public static VRDeviceInfoEditor[] GetAllVRDeviceInfo(BuildTargetGroup targetGroup);

        extern public static VRDeviceInfoEditor[] GetAllVRDeviceInfoByTarget(BuildTarget target);

        extern public static bool GetVREnabledOnTargetGroup(BuildTargetGroup targetGroup);

        extern public static void SetVREnabledOnTargetGroup(BuildTargetGroup targetGroup, bool value);

        extern public static string[] GetVREnabledDevicesOnTargetGroup(BuildTargetGroup targetGroup);

        extern public static string[] GetVREnabledDevicesOnTarget(BuildTarget target);

        [NativeMethod("SetVREnabledDevicesOnTargetGroup")]
        extern public static void NativeSetVREnabledDevicesOnTargetGroup(BuildTargetGroup targetGroup, string[] devices);
        public static void SetVREnabledDevicesOnTargetGroup(BuildTargetGroup targetGroup, string[] devices)
        {
            NativeSetVREnabledDevicesOnTargetGroup(targetGroup, devices);
            SetDeviceListDirty(targetGroup);
        }
    }
}

namespace UnityEditorInternal
{
    [NativeHeader("Runtime/Misc/PlayerSettings.h")]
    [StaticAccessor("GetPlayerSettings()", StaticAccessorType.Dot)]
    internal class PlayerSettingsOculus
    {
        public static extern bool sharedDepthBuffer
        {
            [NativeMethod("GetOculusSharedDepthBufferEnabled")]
            get;
            [NativeMethod("SetOculusSharedDepthBufferEnabled")]
            set;
        }

        public static extern bool dashSupport
        {
            [NativeMethod("GetOculusDashSupportEnabled")]
            get;
            [NativeMethod("SetOculusDashSupportEnabled")]
            set;
        }

        public static extern bool lowOverheadMode
        {
            [NativeMethod("GetOculusLowOverheadModeEnabled")]
            get;
            [NativeMethod("SetOculusLowOverheadModeEnabled")]
            set;
        }

        public static extern bool protectedContext
        {
            [NativeMethod("GetOculusProtectedContextEnabled")]
            get;
            [NativeMethod("SetOculusProtectedContextEnabled")]
            set;
        }

        public static extern bool v2Signing
        {
            [NativeMethod("GetOculusV2SigningEnabled")]
            get;
            [NativeMethod("SetOculusV2SigningEnabled")]
            set;
        }
    }

    [NativeHeader("Runtime/Misc/PlayerSettings.h")]
    [StaticAccessor("GetPlayerSettings()", StaticAccessorType.Dot)]
    internal class PlayerSettings360StereoCapture
    {
        public static extern bool enable360StereoCapture
        {
            get;
            set;
        }
    }
}

// When Nested classes is supported for bindings, the above Internal only class should be removed and
// the below class should be updated with the proper PlayerSettings calls.
namespace UnityEditor
{
    partial class PlayerSettings
    {
        public class VROculus
        {
            public static bool sharedDepthBuffer
            {
                get { return UnityEditorInternal.PlayerSettingsOculus.sharedDepthBuffer; }
                set { UnityEditorInternal.PlayerSettingsOculus.sharedDepthBuffer = value; }
            }

            public static bool dashSupport
            {
                get { return UnityEditorInternal.PlayerSettingsOculus.dashSupport; }
                set { UnityEditorInternal.PlayerSettingsOculus.dashSupport = value; }
            }

            public static bool lowOverheadMode
            {
                get { return UnityEditorInternal.PlayerSettingsOculus.lowOverheadMode; }
                set { UnityEditorInternal.PlayerSettingsOculus.lowOverheadMode = value; }
            }

            public static bool protectedContext
            {
                get { return UnityEditorInternal.PlayerSettingsOculus.protectedContext; }
                set { UnityEditorInternal.PlayerSettingsOculus.protectedContext = value; }
            }

            public static bool v2Signing
            {
                get { return UnityEditorInternal.PlayerSettingsOculus.v2Signing; }
                set { UnityEditorInternal.PlayerSettingsOculus.v2Signing = value; }
            }
        }
    }

    partial class PlayerSettings
    {
        public static bool enable360StereoCapture
        {
            get { return UnityEditorInternal.PlayerSettings360StereoCapture.enable360StereoCapture; }
            set { UnityEditorInternal.PlayerSettings360StereoCapture.enable360StereoCapture = value; }
        }
    }
}
