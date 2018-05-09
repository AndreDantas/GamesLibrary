using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class ReversiNode : Node
{

    public ReversiPiece pieceOnNode;
    public ReversiNode(Position position) : base(position)
    {
    }

    public ReversiNode(ReversiNode other) : base(other)
    {
        if (other.pieceOnNode != null)
            pieceOnNode = new ReversiPiece(other.pieceOnNode);
        else
            pieceOnNode = null;
    }

    public ReversiNode(ReversiNode other, ReversiBoard board) : base(other)
    {
        if (other.pieceOnNode != null)
        {
            pieceOnNode = new ReversiPiece(other.pieceOnNode);
            pieceOnNode.board = board;
        }
        else
            pieceOnNode = null;
    }


}
