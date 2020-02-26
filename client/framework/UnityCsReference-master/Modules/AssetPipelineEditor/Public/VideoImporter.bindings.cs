// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using Object = UnityEngine.Object;

namespace UnityEditor
{

    // AssetImporter for importing VideoClip
    [NativeHeader("Editor/Src/Video/VideoClipTranscode.h")]
    public enum VideoCodec
    {
        Auto = 0,
        H264 = 1,
        H265 = 3,
        VP8 = 2,
    }

    [NativeHeader("Modules/Video/Public/Base/VideoMediaTypes.h")]
    public enum VideoBitrateMode
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    [NativeHeader("Editor/Src/Video/VideoClipTranscode.h")]
    public enum VideoDeinterlaceMode
    {
        Off = 0,
        Even = 1,
        Odd = 2
    }

    [NativeHeader("Editor/Src/Video/VideoClipTranscode.h")]
    internal enum VideoColorSpace
    {
        sRGB = 0,
        Linear = 3,
    }

    [NativeHeader("Modules/AssetPipelineEditor/Public/VideoClipImporter.h")]
    public enum VideoResizeMode
    {
        OriginalSize = 0,
        ThreeQuarterRes = 1,
        HalfRes = 2,
        QuarterRes = 3,
        Square1024 = 4,
        Square512 = 5,
        Square256 = 6,
        CustomSize = 7
    }

    [NativeHeader("Editor/Src/Video/VideoClipTranscode.h")]
    public enum VideoSpatialQuality
    {
        LowSpatialQuality = 0,
        MediumSpatialQuality = 1,
        HighSpatialQuality = 2
    }

    [NativeHeader("Modules/AssetPipelineEditor/Public/VideoClipImporter.h")]
    public enum VideoEncodeAspectRatio
    {
        NoScaling = 0,
        Stretch   = 5
    }

    [RequiredByNativeCode]
    [NativeAsStruct]
    [StructLayout(LayoutKind.Sequential)]
    [NativeType(Header = "Modules/AssetPipelineEditor/Public/VideoClipImporter.h")]
    [Serializable]
    public partial class VideoImporterTargetSettings
    {
        public bool                   enableTranscoding;
        public VideoCodec             codec;
        [NativeName("resizeFormat")]
        public VideoResizeMode        resizeMode;
        public VideoEncodeAspectRatio aspectRatio;
        public int                    customWidth;
        public int                    customHeight;
        public VideoBitrateMode       bitrateMode;
        public VideoSpatialQuality    spatialQuality;
    }

