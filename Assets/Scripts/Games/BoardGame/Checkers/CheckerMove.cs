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

    public CheckerMove(Position start, Position end, bool isCapture) : base(start, end)
    {
        this.isCapture = isCapture;
    }

    public static bool operator ==(CheckerMove a, CheckerMove b)
    {
        // If both are null, or both are same instance, return true.
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (((object)a == null) || ((object)b == null))
        {
            return false;
        }

        return a.start == b.start && a.end == b.end;

    }
    public static bool operator !=(CheckerMove a, CheckerMove b)
    {
        return !(a == b);
    }



    public override bool Equals(System.Object obj)
    {
        // If parameter is null return false.
        if (obj == null)
        {
            return false;
        }

        // If parameter cannot be cast to Point return false.
        CheckerMove p = obj as CheckerMove;
        if ((System.Object)p == null)
        {
            return false;
        }


        // Return true if the fields match:
        return start == p.start && end == p.end;
    }

    public bool Equals(CheckerMove p)
    {
        // If parameter is null return false:
        if ((object)p == null)
        {
            return false;
        }

        // Return true if the fields match:
        return start == p.start && end == p.end;
    }

    public override int GetHashCode()
    {
        return (start.x ^ start.y) * (end.x ^ end.y);
    }

    public override string ToString()
    {
        return "Move: (" + start.x + "," + start.y + ") to (" + end.x + "," + end.y + ") - Capture: " + isCapture;
    }
}


