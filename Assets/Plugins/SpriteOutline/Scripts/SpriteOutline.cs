﻿//script originally by Ryan Nielson (http://nielson.io/2016/04/2d-sprite-outlines-in-unity/)
using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    public Color color = Color.white;

    [Range(0, 16)]
    public int outlineSize = 1;
    public bool outlineBorderNotInternal = true; //added by Chris Garcia (thespinforce@gmail.com)
    public bool outline;
    private SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateOutline(true);
    }

    void OnDisable()
    {
        UpdateOutline(false);
    }

    void Update()
    {
        UpdateOutline(outline);
    }

    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        mpb.SetFloat("_OutlineBorderNotInternal", outlineBorderNotInternal ? 1 : 0); //added by Chris Garcia (thespinforce@gmail.com)
        spriteRenderer.SetPropertyBlock(mpb);
    }
}