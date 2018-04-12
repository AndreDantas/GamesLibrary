using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChangeToRandomColor))]
public class ImageRandomColorChange : MonoBehaviour
{

    ChangeToRandomColor colorChange;
    public Image img;
    // Use this for initialization
    void Start()
    {
        colorChange = GetComponent<ChangeToRandomColor>();
        if (!img)
            img = GetComponent<Image>();
        if (img)
            colorChange.SetOnValueChangeCallback(color => img.color = new Color(color.r, color.g, color.b, img.color.a));
    }

}
