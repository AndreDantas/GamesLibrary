using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Move
{

    public Position start;
    public Position end;

    public Move(Position start, Position end)
    {
        this.start = start;
        this.end = end;
    }
}
