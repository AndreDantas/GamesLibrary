using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesPanel : GamePanel
{
    public Slideshow slideshow;

    public override IEnumerator Enter()
    {
        if (slideshow)
        {
            slideshow.Init();
            slideshow.ResetSlides();
        }
        yield return base.Enter();

    }

}
