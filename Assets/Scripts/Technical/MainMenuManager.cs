using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    public static int MainMenuLevelBuild = 0;
    public static float fadeTime = 0.8f;  // the fading speed
    GameObject fadeImage;
    private void Awake()
    {
        fadeImage = Instantiate(Resources.Load("Black") as GameObject);
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        BeginFade(-1);
        instance = this;
    }

    public void BeginFade(int direction)
    {
        if (fadeImage == null)
            return;
        fadeImage.transform.localPosition = Vector3.zero;
        fadeImage.transform.localScale = new Vector3(UtilityFunctions.ScreenWidth, UtilityFunctions.ScreenHeight, 0f);
        SpriteRenderer sr = fadeImage.GetComponent<SpriteRenderer>();
        if (Mathf.Sign(direction) < 0)
            sr.FadeOut(fadeTime);
        else
            sr.FadeIn(fadeTime);
    }
    public void LoadLevel(int level)
    {
        StartCoroutine(LevelLoad(level));
    }

    IEnumerator LevelLoad(int level)
    {
        BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(level);
    }
}
