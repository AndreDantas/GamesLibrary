using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class OptionsButtonController : MonoBehaviour
{

    public float animTime = 1f;
    public bool isAnimating { get; internal set; }
    public bool isOpen { get; internal set; }
    float barHeight;
    public RectTransform iconImage;
    public RectTransform optionsBar;
    public ClickOffRectTransform clickOff;


    private void Start()
    {
        if (optionsBar)
        {
            barHeight = optionsBar.rect.height;
            optionsBar.sizeDelta = new Vector2(optionsBar.rect.width, 0f);
            optionsBar.gameObject.Deactivate();


        }
        clickOff?.HideInvisibleBG();

    }

    public void OpenBar()
    {
        if (isAnimating || isOpen)
            return;
        StartCoroutine(Opening());
    }

    IEnumerator Opening()
    {
        isAnimating = true;

        if (iconImage)
            iconImage.RotateToLocal(iconImage.localEulerAngles + new Vector3(0, 0, -360), animTime, EasingEquations.EaseOutCubic);
        if (optionsBar)
        {
            optionsBar.gameObject.Activate();
            optionsBar.sizeDelta = new Vector2(optionsBar.rect.width, 0f);
            optionsBar.ScaleTo(new Vector2(optionsBar.rect.width, barHeight), animTime, EasingEquations.EaseOutCubic);
        }

        yield return new WaitForSeconds(animTime);
        isOpen = true;
        isAnimating = false;
        clickOff?.ShowInvisibleBG();
        yield break;
    }

    public void ToggleOpen()
    {
        if (isOpen)
            CloseBar();
        else
            OpenBar();
    }

    public void CloseBar()
    {
        if (isAnimating || !isOpen)
            return;
        StartCoroutine(Closing());
    }
    IEnumerator Closing()
    {
        isAnimating = true;
        if (iconImage)
            iconImage.RotateToLocal(iconImage.localEulerAngles + new Vector3(0, 0, 360f), animTime, EasingEquations.EaseOutCubic);
        if (optionsBar)
        {
            optionsBar.sizeDelta = new Vector2(optionsBar.rect.width, barHeight);
            optionsBar.ScaleTo(new Vector2(optionsBar.rect.width, 0f), animTime, EasingEquations.EaseOutCubic);
        }
        clickOff?.HideInvisibleBG();
        yield return new WaitForSeconds(animTime);
        isOpen = false;
        isAnimating = false;

        yield break;
    }
}
