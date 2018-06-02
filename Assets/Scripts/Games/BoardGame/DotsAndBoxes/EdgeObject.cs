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
    public Player owner;
    public Position pos;
    public Vector3 start;
    public Vector3 end;
    public SpriteRenderer sr { get; internal set; }
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        sr.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //sr.color = sr.color.ChangeAlpha(Mathf.Abs(1f - sr.color.a));
        board?.OnClick(this);
    }

    public IEnumerator Activate(Color c, Player player, float animTime = 0f)
    {

        owner = player;
        if (sr)
        {
            sr.color.ChangeAlpha(0f);
            sr.enabled = true;

            sr.ChangeColorTo(c, animTime);
            yield return new WaitForSeconds(animTime);
            sr.color = c;
        }
    }

    public void Deactivate()
    {

        if (sr)
        {
            sr.enabled = false;

        }
    }
}
