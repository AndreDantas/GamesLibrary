using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackgroundScroll : MonoBehaviour
{

    [SerializeField]
    Material material;
    public float oneLoopTime = 10f;
    public float parallax = 2f;
    public bool scrollX = false;
    public bool scrollY = true;
    // Use this for initialization

    void Start()
    {
        if (material == null)
            material = GetComponent<Image>().materialForRendering;
    }

    // Update is called once per frame
    void Update()
    {
        if (material != null)
        {
            Vector2 offSet = material.mainTextureOffset;
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
            material.mainTextureOffset = offSet;
        }


    }
}
