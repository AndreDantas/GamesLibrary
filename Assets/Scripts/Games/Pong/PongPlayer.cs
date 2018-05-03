using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongPlayer : MonoBehaviour
{
    /// <summary>
    /// The player's racket.
    /// </summary>
    public Racket racket;
    protected Rigidbody2D racketRb;
    public float edgesDistance = 0.1f;
    /// <summary>
    /// The touch area for player movement.
    /// </summary>
    public Bounds touchArea;
    public bool controlOn { get; set; }
    public bool isAI = false;
    protected int touchId = -1;
    protected Vector3 velocity;
    protected PongBall _ball;
    protected float random;
    public float aiSpeed;
    protected float lerpSpeed = 1f;
    public PongBall ball
    {
        get
        {
            return _ball;
        }
        set
        {
            _ball = value;
            ballRb = _ball?.GetComponent<Rigidbody2D>();
        }
    }
    protected Rigidbody2D ballRb;
    protected virtual void Start()
    {
        if (racket)
        {
            racket.player = this;
            racketRb = racket.GetComponent<Rigidbody2D>();
        }
    }

    private void FixedUpdate()
    {
        if (isAI && ball != null && racket != null)
        {
            AIMovement();
        }
    }

    protected virtual void ClampRacketMovement()
    {
        if (racket)
            racket.transform.localPosition = new Vector3(Mathf.Clamp(racket.transform.localPosition.x,
                                                         -UtilityFunctions.ScreenWidth / 2f + racket.transform.localScale.x / 2f + edgesDistance,
                                                         UtilityFunctions.ScreenWidth / 2f - racket.transform.localScale.x / 2f - edgesDistance),
                                                         racket.transform.localPosition.y,
                                                         racket.transform.localPosition.z);
    }

    protected void AIMovement()
    {
        racket.transform.localPosition = new Vector3(Vector3.SmoothDamp(racket.transform.localPosition, ball.transform.position, ref velocity, aiSpeed).x,
                                                       racket.transform.localPosition.y,
                                                       racket.transform.localPosition.z);
        ClampRacketMovement();
    }

    protected virtual void OnBallHit(PongBall ball)
    {
        random = Random.Range(-0.4f, 0.4f);
    }

    protected virtual void OnDrawGizmos()
    {
        UtilityFunctions.DrawBounds(touchArea);
    }
    protected virtual void OnEnable()
    {
        StartCoroutine(IEOnEnable());
    }

    protected virtual void OnDisable()
    {
        if (Application.platform == RuntimePlatform.Android)
            ScreenTouch.instance.OnScreenTouch -= OnScreenTouch;
        else
            ScreenTouch.instance.OnScreenClickHold -= OnMouseHold;
        racket.OnBallHit -= OnBallHit;
    }

    protected virtual IEnumerator IEOnEnable()
    {
        yield return null;
        if (Application.platform == RuntimePlatform.Android)
            ScreenTouch.instance.OnScreenTouch += OnScreenTouch;
        else
            ScreenTouch.instance.OnScreenClickHold += OnMouseHold;
        racket.OnBallHit += OnBallHit;
    }

    /// <summary>
    /// Moves the racket on the X axis.
    /// </summary>
    /// <param name="position"></param>
    public virtual void MoveRacket(Vector2 position)
    {
        if (!racket)
            return;

        if (!controlOn || isAI)
            return;
        Vector3 movePos = new Vector3(Mathf.Clamp(position.x,
                                                      -UtilityFunctions.ScreenWidth / 2f + racket.transform.localScale.x / 2f + edgesDistance,
                                                      UtilityFunctions.ScreenWidth / 2f - racket.transform.localScale.x / 2f - edgesDistance),
                                                      racket.transform.localPosition.y,
                                                      racket.transform.localPosition.z);
        //racket.transform.position = Vector3.SmoothDamp(racket.transform.position, movePos, ref velocity, 0.1f);
        racket.transform.localPosition = Vector3.MoveTowards(racket.transform.localPosition, movePos, racket.racketSpeed);
    }

    protected virtual void OnScreenTouch(List<Touch> touches)
    {
        // Move racket if in touch area

        for (int i = 0; i < touches.Count; i++)
        {
            Touch touch = touches[i];
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            if (Mathf.Sign(touchPos.y) == Mathf.Sign(racket.orientation))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    touchId = touch.fingerId;
                }
                else
                {
                    if (touchId == touch.fingerId)
                    {
                        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                            touchId = -1;
                        MoveRacket(touchPos);
                        return;
                    }

                }

            }
        }

    }



    protected virtual void OnMouseHold(Vector2 pos)
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
