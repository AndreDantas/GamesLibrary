using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
[System.Serializable]
public struct OptionsSettings
{
    public bool flipPieces;
}
public class ChessPanel : GamePanel
{

    public ChessBoardgame chessBoardGame;
    public GameObject chessObject;
    public bool vsAI = false;
    protected OptionsSettings optionsSettings = new OptionsSettings();

    public void SetGameAI(bool vsAi)
    {
        vsAI = vsAi;
        if (chessBoardGame.board != null)
        {
            chessBoardGame.board.isInit = false;
        }
    }


    public override IEnumerator Enter()
    {
        if (moving)
            yield break;

        if (chessObject)
            chessObject.SetActive(false);
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

        if (chessObject)
        {
            chessObject.transform.position = new Vector3(end.x + UtilityFunctions.ScreenWidth, chessObject.transform.position.y, chessObject.transform.position.z);
            chessObject.SetActive(true);
            if (chessBoardGame.board != null ? !chessBoardGame.board.isInit : true)
            {
                chessBoardGame.vsAI = vsAI;
                if (!vsAI) chessBoardGame.PrepareGame();
                else
                {

                    chessBoardGame.PrepareGameAI();
                }
            }
            yield return new WaitForSeconds(animTime / 2f);
            chessObject.transform.MoveTo(new Vector3(end.x, chessObject.transform.position.y, chessObject.transform.position.z), animTime);

            yield return new WaitForSeconds(animTime / 2f);
        }
        yield return new WaitForSeconds(animTime / 2f);
        chessBoardGame.canClick = true;
        moving = false;
    }

    public override IEnumerator Exit()
    {
        if (moving)
            yield break;

        moving = true;
        chessBoardGame.canClick = false;
        chessBoardGame.StopAllCoroutines();
        chessBoardGame.GameExit();
        Vector2 start = screenCenter;
        Vector2 end = DefaultStartPosition(-1);
        transform.localPosition = start;
        if (chessObject)
        {
            chessObject.transform.position = new Vector3(start.x, chessObject.transform.position.y, chessObject.transform.position.z);
            chessObject.SetActive(true);
            chessObject.transform.MoveTo(new Vector3(start.x - UtilityFunctions.ScreenWidth, chessObject.transform.position.y, chessObject.transform.position.z), animTime);
            yield return new WaitForSeconds(animTime / 2f);
        }

        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime);

        if (chessObject)
            chessObject.SetActive(false);
        gameObject.SetActive(false);
        moving = false;
    }

    public override void OnBack()
    {
        ModalWindow.Choice("Sair da partida?", base.OnBack);
    }
}
