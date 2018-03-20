using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScorePanel : GamePanel
{
    public List<TeamInfo> teamsScore;
    public TextMeshProUGUI teamsScoreText;

    public override IEnumerator Enter()
    {
        if (teamsScore != null)
        {
            teamsScoreText.text = "";
            foreach (TeamInfo team in teamsScore)
            {
                teamsScoreText.text += team.teamName + " : " + team.teamScore + " pts \n \n";
            }
        }
        yield return base.Enter();
    }
}
