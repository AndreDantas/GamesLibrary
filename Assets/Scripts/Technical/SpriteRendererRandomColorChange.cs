using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererRandomColorChange : MonoBehaviour
{

    ChangeToRandomColor colorChange;
    public SpriteRenderer sprite;
    // Use this for initialization
    void Start()
    {
        if (colorChange)
        {
            if (!sprite)
                sprite = GetComponent<SpriteRenderer>();
            if (sprite)
                colorChange.SetOnValueChangeCallback(color => sprite.color = new Color(color.r, color.g, color.b, sprite.color.a));
        }
    }

}
