using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class ReversiPanel : GamePanel
{

    public ReversiBoardGame reversiBoardGame;
    public GameObject reversiObjects;

    public bool vsAI;

    public void SetGameAI(bool vsAi)
    {
        vsAI = vsAi;
        if (reversiBoardGame.board != null)
        {
            reversiBoardGame.board.isInit = false;
        }
    }

    public override IEnumerator Enter()
    {
        if (moving)
            yield break;

        if (reversiObjects)
            reversiObjects.SetActive(false);

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

        if (reversiObjects)
        {
            reversiObjects.transform.position = new Vector3(end.x + UtilityFunctions.ScreenWidth, reversiObjects.transform.position.y, reversiObjects.transform.position.z);
            reversiObjects.SetActive(true);
            if (reversiBoardGame != null ? !reversiBoardGame.board.isInit : true)
            {
                reversiBoardGame.vsAI = vsAI;
                if (!vsAI) reversiBoardGame.PrepareGame();
                else
                {

                    reversiBoardGame.PrepareGameAI();
                }

            }
            yield return new WaitForSeconds(animTime / 2f);
            reversiObjects.transform.MoveTo(new Vector3(end.x, reversiObjects.transform.position.y, reversiObjects.transform.position.z), animTime);
            yield return new WaitForSeconds(animTime / 2f);
        }
        yield return new WaitForSeconds(animTime / 2f);
        if (reversiBoardGame.showHints)
            reversiBoardGame.RenderHints();
        reversiBoardGame.canClick = true;
        moving = false;
    }

    public override IEnumerator Exit()
    {
        if (moving)
            yield break;

        moving = true;
        reversiBoardGame.canClick = false;
        Vector2 start = screenCenter;
        Vector2 end = DefaultStartPosition(-1);
        transform.localPosition = start;
        if (reversiObjects)
        {
            reversiObjects.transform.position = new Vector3(start.x, reversiObjects.transform.position.y, reversiObjects.transform.position.z);
            reversiObjects.SetActive(true);
            reversiObjects.transform.MoveTo(new Vector3(start.x - UtilityFunctions.ScreenWidth, reversiObjects.transform.position.y, reversiObjects.transform.position.z), animTime);
            yield return new WaitForSeconds(animTime / 2f);
        }

        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime);

        if (reversiObjects)
            reversiObjects.SetActive(false);
        gameObject.SetActive(false);
        moving = false;
    }

    public override void OnBack()
    {
        ModalWindow.Choice("Sair da partida?", base.OnBack);
    }
}
