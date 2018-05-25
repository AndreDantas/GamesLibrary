using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public abstract class Boardgame : Game
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
    public float indicatorScale = 0.5f;


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
    public bool ValidCoordinate(Position pos)
    {
        int x = pos.x;
        int y = pos.y;


        return ValidCoordinate(x, y);
    }
    public bool ValidCoordinate(int x, int y)
    {


        if (x < 0 || x >= columns)
            return false;
        if (y < 0 || y >= rows)
            return false;

        return true;
    }
    public virtual void OnClick(Position pos)
    {

    }

    protected void OnApplicationQuit()
    {
        GameExit();

    }

    public virtual void GameExit()
    {

    }

    private void OnDestroy()
    {
        GameExit();
    }

    private void OnDisable()
    {
        GameExit();
    }

}
