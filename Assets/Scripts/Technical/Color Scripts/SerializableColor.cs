using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class SerializableColor
{
    public float R;
    public float G;
    public float B;
    public float A;
    public SerializableColor(Color color)
    {

        R = color.r;
        G = color.g;
        B = color.b;
        A = color.a;
    }
    public SerializableColor(SerializableColor color)
    {

        R = color.R;
        G = color.G;
        B = color.B;
        A = color.A;
    }
    public Color GetColor()
    {
        return new Color(R, G, B, A);
    }

    public static implicit operator Color(SerializableColor c)
    {
        return c == null ? new Color(0, 0, 0, 1) : c.GetColor();
    }

    public static implicit operator SerializableColor(Color c)
    {
        return new SerializableColor(c);
    }
}