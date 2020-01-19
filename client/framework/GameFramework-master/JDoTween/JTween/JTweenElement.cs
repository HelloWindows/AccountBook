using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTween {
    /// <summary>
    /// 动画元素
    /// </summary>
    public enum JTweenElement {
        None = 0,
        /// <summary>
        /// 音效混合器
        /// </summary>
        AudioMixer = 1,
        /// <summary>
        /// 音源
        /// </summary>
        AudioSource = 2,
        /// <summary>
        /// 相机
        /// </summary>
        Camera = 3,
        /// <summary>
        /// 灯光
        /// </summary>
        Light = 4,
        /// <summary>
        /// 线条渲染
        /// </summary>
        LineRenderer = 5,
        /// <summary>
        /// 材质球
        /// </summary>
        Material = 6,
        /// <summary>
        /// 刚体
        /// </summary>
        Rigidbody = 7,
        /// <summary>
        /// 2D刚体
        /// </summary>
        Rigidbody2D = 8,
        /// <summary>
        /// 精灵渲染
        /// </summary>
        SpriteRenderer = 9,
        /// <summary>
        /// 拖尾渲染
        /// </summary>
        TrailRenderer = 10,
        /// <summary>
        /// 3D基础组件
        /// </summary>
        Transform = 11,
        /// <summary>
        /// 画布组
        /// </summary>
        CanvasGroup = 12,
        /// <summary>
        /// 图片
        /// </summary>
        Image = 13,
        /// <summary>
        /// 层级元素
        /// </summary>
        LayoutElement = 14,
        /// <summary>
        /// 轮廓
        /// </summary>
        Outline = 15,
        /// <summary>
        /// UGUI基础组件
        /// </summary>
        RectTransform = 16,
        /// <summary>
        /// 滑动组件
        /// </summary>
        ScrollRect = 17,
        /// <summary>
        /// 进度条组件
        /// </summary>
        Slider = 18,
        /// <summary>
        /// 文本
        /// </summary>
        Text = 19,
        tk2dBaseSprite = 20,
        tk2dSlicedSprite = 21,
        tk2dTextMesh = 22,
        TextMeshPro = 23,
    } // end enum JTweenElement
} // end namespace JTween
