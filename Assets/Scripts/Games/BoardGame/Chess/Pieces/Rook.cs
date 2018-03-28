﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public Rook(Position pos) : base(pos)
    {

        type = ChessPieceType.ROOK;
    }
    public Rook(Rook other) : base(other)
    {

    }

    public override ChessPiece GetCopy()
    {
        return new Rook(this);
    }


    public override List<Move> GetPossibleMovement()
    {
        List<Move> moves = new List<Move>();

        for (var d = 0; d < 4; d++)
        {
            int dx = 0;
            int dy = 0;
            if (d == 0)
            {
                dx = 0;
                dy = 1;
            }
            else if (d == 1)
            {
                dx = 0;
                dy = -1;
            }
            else if (d == 2)
            {
                dx = -1;
                dy = 0;
            }
            else if (d == 3)
            {
                dx = 1;
                dy = 0;
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