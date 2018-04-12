using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackgroundScroll : MonoBehaviour
{

    public Material material;
    Material materialCopy;
    public Image background;
    public float oneLoopTime = 10f;
    public float parallax = 2f;
    public bool scrollX = false;
    public bool scrollY = true;

    void Start()
    {
        if (background == null)
            background = GetComponent<Image>();
        if (background != null && material != null)
        {
            materialCopy = background.material = new Material(material);
        }
    }


    void Update()
    {

        ScrollBackground();
    }

    public void ScrollBackground()
    {
        if (materialCopy != null)
        {
            Vector2 offSet = materialCopy.mainTextureOffset;
            if (scrollY)
            {
                offSet.y += Time.deltaTime / oneLoopTime / parallax;
                if (offSet.y > 1)
                    offSet = new Vector2(offSet.x, 0f);
                if (offSet.y < 0)
                    offSet = new Vector2(offSet.x, 1f);
            }
            if (scrollX)
            {
                offSet.x += Time.deltaTime / oneLoopTime / parallax;
                if (offSet.x > 1)
                    offSet = new Vector2(0f, offSet.y);
                if (offSet.x < 0)
                    offSet = new Vector2(1f, offSet.y);
            }
            materialCopy.mainTextureOffset = offSet;
        }

    }
}
