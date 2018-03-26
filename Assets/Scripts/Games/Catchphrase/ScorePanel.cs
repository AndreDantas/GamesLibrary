using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
[System.Serializable]
public struct TeamScoreInfo
{
    public Image teamColor;
    public TextMeshProUGUI teamName;
    public TextMeshProUGUI teamScore;
}
public struct TeamWord
{
    public string teamName;
    public string word;
    public bool correct;
}
public class ScorePanel : GamePanel
{
    [HideInInspector]
    public TeamInfo team1Result;
    [HideInInspector]
    public TeamInfo team2Result;
    public TeamScoreInfo team1;
    public TeamScoreInfo team2;
    public Color wordListColor = Color.black;
    public Color correctWord = Color.green;
    public Color wrongWord = Color.red;
    public GameObject wordListParent;
    public List<TeamWord> wordList = new List<TeamWord>();
    public override IEnumerator Enter()
    {
        team1.teamColor.color = team1Result.teamColor;
        team1.teamName.text = team1Result.teamName;
        team1.teamScore.text = team1Result.teamScore.ToString();

        team2.teamColor.color = team2Result.teamColor;
        team2.teamName.text = team2Result.teamName;
        team2.teamScore.text = team2Result.teamScore.ToString();
        SetWordList(wordList);
        yield return base.Enter();
    }

    public void SetWordList(List<TeamWord> wordList)
    {

        if (wordListParent)
        {
            List<GameObject> destroyList = new List<GameObject>();
            foreach (Transform child in wordListParent.transform)
            {
                destroyList.Add(child.gameObject);
            }

            for (int i = destroyList.Count - 1; i >= 0; i--)
            {
                Destroy(destroyList[i]);
            }

            foreach (TeamWord w in wordList)
            {
                GameObject word = new GameObject("Word: " + w.word + " - Team: " + w.teamName);
                word.transform.SetParent(wordListParent.transform);
                word.transform.localScale = Vector3.one;
                TextMeshProUGUI text = word.AddComponent<TextMeshProUGUI>();
                text.enableAutoSizing = true;
                text.alignment = TextAlignmentOptions.Midline;
                text.fontSizeMax = 50;
                text.richText = true;
                text.text = string.Format("{0} - {1}  <sprite=\"Wrong-Correct\" name=\"{3}\" color={2} >",
                                          w.word,
                                          w.teamName,
                                          w.correct ? ColorTypeConverter.ToRGBHex(correctWord) : ColorTypeConverter.ToRGBHex(wrongWord),
                                          w.correct ? "Correct" : "Wrong");
                text.color = wordListColor;
                text.enableWordWrapping = false;
            }
        }
    }
}
