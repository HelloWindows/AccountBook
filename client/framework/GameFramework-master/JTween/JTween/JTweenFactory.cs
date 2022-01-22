using Json;
using JTween.AudioSource;
using JTween.Camera;
using JTween.CanvasGroup;
using JTween.Graphic;
using JTween.Image;
using JTween.LayoutElement;
using JTween.Light;
using JTween.LineRenderer;
using JTween.Material;
using JTween.Outline;
using JTween.RectTransform;
using JTween.Rigidbody;
using JTween.Rigidbody2D;
using JTween.ScrollRect;
using JTween.Slider;
using JTween.SpriteRenderer;
using JTween.Text;
using JTween.TrailRenderer;
using JTween.Transform;

namespace JTween {
    public class JTweenFactory {

        public static JTweenBase CreateTween(IJsonNode json) {
            if (!json.Contains("tweenType")) {
                UnityEngine.Debug.LogError("JTweenFactory CreateTween json tweenType is error");
                return null;
            } // end if
            if (!json.Contains("tweenElement")) {
                UnityEngine.Debug.LogError("JTweenFactory CreateTween json tweenElement is error");
                return null;
            } // end if
            int tweenType = json.GetInt("tweenType");
            int tweenElement = json.GetInt("tweenElement");
            return CreateTween(tweenElement, tweenType);
        }

        public static JTweenBase CreateTween(int tweenElement, int tweenType) {
            JTweenElement type = (JTweenElement)tweenElement;
            switch (type) {
                case JTweenElement.AudioSource:
                    return CreateAudioSourceTween(tweenType);
                case JTweenElement.Camera:
                    return CreateCameraTween(tweenType);
                case JTweenElement.Light:
                    return CreateLightTween(tweenType);
                case JTweenElement.LineRenderer:
                    return CreateLineRendererTween(tweenType);
                case JTweenElement.Material:
                    return CreateMaterialTween(tweenType);
                case JTweenElement.Rigidbody:
                    return CreateRigidbodyTween(tweenType);
                case JTweenElement.Rigidbody2D:
                    return CreateRigidbody2DTween(tweenType);
                case JTweenElement.SpriteRenderer:
                    return CreateSpriteRendererTween(tweenType);
                case JTweenElement.TrailRenderer:
                    return CreateTrailRendererTween(tweenType);
                case JTweenElement.Transform:
                    return CreateTransformTween(tweenType);
                case JTweenElement.CanvasGroup:
                    return CreateCanvasGroupTween(tweenType);
                case JTweenElement.Graphic:
                    return CreateGraphicTween(tweenType);
                case JTweenElement.Image:
                    return CreateImageTween(tweenType);
                case JTweenElement.LayoutElement:
                    return CreateLayoutElementTween(tweenType);
                case JTweenElement.Outline:
                    return CreateOutlineTween(tweenType);
                case JTweenElement.RectTransform:
                    return CreateRectTransformTween(tweenType);
                case JTweenElement.ScrollRect:
                    return CreateScrollRectTween(tweenType);
                case JTweenElement.Slider:
                    return CreateSliderTween(tweenType);
                case JTweenElement.Text:
                    return CreateTextTween(tweenType);
                default:
                    UnityEngine.Debug.LogError("CreateTween tweenElement is " + type.ToString());
                    return null;
            } // end swtich
        }

        private static JTweenBase CreateAudioSourceTween(int tweenType) {
            JTweenAudioSource type = (JTweenAudioSource)tweenType;
            switch (type) {
                case JTweenAudioSource.Fade:
                    return new JTweenAudioSourceFade();
                case JTweenAudioSource.Pitch:
                    return new JTweenAudioSourcePitch();
                default:
                    UnityEngine.Debug.LogError("CreateAudioSourceTween tweenType is " + type.ToString());
                    return null;
            } // end swtich
        }

