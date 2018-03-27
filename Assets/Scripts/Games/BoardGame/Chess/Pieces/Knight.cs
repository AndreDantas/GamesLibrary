using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{

    public Knight(Position pos) : base(pos)
    {

        type = ChessPieceType.KNIGHT;
    }

    public Knight(Knight other) : base(other)
    {

    }

    public override ChessPiece GetCopy()
    {
        return new Knight(this);
    }


    public override List<Move> GetPossibleMovement()
    {
        List<Move> moves = new List<Move>();
        ChessBoard board = (ChessBoard)this.board;
        for (var d = 0; d < 8; d++)
        {
            int dx = 0;
            int dy = 0;
            if (d == 0)
            {
                dx = 2;
                dy = -1;
            }
            else if (d == 1)
            {
                dx = 2;
                dy = 1;
            }
            else if (d == 2)
            {
                dx = -2;
                dy = -1;
            }
            else if (d == 3)
            {
                dx = -2;
                dy = 1;
            }
            else if (d == 4)
            {
                dx = 1;
                dy = 2;
            }
            else if (d == 5)
            {
                dx = -1;
                dy = 2;
            }
            else if (d == 6)
            {
                dx = -1;
                dy = -2;
            }
            else if (d == 7)
            {
                dx = 1;
                dy = -2;
            }
            Position newPosition = new Position(pos);
            newPosition.x += dx;
            newPosition.y += dy;
            if (board.IsPositionEmpty(newPosition))
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
        return moves;
    }

    public override bool ValidPosition(Position pos)
    {
        return base.ValidPosition(pos);
    }
}
