using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class PongPanel : GamePanel
{

    public PongGameController pongGame;
    public RectTransform mainMenuButton;
    public GameObject pongObjects;
    public GameObject background;
    public float hideMainMenuButtonDistance = 120f;
    public override IEnumerator Enter()
    {
        Vector2 end = screenCenter;
        if (mainMenuButton)
        {

            mainMenuButton.MoveTo(new Vector2(mainMenuButton.anchoredPosition.x, mainMenuButton.anchoredPosition.y + hideMainMenuButtonDistance), 0.2f);

        }
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
        if (mainMenuButton)
        {
            mainMenuButton.MoveTo(new Vector2(mainMenuButton.anchoredPosition.x, mainMenuButton.anchoredPosition.y - hideMainMenuButtonDistance), 0.2f);
        }
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
            ModalWindow.Choice("Sair da partida?", base.OnBack);
            return;
        }
        base.OnBack();
    }


}
