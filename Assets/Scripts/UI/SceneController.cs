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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OnBack != null)
                OnBack();
        }
    }

    public virtual IEnumerator Init()
    {
        yield return null;
        if (panels != null)
        {
            foreach (GamePanel g in panels)
            {
                g.gameObject.SetActive(false);
            }
        }
        if (current)
        {
            current.gameObject.SetActive(true);
            current.CenterPanel();
        }
    }

    public void ChangePanel(GamePanel other = null)
    {
        if (other == null || moving)
            return;
        StartCoroutine(IEChangePanel(other));
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
