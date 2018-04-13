using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteSwap : MonoBehaviour
{

    public List<Sprite> sprites;
    public Image image;
    int turnIndex = 0;

    public void SwapSprite()
    {
        if (image == null || sprites == null)
            return;

        turnIndex = (turnIndex + 1) % sprites.Count;
        image.sprite = sprites[turnIndex];

    }

    public void SetSprite(int index)
    {
        if (sprites != null ? sprites.Count > 0 : false)
        {
            if (index >= 0 && index < sprites.Count)
            {
                image.sprite = sprites[index];
            }

        }
    }


}
