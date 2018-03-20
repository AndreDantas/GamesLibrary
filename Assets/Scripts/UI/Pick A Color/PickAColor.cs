using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PickAColor : MonoBehaviour
{

    public Image image;
    public TextMeshProUGUI teamNameText;
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

    public string teamName
    {
        get
        {
            if (teamNameText)
                return teamNameText.text;
            else
                return "";
        }
    }
    // Use this for initialization
    void Start()
    {
        colorPicker.SetOnValueChangeCallback(color => image.color = color);
        //colorPicker.SetRandomColor();
        image.color = colorPicker.Color;

    }

    // Update is called once per frame
    void Update()
    {

    }

}
