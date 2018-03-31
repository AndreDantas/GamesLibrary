using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CheckerTile : BoardgameTile
{

    public GameObject checkerPiece;
    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        if (boardGame != null)
        {
            CheckersBoardgame chessBoard = boardGame as CheckersBoardgame;
            chessBoard.OnClick(pos);
        }
    }
}
