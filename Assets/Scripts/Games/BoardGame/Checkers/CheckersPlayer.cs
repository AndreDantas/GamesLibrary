using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CheckersPlayer : Player
{

    public Orientation orientation;

    public CheckersPlayer()
    {

    }
    public CheckersPlayer(Orientation o)
    {
        orientation = o;
    }
}