        private static JTweenBase CreateCameraTween(int tweenType) {
            JTweenCamera type = (JTweenCamera)tweenType;
            switch (type) {
                case JTweenCamera.Aspect:
                    return new JTweenCameraAspect();
                case JTweenCamera.Color:
                    return new JTweenCameraColor();
                case JTweenCamera.FCP:
                    return new JTweenCameraFCP();
                case JTweenCamera.FOV:
                    return new JTweenCameraFOV();
                case JTweenCamera.NCP:
                    return new JTweenCameraNCP();
                case JTweenCamera.OrthoSize:
                    return new JTweenCameraOrthoSize();
                case JTweenCamera.PixelRect:
                    return new JTweenCameraPixelRect();
                case JTweenCamera.Rect:
                    return new JTweenCameraRect();
                case JTweenCamera.ShakePosition:
                    return new JTweenCameraShakePosition();
                case JTweenCamera.ShakeRotation:
                    return new JTweenCameraShakeRotation();
                default:
                    UnityEngine.Debug.LogError("CreateCameraTween tweenType is " + type.ToString());
                    return null;
            } // end swtich
        }

        private static JTweenBase CreateLightTween(int tweenType) {
            JTweenLight type = (JTweenLight)tweenType;
            switch (type) {
                case JTweenLight.Color:
                    return new JTweenLightColor();
                case JTweenLight.Intensity:
                    return new JTweenLightIntensity();
                case JTweenLight.ShadowStrength:
                    return new JTweenLightShadowStrength();
                case JTweenLight.BlendableColor:
                    return new JTweenLightBlendableColor();
                default:
                    UnityEngine.Debug.LogError("CreateLightTween tweenType is " + type.ToString());
                    return null;
            } // end swtich
        }

