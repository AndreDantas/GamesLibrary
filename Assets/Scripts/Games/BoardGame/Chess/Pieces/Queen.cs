using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Queen : ChessPiece
{

    public Queen(Position pos) : base(pos)
    {

        type = ChessPieceType.QUEEN;
    }
    public Queen(Queen other) : base(other)
    {

    }

    public override ChessPiece GetCopy()
    {
        return new Queen(this);
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
            do
            {
                newPosition = new Position(newPosition);
                newPosition.x += dx;
                newPosition.y += dy;
                if (board.IsPositionEmpty(newPosition))
                {
                    Move currentMove = new Move(pos, newPosition);

                    moves.Add(currentMove);

                }
                else // Attack
                {
                    ChessPiece piece = board.GetPiece(newPosition) as ChessPiece;
                    if (piece != null && piece.player != player)
                    {
                        Move currentMove = new Move(pos, newPosition);

                        moves.Add(currentMove);

                    }
                    break;
                }
            } while (true);
        }
        return moves;
    }

    public override bool ValidPosition(Position pos)
    {
        return base.ValidPosition(pos);
    }
}
