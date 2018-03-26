using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Position pos;

    /// <summary>
    /// The min walk cost of the Node.
    /// </summary>
    public static int MinCost = 1;
    /// <summary>
    /// The max walk cost of the Node.
    /// </summary>
    public static int MaxCost = 1;

    /// <summary>
    /// The piece on the Node.
    /// </summary>
    public Piece pieceOnNode;

    public Node(Position position)
    {
        this.pos = position;
    }
}
