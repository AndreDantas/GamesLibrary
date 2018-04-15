using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PongPanel : GamePanel
{

    public PongGameController pongGame;
    public GameObject pongObjects;
    public GameObject background;
    public override IEnumerator Enter()
    {
        Vector2 end = screenCenter;
        if (pongObjects)
        {


            pongObjects.SetActive(true);
            pongObjects.transform.position = new Vector3(end.x + UtilityFunctions.ScreenWidth, pongObjects.transform.position.y, pongObjects.transform.position.z);

            pongGame.controlOn = false;
            if (pongGame != null ? !pongGame.gameRunning : false)
                pongGame.PrepareGame();

            if (pongGame)
                pongGame.canPause = false;
            pongObjects.transform.MoveTo(new Vector3(end.x, pongObjects.transform.position.y, pongObjects.transform.position.z), animTime);
        }
        if (background)
            background.SetActive(false);
        Vector2 start = DefaultStartPosition(1);
        transform.localPosition = start;


        yield return null;
        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime);
        //if (pongGame != null ? !pongGame.gameRunning : false)
        // pongGame.BeginGame();

        if (pongGame)
        {
            pongGame.controlOn = true;
            pongGame.canPause = true;
        }
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
            background.SetActive(true);
    }


    public override void OnBack()
    {

        pongGame.OnExitGame();

        base.OnBack();
    }
}
