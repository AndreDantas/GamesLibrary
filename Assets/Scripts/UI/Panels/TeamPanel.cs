using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TeamPanel : GamePanel
{

    public List<PickAColor> teams;
    public InputField roundsInput;
    public InputField roundTimeInput;
    public CatchphrasePanel gamePanel;

    public override IEnumerator Enter()
    {
        ResetSettings();
        yield return base.Enter();

    }


    public void ResetSettings()
    {
        StartCoroutine(ResetSett());
    }

    public void InitTeams()
    {
        if (gamePanel == null)
            return;

        List<TeamInfo> newTeams = new List<TeamInfo>();
        for (int i = 0; i < teams.Count; i++)
        {
            TeamInfo temp = new TeamInfo();
            temp.teamColor = teams[i].color;
            temp.teamName = teams[i].teamName;
            if (temp.teamName.Trim() == "")
                temp.teamName = "Team " + (i + 1);
            temp.teamScore = 0;
            newTeams.Add(temp);
        }
        int rounds;
        int roundTime;
        int.TryParse(roundTimeInput != null ? roundTimeInput.text : "30", out roundTime);
        int.TryParse(roundsInput != null ? roundsInput.text : "1", out rounds);
        gamePanel.SetGameStats(newTeams, rounds, roundTime);
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

    IEnumerator ResetSett()
    {
        if (teams != null ? teams.Count == 0 : true)
            yield break;
        roundsInput.text = "2";
        roundTimeInput.text = "30";
        foreach (PickAColor p in teams)
        {
            p.colorPicker.SetRandomColor();
            yield return null;
        }
    }
}
