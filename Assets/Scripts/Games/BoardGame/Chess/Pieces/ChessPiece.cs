using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ChessPiece : Piece
{
    public ChessPieceType type;
    public int value = 1;
    public Position startPosition;
    public ChessBoard board;

    public ChessPiece()
    {

    }
    public ChessPiece(Position position) : base(position)
    {
    }
    public ChessPiece(Position position, ChessPlayer player) : base(position)
    {
        this.player = player;
    }
    public ChessPiece(ChessPiece other) : base(other)
    {
        if (other != null)
        {
            type = other.type;
            value = other.value;
            startPosition = other.startPosition;
            hasMoved = other.hasMoved;
            player = other.player;
            board = other.board;
        }
    }
    public ChessPiece(Piece piece) : base(piece)
    {
        player = piece.player as ChessPlayer;
    }

    public virtual void MoveToPos(Move move)
    {
        if (board.nodes[move.end.x, move.end.y].pieceOnNode != null)
        {
            board.RemovePiece(move.end);
        }
        board.Move(move);
        hasMoved = true;
    }

    /// <summary>
    /// Remove Moves from a list that could put the player in check.
    /// </summary>
    /// <param name="moves">The list of moves to check</param>
    /// <returns></returns>
    public static List<Move> RemoveMovesPlayerInCheck(List<Move> moves, ChessBoard board, ChessPlayer player)
    {
        if (board == null)
            return null;

        List<Move> possibleMove = new List<Move>();

        foreach (Move m in moves)
        {
            ChessBoard boardAfterMove = board.BoardAfterMove(m);
            if (!boardAfterMove.IsPlayerInCheck(player))
            {
                possibleMove.Add(m);
            }
        }

        return possibleMove;
    }

    public override string ToString()
    {
        if (startPosition == null)
            startPosition = new Position(0, 0);
        if (pos == null)
            pos = new Position(0, 0);
        string s = string.Format("Piece: {0} \n" +
                                "Start Position: ({1},{2}) - Current Position: ({3},{4}) \n" +
                                "Player: {5} \n" +
                                "Board: {6}", type, startPosition.x, startPosition.y, pos.x, pos.y, player, board);
        return s;
    }

    /// <summary>
    /// Returns a list of the piece's possible movements.
    /// </summary>
    /// <returns></returns>
    public virtual List<Move> GetPossibleMovement()
    {

        return null;
    }

    public virtual ChessPiece GetCopy()
    {
        return null;
    }
}
