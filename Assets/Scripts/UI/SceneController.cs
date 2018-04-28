using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public List<GamePanel> panels = new List<GamePanel>();
    public delegate void OnBackEventHandler();
    public static OnBackEventHandler OnBack;
    bool mainMenuButtonShowing = true;
    public float hideMainMenuButtonDistance = 135f;
    public Button mainMenuButton { get; internal set; }
    public GamePanel current;
    bool canMove;
    bool moving;
    GameObject fadeImage;

    float fadeCount;
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
        mainMenuButton = GameObject.FindGameObjectWithTag("MainMenuButton").GetComponent<Button>();
        FindObjectOfType<EventSystem>().pixelDragThreshold = 30;
    }
    private void Start()
    {
        StartCoroutine(Init());
    }

    public void BeginFade(int direction)
    {
        if (fadeImage == null)
            return;
        fadeImage.transform.localPosition = Vector3.zero;
        fadeImage.transform.localScale = new Vector3(UtilityFunctions.ScreenWidth, UtilityFunctions.ScreenHeight, 0f);
        SpriteRenderer sr = fadeImage.GetComponent<SpriteRenderer>();
        if (Mathf.Sign(direction) < 0)
            sr.FadeOut(MainMenuManager.fadeTime);
        else
            sr.FadeIn(MainMenuManager.fadeTime);
    }

    public static void HideMainMenuButton()
    {
        if (instance.mainMenuButton != null && instance.mainMenuButtonShowing)
        {
            instance.mainMenuButton.interactable = false;
            RectTransform rect = instance.mainMenuButton.transform as RectTransform;
            rect.MoveTo(new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + instance.hideMainMenuButtonDistance), 0.2f);
            instance.mainMenuButtonShowing = false;
        }

    }
    public static void ShowMainMenuButton()
    {
        if (instance.mainMenuButton != null && !instance.mainMenuButtonShowing)
        {
            instance.mainMenuButton.interactable = true;
            RectTransform rect = instance.mainMenuButton.transform as RectTransform;
            rect.MoveTo(new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y - instance.hideMainMenuButtonDistance), 0.2f);
            instance.mainMenuButtonShowing = true;
        }
    }

    public void GoToMainMenu()
    {
        StartCoroutine(GoMainMenu());

    }

    IEnumerator GoMainMenu()
    {
        BeginFade(1);
        canMove = false;
        moving = true;
        yield return new WaitForSeconds(MainMenuManager.fadeTime);
        SceneManager.LoadScene(MainMenuManager.MainMenuLevelBuild);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OnBack != null)
                OnBack();
        }
    }

    public void SetCanMove(bool lockMove)
    {
        canMove = lockMove;
    }

    public virtual IEnumerator Init()
    {

        if (panels != null)
        {
            foreach (GamePanel g in panels)
            {
                if (g == null)
                    continue;
                g.gameObject.SetActive(false);
            }
        }
        yield return null;

        if (current == null)
        {
            if (panels != null ? panels.Count > 0 : false)
                current = panels[0];
        }
        if (current)
        {
            current.gameObject.SetActive(true);
            current.CenterPanel();
        }
        canMove = true;
    }

    public void ChangePanel(GamePanel other = null)
    {
        if (other == null || moving || !canMove)
            return;
        StartCoroutine(IEChangePanel(other));
    }

    public static void LockPanel()
    {
        if (instance)
            instance.canMove = false;
    }

    public static void UnlockPanel()
    {
        if (instance)
            instance.canMove = true;
    }

    public IEnumerator IEChangePanel(GamePanel other)
    {

        if (panels != null ? !panels.Contains(other) : true)
        {
            yield break;
        }
        moving = true;
        if (current)
            yield return current.Exit();
        yield return null;
        other.gameObject.SetActive(true);

        yield return other.Enter();
        current = other;
        moving = false;
    }
}
