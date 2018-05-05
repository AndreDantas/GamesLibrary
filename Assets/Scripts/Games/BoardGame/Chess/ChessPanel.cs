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
    public GameObject chessOptions;

    public Toggle flipPieces;
    protected OptionsSettings optionsSettings = new OptionsSettings();


    public void OpenChessOptions()
    {
        if (chessOptions == null)
            return;

        ObjectFocus focus = FindObjectOfType<ObjectFocus>();
        List<GameObject> objs = new List<GameObject>();
        chessOptions.SetActive(true);
        objs.Add(chessOptions);
        focus.SetFocusObjects(objs);
        focus.OnDisableFocus += CloseChessOptions;
        focus.EnableFocus();
    }

    public void CloseChessOptions()
    {
        if (chessOptions == null)
            return;

        chessOptions.SetActive(false);
        ObjectFocus focus = FindObjectOfType<ObjectFocus>();
        if (focus)
        {
            focus.OnDisableFocus -= CloseChessOptions;
            focus.DisableFocus();
        }
        SaveOptions();

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
                chessBoardGame.PrepareGame();
            }
            yield return new WaitForSeconds(animTime / 2f);
            chessObject.transform.MoveTo(new Vector3(end.x, chessObject.transform.position.y, chessObject.transform.position.z), animTime);
            ConfigureOptions();
            yield return new WaitForSeconds(animTime / 2f);
        }
        yield return new WaitForSeconds(animTime / 2f);
        chessBoardGame.canClick = true;
        moving = false;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        LoadOptions();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        SaveOptions();
    }

    /// <summary>
    /// Load options settings.
    /// </summary>
    public void LoadOptions()
    {
        if (PlayerPrefs.HasKey("chess_flipPieces"))
        {
            optionsSettings.flipPieces = PlayerPrefs.GetInt("chess_flipPieces") == 1 ? true : false;

        }
    }

    /// <summary>
    /// Save options settings.
    /// </summary>
    public void SaveOptions()
    {
        PlayerPrefs.SetInt("chess_flipPieces", optionsSettings.flipPieces ? 1 : 0);

    }

    /// <summary>
    /// Configure the elements in the options window.
    /// </summary>
    public void ConfigureOptions()
    {
        if (flipPieces)
        {
            flipPieces.isOn = optionsSettings.flipPieces;
        }
    }

    public void FlipPieces(bool flip)
    {
        if (chessBoardGame)
        {
            chessBoardGame.FlipDarkSidePieces(flip);
            optionsSettings.flipPieces = flip;
        }
    }


    public override IEnumerator Exit()
    {
        if (moving)
            yield break;

        moving = true;
        chessBoardGame.canClick = false;
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


}
