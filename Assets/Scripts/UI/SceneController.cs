using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    public List<GamePanel> panels = new List<GamePanel>();
    public GamePanel current;
    bool moving;
    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {
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
