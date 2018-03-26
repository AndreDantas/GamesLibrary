using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlayer : Player
{

    public int orientation;

    public ChessPlayer(int orientation)
    {
        this.orientation = MathOperations.Sign(orientation);
    }

}
