using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PongMatchSettings
{
    public int scoreTarget = 5;

    public int player1Score;
    public int player2Score;

    public PongMatchSettings()
    {

    }
}

// Set up match score indicator - text 
// Create ball trail
// increase ball velocity with hit
// Add pause and quit button
public class PongGameController : MonoBehaviour
{
    /// <summary>
    /// The top Player.
    /// </summary>
    public PongPlayer topPlayer;
    /// <summary>
    /// The bottom player.
    /// </summary>
    public PongPlayer bottomPlayer;
    /// <summary>
    /// The prefab of the ball. 
    /// </summary>
    public GameObject ballPrefab;
    GameObject ballObj;
    Ball ball;
    public PongMatchSettings mathcSettings = new PongMatchSettings();
    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI player2Score;
    public GameObject pauseButton;
    public float initialBallSpeed = 5f;
    public bool canPause = true;
    bool isPaused;
    bool _controlOn;
    Vector2 ballVelocity;
    public bool controlOn
    {
        get
        {
            return _controlOn;
        }

        set
        {
            _controlOn = value;
            if (topPlayer)
                topPlayer.controlOn = value;
            if (bottomPlayer)
                bottomPlayer.controlOn = value;
        }
    }
    public ScreenBounds bounds;

    private void Awake()
    {

    }

    /// <summary>
    /// Used to prepare the game, setting up the ball and the rackets' positions.
    /// </summary>
    public void PrepareGame()
    {
        ResumeGame();
        if (ballPrefab != null)
        {
            Destroy(ballObj);
            ballObj = Instantiate(ballPrefab, transform.parent);

            ball = ballObj.GetComponent<Ball>();
            if (ball)
            {
                ResetBall();
            }
        }

        if (bounds)
        {
            bounds.CreateWalls();
        }

        mathcSettings.player1Score = mathcSettings.player2Score = 0;
        if (player1Score)
            player1Score.text = "0";
        if (player2Score)
            player2Score.text = "0";

        if (topPlayer)
        {
            HitPlayerWall hitwall = bounds.topWall.AddComponent<HitPlayerWall>(); // Add hit detection to wall behind player
            hitwall.player = topPlayer;
            hitwall.OnHitWall += OnHitWall;
            topPlayer.racket.orientation = 1;
            topPlayer.racket.racketPositionY = 3f;
            topPlayer.racket.SetUpRacket();

            // Set Touch area
            topPlayer.touchArea = new Bounds();
            topPlayer.touchArea.center = new Vector3(0, UtilityFunctions.ScreenHeight / 4f, 0);
            topPlayer.touchArea.size = new Vector3(UtilityFunctions.ScreenWidth, UtilityFunctions.ScreenHeight / 2f, 0);

        }
        if (bottomPlayer)
        {
            HitPlayerWall hitwall = bounds.bottomWall.AddComponent<HitPlayerWall>(); // Add hit detection to wall behind player
            hitwall.player = bottomPlayer;
            hitwall.OnHitWall += OnHitWall;
            bottomPlayer.racket.orientation = -1;
            bottomPlayer.racket.racketPositionY = 3f;
            bottomPlayer.racket.SetUpRacket();

            // Set Touch area
            bottomPlayer.touchArea = new Bounds();
            bottomPlayer.touchArea.center = new Vector3(0, -UtilityFunctions.ScreenHeight / 4f, 0);
            bottomPlayer.touchArea.size = new Vector3(UtilityFunctions.ScreenWidth, UtilityFunctions.ScreenHeight / 2f, 0);

        }

        canPause = true;

    }


    public void BeginGame()
    {

        StartCoroutine(StartRound());
    }

    /// <summary>
    /// Pauses game.
    /// </summary>
    public void PauseGame()
    {
        if (isPaused || !canPause)
            return;
        controlOn = false;
        ballVelocity = new Vector2(ball.rb.velocity.x, ball.rb.velocity.y);
        ball.rb.velocity = Vector2.zero;
        ball.rb.isKinematic = true;
        isPaused = true;
        if (pauseButton)
            pauseButton.gameObject.GetComponentInChildren<SpriteSwap>().SetSprite(1);
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();

    }

    /// <summary>
    /// Resumes game.
    /// </summary>
    public void ResumeGame()
    {
        if (!isPaused || !canPause)
            return;
        controlOn = true;
        ball.rb.velocity = ballVelocity;
        ball.rb.isKinematic = false;
        isPaused = false;
        if (pauseButton)
            pauseButton.gameObject.GetComponentInChildren<SpriteSwap>().SetSprite(0);
    }



    /// <summary>
    /// Resets ball to the center.
    /// </summary>
    public void ResetBall()
    {
        if (ball)
        {
            ball.spriteRender.color = Color.white;
            ball.gameObject.transform.localPosition = Vector3.zero;
            ball.rb.velocity = Vector3.zero;
        }
    }

    IEnumerator StartRound()
    {

        //canPause = false;
        ResetBall();
        yield return WaitForSecondsOrPause(1f);
        ball.ShootBall();
        //canPause = true;

    }

    IEnumerator WaitForSecondsOrPause(float time)
    {
        float count = 0;
        while (count < time)
        {
            if (!isPaused)
            {
                count += Time.deltaTime;
            }
            yield return null;
        }
    }

    IEnumerator EndRound()
    {

        // Update scores and check for winner;
        yield return null;
        yield return StartRound();
    }

    /// <summary>
    /// When the ball hits a wall behind the player:
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="wall"></param>
    public void OnHitWall(GameObject hit, HitPlayerWall wall)
    {
        if (ball)
            if (hit == ball.gameObject)
            {
                // Add Score and Match end
                StartCoroutine(EndRound());

            }
    }
}
