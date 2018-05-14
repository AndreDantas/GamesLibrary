using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class Connect4Panel : GamePanel
{

    public Connect4Boardgame connectBoardGame;
    public GameObject connectObjects;

    public bool vsAI;

    public void SetGameAI(bool vsAi)
    {
        vsAI = vsAi;
        if (connectBoardGame.board != null)
        {
            connectBoardGame.board.isInit = false;
        }
    }

    public override IEnumerator Enter()
    {
        if (moving)
            yield break;

        if (connectObjects)
            connectObjects.SetActive(false);

        moving = true;

        foreach (GameObject obj in panelObjects)
        {
            obj.gameObject.SetActive(false);
        }

        Vector2 start = DefaultStartPosition(1);
        Vector2 end = screenCenter;
        transform.localPosition = start;


        yield return null;
        transform.MoveToLocal(end, animTime);

        if (connectObjects)
        {
            connectObjects.transform.position = new Vector3(end.x + UtilityFunctions.ScreenWidth, connectObjects.transform.position.y, connectObjects.transform.position.z);
            connectObjects.SetActive(true);
            if (connectBoardGame != null ? !connectBoardGame.board.isInit : true)
            {
                connectBoardGame.vsAI = vsAI;
                if (!vsAI) connectBoardGame.PrepareGame();
                else
                {

                    connectBoardGame.PrepareGameAI();
                }
            }
            yield return new WaitForSeconds(animTime / 2f);
            connectObjects.transform.MoveTo(new Vector3(end.x, connectObjects.transform.position.y, connectObjects.transform.position.z), animTime);
            yield return new WaitForSeconds(animTime / 2f);
        }
        yield return new WaitForSeconds(animTime / 2f);
        connectBoardGame.canClick = true;
        moving = false;
    }

    public override IEnumerator Exit()
    {
        if (moving)
            yield break;

        moving = true;
        connectBoardGame.canClick = false;
        Vector2 start = screenCenter;
        Vector2 end = DefaultStartPosition(-1);
        transform.localPosition = start;
        if (connectObjects)
        {
            connectObjects.transform.position = new Vector3(start.x, connectObjects.transform.position.y, connectObjects.transform.position.z);
            connectObjects.SetActive(true);
            connectObjects.transform.MoveTo(new Vector3(start.x - UtilityFunctions.ScreenWidth, connectObjects.transform.position.y, connectObjects.transform.position.z), animTime);
            yield return new WaitForSeconds(animTime / 2f);
        }

        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime);

        if (connectObjects)
            connectObjects.SetActive(false);
        gameObject.SetActive(false);
        moving = false;
    }

    public override void OnBack()
    {
        ModalWindow.Choice("Sair da partida?", base.OnBack);
    }
}
