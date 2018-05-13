using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChessTile : BoardgameTile
{

    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        if (boardGame != null)
        {
            ChessBoardgame chessBoard = boardGame as ChessBoardgame;
            chessBoard.OnClick(pos);
        }
    }
}
