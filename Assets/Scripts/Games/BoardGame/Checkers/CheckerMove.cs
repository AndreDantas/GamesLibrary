using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CheckerMove : Move
{
    public CheckerMove previous;
    public CheckerMove next;
    public bool isCapture = false;

    public CheckerMove(Position start, Position end) : base(start, end)
    {
    }
}
