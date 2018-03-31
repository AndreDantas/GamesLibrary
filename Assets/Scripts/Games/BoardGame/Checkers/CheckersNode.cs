using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckersNode : Node
{


    public Checker checkerOnNode;

    public CheckersNode(Position pos) : base(pos)
    {

    }

    public CheckersNode(CheckersNode other)
    {
        if (other.checkerOnNode != null)
            checkerOnNode = new Checker(other.checkerOnNode);
        else
            checkerOnNode = null;
    }
    public CheckersNode(CheckersNode other, CheckersBoard board)
    {
        if (other.checkerOnNode != null)
        {
            checkerOnNode = new Checker(other.checkerOnNode);
            checkerOnNode.board = board;
        }
        else
            checkerOnNode = null;
    }
}
