using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
public class EdgeObject : MonoBehaviour, IPointerClickHandler
{
    public EdgePosition orientation;
    public DotsAndBoxesBoardgame board;
    public bool clicked = false;
    public Position pos;
    public Vector3 start;
    public Vector3 end;
    SpriteRenderer sr;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        sr.color = sr.color.ChangeAlpha(0.1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        sr.color = sr.color.ChangeAlpha(1f);
        board?.OnClick(pos, orientation);
    }
}
