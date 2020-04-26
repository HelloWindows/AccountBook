using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTween {
    /// <summary>
    /// 音效混合器
    /// </summary>
    public enum JTweenAudioMixer : int {
        None = 0,
        /// <summary>
        /// 值动画
        /// </summary>
        Float = 1,
    } // end enum JTweenAudioMixer
    
    /// <summary>
    /// 音源
    /// </summary>
    public enum JTweenAudioSource : int {
        None = 0,
        /// <summary>
        /// 渐变
        /// </summary>
        Fade = 1,
        /// <summary>
        /// 音调
        /// </summary>
        Pitch = 2,
    } // end enum JTweenAudioSource
    
    /// <summary>
    /// 相机
    /// </summary>
    public enum JTweenCamera : int {
        None = 0,
        /// <summary>
        /// 方位
        /// </summary>
        Aspect = 1,
        /// <summary>
        /// 颜色
        /// </summary>
        Color = 2,
        /// <summary>
        /// 远端裁剪
        /// </summary>
        FCP = 3,
        /// <summary>
        /// 视场
        /// </summary>
        FOV = 4,
        /// <summary>
        /// 进端裁剪
        /// </summary>
        NCP = 5,
        /// <summary>
        /// 正交投影
        /// </summary>
        OrthoSize = 6,
        /// <summary>
        /// 像素矩形
        /// </summary>
        PixelRect = 7,
        /// <summary>
        /// 矩形
        /// </summary>
        Rect = 8,
        /// <summary>
        /// 摇晃位置
        /// </summary>
        ShakePosition = 9,
        /// <summary>
        /// 摇晃角度
        /// </summary>
        ShakeRotation = 10,
    } // end enum JTweenCamera

    /// <summary>
    /// 灯光
    /// </summary>
    public enum JTweenLight : int {
        None = 0,
        /// <summary>
        /// 颜色
        /// </summary>
        Color = 1,
        /// <summary>
        /// 光照强度
        /// </summary>
        Intensity = 2,
        /// <summary>
        /// 阴影强度
        /// </summary>
        ShadowStrength = 3,
        /// <summary>
        /// 混合颜色
        /// </summary>
        BlendableColor = 4,
    } // end enum JTweenLight

    /// <summary>
    /// 线条渲染
    /// </summary>
    public enum JTweenLineRenderer : int {
        None = 0,
        /// <summary>
        /// 颜色
        /// </summary>
        Color = 1,
    } // end enum JTweenLineRenderer
    
    /// <summary>
    /// 材质
    /// </summary>
    public enum JTweenMaterial : int {
        None = 0,
        /// <summary>
        /// 颜色
        /// </summary>
        Color = 1,
        /// <summary>
        /// 渐变
        /// </summary>
        Fade = 2,
        /// <summary>
        /// 插值
        /// </summary>
        Float = 3,
        /// <summary>
        /// 梯度颜色
        /// </summary>
        GradientColor = 4,
        /// <summary>
        /// 偏移
        /// </summary>
        Offset = 5,
        /// <summary>
        /// 平铺
        /// </summary>
        Tiling = 6,
        /// <summary>
        /// 四位数
        /// </summary>
        Vector = 7,
        /// <summary>
        /// 混合颜色
        /// </summary>
        BlendableColor = 8,
    } // end enum JTweenMaterial

    /// <summary>
    /// 刚体
    /// </summary>
    public enum JTweenRigidbody : int {
        None = 0,
        /// <summary>
        /// 移动
        /// </summary>
        Move = 1,
        /// <summary>
        /// 跳跃
        /// </summary>
        Jump = 2,
        /// <summary>
        /// 旋转
        /// </summary>
        Rotate = 3,
        /// <summary>
        /// 朝向
        /// </summary>
        LookAt = 4,
        /// <summary>
        /// 螺旋
        /// </summary>
        Spiral = 5
    } // end enum JTweenRigidbody

    /// <summary>
    /// 刚体2D
    /// </summary>
    public enum JTweenRigidbody2D : int {
        None = 0,
        /// <summary>
        /// 移动
        /// </summary>
        Move = 1,
        /// <summary>
        /// 跳跃
        /// </summary>
        Jump = 2,
        /// <summary>
        /// 旋转
        /// </summary>
        Rotate = 3,
    } // end enum JTweenRigidbody2D

    /// <summary>
    /// 精灵渲染
    /// </summary>
    public enum JTweenSpriteRenderer : int {
        None = 0,
        /// <summary>
        /// 颜色
        /// </summary>
        Color = 1,
        /// <summary>
        ///  渐变
        /// </summary>
        Fade = 2,
        /// <summary>
        /// 梯度颜色
        /// </summary>
        GradientColor = 3,
        /// <summary>
        /// 混合颜色
        /// </summary>
        BlendableColor = 4,
    } // end enum JTweenSpriteRenderer

    /// <summary>
    /// 拖尾渲染
    /// </summary>
    public enum JTweenTrailRenderer : int {
        None = 0,
        /// <summary>
        /// 尺寸
        /// </summary>
        Resize = 1,
        /// <summary>
        /// 持续时间
        /// </summary>
        Time = 2,
    } // end enum JTweenTrailRenderer

    /// <summary>
    /// 3D基础组件
    /// </summary>
    public enum JTweenTransform : int {
        None = 0,
        /// <summary>
        /// 时间坐标移动
        /// </summary>
        Move = 1,
        /// <summary>
        /// 本地坐标移动
        /// </summary>
        LocalMove = 2,
        /// <summary>
        /// 世界坐标跳跃
        /// </summary>
        Jump = 3,
        /// <summary>
        /// 本地坐标跳跃
        /// </summary>
        LocalJump = 4,
        /// <summary>
        /// 世界坐标欧拉旋转
        /// </summary>
        Rotate = 5,
        /// <summary>
        /// 世界坐标四元数旋转
        /// </summary>
        Quaternion = 6,
        /// <summary>
        /// 本地坐标欧拉旋转
        /// </summary>
        LocalRotate = 7,
        /// <summary>
        /// 本地坐标四元数旋转
        /// </summary>
        LocalQuaternion = 8,
        /// <summary>
        /// 朝向
        /// </summary>
        LookAt = 9,
        /// <summary>
        /// 尺寸比例
        /// </summary>
        Scale = 10,
        /// <summary>
        /// 坐标动效
        /// </summary>
        PunchPosition = 11,
        /// <summary>
        /// 旋转动效
        /// </summary>
        PunchRatation = 12,
        /// <summary>
        /// 缩放动效
        /// </summary>
        PunchScale = 13,
        /// <summary>
        /// 坐标摇晃
        /// </summary>
        ShakePosition = 14,
        /// <summary>
        /// 旋转摇晃
        /// </summary>
        ShakeRotation = 15,
        /// <summary>
        /// 缩放摇晃
        /// </summary>
        ShakeScale = 16,
        /// <summary>
        /// 世界坐标路径
        /// </summary>
        Path = 17,
        /// <summary>
        /// 本地坐标路径
        /// </summary>
        LocalPath = 18,
        /// <summary>
        /// 混合世界坐标位移
        /// </summary>
        BlendableMove = 19,
        /// <summary>
        /// 混合本地坐标位移
        /// </summary>
        BlendableLocalMove = 20,
        /// <summary>
        /// 混合世界坐标旋转
        /// </summary>
        BlendableRotate = 21,
        /// <summary>
        /// 混合本地坐标旋转
        /// </summary>
        BlendableLocalRotate = 22,
        /// <summary>
        /// 混合缩放
        /// </summary>
        BlendableScale = 23,
        /// <summary>
        /// 螺旋动效
        /// </summary>
        Spiral = 24,
    } // end enum JTweenTransform

    /// <summary>
    /// 画布组
    /// </summary>
    public enum JTweenCanvasGroup : int {
        None = 0,
        /// <summary>
        /// 渐变
        /// </summary>
        Fade = 1,
    } // end enum JTweenCanvasGroup

    /// <summary>
    /// 图像
    /// </summary>
    public enum JTweenGraphic : int {
        None = 0,
        /// <summary>
        /// 颜色
        /// </summary>
        Color = 1,
        /// <summary>
        /// 渐变
        /// </summary>
        Fade = 2,
        /// <summary>
        /// 混合颜色
        /// </summary>
        BlendableColor = 3,
    } // end enum JTweenGraphic

    /// <summary>
    /// 图片
    /// </summary>
    public enum JTweenImage : int {
        None = 0,
        /// <summary>
        /// 颜色
        /// </summary>
        Color = 1,
        /// <summary>
        /// 渐变
        /// </summary>
        Fade = 2,
        /// <summary>
        /// 填充
        /// </summary>
        FillAmount = 3,
        /// <summary>
        /// 梯度颜色
        /// </summary>
        GradientColor = 4,
        /// <summary>
        /// 混合颜色
        /// </summary>
        BlendableColor = 5,
    } // end enum JTweenImage

    /// <summary>
    /// 层级元素
    /// </summary>
    public enum JTweenLayoutElement : int {
        None = 0,
        /// <summary>
        /// 固定尺寸
        /// </summary>
        FlexibleSize = 1,
        /// <summary>
        /// 最小尺寸
        /// </summary>
        MinSize = 2,
        /// <summary>
        /// 优先尺寸
        /// </summary>
        PreferredSize = 3,
    } // end enum JTweenLayoutElement

    /// <summary>
    /// 轮廓
    /// </summary>
    public enum JTweenOutline : int {
        None = 0,
        /// <summary>
        /// 颜色
        /// </summary>
        Color = 1,
        /// <summary>
        /// 渐变
        /// </summary>
        Fade = 2,
    } // end enum JTweenOutline

    /// <summary>
    /// UGUI基础组件
    /// </summary>
    public enum JTweenRectTransform : int {
        None = 0,
        /// <summary>
        /// 最大锚点
        /// </summary>
        AnchorMax = 1,
        /// <summary>
        /// 最小锚点
        /// </summary>
        AnchorMin = 2,
        /// <summary>
        /// 锚点坐标
        /// </summary>
        AnchorPos = 3,
        /// <summary>
        /// 锚点3D坐标
        /// </summary>
        AnchorPos3D = 4,
        /// <summary>
        /// 锚点跳跃
        /// </summary>
        JumpAnchorPos = 5,
        /// <summary>
        /// 中心点
        /// </summary>
        Pivot = 6,
        /// <summary>
        /// 坐标动效
        /// </summary>
        PunchAnchorPos = 7,
        /// <summary>
        /// 坐标摇晃
        /// </summary>
        ShakeAnchorPos = 8,
        /// <summary>
        /// 尺寸
        /// </summary>
        SizeDelta = 9,
    } // end enum JTweenRectTransform

    /// <summary>
    /// 滑动组件
    /// </summary>
    public enum JTweenScrollRect : int {
        None = 0,
        /// <summary>
        /// 归一化坐标
        /// </summary>
        NormalizedPos = 1,
        /// <summary>
        /// 水平坐标
        /// </summary>
        HorizontalPos = 2,
        /// <summary>
        /// 竖直坐标
        /// </summary>
        VerticalPos = 3,
    } // end enum JTweenScrollRect
    
    /// <summary>
    /// 进度条
    /// </summary>
    public enum JTweenSlider : int {
        None = 0,
        /// <summary>
        /// 插值
        /// </summary>
        Value = 1,
    } // end enum JTweenSlider

    public enum JTweenText : int {
        None = 0,
        /// <summary>
        /// 颜色
        /// </summary>
        Color = 1,
        /// <summary>
        /// 渐变
        /// </summary>
        Fade = 2,
        /// <summary>
        /// 文本
        /// </summary>
        Text = 3,
        /// <summary>
        /// 混合颜色
        /// </summary>
        BlendableColor = 4
    } // end enum JTweenText

    public enum JTweentk2dBaseSprite : int {
        None = 0,
        /// <summary>
        /// 缩放
        /// </summary>
        Scale = 1,
        /// <summary>
        /// 颜色
        /// </summary>
        Color = 2,
        /// <summary>
        /// 渐变
        /// </summary>
        Fade = 3,
    } // end enum JTweentk2dBaseSprite

    public enum JTweentk2dSlicedSprite : int {
        None = 0,
        /// <summary>
        /// 缩放
        /// </summary>
        Scale = 1,
    } // end enum JTweentk2dSlicedSprite

    public enum JTweentk2dTextMesh : int {
        None = 0,
        /// <summary>
        /// 颜色
        /// </summary>
        Color = 1,
        /// <summary>
        /// 渐变
        /// </summary>
        Fade = 2,
        /// <summary>
        /// 文本
        /// </summary>
        Text = 3,
    } // end enum JTweentk2dTextMesh

    public enum JTweenTextMeshPro : int {
        None = 0,
        /// <summary>
        /// 缩放
        /// </summary>
        Scale = 1,
        /// <summary>
        /// 颜色
        /// </summary>
        Color = 2,
        /// <summary>
        /// 前部颜色
        /// </summary>
        FaceColor = 3,
        /// <summary>
        /// 前部渐变
        /// </summary>
        FaceFade = 4,
        /// <summary>
        /// 渐变
        /// </summary>
        Fade = 5,
        /// <summary>
        /// 字体大小
        /// </summary>
        FontSize = 6,
        /// <summary>
        /// 光亮颜色
        /// </summary>
        GlowColor = 7,
        /// <summary>
        /// 最大可变字符
        /// </summary>
        MaxVisibaleCharacters = 8,
        /// <summary>
        /// 轮廓颜色
        /// </summary>
        OutlineColor = 9,
        /// <summary>
        /// 文本
        /// </summary>
        Text = 10
    } // end enum JTweenTextMeshPro
} // end namespace JTween