    [NativeConditional("ENABLE_VIDEO")]
    [NativeHeader("Modules/AssetPipelineEditor/Public/VideoClipImporter.h")]
    [NativeHeader("Modules/AssetPipelineEditor/Public/VideoClipImporter.bindings.h")]
    public partial class VideoClipImporter : AssetImporter
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("VideoClipImporter.quality has no effect anymore (was only used for MovieTexture which is removed)", false)]
        public float quality { get { return 1.0f; } set {} }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("VideoClipImporter.linearColor has no effect anymore (was only used for MovieTexture which is removed)", false)]
        public bool linearColor { get { return false; } set {} }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("VideoClipImporter.useLegacyImporter has no effect anymore (was only used for MovieTexture which is removed)", false)]
        public bool useLegacyImporter { get { return false; } set {} }

        public extern ulong sourceFileSize { get; }
        public extern ulong outputFileSize { get; }

        public extern int frameCount { get; }

        public extern double frameRate { get; }

        // Encode RGB / RGBA Video
        [NativeProperty("EncodeAlpha")]
        public extern bool keepAlpha { get; set; }
        public extern bool sourceHasAlpha { get; }

        // Interlaced / Progressive Video
        [NativeProperty("Deinterlace")]
        public extern VideoDeinterlaceMode deinterlaceMode { get; set; }

        // Flip Image Vertically
        public extern bool flipVertical { get; set; }
        // Flip Image Horizontal
        public extern bool flipHorizontal { get; set; }

        // Import Audio
        public extern bool importAudio { get; set; }

        [NativeName("sRGBClip")]
        public extern bool sRGBClip { get; set; }

        public VideoImporterTargetSettings defaultTargetSettings
        {
            get { return GetTargetSettings(VideoClipImporter.defaultTargetName); }
            set { SetTargetSettings(VideoClipImporter.defaultTargetName, value); }
        }

        public VideoImporterTargetSettings GetTargetSettings(string platform)
        {
            BuildTargetGroup platformGroup = GetBuildTargetGroup("GetTargetSetting", platform);
            return Internal_GetTargetSettings(platformGroup);
        }

        internal VideoImporterTargetSettings Internal_GetTargetSettings(BuildTargetGroup group)
        {
            return Private_GetTargetSettings(group) as VideoImporterTargetSettings;
        }

        [FreeFunction(Name = "VideoImporterBindings::GetTargetSettings", HasExplicitThis = true)]
        private extern object Private_GetTargetSettings(BuildTargetGroup group);

        public void SetTargetSettings(string platform, VideoImporterTargetSettings settings)
        {
            var platformGroup = GetBuildTargetGroup("SetTargetSettings", platform);
            Internal_SetTargetSettings(platformGroup, settings);
        }

        [NativeName("SetTargetSettings")]
        internal extern void Internal_SetTargetSettings(BuildTargetGroup group, VideoImporterTargetSettings settings);

        public void ClearTargetSettings(string platform)
        {
            var platformGroup = GetBuildTargetGroup("ClearTargetSettings", platform, false);
            Internal_ClearTargetSettings(platformGroup);
        }

        private BuildTargetGroup GetBuildTargetGroup(string methodName, string platform, bool acceptDefault = true)
        {
            if (!acceptDefault &&
                platform.Equals(VideoClipImporter.defaultTargetName, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Cannot call VideoClipImporter." + methodName + " for the default VideoImporterTargetSettings.");

            BuildTargetGroup platformGroup = BuildPipeline.GetBuildTargetGroupByName(platform);
            if (!platform.Equals(VideoClipImporter.defaultTargetName, StringComparison.OrdinalIgnoreCase) && platformGroup == BuildTargetGroup.Unknown)
            {
                var platformList = "'Standalone', 'Android', 'iOS', 'Lumin', 'PS4', 'Switch', 'tvOS', 'WebGL', 'WSA', 'WebGL' or 'XboxOne'";
                if (acceptDefault)
                    platformList = "'Default', " + platformList;

                throw new ArgumentException(
                    "Unknown platform passed to VideoClipImporter." + methodName + " (" + platform + "), please use one of " +
                    platformList + ".");
            }

            return platformGroup;
        }

        [NativeName("ClearTargetSettings")]
        internal extern void Internal_ClearTargetSettings(BuildTargetGroup group);

        // Preview
        [NativeName("StartPreview")]
        public extern void PlayPreview();
        public extern void StopPreview();
        public extern bool isPlayingPreview
        {
            [NativeName("Started")]
            get;
        }
        public extern Texture GetPreviewTexture();

        internal extern static string defaultTargetName
        {
            [NativeName("DefaultSettingsName")]
            get;
        }

        [FreeFunction("VideoImporterBindings::EqualsDefaultTargetSettings", HasExplicitThis = true)]
        internal extern bool EqualsDefaultTargetSettings(VideoImporterTargetSettings settings);

        public extern string GetResizeModeName(VideoResizeMode mode);

        [NativeName("GetDefaultResizeWidth")]
        public extern int GetResizeWidth(VideoResizeMode mode);
        [NativeName("GetDefaultResizeHeight")]
        public extern int GetResizeHeight(VideoResizeMode mode);

        public extern ushort sourceAudioTrackCount { get; }
        public extern ushort GetSourceAudioChannelCount(ushort audioTrackIdx);
        public extern uint GetSourceAudioSampleRate(ushort audioTrackIdx);

        public extern int pixelAspectRatioNumerator { get; }
        public extern int pixelAspectRatioDenominator { get; }

        public extern bool transcodeSkipped { get; }

        [NativeMethod("operator==")]
        extern public bool Equals(VideoClipImporter rhs);
    }

}
