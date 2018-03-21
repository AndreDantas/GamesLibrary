using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : GamePanel
{
    public float buttonAnimTime = 0.3f;
    public List<GameObject> Buttons = new List<GameObject>();
    private float[] buttonsY;
    protected override void OnEnable()
    {
        base.OnEnable();
        if (Buttons != null)
        {
            buttonsY = new float[Buttons.Count];
            int counter = 0;
            foreach (GameObject button in Buttons)
            {
                buttonsY[counter] = button.transform.localPosition.y;
                counter++;
            }
        }

    }
    public override IEnumerator Enter()
    {
        if (moving)
            yield break;

        moving = true;
        if (Buttons == null ? true : Buttons.Count == 0)
            yield return base.Enter();


        RectTransform rect = transform as RectTransform;
        Vector2 start = new Vector2(rect.rect.width + screenCenter.x, screenCenter.y);
        Vector2 end = screenCenter;
        transform.localPosition = start;
        foreach (GameObject button in Buttons)
        {
            button.gameObject.SetActive(false);
        }
        yield return null;
        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime / 2f);

        for (int i = 0; i < Buttons.Count; i++)
        {

            RectTransform buttonRect = Buttons[i].transform as RectTransform;

            buttonRect.localPosition = new Vector3(start.x, buttonsY[i], buttonRect.localPosition.z);

            Buttons[i].gameObject.SetActive(true);
            Buttons[i].transform.MoveToLocal(new Vector3(end.x, buttonsY[i], buttonRect.localPosition.z), buttonAnimTime);
            yield return new WaitForSeconds(buttonAnimTime / 2f);
        }
        yield return new WaitForSeconds(buttonAnimTime / 2f);

        moving = false;
    }

    public override IEnumerator Exit()
    {
        if (moving)
            yield break;

        moving = true;
        if (Buttons == null ? true : Buttons.Count == 0)
            yield return base.Exit();

        RectTransform rect = transform as RectTransform;
        Vector2 start = screenCenter;
        Vector2 end = new Vector2(-rect.rect.width + screenCenter.x, screenCenter.y);
        transform.localPosition = start;

        for (int i = Buttons.Count - 1; i >= 0; i--)
        {

            RectTransform buttonRect = Buttons[i].transform as RectTransform;

            buttonRect.localPosition = new Vector3(start.x, buttonsY[i], buttonRect.localPosition.z);

            Buttons[i].gameObject.SetActive(true);
            Buttons[i].transform.MoveToLocal(new Vector3(end.x, buttonsY[i], buttonRect.localPosition.z), buttonAnimTime);
            yield return new WaitForSeconds(buttonAnimTime / 2f);
        }



        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime);

        gameObject.SetActive(false);
        moving = false;
    }

    public override void OnBack()
    {
        base.OnBack();
    }
}
