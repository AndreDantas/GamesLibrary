﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesPanel : GamePanel
{
    public Slideshow slideshow;

    public override IEnumerator Enter()
    {
        yield return base.Enter();
        if (slideshow)
            slideshow.Init();
    }

}
