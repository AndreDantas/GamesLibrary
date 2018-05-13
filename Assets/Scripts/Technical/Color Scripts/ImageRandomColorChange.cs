using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class ImageRandomColorChange : ChangeToRandomColor
{

    public Image img;
    // Use this for initialization
    void Start()
    {
        if (!img)
            img = GetComponent<Image>();
        if (img)
            SetOnValueChangeCallback(color => img.color = new Color(color.r, color.g, color.b, img.color.a));
    }

}
