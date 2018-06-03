using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using CielaSpike;
public class AppIntro : MonoBehaviour
{
    public static float animTime = 1f;
    public static bool introComplete = false;
    public RectTransform titleRT;
    public RectTransform pocketText;
    public Image gameButtonsOverlay;
    Vector2 pocketOrigin;
    public RectTransform gamesText;
    Vector2 gameOrigin;
    public RectTransform settingsButton;
    Vector2 settingsOrigin;
    public RectTransform gamesButtonsRT;

    private void Start()
    {

        StartIntro();

    }

    public void StartIntro()
    {
        if (!introComplete)
        {
            //Position elements
            pocketOrigin = pocketText.anchoredPosition;
            gameOrigin = gamesText.anchoredPosition;
            settingsOrigin = settingsButton.anchoredPosition;
            settingsButton.anchoredPosition = new Vector2(settingsButton.anchoredPosition.x, 150);
            pocketText.anchoredPosition = new Vector2(-pocketText.rect.width, pocketText.anchoredPosition.y);
            gamesText.anchoredPosition = new Vector2(gamesText.rect.width, gamesText.anchoredPosition.y);
            titleRT.anchorMax = Vector2.one;
            titleRT.anchorMin = Vector2.zero;
            gamesButtonsRT.anchorMax = new Vector2(1, 0.7f);
            gamesButtonsRT.anchoredPosition = new Vector2(gamesButtonsRT.anchoredPosition.x, -gamesButtonsRT.rect.height);
            //gamesButtonsRT.anchorMax = new Vector2(1, 0);
            //gameButtonsOverlay.color.ChangeAlpha(1f);
            gameButtonsOverlay.gameObject.Deactivate();
            StartCoroutine(IntroSetUp());
        }
        else
        {
            titleRT.anchorMin = new Vector2(0, 0.7f);
            gamesButtonsRT.anchorMax = new Vector2(1, 0.7f);
            gameButtonsOverlay.gameObject.Deactivate();
        }
        introComplete = true;
        //StartCoroutine(IntroSetUp());
    }

    IEnumerator IntroSetUp()
    {
        yield return new WaitForSeconds(animTime / 2f);
        yield return CenterTitle();
        StartCoroutine(PlaceObjects());


    }

    IEnumerator CenterTitle()
    {

        pocketText.MoveTo(pocketOrigin, animTime, EasingEquations.EaseInOutCubic);
        gamesText.MoveTo(gameOrigin, animTime, EasingEquations.EaseInOutCubic);

        yield return new WaitForSeconds(animTime);


    }
    IEnumerator PlaceObjects()
    {
        titleRT.SetAnchorsMinTo(new Vector2(0, 0.7f), animTime, EasingEquations.EaseInOutCubic);
        //gamesButtonsRT.SetAnchorsMaxTo(new Vector2(1, 0.7f), animTime, EasingEquations.EaseInOutCubic);
        gamesButtonsRT.MoveTo(new Vector2(gamesButtonsRT.anchoredPosition.x, 0), animTime, EasingEquations.EaseInOutCubic);
        settingsButton.MoveTo(settingsOrigin, animTime, EasingEquations.EaseInOutCubic);
        //gameButtonsOverlay.FadeOut(animTime);
        yield return new WaitForSeconds(animTime);
        gameButtonsOverlay.gameObject.Deactivate();
        gameButtonsOverlay.gameObject.SetActive(false);
        animTime = 0f;
    }

}
