using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class TransformExtensions
{
    #region Transform
    public static Tweener MoveTo(this Transform t, Vector3 position)
    {
        return MoveTo(t, position, Tweener.DefaultDuration);
    }

    public static Tweener MoveTo(this Transform t, Vector3 position, float duration)
    {
        return MoveTo(t, position, duration, Tweener.DefaultEquation);
    }

    public static Tweener MoveTo(this Transform t, Vector3 position, float duration, Func<float, float, float, float> equation)
    {
        TransformPositionTweener tweener = t.gameObject.AddComponent<TransformPositionTweener>();
        tweener.startValue = t.position;
        tweener.endValue = position;
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }

    public static Tweener MoveToLocal(this Transform t, Vector3 position)
    {
        return MoveToLocal(t, position, Tweener.DefaultDuration);
    }

    public static Tweener MoveToLocal(this Transform t, Vector3 position, float duration)
    {
        return MoveToLocal(t, position, duration, Tweener.DefaultEquation);
    }

    public static Tweener MoveToLocal(this Transform t, Vector3 position, float duration, Func<float, float, float, float> equation)
    {
        TransformLocalPositionTweener tweener = t.gameObject.AddComponent<TransformLocalPositionTweener>();
        tweener.startValue = t.localPosition;
        tweener.endValue = position;
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }
    public static Tweener ScaleTo(this Transform t, Vector3 scale)
    {
        return ScaleTo(t, scale, Tweener.DefaultDuration);
    }

    public static Tweener ScaleTo(this Transform t, Vector3 scale, float duration)
    {
        return ScaleTo(t, scale, duration, Tweener.DefaultEquation);
    }

    public static Tweener ScaleTo(this Transform t, Vector3 scale, float duration, Func<float, float, float, float> equation)
    {
        TransformScaleTweener tweener = t.gameObject.AddComponent<TransformScaleTweener>();
        tweener.startValue = t.localScale;
        tweener.endValue = scale;
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }
    public static Tweener RotateTo(this Transform t, Vector3 rotation)
    {
        return RotateTo(t, rotation, Tweener.DefaultDuration);
    }

    public static Tweener RotateTo(this Transform t, Vector3 rotation, float duration)
    {
        return RotateTo(t, rotation, duration, Tweener.DefaultEquation);
    }


    public static Tweener RotateTo(this Transform t, Vector3 rotation, float duration, Func<float, float, float, float> equation)
    {
        TransformRotationTweener tweener = t.gameObject.AddComponent<TransformRotationTweener>();
        tweener.startValue = t.eulerAngles;
        tweener.endValue = rotation;
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }

    public static Tweener RotateToLocal(this Transform t, Vector3 rotation)
    {
        return RotateToLocal(t, rotation, Tweener.DefaultDuration);
    }

    public static Tweener RotateToLocal(this Transform t, Vector3 rotation, float duration)
    {
        return RotateToLocal(t, rotation, duration, Tweener.DefaultEquation);
    }


    public static Tweener RotateToLocal(this Transform t, Vector3 rotation, float duration, Func<float, float, float, float> equation)
    {
        TransformRotationTweener tweener = t.gameObject.AddComponent<TransformRotationTweener>();
        tweener.startValue = t.localEulerAngles;
        tweener.endValue = rotation;
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }
    #endregion

    #region RectTransform
    public static Tweener MoveTo(this RectTransform t, Vector2 position)
    {
        return MoveTo(t, position, Tweener.DefaultDuration);
    }

    public static Tweener MoveTo(this RectTransform t, Vector2 position, float duration)
    {
        return MoveTo(t, position, duration, Tweener.DefaultEquation);
    }

    public static Tweener MoveTo(this RectTransform t, Vector2 position, float duration, Func<float, float, float, float> equation)
    {
        RectTransformAnchoredPositionTweener tweener = t.gameObject.AddComponent<RectTransformAnchoredPositionTweener>();
        tweener.startValue = t.anchoredPosition;
        tweener.endValue = position;
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }

    public static Tweener ScaleTo(this RectTransform t, Vector2 size)
    {
        return ScaleTo(t, size, Tweener.DefaultDuration);
    }

    public static Tweener ScaleTo(this RectTransform t, Vector2 size, float duration)
    {
        return ScaleTo(t, size, duration, Tweener.DefaultEquation);
    }

    public static Tweener ScaleTo(this RectTransform t, Vector2 size, float duration, Func<float, float, float, float> equation)
    {
        RectTransformSizeTweener tweener = t.gameObject.AddComponent<RectTransformSizeTweener>();
        tweener.startValue = t.sizeDelta;
        tweener.endValue = size;
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }
    public static Tweener SetAnchorsMaxTo(this RectTransform t, Vector2 size)
    {
        return SetAnchorsMaxTo(t, size, Tweener.DefaultDuration);
    }

    public static Tweener SetAnchorsMaxTo(this RectTransform t, Vector2 size, float duration)
    {
        return SetAnchorsMaxTo(t, size, duration, Tweener.DefaultEquation);
    }
    public static Tweener SetAnchorsMaxTo(this RectTransform t, Vector2 anchors, float duration, Func<float, float, float, float> equation)
    {
        RectTransformAnchorsMaxTweener tweener = t.gameObject.AddComponent<RectTransformAnchorsMaxTweener>();
        tweener.startValue = t.anchorMax;
        tweener.endValue = anchors;
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }
    public static Tweener SetAnchorsMinTo(this RectTransform t, Vector2 size)
    {
        return SetAnchorsMinTo(t, size, Tweener.DefaultDuration);
    }

    public static Tweener SetAnchorsMinTo(this RectTransform t, Vector2 size, float duration)
    {
        return SetAnchorsMinTo(t, size, duration, Tweener.DefaultEquation);
    }
    public static Tweener SetAnchorsMinTo(this RectTransform t, Vector2 anchors, float duration, Func<float, float, float, float> equation)
    {
        RectTransformAnchorsMinTweener tweener = t.gameObject.AddComponent<RectTransformAnchorsMinTweener>();
        tweener.startValue = t.anchorMin;
        tweener.endValue = anchors;
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }
    #endregion
}