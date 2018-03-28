using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Orientation
{
    UP,
    DOWN
}
[System.Serializable]
public class ChessPlayer : Player
{

    public Orientation orientation;
    public bool inCheck = false;
    public ChessPlayer(Orientation orientation)
    {
        this.orientation = orientation;
    }

    public override string ToString()
    {
        return "Player - Orientation: " + orientation;
    }
}
