using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Move : System.Object
{

    public Position start;
    public Position end;

    public Move(Position start, Position end)
    {
        this.start = start;
        this.end = end;
    }

    public static bool operator ==(Move a, Move b)
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
    public static bool operator !=(Move a, Move b)
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
        Move p = obj as Move;
        if ((System.Object)p == null)
        {
            return false;
        }


        // Return true if the fields match:
        return start == p.start && end == p.end;
    }

    public bool Equals(Move p)
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
        return "Start: " + start + " - End: " + end;
    }
}
