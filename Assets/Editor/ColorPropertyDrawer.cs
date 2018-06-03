#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Color))]
public class ColorPropertyDrawer : PropertyDrawer
{
    private const float HEX_FIELD_WIDTH = 55.0f;
    private const string HEX_FORMAT = "X2";

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw the color GUI
        Rect colorPosition = position;
        colorPosition.xMax -= HEX_FIELD_WIDTH;
        EditorGUI.PropertyField(colorPosition, property, new GUIContent(label.text));

        // Draw the hex text field
        Rect hexPosition = position;
        hexPosition.xMin += colorPosition.width;
        string previousHex = ColorToHex(property.colorValue);
        string hex = EditorGUI.TextField(hexPosition, previousHex);

        // Only update the color if the hex actually changed
        if (hex != previousHex)
        {
            Color32 color = property.colorValue;
            HexToColor(hex, ref color);
            property.colorValue = color;
        }

        EditorGUI.EndProperty();
    }

    private string ColorToHex(Color32 color)
    {
        return color.r.ToString(HEX_FORMAT) + color.g.ToString(HEX_FORMAT) + color.b.ToString(HEX_FORMAT);
    }

    private void HexToColor(string hex, ref Color32 color)
    {
        if (!string.IsNullOrEmpty(hex) && hex.Length == 6)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;

            if (byte.TryParse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out r) &&
                byte.TryParse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out g) &&
                byte.TryParse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out b))
            {
                color.r = r;
                color.g = g;
                color.b = b;
            }
        }
    }
}
#endif