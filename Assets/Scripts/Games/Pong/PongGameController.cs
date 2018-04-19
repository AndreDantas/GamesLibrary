using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class PongMatchSettings
{
    public int scoreTarget = 5;

    public int bottomPlayerScore;
    public int topPlayerScore;

    public PongMatchSettings()
    {

    }
}

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
    public AudioClip hitRacketSound;
    public AudioClip hitWallSound;
    public TextMeshProUGUI bottomPlayerScore;
    public TextMeshProUGUI topPlayerScore;
    public Image topPlayerGrid;
    public Image bottomPlayerGrid;
    public GameObject pauseButton;
    public GameObject restartMatchButton;
    public Button middleButton;
    public GameObject ballShootIndicator;
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
    float timeToStart = 3f;
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
        if (middleButton)
            middleButton.gameObject.SetActive(false);
        if (restartMatchButton)
            restartMatchButton.SetActive(false);
        if (ballShootIndicator)
            ballShootIndicator.SetActive(false);
        gameObject.AddAudio(hitRacketSound, false, false, 0.6f);
        gameObject.AddAudio(hitWallSound, false, false, 0.6f);
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
            if (ballShootIndicator ? ballShootIndicator.activeSelf == true : false)
                ballShootIndicator.SetActive(false);
            timeToStartCount = 0f;
        }
    }
    /// <summary>
    /// Used to prepare the game, setting up the ball and the rackets' positions.
    /// </summary>
    public void PrepareGame()
    {
        if (middleButton)
            middleButton.gameObject.SetActive(false);
        StartCoroutine(SetUpGame());
    }

    IEnumerator SetUpGame()
    {
        yield return null;
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
        if (bottomPlayerScore)
        {
            bottomPlayerScore.text = "0";
            bottomPlayerScore.color = bottomPlayer.racket.spriteRender.color;
        }
        if (topPlayerScore)
        {
            topPlayerScore.text = "0";
            topPlayerScore.color = topPlayer.racket.spriteRender.color;
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
        if (middleButton)
        {
            middleButton.gameObject.SetActive(true);
            middleButton.onClick.RemoveAllListeners();
            middleButton.onClick.AddListener(BeginGame);
            TextMeshProUGUI text = middleButton.GetComponentInChildren<TextMeshProUGUI>();
            if (text)
            {
                text.text = "Começar";
            }
        }
        if (restartMatchButton)
            restartMatchButton.SetActive(false);
        controlOn = false;
    }

    /// <summary>
    /// Begins the game.
    /// </summary>
    public void BeginGame()
    {

        if (middleButton)
            middleButton.gameObject.SetActive(false);
        if (pauseButton)
            pauseButton.SetActive(true);
        if (restartMatchButton)
            restartMatchButton.SetActive(true);
        if (ball)
            ball.gameObject.SetActive(true);
        ResetBall();
        gameRunning = true;
        controlOn = true;
        canPause = true;
        PrepareToShootBall();
    }

    /// <summary>
    /// When the game ends.
    /// </summary>
    public void EndGame()
    {
        canPause = false;
        gameRunning = false;
        ball.gameObject.SetActive(false);
        //PrepareGame();
        if (matchSettings.topPlayerScore >= matchSettings.scoreTarget)
        {
            topPlayerScore.text = "Venceu!";
        }
        else
        {
            bottomPlayerScore.text = "Venceu!";
        }
        if (restartMatchButton)
            restartMatchButton.SetActive(false);
        if (pauseButton)
        {
            pauseButton.gameObject.GetComponentInChildren<SpriteSwap>().SetSprite(0);
            pauseButton.gameObject.SetActive(false);
        }
        if (middleButton)
        {
            middleButton.gameObject.SetActive(true);
            middleButton.onClick.RemoveAllListeners();
            middleButton.onClick.AddListener(PrepareGame);
            TextMeshProUGUI text = middleButton.GetComponentInChildren<TextMeshProUGUI>();
            if (text)
            {
                text.text = "Recomeçar";
            }
        }
    }


    public void RestartGame()
    {
        PauseGame();
        ModalWindow.Choice("Recomeçar partida?", PrepareGame);
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

    void PrepareToShootBall()
    {
        if (ball.trailController)
            ball.trailController.trailEnabled = false;

        hitCount = 0;
        currentBallSpeed = initialBallSpeed;
        ball.ballSpeed = currentBallSpeed;
        if (ballShootIndicator)
        {

            ballShootIndicator.SetActive(true);
            SpriteRenderer sr = ballShootIndicator.GetComponent<SpriteRenderer>();
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
            sr.FadeIn(timeToStart);
            ballShootIndicator.transform.localPosition = Vector3.zero;
            ballShootIndicator.transform.localScale = Vector3.one;
            ballShootIndicator.transform.ScaleTo(Vector3.zero, timeToStart * 1.2f);
        }
        starting = true;
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
        if (matchSettings.bottomPlayerScore >= matchSettings.scoreTarget || matchSettings.topPlayerScore >= matchSettings.scoreTarget)
        {
            EndGame();
            yield break;
        }
        // Update scores and check for winner;
        PrepareToShootBall();

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
                if (wall.player == bottomPlayer)
                    matchSettings.topPlayerScore++;
                else
                    matchSettings.bottomPlayerScore++;
                UpdateScores();
                gameObject.PlayAudio(hitWallSound);
                StartCoroutine(EndRound());


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
        gameObject.PlayAudio(hitRacketSound);
    }

    public void UpdateScores()
    {
        if (bottomPlayerScore)
            bottomPlayerScore.text = matchSettings.bottomPlayerScore.ToString();
        if (topPlayerScore)
            topPlayerScore.text = matchSettings.topPlayerScore.ToString();
    }
}
