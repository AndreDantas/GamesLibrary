using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public Pawn(Position pos) : base(pos)
    {
        type = ChessPieceType.PAWN;
    }

    public override List<Move> GetPossibleMovement()
    {
        List<Move> moves = new List<Move>();
        ChessPlayer player = this.player as ChessPlayer;
        Chess board = this.board as Chess;
        int dy = -MathOperations.Sign(player.orientation);

        Position newPos = new Position(pos.x, pos.y + dy);


        if (board.IsPositionEmpty(newPos))
        {

            Move currentMove = new Move(pos, newPos);
            Chess boardAfterMove = new Chess(board.BoardAfterMove(currentMove));

            if (!boardAfterMove.IsPlayerInCheck(player))
            {

                moves.Add(currentMove);
            }

            newPos = new Position(pos.x, pos.y + dy * 2);
            if (board.IsPositionEmpty(newPos) && pos == startPosition)
            {
                currentMove = new Move(pos, newPos);
                boardAfterMove = new Chess(board.BoardAfterMove(currentMove));
                if (!boardAfterMove.IsPlayerInCheck(player))
                {
                    moves.Add(currentMove);
                }
            }
        }

        newPos = new Position(pos.x + 1, pos.y + dy);
        ChessPiece piece = board.GetPiece(newPos) as ChessPiece;
        if (piece != null && piece.player != player)
        {
            Move currentMove = new Move(pos, newPos);
            Chess boardAfterMove = new Chess(board.BoardAfterMove(currentMove));
            if (!boardAfterMove.IsPlayerInCheck(player))
            {
                moves.Add(currentMove);
            }
        }

        newPos = new Position(pos.x - 1, pos.y + dy);
        piece = board.GetPiece(newPos) as ChessPiece;
        if (piece != null && piece.player != player)
        {
            Move currentMove = new Move(pos, newPos);
            Chess boardAfterMove = new Chess(board.BoardAfterMove(currentMove));
            if (!boardAfterMove.IsPlayerInCheck(player))
            {
                moves.Add(currentMove);
            }
        }

        return moves;
    }

    public override bool ValidPosition(Position pos)
    {
        throw new NotImplementedException();
    }

}
