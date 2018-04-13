using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformEffector2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Racket : MonoBehaviour
{
    /// <summary>
    /// The racket's player.
    /// </summary>
    public PongPlayer player;
    /// <summary>
    /// The racket's screen orientation.
    /// </summary>
    public int orientation = -1;

    public float racketSpeed = 0.1f;
    public float racketHeight = 0.25f;
    public float racketPositionY = 2.5f;
    public PlatformEffector2D platformEffector { get; internal set; }
    public SpriteRenderer spriteRender { get; internal set; }

    // Use this for initialization
    void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        // SetUpRacket();
    }

    private void OnValidate()
    {
        // SetUpRacket();
        if (orientation < 0)
            orientation = -1;
        if (orientation >= 0)
            orientation = 1;
    }

    public void SetUpRacket()
    {
        SetRacketWidth(UtilityFunctions.ScreenWidth / 4f);
        SetRacketPositionY();
    }

    /// <summary>
    /// Sets the width of the racket.
    /// </summary>
    /// <param name="width"></param>
    public void SetRacketWidth(float width)
    {
        transform.localScale = new Vector3(width, racketHeight, 1f);
    }

    /// <summary>
    /// Sets the Y position of the racket.
    /// </summary>
    public void SetRacketPositionY()
    {
        transform.localPosition = new Vector3(0, Mathf.Sign(orientation) * (UtilityFunctions.ScreenHeight - UtilityFunctions.ScreenHeight / racketPositionY) / 2f, 0);

    }

}
