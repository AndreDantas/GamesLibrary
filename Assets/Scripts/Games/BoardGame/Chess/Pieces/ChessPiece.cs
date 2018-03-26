using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : Piece
{
    public ChessPieceType type;
    public int value = 1;
    public Position startPosition;
    public ChessPiece(Position position) : base(position)
    {
    }

    /// <summary>
    /// Remove Moves from a list that could put the player in check.
    /// </summary>
    /// <param name="moves">The list of moves to check</param>
    /// <returns></returns>
    public List<Move> RemoveMovesPlayerInCheck(List<Move> moves)
    {
        if (board == null)
            return null;
        List<Move> possibleMove = new List<Move>();

        foreach (Move m in moves)
        {
            Chess boardAfterMove = (Chess)board.BoardAfterMove(m);
            if (!boardAfterMove.IsPlayerInCheck(player))
            {
                possibleMove.Add(m);
            }
        }

        return possibleMove;
    }
}
