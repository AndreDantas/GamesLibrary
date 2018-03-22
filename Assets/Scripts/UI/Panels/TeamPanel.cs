using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TeamPanel : GamePanel
{

    public InputField roundsInput;
    public InputField roundTimeInput;
    public CatchphrasePanel gamePanel;
    public List<GameObject> panelObjects = new List<GameObject>();
    public List<TeamInfo> teams = new List<TeamInfo>();
    private float[] objectsY;
    protected override void OnEnable()
    {
        base.OnEnable();
        if (panelObjects != null)
        {
            objectsY = new float[panelObjects.Count];
            int counter = 0;
            foreach (GameObject button in panelObjects)
            {
                objectsY[counter] = button.transform.localPosition.y;
                counter++;
            }
        }

    }

    public override IEnumerator Enter()
    {
        if (moving)
            yield break;
        if (roundsInput)
            roundsInput.text = "2";
        if (roundTimeInput)
            roundTimeInput.text = "30";
        moving = true;
        if (panelObjects == null ? true : panelObjects.Count == 0)
            yield return base.Enter();


        RectTransform rect = transform as RectTransform;
        Vector2 start = new Vector2(rect.rect.width + screenCenter.x, screenCenter.y);
        Vector2 end = screenCenter;
        transform.localPosition = start;
        foreach (GameObject button in panelObjects)
        {
            button.gameObject.SetActive(false);
        }
        yield return null;
        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime / 2f);

        for (int i = 0; i < panelObjects.Count; i++)
        {

            RectTransform buttonRect = panelObjects[i].transform as RectTransform;

            buttonRect.localPosition = new Vector3(start.x, objectsY[i], buttonRect.localPosition.z);

            panelObjects[i].gameObject.SetActive(true);
            panelObjects[i].transform.MoveToLocal(new Vector3(end.x, objectsY[i], buttonRect.localPosition.z), animTime);
            yield return new WaitForSeconds(animTime / 2f);
        }
        yield return new WaitForSeconds(animTime / 2f);

        moving = false;

    }

    public override IEnumerator Exit()
    {
        if (moving)
            yield break;

        moving = true;
        if (panelObjects == null ? true : panelObjects.Count == 0)
            yield return base.Exit();

        RectTransform rect = transform as RectTransform;
        Vector2 start = screenCenter;
        Vector2 end = new Vector2(-rect.rect.width + screenCenter.x, screenCenter.y);
        transform.localPosition = start;

        for (int i = panelObjects.Count - 1; i >= 0; i--)
        {

            RectTransform buttonRect = panelObjects[i].transform as RectTransform;

            buttonRect.localPosition = new Vector3(start.x, objectsY[i], buttonRect.localPosition.z);

            panelObjects[i].gameObject.SetActive(true);
            panelObjects[i].transform.MoveToLocal(new Vector3(end.x, objectsY[i], buttonRect.localPosition.z), animTime);
            yield return new WaitForSeconds(animTime / 2f);
        }



        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime);

        gameObject.SetActive(false);
        moving = false;
    }

    public void InitTeams()
    {
        if (gamePanel == null)
            return;

        int rounds;
        int roundTime;
        int.TryParse(roundTimeInput != null ? roundTimeInput.text.Trim() != "" ? roundTimeInput.text : "30" : "30", out roundTime);
        int.TryParse(roundsInput != null ? roundsInput.text.Trim() != "" ? roundsInput.text : "1" : "1", out rounds);
        //Set game panel teams
        gamePanel.SetGameStats(teams, rounds, roundTime);
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
