﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Ball : MonoBehaviour
{
    public Rigidbody2D rb { get; internal set; }
    public SpriteRenderer spriteRender { get; internal set; }
    public float ballSpeed = 2f;
    public PongPlayer currentPlayer;
    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        //Invoke("ResetBall", 1);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PongRacket" && collision.enabled)
        {

            Racket racket = collision.gameObject.GetComponent<Racket>();
            if (racket)
            {
                currentPlayer = racket.player;
                float x = HitFactor(transform.position, collision.gameObject.transform.position, collision.collider.bounds.size.x);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f * -Mathf.Sign(rb.velocity.y), transform.position.z);
                Vector2 dir = new Vector2(x, -Mathf.Sign(rb.velocity.y)).normalized;
                rb.velocity = dir * ballSpeed;
                spriteRender.color = racket.spriteRender.color;
            }

        }

    }

    float HitFactor(Vector2 ballPos, Vector2 racketPos,
                float racketWidth)
    {
        // ascii art:
        // || -1 <- left 
        // ||
        // ||  0 <- middle 
        // ||
        // ||  1 <- right

        return (ballPos.x - racketPos.x) / racketWidth;
    }

    public void ShootBall(int directionX, int directionY)
    {
        transform.localPosition = Vector3.zero;
        currentPlayer = null;
        rb.velocity = new Vector2(Mathf.Sign(directionX), Mathf.Sign(directionY)).normalized * ballSpeed;
    }

    public void ShootBall(int directionY)
    {
        ShootBall(Random.Range(-1, 1), directionY);
    }


    public void ShootBall()
    {
        ShootBall(Random.Range(-1, 1), Random.Range(-1, 1));
    }
}