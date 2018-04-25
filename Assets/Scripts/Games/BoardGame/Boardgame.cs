﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boardgame : MonoBehaviour
{
    [Header("Board Settings")]
    public int columns = 8;
    public int rows = 8;
    [ReadOnly]
    public float boardWidth;
    [ReadOnly]
    public float boardHeight;

    public GameObject playerTurnIndicator;
    public GameObject playerTurnBorder;


    /// <summary>
    /// Function to render the map. 
    /// </summary>
    public abstract void RenderMap();

}
