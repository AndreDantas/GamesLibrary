using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PongMatchSettings
{
    public int scoreTarget = 5;

    public int bottomPlayerScore;
    public int topPlayerScore;

    public PongMatchSettings()
    {

    }
}

// Set up match score indicator - text 
// Make players grid on pong bounds.
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
    public PongMatchSettings matchSettings = new PongMatchSettings();
    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI player2Score;
    public Image topPlayerGrid;
    public Image bottomPlayerGrid;
    public GameObject pauseButton;
    public GameObject restartMatchButton;
    public GameObject startMatchButton;
    public PongBounds pongBounds;
    public GameObject pauseOverlay;
    public float initialBallSpeed = 5f;
    public float currentBallSpeed { get; internal set; }
    public float maxBallSpeed = 10f;
    [Range(0, 99)]
    public int ballHitsToMaxSpeed = 10;
    int hitCount;
    public bool canPause = true;
    public bool gameRunning { get; internal set; }
    bool isPaused;
    bool _controlOn;
    Vector2 ballVelocity;
    bool starting = false;
    float timeToStart = 1f;
    float timeToStartCount = 0;
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
        if (startMatchButton)
            startMatchButton.SetActive(false);
        if (restartMatchButton)
            restartMatchButton.SetActive(false);
    }

    private void Update()
    {
        if (starting)
        {
            if (!isPaused)
            {
                if (timeToStartCount >= timeToStart)
                {
                    if (ball.trailController)
                        ball.trailController.trailEnabled = true;

                    ball.ShootBall();
                    starting = false;
                }
                timeToStartCount += Time.deltaTime;
            }
        }
        else
        {
            timeToStartCount = 0f;
        }
    }
    /// <summary>
    /// Used to prepare the game, setting up the ball and the rackets' positions.
    /// </summary>
    public void PrepareGame()
    {
        ResumeGame();
        isPaused = false;
        gameRunning = false;
        starting = false;
        timeToStartCount = 0;
        if (pauseButton)
        {
            pauseButton.gameObject.GetComponentInChildren<SpriteSwap>().SetSprite(0);
            pauseButton.gameObject.SetActive(false);
        }
        if (pauseOverlay)
            pauseOverlay.SetActive(false);

        // Setting up Ball
        if (ballPrefab != null)
        {
            Destroy(ballObj);
            ballObj = Instantiate(ballPrefab, transform.parent);

            ball = ballObj.GetComponent<Ball>();
            if (ball)
            {
                TrailRendererController trc = ball.trailController;
                if (trc)
                {
                    trc.SetTrailColor(Color.white);
                    trc.trailTime = 0.3f;
                    trc.startWidth = 0.15f;
                    trc.endWidth = 0.05f;
                    trc.SetTrailColor(Color.white);
                    trc.UpdateTrail();
                    trc.trailEnabled = false;

                }
                ResetBall();
                currentBallSpeed = initialBallSpeed;
                ball.ballSpeed = currentBallSpeed;
                ball.OnHitRacket += OnBallHitRacket;
                ball.gameObject.SetActive(false);
            }
        }

        if (bounds)
        {
            bounds.CreateWalls();
        }

        matchSettings.bottomPlayerScore = matchSettings.topPlayerScore = 0;
        if (player1Score)
        {
            player1Score.text = "0";
            player1Score.color = bottomPlayer.racket.spriteRender.color;
        }
        if (player2Score)
        {
            player2Score.text = "0";
            player2Score.color = topPlayer.racket.spriteRender.color;
        }

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

            if (topPlayerGrid)
            {
                Color color = topPlayer.racket.spriteRender.color;
                topPlayerGrid.color = new Color(color.r, color.g, color.b, topPlayerGrid.color.a);
            }
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
            if (bottomPlayerGrid)
            {
                Color color = bottomPlayer.racket.spriteRender.color;
                bottomPlayerGrid.color = new Color(color.r, color.g, color.b, bottomPlayerGrid.color.a);
            }
        }

        canPause = true;
        if (startMatchButton)
            startMatchButton.SetActive(true);
        if (restartMatchButton)
            restartMatchButton.SetActive(false);

    }

    /// <summary>
    /// Begins the game.
    /// </summary>
    public void BeginGame()
    {

        if (startMatchButton)
            startMatchButton.SetActive(false);
        if (pauseButton)
            pauseButton.SetActive(true);
        if (restartMatchButton)
            restartMatchButton.SetActive(true);
        if (ball)
            ball.gameObject.SetActive(true);
        ResetBall();
        gameRunning = true;
        starting = true;

    }

    /// <summary>
    /// When the game ends.
    /// </summary>
    public void EndGame()
    {
        gameRunning = false;
        ball.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        PauseGame();
        ModalWindow.Choice("Reiniciar partida?", PrepareGame);
    }

    public void OnExitGame()
    {

        if (ball)
        {

            ball.trailController.trailRender.enabled = false;
            ball.trailController.trailEnabled = false;
        }
    }

    /// <summary>
    /// Pauses game.
    /// </summary>
    public void PauseGame()
    {
        if (isPaused || !canPause || !gameRunning)
            return;
        controlOn = false;
        ballVelocity = new Vector2(ball.rb.velocity.x, ball.rb.velocity.y);
        ball.rb.velocity = Vector2.zero;
        ball.rb.isKinematic = true;
        isPaused = true;
        if (pauseButton)
            pauseButton.gameObject.GetComponentInChildren<SpriteSwap>().SetSprite(1);
        if (pauseOverlay)
            pauseOverlay.SetActive(true);
        Time.timeScale = 0f;
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
        if (ball)
        {
            ball.trailController.trailRender.enabled = true;
            ball.trailController.trailEnabled = true;
        }
        if (pauseButton)
            pauseButton.gameObject.GetComponentInChildren<SpriteSwap>().SetSprite(0);
        if (pauseOverlay)
            pauseOverlay.SetActive(false);
        Time.timeScale = 1f;
    }



    /// <summary>
    /// Resets ball to the center.
    /// </summary>
    public void ResetBall()
    {
        if (ball)
        {
            ball.spriteRender.color = Color.white;
            ball.trailController.SetTrailColor(Color.white);
            ball.gameObject.transform.localPosition = new Vector3(0, 0, ball.gameObject.transform.position.z);
            ball.rb.velocity = Vector3.zero;
        }
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

    /// <summary>
    /// End of the round. (The ball touched a wall behind a player)
    /// </summary>
    /// <returns></returns>
    IEnumerator EndRound()
    {
        ResetBall();
        // Update scores and check for winner;
        if (ball.trailController)
            ball.trailController.trailEnabled = false;
        hitCount = 0;
        currentBallSpeed = initialBallSpeed;
        ball.ballSpeed = currentBallSpeed;
        yield return null;
        starting = true;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            PauseGame();
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            PauseGame();
    }

    /// <summary>
    /// When the ball hits a wall behind the player.
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
                if (wall.player == bottomPlayer)
                    matchSettings.topPlayerScore++;
                else
                    matchSettings.bottomPlayerScore++;
                UpdateScores();
            }
    }

    /// <summary>
    /// When the ball hits a racket.
    /// </summary>
    /// <param name="racket"></param>
    public void OnBallHitRacket(Ball ball, Racket racket)
    {
        hitCount++;
        hitCount = UtilityFunctions.ClampMax(hitCount, ballHitsToMaxSpeed);
        float progress = UtilityFunctions.Map(0, ballHitsToMaxSpeed, 0f, 1f, hitCount);
        float newSpeed = Mathf.Lerp(initialBallSpeed, maxBallSpeed, Mathf.Lerp(0f, 1f, progress));
        ball.ChangeSpeed(newSpeed);
    }

    public void UpdateScores()
    {
        if (player1Score)
            player1Score.text = matchSettings.bottomPlayerScore.ToString();
        if (player2Score)
            player2Score.text = matchSettings.topPlayerScore.ToString();
    }
}
