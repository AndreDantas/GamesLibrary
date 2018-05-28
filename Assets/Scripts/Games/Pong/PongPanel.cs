using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class PongPanel : GamePanel
{

    public PongGameController pongGame;
    public GameObject pongObjects;
    public GameObject background;
    public override IEnumerator Enter()
    {
        Vector2 end = screenCenter;
        SceneController.HideMainMenuButton();
        if (pongObjects)
        {


            pongObjects.SetActive(true);
            pongObjects.transform.position = new Vector3(end.x + UtilityFunctions.ScreenWidth, pongObjects.transform.position.y, pongObjects.transform.position.z);

            pongGame.controlOn = false;
            pongGame.PrepareGame();

            if (pongGame)
                pongGame.canPause = false;
            pongObjects.transform.MoveTo(new Vector3(end.x, pongObjects.transform.position.y, pongObjects.transform.position.z), animTime);
        }
        if (background)
        {
            Image i = background.GetComponent<Image>();
            if (i)
            {
                Color c = i.color;
                i.ChangeColorTo(new Color(c.r, c.g, c.b, 0f));
            }
        }

        Vector2 start = DefaultStartPosition(1);
        transform.localPosition = start;


        yield return null;
        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime);

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (pongObjects)
        {
            pongObjects.SetActive(false);
        }
    }
    public override IEnumerator Exit()
    {
        SceneController.ShowMainMenuButton();
        Vector2 start = screenCenter;
        if (pongGame)
        {

            if (pongGame.gameRunning)
            {
                pongGame.PauseGame();
                pongGame.canPause = false;
            }
            pongGame.OnExitGame();
            Time.timeScale = 1f;
        }
        if (pongObjects)
        {
            pongObjects.transform.position = new Vector3(start.x, pongObjects.transform.position.y, pongObjects.transform.position.z);
            pongObjects.SetActive(true);
            pongObjects.transform.MoveTo(new Vector3(start.x - UtilityFunctions.ScreenWidth, pongObjects.transform.position.y, pongObjects.transform.position.z), animTime);

        }
        yield return base.Exit();
        pongObjects.SetActive(false);
        if (background)
        {
            Image i = background.GetComponent<Image>();
            if (i)
            {
                Color c = i.color;
                i.ChangeColorTo(new Color(c.r, c.g, c.b, 0.5f));
            }
        }
    }


    public override void OnBack()
    {

        if (pongGame)
        {
            pongGame.PauseGame();
            ModalWindow.Choice(GameTranslations.EXIT_MATCH_CONFIRM.Get(), base.OnBack);
            return;
        }
        base.OnBack();
    }


}
