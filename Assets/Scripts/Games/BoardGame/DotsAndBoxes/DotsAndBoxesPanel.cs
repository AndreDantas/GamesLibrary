using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class DotsAndBoxesPanel : GamePanel
{

    public DotsAndBoxesBoardgame dotsAndBoxesBoardGame;
    public GameObject dotsAndBoxesObjects;

    public bool vsAI;

    public void SetGameAI(bool vsAi)
    {
        vsAI = vsAi;
        if (dotsAndBoxesBoardGame.board != null)
        {
            dotsAndBoxesBoardGame.board.isInit = false;
        }
    }

    public override IEnumerator Enter()
    {
        if (moving)
            yield break;

        if (dotsAndBoxesObjects)
            dotsAndBoxesObjects.SetActive(false);

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

        if (dotsAndBoxesObjects)
        {
            dotsAndBoxesObjects.transform.position = new Vector3(end.x + UtilityFunctions.ScreenWidth, dotsAndBoxesObjects.transform.position.y, dotsAndBoxesObjects.transform.position.z);
            dotsAndBoxesObjects.SetActive(true);
            if (dotsAndBoxesBoardGame != null ? !dotsAndBoxesBoardGame.board.isInit : true)
            {
                dotsAndBoxesBoardGame.vsAI = vsAI;
                if (!vsAI) dotsAndBoxesBoardGame.PrepareGame();
                else
                {

                    dotsAndBoxesBoardGame.PrepareGameAI();
                }

            }
            yield return new WaitForSeconds(animTime / 2f);
            dotsAndBoxesObjects.transform.MoveTo(new Vector3(end.x, dotsAndBoxesObjects.transform.position.y, dotsAndBoxesObjects.transform.position.z), animTime);
            yield return new WaitForSeconds(animTime / 2f);
        }
        yield return new WaitForSeconds(animTime / 2f);

        dotsAndBoxesBoardGame.canClick = true;
        moving = false;
    }

    public override IEnumerator Exit()
    {
        if (moving)
            yield break;

        moving = true;
        dotsAndBoxesBoardGame.canClick = false;
        Vector2 start = screenCenter;
        Vector2 end = DefaultStartPosition(-1);
        transform.localPosition = start;
        if (dotsAndBoxesObjects)
        {
            dotsAndBoxesObjects.transform.position = new Vector3(start.x, dotsAndBoxesObjects.transform.position.y, dotsAndBoxesObjects.transform.position.z);
            dotsAndBoxesObjects.SetActive(true);
            dotsAndBoxesObjects.transform.MoveTo(new Vector3(start.x - UtilityFunctions.ScreenWidth, dotsAndBoxesObjects.transform.position.y, dotsAndBoxesObjects.transform.position.z), animTime);
            yield return new WaitForSeconds(animTime / 2f);
        }

        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime);

        if (dotsAndBoxesObjects)
            dotsAndBoxesObjects.SetActive(false);
        gameObject.SetActive(false);
        moving = false;
    }

    public override void OnBack()
    {
        ModalWindow.Choice(GameTranslations.EXIT_MATCH_CONFIRM.Get(), base.OnBack);
    }
}
