using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    public static int MainMenuLevelBuild = 0;
    public static float fadeTime = 0.3f;  // the fading speed
    private bool isLoading = false;
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
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ConfirmAppExit();
        }
    }

    void QuitApp()
    {
#if UNITY_EDITOR    
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void ConfirmAppExit()
    {

        ModalWindow.Choice(SystemTranslations.CLOSE_APPLICATION.Get(), QuitApp);
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
        if (isLoading)
            return;
        StartCoroutine(LevelLoad(level));
    }

    IEnumerator LevelLoad(int level)
    {
        isLoading = true;
        BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(level);
        isLoading = false;
    }
}
