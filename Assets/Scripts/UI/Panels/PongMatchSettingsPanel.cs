﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PongMatchSettingsPanel : GamePanel
{
    public ValueSelectUI scoreTarget;
    public OptionSelectUI matchSpeed;
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

        if (matchSpeed)
        {
            matchSpeed.options = new List<StringObjectPair> { new StringObjectPair("Normal", 1), new StringObjectPair("Rápida", 0) };
            matchSpeed.UpdateUI();
            matchSpeed.OnOptionChanged.AddListener(OnOptionChanged);
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
        {
            pongGame.matchSettings.scoreTarget = value;
        }
    }

    public void OnOptionChanged(object obj)
    {
        if (pongGame)
        {

            if ((int)obj == 0)
            {
                pongGame.initialBallSpeed = 6f;
                pongGame.maxBallSpeed = 15f;
                pongGame.ballHitsToMaxSpeed = 15;
                pongGame.topPlayer.racket.racketSpeed = pongGame.bottomPlayer.racket.racketSpeed = 1f;
            }
            else
            {
                pongGame.initialBallSpeed = 5f;
                pongGame.maxBallSpeed = 10f;
                pongGame.ballHitsToMaxSpeed = 10;
                pongGame.topPlayer.racket.racketSpeed = pongGame.bottomPlayer.racket.racketSpeed = 0.13f;
            }
        }
    }

}
