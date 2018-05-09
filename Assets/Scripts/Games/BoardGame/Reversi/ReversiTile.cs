using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
public class ReversiTile : BoardgameTile
{

    public GameObject piece;
    public override void OnPointerClick(PointerEventData pointerEventData)
    {

        if (boardGame != null)
        {
            ReversiBoardGame Board = boardGame as ReversiBoardGame;
            Board.OnClick(pos);
        }
    }
}
