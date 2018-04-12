using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTouch : MonoBehaviour
{

    public delegate void ScreenTouchEventHandler(List<Touch> touches);
    public event ScreenTouchEventHandler OnScreenTouch;
    public delegate void ScreenClickEventHandler(Vector2 click);
    public event ScreenClickEventHandler OnScreenClick;
    public event ScreenClickEventHandler OnScreenClickHold;
    public static ScreenTouch instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Update()
    {

        List<Touch> touches = UtilityFunctions.ToList(Input.touches);
        if (touches != null ? touches.Count > 0 : false)
        {

            if (UtilityFunctions.IsPointerOverUIObject())
                return;
            if (OnScreenTouch != null)
                OnScreenTouch(touches);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (UtilityFunctions.IsPointerOverUIObject())
                return;
            if (OnScreenClick != null)
                OnScreenClick(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetMouseButton(0))
        {
            if (UtilityFunctions.IsPointerOverUIObject())
                return;
            if (OnScreenClickHold != null)
                OnScreenClickHold(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
