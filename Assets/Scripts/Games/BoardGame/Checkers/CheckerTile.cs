using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CheckerTile : BoardgameTile
{


    public override void OnPointerClick(PointerEventData pointerEventData)
    {

        if (boardGame != null)
        {
            CheckersBoardgame Board = boardGame as CheckersBoardgame;
            Board.OnClick(pos);
        }
    }
}
