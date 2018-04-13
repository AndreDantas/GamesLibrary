using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HitPlayerWall : MonoBehaviour
{
    public PongPlayer player;
    public delegate void OnHitEventHandler(GameObject hit, HitPlayerWall wall);
    public event OnHitEventHandler OnHitWall;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (OnHitWall != null || collision.enabled)
        {
            OnHitWall(collision.gameObject, this);
        }
    }
}
