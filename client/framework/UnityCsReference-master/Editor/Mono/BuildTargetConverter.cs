// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using UnityEngine;

namespace UnityEditor
{
    internal class BuildTargetConverter
    {
        public static RuntimePlatform? TryConvertToRuntimePlatform(BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.Android:
                    return RuntimePlatform.Android;
                case BuildTarget.PS4:
                    return RuntimePlatform.PS4;
                case BuildTarget.StandaloneLinux64:
                    return RuntimePlatform.LinuxPlayer;
                case BuildTarget.StandaloneOSX:
                    return RuntimePlatform.OSXPlayer;
                case BuildTarget.StandaloneWindows:
                    return RuntimePlatform.WindowsPlayer;
                case BuildTarget.StandaloneWindows64:
                    return RuntimePlatform.WindowsPlayer;
                case BuildTarget.Switch:
                    return RuntimePlatform.Switch;
                case BuildTarget.WSAPlayer:
                    return RuntimePlatform.WSAPlayerARM;
                case BuildTarget.XboxOne:
                    return RuntimePlatform.XboxOne;
                case BuildTarget.iOS:
                    return RuntimePlatform.IPhonePlayer;
                case BuildTarget.tvOS:
                    return RuntimePlatform.tvOS;
                case BuildTarget.WebGL:
                    return RuntimePlatform.WebGLPlayer;
                case BuildTarget.Lumin:
                    return RuntimePlatform.Lumin;
                default:
                    return null;
            }
        }
    }
}
