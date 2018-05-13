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
    public int rows = 8;
    /// <summary>
    /// Number of columns. Can't be less than 1.
    /// </summary>
    public int columns = 8;
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

    public bool ValidCoordinate(Position pos)
    {
        if (pos != null)
            return ValidCoordinate(pos.x, pos.y);
        else
            return false;
    }
    /// <summary>
    /// Checks if the coordinate is valid in this map.
    /// </summary>
    public bool ValidCoordinate(Node node)
    {
        if (node == null)
            return false;

        return ValidCoordinate(node.pos);
    }
    /// <summary>
    /// Checks if the coordinate is valid in this map.
    /// </summary>
    public bool ValidCoordinate(int x, int y)
    {
        if (x < 0 || x >= columns)
            return false;
        if (y < 0 || y >= rows)
            return false;

        return true;
    }

    /// <summary>
    /// Checks if the coordinate is valid in this map.
    /// </summary>
    public bool ValidCoordinate(Vector2 worldPos)
    {

        int tilePosX = (int)Mathf.Floor(worldPos.x);
        int tilePosY = (int)Mathf.Floor(worldPos.y);

        return ValidCoordinate(tilePosX, tilePosY);
    }

}
