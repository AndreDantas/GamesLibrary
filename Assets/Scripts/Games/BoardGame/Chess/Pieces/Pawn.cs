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
    public Pawn(Position pos, ChessPlayer player) : base(pos, player)
    {
        type = ChessPieceType.PAWN;
    }

    public Pawn(Pawn other) : base(other)
    {
        type = ChessPieceType.PAWN;
    }

    public override ChessPiece GetCopy()
    {
        return new Pawn(this);
    }


    public override List<Move> GetPossibleMovement()
    {

        List<Move> moves = new List<Move>();
        ChessPlayer player = (ChessPlayer)this.player;
        int dy = player.orientation == Orientation.DOWN ? 1 : -1;
        Position newPos = new Position(pos.x, pos.y + dy);

        if (board.IsPositionEmpty(newPos))
        {

            Move currentMove = new Move(pos, newPos);
            moves.Add(currentMove);

            newPos = new Position(pos.x, pos.y + dy * 2);
            if (board.IsPositionEmpty(newPos) && pos == startPosition)
            {
                currentMove = new Move(pos, newPos);

                moves.Add(currentMove);

            }
        }

        newPos = new Position(pos.x + 1, pos.y + dy);
        ChessPiece piece = board.GetPiece(newPos) as ChessPiece;
        if (piece != null && piece.player != player)
        {
            Move currentMove = new Move(pos, newPos);

            moves.Add(currentMove);

        }

        newPos = new Position(pos.x - 1, pos.y + dy);
        piece = board.GetPiece(newPos) as ChessPiece;
        if (piece != null && piece.player != player)
        {
            Move currentMove = new Move(pos, newPos);

            moves.Add(currentMove);

        }

        return moves;
    }

    public override bool ValidPosition(Position pos)
    {
        throw new NotImplementedException();
    }

}
