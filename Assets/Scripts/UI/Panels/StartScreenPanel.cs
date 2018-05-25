using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenPanel : GamePanel
{

    public override void OnBack()
    {
        if (!ModalWindow.IsActive())
            ModalWindow.Choice(SceneController.EXIT_TO_MAINMENU.GetTextFromMainLanguage(), SceneController.instance.GoToMainMenu);
    }
}
