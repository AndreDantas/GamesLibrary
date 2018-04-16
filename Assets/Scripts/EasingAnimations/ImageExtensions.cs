using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public static class ImageExtensions
{
    public static Tweener ChangeColorTo(this Image i, Color color)
    {
        return ChangeColorTo(i, color, Tweener.DefaultDuration);

    }
    public static Tweener ChangeColorTo(this Image i, Color color, float duration)
    {
        return ChangeColorTo(i, color, duration, Tweener.DefaultEquation);

    }
    public static Tweener ChangeColorTo(this Image i, Color color, float duration, Func<float, float, float, float> equation)
    {
        ImageColorTweener tweener = i.gameObject.AddComponent<ImageColorTweener>();
        tweener.image = i;
        tweener.startColor = i.color;
        tweener.endColor = color;
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }
    public static Tweener FadeIn(this Image i)
    {
        return FadeIn(i, Tweener.DefaultDuration);

    }
    public static Tweener FadeIn(this Image i, float duration)
    {
        return FadeIn(i, duration, Tweener.DefaultEquation);

    }

    public static Tweener FadeIn(this Image i, float duration, Func<float, float, float, float> equation)
    {
        ImageColorTweener tweener = i.gameObject.AddComponent<ImageColorTweener>();
        tweener.image = i;
        tweener.startColor = new Color(i.color.r, i.color.g, i.color.b, 0f);
        tweener.endColor = new Color(i.color.r, i.color.g, i.color.b, 1f);
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }
    public static Tweener FadeOut(this Image i)
    {
        return FadeOut(i, Tweener.DefaultDuration);

    }
    public static Tweener FadeOut(this Image i, float duration)
    {
        return FadeOut(i, duration, Tweener.DefaultEquation);

    }

    public static Tweener FadeOut(this Image i, float duration, Func<float, float, float, float> equation)
    {
        ImageColorTweener tweener = i.gameObject.AddComponent<ImageColorTweener>();
        tweener.image = i;
        tweener.startColor = new Color(i.color.r, i.color.g, i.color.b, 1f);
        tweener.endColor = new Color(i.color.r, i.color.g, i.color.b, 0f);
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }


    public static Tweener ChangeColorTo(this SpriteRenderer i, Color color)
    {
        return ChangeColorTo(i, color, Tweener.DefaultDuration);

    }
    public static Tweener ChangeColorTo(this SpriteRenderer i, Color color, float duration)
    {
        return ChangeColorTo(i, color, duration, Tweener.DefaultEquation);

    }
    public static Tweener ChangeColorTo(this SpriteRenderer i, Color color, float duration, Func<float, float, float, float> equation)
    {
        SpriteRendererTweener tweener = i.gameObject.AddComponent<SpriteRendererTweener>();
        tweener.sprite = i;
        tweener.startColor = i.color;
        tweener.endColor = color;
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }

    public static Tweener FadeIn(this SpriteRenderer i)
    {
        return FadeIn(i, Tweener.DefaultDuration);

    }
    public static Tweener FadeIn(this SpriteRenderer i, float duration)
    {
        return FadeIn(i, duration, Tweener.DefaultEquation);

    }

    public static Tweener FadeIn(this SpriteRenderer i, float duration, Func<float, float, float, float> equation)
    {
        SpriteRendererTweener tweener = i.gameObject.AddComponent<SpriteRendererTweener>();
        tweener.sprite = i;
        tweener.startColor = new Color(i.color.r, i.color.g, i.color.b, 0f);
        tweener.endColor = new Color(i.color.r, i.color.g, i.color.b, 1f);
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }
    public static Tweener FadeOut(this SpriteRenderer i)
    {
        return FadeOut(i, Tweener.DefaultDuration);

    }
    public static Tweener FadeOut(this SpriteRenderer i, float duration)
    {
        return FadeOut(i, duration, Tweener.DefaultEquation);

    }

    public static Tweener FadeOut(this SpriteRenderer i, float duration, Func<float, float, float, float> equation)
    {
        SpriteRendererTweener tweener = i.gameObject.AddComponent<SpriteRendererTweener>();
        tweener.sprite = i;
        tweener.startColor = new Color(i.color.r, i.color.g, i.color.b, 1f);
        tweener.endColor = new Color(i.color.r, i.color.g, i.color.b, 0f);
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }

}
