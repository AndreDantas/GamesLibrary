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
        if (pongObjects)
        {
            Vector2 end = screenCenter;

            pongObjects.transform.position = new Vector3(end.x + UtilityFunctions.ScreenWidth, pongObjects.transform.position.y, pongObjects.transform.position.z);
            pongObjects.SetActive(true);
            pongGame.controlOn = false;
            if (pongGame != null)
                pongGame.PrepareGame();

            pongObjects.transform.MoveTo(new Vector3(end.x, pongObjects.transform.position.y, pongObjects.transform.position.z), animTime);
        }
        if (background)
            background.SetActive(false);
        yield return base.Enter();
        if (pongGame != null)
            pongGame.BeginGame();
        pongGame.controlOn = true;

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
            pongGame.ResetBall();
            pongGame.controlOn = false;
        }
        if (pongObjects)
        {
            pongObjects.transform.position = new Vector3(start.x, pongObjects.transform.position.y, pongObjects.transform.position.z);
            pongObjects.SetActive(true);
            pongObjects.transform.MoveTo(new Vector3(start.x - UtilityFunctions.ScreenWidth, pongObjects.transform.position.y, pongObjects.transform.position.z), animTime);

        }
        yield return base.Exit();
        if (background)
            background.SetActive(true);
    }
}
