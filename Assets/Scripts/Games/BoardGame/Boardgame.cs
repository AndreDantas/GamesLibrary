using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
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
    public float indicatorScale = 0.6f;


    protected virtual void Start()
    {
        if (playerTurnBorder)
            playerTurnBorder.SetActive(false);
        if (playerTurnIndicator)
        {
            playerTurnIndicator.SetActive(false);
        }
    }
    /// <summary>
    /// Function to render the map. 
    /// </summary>
    public abstract void RenderMap();
    public void ToggleMuteGame()
    {
        gameObject.ToggleMute();
    }

}
