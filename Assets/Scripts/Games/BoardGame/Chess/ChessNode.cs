using UnityEngine;
using System.Collections;
[System.Serializable]
public class ChessNode : Node
{
    public ChessPiece pieceOnNode;

    public ChessNode(Position position) : base(position)
    {
    }

    public ChessNode(ChessNode other) : base(other)
    {
        if (other.pieceOnNode != null)
            pieceOnNode = other.pieceOnNode.GetCopy();
        else
            pieceOnNode = null;
    }

    public ChessNode(ChessNode other, ChessBoard board) : base(other)
    {
        if (other.pieceOnNode != null)
        {
            pieceOnNode = other.pieceOnNode.GetCopy();
            pieceOnNode.board = board;
        }
        else
            pieceOnNode = null;
    }
}
