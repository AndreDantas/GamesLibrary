using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{

    public King(Position pos) : base(pos)
    {

        type = ChessPieceType.KING;
    }
    public King(King other) : base(other)
    {
        type = ChessPieceType.KING;
    }

    public override ChessPiece GetCopy()
    {
        return new King(this);
    }


    public override List<Move> GetPossibleMovement()
    {
        List<Move> moves = new List<Move>();

        for (var d = 0; d < 8; d++)
        {
            int dx = 0;
            int dy = 0;
            if (d == 0)
            {
                dx = 1;
                dy = 1;
            }
            else if (d == 1)
            {
                dx = 1;
                dy = -1;
            }
            else if (d == 2)
            {
                dx = -1;
                dy = -1;
            }
            else if (d == 3)
            {
                dx = -1;
                dy = 1;
            }
            else if (d == 4)
            {
                dx = 0;
                dy = 1;
            }
            else if (d == 5)
            {
                dx = 1;
                dy = 0;
            }
            else if (d == 6)
            {
                dx = -1;
                dy = 0;
            }
            else if (d == 7)
            {
                dx = 0;
                dy = -1;
            }
            Position newPosition = new Position(pos);
            newPosition.x += dx;
            newPosition.y += dy;
            if (base.board.IsPositionEmpty(newPosition))
            {
                Move currentMove = new Move(pos, newPosition);


                moves.Add(currentMove);

            }
            else // Attack
            {
                ChessPiece piece = board.GetPiece(newPosition);
                if (piece != null && piece.player != player)
                {
                    Move currentMove = new Move(pos, newPosition);

                    moves.Add(currentMove);

                }
            }
        }
        if (!hasMoved)
        {
            ChessPiece cp = board.GetPiece(new Position(0, pos.y)) as ChessPiece;
            if (cp != null)
            {
                if (cp.type == ChessPieceType.ROOK && !cp.hasMoved && cp.player == player)
                {

                }
            }
        }
        return moves;
    }

    public override bool ValidPosition(Position pos)
    {
        return base.ValidPosition(pos);
    }
}
