﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenPanel : GamePanel
{

    public override void OnBack()
    {
        if (!ModalWindow.IsActive())
            ModalWindow.Choice("Ir para o menu principal?", SceneController.instance.GoToMainMenu);
    }
}
