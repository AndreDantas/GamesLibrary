using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum MapNavigation
{
    /// <summary>
    /// Vertical and horizontal movement.
    /// </summary>
    Cross,
    /// <summary>
    /// 8 directions movement.
    /// </summary>
    Diagonals
}
[System.Serializable]
public enum NodeDistance
{
    Manhattan,
    Euclidean
}
[System.Serializable]
public class Board
{

    /// <summary>
    /// Type of navigation.
    /// </summary>
    public MapNavigation mapNavigation = MapNavigation.Cross;
    /// <summary>
    /// Number of rows. Can't be less than 1.
    /// </summary>
    public int rows = 10;
    /// <summary>
    /// Number of columns. Can't be less than 1.
    /// </summary>
    public int columns = 10;
    /// <summary>
    ///The X offset of each node.
    /// </summary>
    public float nodeOffsetX = 0.5f;
    /// <summary>
    ///The Y offset of each node.
    /// </summary>
    public float nodeOffsetY = 0.5f;

    /// <summary>
    /// If the board has been initiated.
    /// </summary>
    public bool isInit;

    public virtual void InitBoard()
    {
    }

    /// <summary>
    /// Distance between two nodes.
    /// </summary>
    public static float Distance(ChessNode a, ChessNode b, NodeDistance distance = NodeDistance.Manhattan)
    {
        return Distance(a.pos, b.pos, distance);
    }
    public static float Distance(Position a, Position b, NodeDistance distance = NodeDistance.Manhattan)
    {
        if (distance == NodeDistance.Manhattan)
        {
            return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y)) * ChessNode.MinCost;
        }
        else
            return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2));
    }
}
