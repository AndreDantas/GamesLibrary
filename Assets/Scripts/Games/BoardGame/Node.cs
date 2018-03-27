using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public Node()
    {

    }
    public Node(Position position)
    {
        this.pos = position;
    }

    public Node(Node oldNode)
    {
        if (oldNode == null)
            return;
        pos = oldNode.pos;


    }


}
