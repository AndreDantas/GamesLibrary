using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PickAColor : MonoBehaviour
{

    public Image image;
    public ObjectFocus objFocus;
    public CUIColorPicker colorPicker;
    public Color color
    {
        get
        {
            if (image)
                return image.color;
            else
                return Color.white;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (!objFocus)
        {
            objFocus = FindObjectOfType<ObjectFocus>();
        }
        colorPicker.SetOnValueChangeCallback(color => image.color = color);
        //colorPicker.SetRandomColor();
        colorPicker.Color = color;

    }


    public void Focus()
    {
        if (objFocus)
        {
            List<GameObject> focus = new List<GameObject>();
            focus.Add(image.gameObject);
            colorPicker.gameObject.SetActive(true);
            focus.Add(colorPicker.gameObject);
            objFocus.SetFocusObjects(focus);
            objFocus.OnDisableFocus += OnDisableFocus;
            objFocus.EnableFocus(true);
        }
    }

    public void OnDisableFocus()
    {
        colorPicker.gameObject.SetActive(false);
        objFocus.OnDisableFocus -= OnDisableFocus;
    }
}
