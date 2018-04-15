using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererController : MonoBehaviour
{
    public TrailRenderer trailRender;
    public float trailTime = 1f;
    public float startWidth = 2f;
    public float endWidth = 1f;
    public Material trailMaterial;
    public Color trailColor = Color.red;
    bool _trailEnabled;
    public bool trailEnabled
    {
        get
        {
            return _trailEnabled;
        }
        set
        {
            _trailEnabled = value;
            if (trailRender != null)
            {

                trailRender.enabled = _trailEnabled;

            }
        }
    }

    private void Awake()
    {
        if (!trailRender)
            trailRender = GetComponent<TrailRenderer>();
    }



    public void UpdateTrail()
    {
        if (!trailRender)
            return;
        if (trailMaterial)
            trailRender.material = trailMaterial;
        trailRender.time = trailTime;
        trailRender.startWidth = startWidth;
        trailRender.endWidth = endWidth;
        if (trailRender.material)
            trailRender.material.color = trailColor;
        trailRender.enabled = _trailEnabled;

    }

    /// <summary>
    /// Sets the trail color.
    /// </summary>
    /// <param name="color"></param>
    public void SetTrailColor(Color color)
    {
        if (!trailRender)
            return;
        trailColor = color;
        UpdateTrail();
    }

    /// <summary>
    /// Sets the start and end width of the trail.
    /// </summary>
    /// <param name="size"></param>
    public void SetTrailWidth(float width)
    {
        if (!trailRender)
            return;
        startWidth = endWidth = width;
        UpdateTrail();
    }

    private void OnEnable()
    {
        UpdateTrail();
    }
}
