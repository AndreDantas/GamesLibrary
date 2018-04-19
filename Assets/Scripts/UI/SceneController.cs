using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public List<GamePanel> panels = new List<GamePanel>();
    public delegate void OnBackEventHandler();
    public static OnBackEventHandler OnBack;
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


    public void GoToMainMenu()
    {
        StartCoroutine(GoMainMenu());

    }

    IEnumerator GoMainMenu()
    {
        BeginFade(1);
        yield return new WaitForSeconds(MainMenuManager.fadeTime);
        SceneManager.LoadScene(MainMenuManager.MainMenuLevelBuild);
    }
    private void LateUpdate()
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
