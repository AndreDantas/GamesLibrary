using UnityEngine;
using UnityEngine.EventSystems;

public class InvisibleBGCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    ClickOffRectTransform _parentCtrl;

    public void setParentCtrl(ClickOffRectTransform ctrl)
    {
        _parentCtrl = ctrl;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _parentCtrl.ClickOff();
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}