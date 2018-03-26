using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
[System.Serializable]
public class BoardgameTile : MonoBehaviour, IPointerClickHandler
{

    public Position pos;
    public Boardgame boardGame;

    public virtual void OnPointerClick(PointerEventData pointerEventData)
    {

    }
}
