using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Position : System.Object
{

    public int x;
    public int y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Position(Position oldPos)
    {
        if (oldPos == null)
            return;
        x = oldPos.x;
        y = oldPos.y;
    }

    /// <summary>
    /// Returns a list of random Positions from an area.
    /// </summary>
    /// <param name="amount">The number of random positions.</param>
    /// <param name="rangeX">The X range of the area</param>
    /// <param name="rangeY">The Y range of the area</param>
    /// <param name="allowDuplicates">If the same position can be generated.</param>
    /// <returns></returns>
    public static List<Position> GenerateRandomPositions(int amount, int rangeX = 8, int rangeY = 8, bool allowDuplicates = false)
    {
        rangeX = UtilityFunctions.ClampMin(rangeX, 1);
        rangeY = UtilityFunctions.ClampMin(rangeY, 1);
        amount = UtilityFunctions.ClampMax(amount, rangeY * rangeX);

        List<Position> result = new List<Position>();
        List<Position> availablePositions = new List<Position>();
        for (int i = 0; i < rangeY; i++)
        {
            for (int j = 0; j < rangeX; j++)
            {
                availablePositions.Add(new Position(j, i));
            }
        }
        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            result.Add(availablePositions[randomIndex]);
            if (!allowDuplicates)
                availablePositions.RemoveAt(randomIndex);
        }

        return result;
    }

    public static bool operator ==(Position a, Position b)
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

        // Return true if the fields match:
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Position a, Position b)
    {
        return !(a == b);
    }

    public static Position operator -(Position a)
    {
        if (a == null)
            return new Position(0, 0);

        return new Position(-a.x, -a.y);
    }
    public static Position operator -(Position a, Position b)
    {
        if (a == null)
            return new Position(0, 0);

        return new Position(a.x - b.x, a.y - b.y);
    }
    public override bool Equals(System.Object obj)
    {
        // If parameter is null return false.
        if (obj == null)
        {
            return false;
        }

        // If parameter cannot be cast to Point return false.
        Position p = obj as Position;
        if ((System.Object)p == null)
        {
            return false;
        }


        // Return true if the fields match:
        return (x == p.x) && (y == p.y);
    }

    public bool Equals(Position p)
    {
        // If parameter is null return false:
        if ((object)p == null)
        {
            return false;
        }

        // Return true if the fields match:
        return (x == p.x) && (y == p.y);
    }

    public override string ToString()
    {
        return "(" + x + "," + y + ")";
    }

    public override int GetHashCode()
    {
        var hashCode = 1502939027;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        return hashCode;
    }

    public static readonly Position Up = new Position(0, 1);
    public static readonly Position Down = new Position(0, -1);
    public static readonly Position Left = new Position(-1, 0);
    public static readonly Position Right = new Position(1, 0);
}
