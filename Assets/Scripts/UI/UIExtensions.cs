using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public static class UIExtensions
{
    #region Tweener
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
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0f);
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
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1f);
        tweener.sprite = i;
        tweener.startColor = new Color(i.color.r, i.color.g, i.color.b, 1f);
        tweener.endColor = new Color(i.color.r, i.color.g, i.color.b, 0f);
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;
    }
    public static Tweener ScrollVerticalTo(this ScrollRect s, float scrollValue)
    {
        return ScrollVerticalTo(s, scrollValue, Tweener.DefaultDuration, Tweener.DefaultEquation);
    }
    public static Tweener ScrollVerticalTo(this ScrollRect s, float scrollValue, float duration)
    {
        return ScrollVerticalTo(s, scrollValue, duration, Tweener.DefaultEquation);
    }

    public static Tweener ScrollVerticalTo(this ScrollRect s, float scrollValue, float duration, Func<float, float, float, float> equation)
    {
        ScrollRectTweener tweener = s.gameObject.AddComponent<ScrollRectTweener>();
        tweener.scroll = s;
        tweener.verticalScroll = true;
        tweener.startValue = s.verticalNormalizedPosition;
        tweener.endValue = Mathf.Clamp01(scrollValue);
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;

    }

    public static Tweener ScrollHorizontalTo(this ScrollRect s, float scrollValue)
    {
        return ScrollHorizontalTo(s, scrollValue, Tweener.DefaultDuration, Tweener.DefaultEquation);
    }
    public static Tweener ScrollHorizontalTo(this ScrollRect s, float scrollValue, float duration)
    {
        return ScrollHorizontalTo(s, scrollValue, duration, Tweener.DefaultEquation);
    }

    public static Tweener ScrollHorizontalTo(this ScrollRect s, float scrollValue, float duration, Func<float, float, float, float> equation)
    {
        ScrollRectTweener tweener = s.gameObject.AddComponent<ScrollRectTweener>();
        tweener.scroll = s;
        tweener.verticalScroll = false;
        tweener.startValue = s.horizontalNormalizedPosition;
        tweener.endValue = Mathf.Clamp01(scrollValue);
        tweener.easingControl.duration = duration;
        tweener.easingControl.equation = equation;
        tweener.easingControl.Play();
        return tweener;

    }
    #endregion
    public static void ClampToWindow(this RectTransform panelRectTransform, RectTransform parentRectTransform)
    {
        if (parentRectTransform == null)
            return;
        Vector3 pos = panelRectTransform.localPosition;

        Vector3 minPosition = parentRectTransform.rect.min - panelRectTransform.rect.min;
        Vector3 maxPosition = parentRectTransform.rect.max - panelRectTransform.rect.max;

        pos.x = Mathf.Clamp(panelRectTransform.localPosition.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(panelRectTransform.localPosition.y, minPosition.y, maxPosition.y);

        panelRectTransform.localPosition = pos;
    }

    public static void ClampToCanvas(this RectTransform panel)
    {

        Canvas[] components = panel.gameObject.GetComponentsInParent<Canvas>();
        if (components != null ? components.Length > 0 : false)
        {
            Canvas canvas = components[components.Length - 1];
            if (canvas)
                ClampToWindow(panel, canvas.transform as RectTransform);
        }
    }


}
