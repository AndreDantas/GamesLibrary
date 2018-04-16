using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PongMatchSettingsPanel : GamePanel
{
    public ValueSelectUI scoreTarget;
    public PongGameController pongGame;

    private void Awake()
    {
        if (scoreTarget)
        {
            scoreTarget.minValue = 1;
            scoreTarget.maxValue = 15;
            scoreTarget.value = 5;
            scoreTarget.OnValueChanged.AddListener(OnScoreChange);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();

    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public void OnScoreChange(int value)
    {
        if (pongGame)
            pongGame.matchSettings.scoreTarget = value;
    }

}