        private static JTweenBase CreateLineRendererTween(int tweenType) {
            JTweenLineRenderer type = (JTweenLineRenderer)tweenType;
            switch (type) {
                case JTweenLineRenderer.Color:
                    return new JTweenLineRendererColor();
                default:
                    UnityEngine.Debug.LogError("CreateLineRendererTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }

        private static JTweenBase CreateMaterialTween(int tweenType) {
            JTweenMaterial type = (JTweenMaterial)tweenType;
            switch (type) {
                case JTweenMaterial.Color:
                    return new JTweenMaterialColor();
                case JTweenMaterial.Fade:
                    return new JTweenMaterialFade();
                case JTweenMaterial.Float:
                    return new JTweenMaterialFloat();
                case JTweenMaterial.Offset:
                    return new JTweenMaterialOffset();
                case JTweenMaterial.Tiling:
                    return new JTweenMaterialTiling();
                case JTweenMaterial.Vector:
                    return new JTweenMaterialVector();
                case JTweenMaterial.BlendableColor:
                    return new JTweenMaterialBlendableColor();
                default:
                    UnityEngine.Debug.LogError("CreateMaterialTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }

        private static JTweenBase CreateRigidbodyTween(int tweenType) {
            JTweenRigidbody type = (JTweenRigidbody)tweenType;
            switch (type) {
                case JTweenRigidbody.Move:
                    return new JTweenRigidbodyMove();
                case JTweenRigidbody.Jump:
                    return new JTweenRigidbodyJump();
                case JTweenRigidbody.Rotate:
                    return new JTweenRigidbodyRotate();
                case JTweenRigidbody.LookAt:
                    return new JTweenRigidbodyLookAt();
                default:
                    UnityEngine.Debug.LogError("CreateRigidbodyTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }

        private static JTweenBase CreateRigidbody2DTween(int tweenType) {
            JTweenRigidbody2D type = (JTweenRigidbody2D)tweenType;
            switch (type) {
                case JTweenRigidbody2D.Move:
                    return new JTweenRigidbody2DMove();
                case JTweenRigidbody2D.Jump:
                    return new JTweenRigidbody2DJump();
                case JTweenRigidbody2D.Rotate:
                    return new JTweenRigidbody2DRotate();
                default:
                    UnityEngine.Debug.LogError("CreateRigidbody2DTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }

        private static JTweenBase CreateSpriteRendererTween(int tweenType) {
            JTweenSpriteRenderer type = (JTweenSpriteRenderer)tweenType;
            switch (type) {
                case JTweenSpriteRenderer.Color:
                    return new JTweenSpriteRendererColor();
                case JTweenSpriteRenderer.Fade:
                    return new JTweenSpriteRendererFade();
                case JTweenSpriteRenderer.BlendableColor:
                    return new JTweenSpriteRendererBlendableColor();
                default:
                    UnityEngine.Debug.LogError("CreateSpriteRendererTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }

        private static JTweenBase CreateTrailRendererTween(int tweenType) {
            JTweenTrailRenderer type = (JTweenTrailRenderer)tweenType;
            switch (type) {
                case JTweenTrailRenderer.Resize:
                    return new JTweenTrailRendererResize();
                case JTweenTrailRenderer.Time:
                    return new JTweenTrailRendererTime();
                default:
                    UnityEngine.Debug.LogError("CreateTrailRendererTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }

        private static JTweenBase CreateTransformTween(int tweenType) {
            JTweenTransform type = (JTweenTransform)tweenType;
            switch (type) {
                case JTweenTransform.Move:
                    return new JTweenTransformMove();
                case JTweenTransform.LocalMove:
                    return new JTweenTransformLocalMove();
                case JTweenTransform.Jump:
                    return new JTweenTransformJump();
                case JTweenTransform.LocalJump:
                    return new JTweenTransformLocalJump();
                case JTweenTransform.Rotate:
                    return new JTweenTransformRotate();
                case JTweenTransform.Quaternion:
                    return new JTweenTransformQuaternion();
                case JTweenTransform.LocalRotate:
                    return new JTweenTransformLocalRotate();
                case JTweenTransform.LocalQuaternion:
                    return new JTweenTransformLocalQuaternion();
                case JTweenTransform.LookAt:
                    return new JTweenTransformLookAt();
                case JTweenTransform.Scale:
                    return new JTweenTransformScale();
                case JTweenTransform.PunchPosition:
                    return new JTweenTransformPunchPosition();
                case JTweenTransform.PunchRatation:
                    return new JTweenTransformPunchRotation();
                case JTweenTransform.PunchScale:
                    return new JTweenTransformPunchScale();
                case JTweenTransform.ShakePosition:
                    return new JTweenTransformShakePosition();
                case JTweenTransform.ShakeRotation:
                    return new JTweenTransformShakeRotation();
                case JTweenTransform.ShakeScale:
                    return new JTweenTransformShakeScale();
                case JTweenTransform.Path:
                    return new JTweenTransformPath();
                case JTweenTransform.LocalPath:
                    return new JTweenTransformLocalPath();
                case JTweenTransform.BlendableMove:
                    return new JTweenTransformBlendableMove();
                case JTweenTransform.BlendableLocalMove:
                    return new JTweenTransformBlendableLocalMove();
                case JTweenTransform.BlendableRotate:
                    return new JTweenTransformBlendableRotate();
                case JTweenTransform.BlendableLocalRotate:
                    return new JTweenTransformBlendableLocalRotate();
                case JTweenTransform.BlendableScale:
                    return new JTweenTransformBlendableScale();
                default:
                    UnityEngine.Debug.LogError("CreateTransformTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }

        private static JTweenBase CreateCanvasGroupTween(int tweenType) {
            JTweenCanvasGroup type = (JTweenCanvasGroup)tweenType;
            switch (type) {
                case JTweenCanvasGroup.Fade:
                    return new JTweenCanvasGroupFade();
                default:
                    UnityEngine.Debug.LogError("CreateCanvasGroupTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }

        private static JTweenBase CreateGraphicTween(int tweenType) {
            JTweenGraphic type = (JTweenGraphic)tweenType;
            switch (type) {
                case JTweenGraphic.Color:
                    return new JTweenGraphicColor();
                case JTweenGraphic.Fade:
                    return new JTweenGraphicFade();
                case JTweenGraphic.BlendableColor:
                    return new JTweenGraphicBlendableColor();
                default:
                    UnityEngine.Debug.LogError("CreateGraphicTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }

        private static JTweenBase CreateImageTween(int tweenType) {
            JTweenImage type = (JTweenImage)tweenType;
            switch (type) {
                case JTweenImage.Color:
                    return new JTweenImageColor();
                case JTweenImage.Fade:
                    return new JTweenImageFade();
                case JTweenImage.FillAmount:
                    return new JTweenImageFillAmount();
                case JTweenImage.BlendableColor:
                    return new JTweenImageBlendableColor();
                default:
                    UnityEngine.Debug.LogError("CreateImageTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }

        private static JTweenBase CreateLayoutElementTween(int tweenType) {
            JTweenLayoutElement type = (JTweenLayoutElement)tweenType;
            switch (type) {
                case JTweenLayoutElement.FlexibleSize:
                    return new JTweenLayoutElementFlexibleSize();
                case JTweenLayoutElement.MinSize:
                    return new JTweenLayoutElementMinSize();
                case JTweenLayoutElement.PreferredSize:
                    return new JTweenLayoutElementPreferredSize();
                default:
                    UnityEngine.Debug.LogError("CreateLayoutElementTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }
        
        private static JTweenBase CreateOutlineTween(int tweenType) {
            JTweenOutline type = (JTweenOutline)tweenType;
            switch (type) {
                case JTweenOutline.Color:
                    return new JTweenOutlineColor();
                case JTweenOutline.Fade:
                    return new JTweenOutlineFade();
                default:
                    UnityEngine.Debug.LogError("CreateOutlineTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }

        private static JTweenBase CreateRectTransformTween(int tweenType) {
            JTweenRectTransform type = (JTweenRectTransform)tweenType;
            switch (type) {
                case JTweenRectTransform.AnchorMax:
                    return new JTweenRectTransformAnchorMax();
                case JTweenRectTransform.AnchorMin:
                    return new JTweenRectTransformAnchorMin();
                case JTweenRectTransform.AnchorPos:
                    return new JTweenRectTransformAnchorPos();
                case JTweenRectTransform.AnchorPos3D:
                    return new JTweenRectTransformAnchorPos3D();
                case JTweenRectTransform.JumpAnchorPos:
                    return new JTweenRectTransformJumpAnchorPos();
                case JTweenRectTransform.Pivot:
                    return new JTweenRectTransformPivot();
                case JTweenRectTransform.PunchAnchorPos:
                    return new JTweenRectTransformPunchAnchorPos();
                case JTweenRectTransform.ShakeAnchorPos:
                    return new JTweenRectTransformShakeAnchorPos();
                case JTweenRectTransform.SizeDelta:
                    return new JTweenRectTransformSizeDelta();
                default:
                    UnityEngine.Debug.LogError("CreateOutlineTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }

        private static JTweenBase CreateScrollRectTween(int tweenType) {
            JTweenScrollRect type = (JTweenScrollRect)tweenType;
            switch (type) {
                case JTweenScrollRect.NormalizedPos:
                    return new JTweenScrollRectNormalizedPos();
                case JTweenScrollRect.HorizontalPos:
                    return new JTweenScrollRectHorizontalPos();
                case JTweenScrollRect.VerticalPos:
                    return new JTweenScrollRectVerticalPos();
                default:
                    UnityEngine.Debug.LogError("CreateScrollRectTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }

        private static JTweenBase CreateSliderTween(int tweenType) {
            JTweenSlider type = (JTweenSlider)tweenType;
            switch (type) {
                case JTweenSlider.Value:
                    return new JTweenSliderValue();
                default:
                    UnityEngine.Debug.LogError("CreateSliderTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }

        private static JTweenBase CreateTextTween(int tweenType) {
            JTweenText type = (JTweenText)tweenType;
            switch (type) {
                case JTweenText.Color:
                    return new JTweenTextColor();
                case JTweenText.Fade:
                    return new JTweenTextFade();
                case JTweenText.Text:
                    return new JTweenTextText();
                case JTweenText.BlendableColor:
                    return new JTweenTextBlendableColor();
                default:
                    UnityEngine.Debug.LogError("CreateTextTween tweenType is " + type.ToString());
                    return null;
            } // end switch
        }
    }
}
