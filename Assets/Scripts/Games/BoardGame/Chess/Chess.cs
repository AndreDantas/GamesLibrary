using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Chess : Board
{
    public ChessPlayer player1;
    public ChessPlayer player2;

    public Chess()
    {

    }
    public Chess(Board oldBoard)
    {
        columns = oldBoard.columns;
        rows = oldBoard.rows;
        nodeOffsetX = oldBoard.nodeOffsetX;
        nodeOffsetY = oldBoard.nodeOffsetY;
        nodes = (Node[,])oldBoard.nodes.Clone();
    }
    /// <summary>
    /// Checks if a player is in check.
    /// </summary>
    /// <param name="playerToCheck">The player to check.</param>
    /// <param name="opponent">The other player.</param>
    /// <returns></returns>
    public bool IsPlayerInCheck(Player playerToCheck)
    {
        List<Move> moves = GetPossibleMoves(playerToCheck == player1 ? player2 : player1);
        for (int i = 0; i < moves.Count; i++)
        {
            Position pos = moves[i].end;
            ChessPiece piece = GetPiece(pos) as ChessPiece;
            if (piece != null)
            {
                if (piece.player == playerToCheck)
                {
                    if (piece.type == ChessPieceType.KING)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }


    /// <summary>
    /// Returns all possible movements from the pieces of the given player.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public List<Move> GetPossibleMoves(Player player)
    {
        List<Move> possibleMoves = new List<Move>();
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Position pos = new Position(i, j);
                Piece current = GetPiece(pos);
                if (current != null)
                {
                    if (current.player == player)
                    {
                        possibleMoves.AddRange(current.GetPossibleMovement());
                    }
                }
            }
        }
        return possibleMoves;
    }



}
