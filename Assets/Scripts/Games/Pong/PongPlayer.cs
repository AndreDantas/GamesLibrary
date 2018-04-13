﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongPlayer : MonoBehaviour
{
    /// <summary>
    /// The player's racket.
    /// </summary>
    public Racket racket;

    public float edgesDistance = 0.1f;
    /// <summary>
    /// The touch area for player movement.
    /// </summary>
    [HideInInspector]
    public Bounds touchArea;
    public bool controlOn { get; set; }
    Vector3 velocity;

    private void Start()
    {
        if (racket)
            racket.player = this;
    }
    private void OnEnable()
    {
        StartCoroutine(IEOnEnable());
    }

    private void OnDisable()
    {
        if (Application.platform == RuntimePlatform.Android)
            ScreenTouch.instance.OnScreenTouch -= OnScreenTouch;
        else
            ScreenTouch.instance.OnScreenClickHold -= OnMouseHold;
    }

    IEnumerator IEOnEnable()
    {
        yield return null;
        if (Application.platform == RuntimePlatform.Android)
            ScreenTouch.instance.OnScreenTouch += OnScreenTouch;
        else
            ScreenTouch.instance.OnScreenClickHold += OnMouseHold;
    }

    /// <summary>
    /// Moves the racket on the X axis.
    /// </summary>
    /// <param name="position"></param>
    public void MoveRacket(Vector2 position)
    {
        if (!controlOn)
            return;
        Vector3 movePos = new Vector3(Mathf.Clamp(position.x,
                                                      -UtilityFunctions.ScreenWidth / 2f + racket.transform.localScale.x / 2f + edgesDistance,
                                                      UtilityFunctions.ScreenWidth / 2f - racket.transform.localScale.x / 2f - edgesDistance),
                                                      racket.transform.position.y,
                                                      racket.transform.position.z);
        //racket.transform.position = Vector3.SmoothDamp(racket.transform.position, movePos, ref velocity, 0.1f);
        racket.transform.position = Vector3.MoveTowards(racket.transform.position, movePos, racket.racketSpeed);
    }

    void OnScreenTouch(List<Touch> touches)
    {
        // Move racket if in touch area

        for (int i = 0; i < touches.Count; i++)
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touches[i].position);
            if (touchArea.Contains(touchPos))
            {
                MoveRacket(touchPos);
                return;
            }
        }
    }



    void OnMouseHold(Vector2 pos)
    {
        if (touchArea.Contains(pos))
        {

            if (racket != null)
            {
                MoveRacket(pos);
                return;
            }
        }
    }
}