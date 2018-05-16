using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Pawn : ChessPiece
{
    public Pawn(Position pos) : base(pos)
    {
        type = ChessPieceType.PAWN;
        value = 10;
    }
    public Pawn(Position pos, ChessPlayer player) : base(pos, player)
    {
        type = ChessPieceType.PAWN;
        value = 10;
    }

    public Pawn(Pawn other) : base(other)
    {
        type = ChessPieceType.PAWN;
    }

    public override ChessPiece GetCopy()
    {
        return new Pawn(this);
    }

    public override float GetPieceValue()
    {
        if ((player as ChessPlayer).orientation == Orientation.DOWN)
        {
            float value = base.GetPieceValue();
            if (PiecePositionEvaluation.PawnEvalWhite.ValidCoordinates(pos.x, pos.y))
                value += (float)PiecePositionEvaluation.PawnEvalWhite[pos.x, pos.y];
            if (pos.y == board.rows - 1)
                value += 50;
            return value;
        }
        else
        {

            float value = base.GetPieceValue();
            if (PiecePositionEvaluation.PawnEvalBlack.ValidCoordinates(pos.x, pos.y))
                value += (float)PiecePositionEvaluation.PawnEvalBlack[pos.x, pos.y];
            if (pos.y == 0)
                value += 50;
            return value;
        }
    }

    public override void MoveToPos(Move move)
    {

        if (Mathf.Abs(move.end.x - move.start.x) == 1 && Mathf.Abs(move.end.y - move.start.y) == 1)
        {
            if (board.nodes[move.end.x, move.end.y].pieceOnNode == null)
            {
                int dx = move.end.x - move.start.x;
                board.nodes[pos.x + dx, pos.y].pieceOnNode = null;
                ChessBoardgame c = GameObject.FindObjectOfType<ChessBoardgame>();
                if (c != null)
                {
                    c.tiles[pos.x + dx, pos.y].piece.SetActive(false);
                    c.tiles[pos.x + dx, pos.y].piece = null;
                }
            }

        }
        base.MoveToPos(move);
    }

    public override List<Move> GetPossibleMovement()
    {

        List<Move> moves = new List<Move>();
        ChessPlayer player = (ChessPlayer)this.player;
        int dy = player.orientation == Orientation.DOWN ? 1 : -1;

        //Check for en passant
        bool enpassant = false;
        ChessPiece testPiece = board.GetPiece(new Position(pos.x - 1, pos.y)); // Pawn on the left

        if (testPiece != null)
        {
            if (testPiece.type == ChessPieceType.PAWN && testPiece.player != player)
            {
                ChessBoardgame boardGame = GameObject.FindObjectOfType<ChessBoardgame>();
                if (boardGame != null && boardGame.movesLog != null ? boardGame.movesLog.Count > 0 : false)
                {
                    Move lastMove = boardGame.movesLog[boardGame.movesLog.Count - 1].move;
                    ChessPiece lastMovePiece = boardGame.movesLog[boardGame.movesLog.Count - 1].piece;
                    if (testPiece == lastMovePiece && Mathf.Abs(lastMove.start.y - lastMove.end.y) == 2)
                    {
                        moves.Add(new Move(pos, new Position(pos.x - 1, pos.y + dy)));
                        enpassant = true;
                    }
                }
            }
        }

        testPiece = board.GetPiece(new Position(pos.x + 1, pos.y)); // Pawn on the right

        if (testPiece != null)
        {
            if (testPiece.type == ChessPieceType.PAWN && testPiece.player != player)
            {
                ChessBoardgame boardGame = GameObject.FindObjectOfType<ChessBoardgame>();
                if (boardGame != null && boardGame.movesLog != null ? boardGame.movesLog.Count > 0 : false)
                {
                    Move lastMove = boardGame.movesLog[boardGame.movesLog.Count - 1].move;
                    ChessPiece lastMovePiece = boardGame.movesLog[boardGame.movesLog.Count - 1].piece;
                    if (testPiece == lastMovePiece && Mathf.Abs(lastMove.start.y - lastMove.end.y) == 2)
                    {
                        moves.Add(new Move(pos, new Position(pos.x + 1, pos.y + dy)));
                        enpassant = true;
                    }
                }
            }
        }

        if (enpassant)
            return moves;

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
