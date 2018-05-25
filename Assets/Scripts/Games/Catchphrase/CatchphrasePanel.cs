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
    public CatchphraseScorePanel scorePanel;
    public List<TeamInfo> teams = new List<TeamInfo>();
    public TextMeshProUGUI roundText;
    public List<PairLanguageText> roundTranslation;
    public TextMeshProUGUI wordText;
    public TextMeshProUGUI teamNameText;
    public TextMeshProUGUI scoreText;
    public Button correctWordButton;
    public Image teamBackground;
    List<TeamWord> wordList;
    bool gameStart;
    bool correctWord;
    string currentWord;
    int currentRound;
    int roundTime;
    Color backgroundOldColor;
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
        backgroundOldColor = teamBackground.color;
        catchphraseController.countdown = roundTime;
        catchphraseController.HideTimer();
        catchphraseController.ResetWordList();
        index = -1;
        currentRound = 1;
        correctWord = false;
        wordList = new List<TeamWord>();
        foreach (TeamInfo team in teams)
        {
            team.teamScore = 0;
        }
    }
    public override IEnumerator Enter()
    {
        HideUI();
        PrepareGame();
        catchphraseController.timer.OnFinish += OnFinishTimer;
        yield return base.Enter();
        IntroPhase();
    }

    public void ConfirmExitGame()
    {
        PauseGame();
        ModalWindow.Choice("Sair da partida?", ForceExitGame, ResumeGame);
    }

    void ForceExitGame()
    {
        teamBackground.color = backgroundOldColor;
        ResumeGame();
        if (onBackPanel != null)
            SceneController.instance.ChangePanel(onBackPanel);
        else
            SceneController.instance.ChangePanel(scorePanel);

        SceneController.ShowMainMenuButton();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    void HideUI()
    {
        catchphraseController.HideTimer();
        roundText.gameObject.SetActive(false);
        wordText.gameObject.SetActive(false);
        teamNameText.gameObject.SetActive(false);
        correctWordButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
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
        roundText.text = roundTranslation.GetTextFromMainLanguage().Trim() + " " + currentRound + "/" + rounds;

        wordText.gameObject.SetActive(false);
        correctWordButton.gameObject.SetActive(false);

        roundText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        teamNameText.gameObject.SetActive(true);

        teamBackground.color = teams[index].teamColor;
        teamNameText.text = teams[index].teamName;
        scoreText.text = "";
        for (int i = 0; i < teams.Count; i++)
        {
            if (i != teams.Count - 1)
            {
                scoreText.text += string.Format("<color={0}>{1} : <b>{2}</b> </color> ",
                                                 ColorTypeConverter.ToRGBHex(teams[i].teamColor),
                                                 teams[i].teamName,
                                                 teams[i].teamScore + "pts");

                scoreText.text += "| ";
            }
            else
            {
                scoreText.text += string.Format("<color={0}><b>{1}</b> : {2}</color> ",
                                                 ColorTypeConverter.ToRGBHex(teams[i].teamColor),
                                                 teams[i].teamScore + "pts",
                                                 teams[i].teamName);

            }

        }
        catchphraseController.HideTimer();
        StartCoroutine(NextRound());
    }

    IEnumerator EndGame()
    {
        HideUI();
        teamBackground.color = backgroundOldColor;
        wordText.gameObject.SetActive(true);
        wordText.text = "Fim de Jogo";
        yield return new WaitForSeconds(2f);
        scorePanel.team1Result = teams[0];
        scorePanel.team2Result = teams[1];
        scorePanel.wordList = wordList;
        SceneController.instance.ChangePanel(scorePanel);
    }

    IEnumerator NextRound()
    {
        if (catchphraseController == null)
            yield break;
        yield return new WaitForSeconds(2f);

        teamNameText.gameObject.SetActive(false);
        correctWordButton.gameObject.SetActive(true);


        wordText.gameObject.SetActive(true);
        wordText.alignment = TextAlignmentOptions.Capline;
        catchphraseController.ShowTimer();
        currentWord = catchphraseController.NewWord();

    }

    IEnumerator EndRound()
    {
        yield return null;
        TeamWord temp = new TeamWord();
        temp.teamName = teams[index].teamName;
        temp.word = currentWord;
        temp.correct = correctWord;
        correctWord = false;
        wordList.Add(temp);
        IntroPhase();
    }

    public void OnFinishTimer()
    {
        StartCoroutine(EndRound());
    }

    public void CorrectWord()
    {
        teams[index].teamScore++;

        correctWord = true;
        StartCoroutine(EndRound());

    }

    public void SetGameStats(List<TeamInfo> teams, int rounds, int roundTime)
    {
        this.teams = teams;
        this.rounds = rounds;
        this.roundTime = roundTime;
    }
    public override void OnBack()
    {
        ConfirmExitGame();
    }
}
