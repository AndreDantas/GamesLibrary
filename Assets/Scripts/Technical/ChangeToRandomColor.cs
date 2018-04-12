using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ChangeToRandomColor : MonoBehaviour
{

    public bool changeColor = true;
    public bool random = true;
    public float changeTime = 1f;
    public void SetOnValueChangeCallback(Action<Color> onValueChange)
    {
        _onValueChange = onValueChange;
    }
    private Action<Color> _onValueChange;
    [SerializeField]
    public List<Color> colorList = new List<Color> { Color.red, Color.green, Color.blue };
    int index = 0;

    float lerpTime;
    [SerializeField]
    [ReadOnly]
    Color startColor;
    [SerializeField]
    [ReadOnly]
    Color endColor;

    public Color EndColor
    {
        get
        {
            return endColor;
        }

        set
        {
            endColor = value;
        }
    }

    public Color StartColor
    {
        get
        {
            return startColor;
        }

        set
        {
            startColor = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (random)
        {
            StartColor = RandomColor();
            EndColor = RandomColor();
        }
        else if (colorList != null ? colorList.Count > 0 : false)
        {
            StartColor = colorList[index];
            index = (index + 1) % colorList.Count;
            EndColor = colorList[index];
        }
    }

    // Update is called once per frame
    void Update()
    {
        ColorChange();
    }

    void ColorChange()
    {
        if (changeColor)
        {
            lerpTime += Time.deltaTime;
            if (lerpTime >= changeTime)
            {
                lerpTime = 0;
                StartColor = EndColor;
                if (random)
                {
                    index = 0;
                    EndColor = RandomColor();
                }
                else if (colorList != null ? colorList.Count > 0 : false)
                {
                    index = (index + 1) % colorList.Count;
                    EndColor = colorList[index];
                }
                else
                {

                }
            }
            float t = lerpTime / changeTime;
            if (_onValueChange != null)
            {
                _onValueChange(Color.Lerp(StartColor, EndColor, t));
            }
        }
    }
    public static Color RandomColor()
    {
        return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
    }
}
