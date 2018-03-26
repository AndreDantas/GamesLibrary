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
    public InputField roundsInput;
    public InputField roundTimeInput;
    public CatchphrasePanel gamePanel;

    public override IEnumerator Enter()
    {
        if (roundsInput)
            roundsInput.text = "2";
        if (roundTimeInput)
            roundTimeInput.text = "30";
        yield return base.Enter();

    }


    public void InitTeams()
    {
        if (gamePanel == null)
            return;

        int rounds;
        int roundTime;
        int.TryParse(roundTimeInput != null ? roundTimeInput.text.Trim() != "" ? roundTimeInput.text : "30" : "30", out roundTime);
        int.TryParse(roundsInput != null ? roundsInput.text.Trim() != "" ? roundsInput.text : "1" : "1", out rounds);
        List<TeamInfo> team = new List<TeamInfo>();

        TeamInfo temp = new TeamInfo();
        temp.teamName = team1.teamInput.text;
        temp.teamColor = team1.teamImg.color;
        team.Add(temp);

        temp = new TeamInfo();
        temp.teamColor = team2.teamImg.color;
        temp.teamName = team2.teamInput.text;
        team.Add(temp);


        //Set game panel teams
        gamePanel.SetGameStats(team, rounds, roundTime);
    }

    public void ValidateRoundInput(string input)
    {
        int n = 0;
        int.TryParse(input, out n);
        n = Mathf.Clamp(n, 1, CatchphrasePanel.MAX_ROUNDS);
        if (roundsInput)
            roundsInput.text = n.ToString();
    }

    public void ValidateRoundTimeInput(string input)
    {
        int n = 0;
        int.TryParse(input, out n);
        n = Mathf.Clamp(n, 10, 300);
        if (roundTimeInput)
            roundTimeInput.text = n.ToString();
    }

}
