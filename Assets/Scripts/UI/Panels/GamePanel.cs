using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : MonoBehaviour
{

    public float animTime = 0.5f;
    public Vector2 screenCenter;

    public bool moving { get; internal set; }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void CenterPanel()
    {
        RectTransform rect = transform as RectTransform;
        rect.localPosition = screenCenter;
    }

    public virtual IEnumerator Enter()
    {
        if (moving)
            yield break;

        moving = true;
        RectTransform rect = transform as RectTransform;
        yield return MovePanel(gameObject, new Vector2(rect.rect.width + screenCenter.x, screenCenter.y), screenCenter, animTime, null);

        moving = false;
    }

    public virtual IEnumerator Exit()
    {
        if (moving)
            yield break;
        moving = true;

        RectTransform rect = transform as RectTransform;
        yield return MovePanel(gameObject, screenCenter, new Vector2(-rect.rect.width + screenCenter.x, screenCenter.y), animTime, null);

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

}
