using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
[System.Serializable]
public class TeamInfo
{

    public string teamName;
    public Color teamColor;
    public int teamScore;
}
public class CatchphrasePanel : GamePanel
{

    public static int MAX_ROUNDS = 10;
    public CatchphraseController catchphraseController;
    public ScorePanel scorePanel;
    public List<TeamInfo> teams = new List<TeamInfo>();
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI wordText;
    public TextMeshProUGUI teamNameText;
    public Button correctWordButton;
    public Image teamBackground;
    bool gameStart;
    int currentRound;
    int roundTime;
    [SerializeField]
    int _rounds;
    public int rounds
    {
        get
        {
            return _rounds;
        }
        set
        {

            _rounds = value;

            _rounds = Mathf.Clamp(_rounds, 1, MAX_ROUNDS);
        }
    }
    int index = -1;


    private void Awake()
    {
        if (catchphraseController == null)
            catchphraseController = GetComponent<CatchphraseController>();
    }

    public void PrepareGame()
    {
        catchphraseController.countdown = roundTime;
        catchphraseController.HideTimer();

        HideUI();

        index = -1;
        currentRound = 1;

        foreach (TeamInfo team in teams)
        {
            team.teamScore = 0;
        }
    }
    public override IEnumerator Enter()
    {
        PrepareGame();
        catchphraseController.timer.OnFinish += OnFinishTimer;
        yield return base.Enter();
        IntroPhase();
    }

    void HideUI()
    {
        catchphraseController.HideTimer();
        roundText.gameObject.SetActive(false);
        wordText.gameObject.SetActive(false);
        teamBackground.gameObject.SetActive(false);
        teamNameText.gameObject.SetActive(false);
        correctWordButton.gameObject.SetActive(false);
    }

    public override IEnumerator Exit()
    {
        catchphraseController.timer.OnFinish -= OnFinishTimer;
        yield return base.Exit();
    }

    void IntroPhase()
    {
        index++;
        if (index >= teams.Count)
        {
            index = 0;
            currentRound++;
        }
        if (currentRound > rounds)
        {
            StartCoroutine(EndGame());
            return;
        }

        roundText.gameObject.SetActive(false);
        wordText.gameObject.SetActive(false);
        correctWordButton.gameObject.SetActive(false);

        teamNameText.gameObject.SetActive(true);
        teamBackground.gameObject.SetActive(true);
        teamBackground.color = teams[index].teamColor;
        teamNameText.text = teams[index].teamName;
        catchphraseController.HideTimer();
        StartCoroutine(NextRound());
    }

    IEnumerator EndGame()
    {
        HideUI();
        wordText.gameObject.SetActive(true);
        wordText.text = "Fim de Jogo";
        yield return new WaitForSeconds(2f);
        scorePanel.teamsScore = teams;
        FindObjectOfType<SceneController>().ChangePanel(scorePanel);
    }

    IEnumerator NextRound()
    {
        if (catchphraseController == null)
            yield break;
        yield return new WaitForSeconds(2f);

        teamNameText.gameObject.SetActive(false);

        roundText.gameObject.SetActive(true);
        correctWordButton.gameObject.SetActive(true);

        roundText.text = "Round " + currentRound + "/" + rounds;
        wordText.gameObject.SetActive(true);
        catchphraseController.ShowTimer();
        catchphraseController.NewWord();
    }

    public void OnFinishTimer()
    {
        IntroPhase();
    }

    public void CorrectWord()
    {
        teams[index].teamScore++;
        IntroPhase();

    }

    public void SetGameStats(List<TeamInfo> teams, int rounds, int roundTime)
    {
        this.teams = teams;
        this.rounds = rounds;
        this.roundTime = roundTime;
    }
}
