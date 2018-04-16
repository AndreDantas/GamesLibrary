using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class Tweener : MonoBehaviour
{
    #region Properties
    public static float DefaultDuration = 1f;
    public static Func<float, float, float, float> DefaultEquation = EasingEquations.EaseInOutQuad;
    public EasingControl easingControl;
    public bool destroyOnComplete = true;
    #endregion
    #region MonoBehaviour
    protected virtual void Awake()
    {
        easingControl = gameObject.AddComponent<EasingControl>();
    }
    protected virtual void OnEnable()
    {
        easingControl.updateEvent += OnUpdate;
        easingControl.completedEvent += OnComplete;
    }
    protected virtual void OnDisable()
    {
        easingControl.updateEvent -= OnUpdate;
        easingControl.completedEvent -= OnComplete;
    }
    protected virtual void OnDestroy()
    {
        if (easingControl != null)
            Destroy(easingControl);
    }
    #endregion
    #region Event Handlers
    protected abstract void OnUpdate(object sender, EventArgs e);
    protected virtual void OnComplete(object sender, EventArgs e)
    {
        if (destroyOnComplete)
            Destroy(this);
    }
    #endregion
}
public abstract class Vector3Tweener : Tweener
{
    public Vector3 startValue;
    public Vector3 endValue;
    public Vector3 currentValue { get; private set; }
    protected override void OnUpdate(object sender, System.EventArgs e)
    {
        currentValue = (endValue - startValue) * easingControl.currentValue + startValue;
    }
}

public class TransformPositionTweener : Vector3Tweener
{
    protected override void OnUpdate(object sender, System.EventArgs e)
    {
        base.OnUpdate(sender, e);
        transform.position = currentValue;
    }
}
public class TransformLocalPositionTweener : Vector3Tweener
{
    protected override void OnUpdate(object sender, System.EventArgs e)
    {
        base.OnUpdate(sender, e);
        transform.localPosition = currentValue;
    }
}
public class TransformScaleTweener : Vector3Tweener
{
    protected override void OnUpdate(object sender, System.EventArgs e)
    {
        base.OnUpdate(sender, e);
        transform.localScale = currentValue;
    }
}

public abstract class ColorTweener : Tweener
{

    public Color startColor;
    public Color endColor;
    public Color currentColor { get; private set; }
    protected override void OnUpdate(object sender, System.EventArgs e)
    {
        currentColor = (endColor - startColor) * easingControl.currentValue + startColor;
    }
}

public class ImageColorTweener : ColorTweener
{
    public Image image;

    protected override void OnUpdate(object sender, EventArgs e)
    {
        base.OnUpdate(sender, e);
        if (image)
            image.color = currentColor;
    }
}

public class SpriteRendererTweener : ColorTweener
{
    public SpriteRenderer sprite;

    protected override void OnUpdate(object sender, EventArgs e)
    {
        base.OnUpdate(sender, e);
        if (sprite)
            sprite.color = currentColor;
    }
}




