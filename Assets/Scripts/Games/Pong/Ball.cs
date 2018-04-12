using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

public class Ball : MonoBehaviour
{
    Rigidbody2D rb;

    public float ballSpeed = 2f;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Invoke("ResetBall", 1);

    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PongRacket" && collision.enabled)
        {

            Racket racket = collision.gameObject.GetComponent<Racket>();
            if (racket)
            {

                float x = HitFactor(transform.position, collision.gameObject.transform.position, collision.collider.bounds.size.x);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f * -Mathf.Sign(rb.velocity.y), transform.position.z);
                Vector2 dir = new Vector2(x, -Mathf.Sign(rb.velocity.y)).normalized;
                rb.velocity = dir * ballSpeed;
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

    public void ResetBall(int direction)
    {
        transform.position = Vector3.zero;
        rb.velocity = new Vector2(Mathf.Sign(Random.Range(-1, 1)), Mathf.Sign(direction)).normalized * ballSpeed;
    }
    public void ResetBall()
    {

        transform.position = Vector3.zero;
        rb.velocity = new Vector2(Mathf.Sign(Random.Range(-1, 1)), Mathf.Sign(Random.Range(-1, 1))).normalized * ballSpeed;
    }
}
