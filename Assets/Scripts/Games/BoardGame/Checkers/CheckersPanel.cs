using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckersPanel : GamePanel
{

    public CheckersBoardgame checkersBoardgame;
    public GameObject checkersObject;

    public bool vsAI;

    public void SetGameAI(bool vsAi)
    {
        vsAI = vsAi;
        if (checkersBoardgame.board != null)
        {
            checkersBoardgame.board.isInit = false;
        }
    }

    public override IEnumerator Enter()
    {
        if (moving)
            yield break;

        if (checkersObject)
            checkersObject.SetActive(false);

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

        if (checkersObject)
        {
            checkersObject.transform.position = new Vector3(end.x + UtilityFunctions.ScreenWidth, checkersObject.transform.position.y, checkersObject.transform.position.z);
            checkersObject.SetActive(true);
            if (checkersBoardgame != null ? !checkersBoardgame.board.isInit : true)
            {
                checkersBoardgame.vsAI = vsAI;
                if (!vsAI) checkersBoardgame.PrepareGame();
                else
                {

                    checkersBoardgame.PrepareGameAI();
                }
            }
            yield return new WaitForSeconds(animTime / 2f);
            checkersObject.transform.MoveTo(new Vector3(end.x, checkersObject.transform.position.y, checkersObject.transform.position.z), animTime);
            yield return new WaitForSeconds(animTime / 2f);
        }
        yield return new WaitForSeconds(animTime / 2f);
        checkersBoardgame.canClick = true;
        moving = false;
    }

    public override IEnumerator Exit()
    {
        if (moving)
            yield break;

        moving = true;
        checkersBoardgame.canClick = false;
        Vector2 start = screenCenter;
        Vector2 end = DefaultStartPosition(-1);
        transform.localPosition = start;
        if (checkersObject)
        {
            checkersObject.transform.position = new Vector3(start.x, checkersObject.transform.position.y, checkersObject.transform.position.z);
            checkersObject.SetActive(true);
            checkersObject.transform.MoveTo(new Vector3(start.x - UtilityFunctions.ScreenWidth, checkersObject.transform.position.y, checkersObject.transform.position.z), animTime);
            yield return new WaitForSeconds(animTime / 2f);
        }

        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime);

        if (checkersObject)
            checkersObject.SetActive(false);
        gameObject.SetActive(false);
        moving = false;
    }

    public override void OnBack()
    {
        ModalWindow.Choice(GameTranslations.EXIT_MATCH_CONFIRM.Get(), base.OnBack);
    }
}
