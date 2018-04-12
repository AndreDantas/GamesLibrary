using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public List<GamePanel> panels = new List<GamePanel>();
    public delegate void OnBackEventHandler();
    public static OnBackEventHandler OnBack;
    public GamePanel current;
    bool canMove;
    bool moving;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    private void Start()
    {
        StartCoroutine(Init());
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

        other.gameObject.SetActive(true);
        yield return other.Enter();
        current = other;
        moving = false;
    }
}
