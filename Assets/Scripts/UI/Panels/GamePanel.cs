using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : MonoBehaviour
{

    public float animTime = 0.5f;
    public Vector2 screenCenter;
    public GamePanel onBackPanel;
    public bool moving { get; internal set; }


    public virtual void CenterPanel()
    {
        RectTransform rect = transform as RectTransform;
        rect.localPosition = screenCenter;
    }
    public Vector2 DefaultStartPosition(int direction)
    {
        RectTransform rect = transform as RectTransform;
        return new Vector2(rect.rect.width * Mathf.Sign(direction) + screenCenter.x, screenCenter.y);
    }

    protected virtual void OnEnable()
    {
        SceneController.OnBack += OnBack;
    }

    protected virtual void OnDisable()
    {
        SceneController.OnBack -= OnBack;
    }
    public virtual IEnumerator Enter()
    {
        if (moving)
            yield break;

        moving = true;
        RectTransform rect = transform as RectTransform;
        yield return MovePanel(gameObject, DefaultStartPosition(1), screenCenter, animTime, null);

        moving = false;
    }

    public virtual IEnumerator Exit()
    {
        if (moving)
            yield break;
        moving = true;

        RectTransform rect = transform as RectTransform;
        yield return MovePanel(gameObject, screenCenter, DefaultStartPosition(-1), animTime, null);
        gameObject.SetActive(false);
        moving = false;
    }

    public static IEnumerator MovePanel(GameObject panel, Vector2 start, Vector2 end, float animTime, Func<float, float, float, float> equation)
    {
        if (equation == null)
        {
            equation = EasingEquations.EaseInOutQuad;
        }
        panel.transform.localPosition = start;
        panel.transform.MoveToLocal(end, animTime, equation);
        yield return new WaitForSeconds(animTime);
    }

    public virtual void OnBack()
    {
        if (onBackPanel && !moving)
        {
            SceneController.instance.ChangePanel(onBackPanel);
        }
    }

}
