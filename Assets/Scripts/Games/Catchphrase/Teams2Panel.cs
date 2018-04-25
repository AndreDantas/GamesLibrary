using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public struct TeamInputInfo
{
    public TMP_InputField teamInput;
    public Image teamImg;
}
public class Teams2Panel : GamePanel
{

    public TeamInputInfo team1;
    public TeamInputInfo team2;
    public Button mainMenuButton;
    public float hideMainMenuButtonDistance = 135f;
    public ValueSelectUI roundsInput;
    public ValueSelectUI roundTimeInput;
    public CatchphrasePanel gamePanel;

    public override IEnumerator Enter()
    {
        if (roundsInput)
        {
            roundsInput.maxValue = 7;
            roundsInput.value = 2;
            roundsInput.minValue = 1;
        }
        if (roundTimeInput)
        {
            roundTimeInput.maxValue = 180;
            roundTimeInput.value = 30;
            roundTimeInput.minValue = 20;
        }
        yield return base.Enter();

    }


    public void InitTeams()
    {
        if (gamePanel == null)
            return;

        int rounds;
        int roundTime;
        roundTime = roundTimeInput != null ? roundTimeInput.value : 30;
        rounds = roundsInput != null ? roundsInput.value : 30;
        List<TeamInfo> team = new List<TeamInfo>();

        TeamInfo temp = new TeamInfo();
        temp.teamName = team1.teamInput.text;
        temp.teamColor = team1.teamImg.color;
        team.Add(temp);

        temp = new TeamInfo();
        temp.teamColor = team2.teamImg.color;
        temp.teamName = team2.teamInput.text;
        team.Add(temp);

        if (mainMenuButton)
        {
            mainMenuButton.interactable = false;
            RectTransform rect = mainMenuButton.transform as RectTransform;
            rect.MoveTo(new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + hideMainMenuButtonDistance), 0.2f);
        }

        //Set game panel teams
        gamePanel.SetGameStats(team, rounds, roundTime);
    }


}
