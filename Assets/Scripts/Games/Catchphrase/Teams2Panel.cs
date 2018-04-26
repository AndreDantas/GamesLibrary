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

        SceneController.HideMainMenuButton();
        //Set game panel teams
        gamePanel.SetGameStats(team, rounds, roundTime);
    }


}
