using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckersPanel : GamePanel
{

    public CheckersBoardgame checkersBoardgame;
    public GameObject checkersObject;
    public GameObject checkersOptions;

    public void OpenCheckersOptions()
    {
        if (checkersOptions == null)
            return;

        ObjectFocus focus = FindObjectOfType<ObjectFocus>();
        List<GameObject> objs = new List<GameObject>();
        checkersOptions.SetActive(true);
        objs.Add(checkersOptions);
        focus.SetFocusObjects(objs);
        focus.OnDisableFocus += CloseCheckersOptions;
        focus.EnableFocus();
    }

    public void CloseCheckersOptions()
    {
        if (checkersOptions == null)
            return;

        if (checkersOptions.activeSelf == false)
            return;

        checkersOptions.SetActive(false);
        ObjectFocus focus = FindObjectOfType<ObjectFocus>();
        if (focus)
        {
            focus.OnDisableFocus -= CloseCheckersOptions;
            focus.DisableFocus();
        }

    }

    public override IEnumerator Enter()
    {
        if (moving)
            yield break;

        if (checkersObject)
            checkersObject.SetActive(false);
        CloseCheckersOptions();
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
            checkersObject.transform.position = new Vector3(end.x + MathOperations.ScreenWidth, checkersObject.transform.position.y, checkersObject.transform.position.z);
            checkersObject.SetActive(true);
            if (checkersBoardgame != null ? !checkersBoardgame.board.isInit : true)
            {
                checkersBoardgame.PrepareGame();
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
        CloseCheckersOptions();
        moving = true;
        checkersBoardgame.canClick = false;
        Vector2 start = screenCenter;
        Vector2 end = DefaultStartPosition(-1);
        transform.localPosition = start;
        if (checkersObject)
        {
            checkersObject.transform.position = new Vector3(start.x, checkersObject.transform.position.y, checkersObject.transform.position.z);
            checkersObject.SetActive(true);
            checkersObject.transform.MoveTo(new Vector3(start.x - MathOperations.ScreenWidth, checkersObject.transform.position.y, checkersObject.transform.position.z), animTime);
            yield return new WaitForSeconds(animTime / 2f);
        }

        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime);

        if (checkersObject)
            checkersObject.SetActive(false);
        gameObject.SetActive(false);
        moving = false;
    }
}
