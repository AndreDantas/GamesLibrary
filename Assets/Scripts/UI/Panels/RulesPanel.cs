using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesPanel : GamePanel
{
    public Slideshow slideshow;
    private void Start()
    {

    }
    public override IEnumerator Enter()
    {
        if (moving)
            yield break;

        moving = true;
        Vector2 start = DefaultStartPosition(1);
        Vector2 end = screenCenter;

        transform.localPosition = start;
        if (slideshow)
            slideshow.transform.localPosition = start;

        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime / 2f);

        if (slideshow)
        {
            slideshow.transform.MoveToLocal(end, animTime);
            yield return new WaitForSeconds(animTime);
        }

        if (slideshow)
            slideshow.Init();
        moving = false;

    }

    public override IEnumerator Exit()
    {
        if (moving)
            yield break;

        moving = true;
        Vector2 start = screenCenter;
        Vector2 end = DefaultStartPosition(-1);

        transform.localPosition = start;
        if (slideshow)
            slideshow.transform.localPosition = start;

        if (slideshow)
        {
            slideshow.transform.MoveToLocal(end, animTime);
            yield return new WaitForSeconds(animTime / 2f);
        }

        transform.MoveToLocal(end, animTime);

        yield return new WaitForSeconds(animTime);
        gameObject.SetActive(false);
        moving = false;
    }

}
