using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
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

    public override void MoveToPos(Move move)
    {

        if (move.end.x == pos.x + 2) // Castling
        {
            ChessPiece rook = board.GetPiece(new Position(board.nodes.GetLength(0) - 1, pos.y));
            if (rook != null)
            {
                ChessBoardgame c = GameObject.FindObjectOfType<ChessBoardgame>();
                if (c != null)
                    c.MovePieceObject(new Move(rook.pos, new Position(pos.x + 1, rook.pos.y)));
                rook.MoveToPos(new Move(rook.pos, new Position(pos.x + 1, rook.pos.y)));

            }
        }
        else if (move.end.x == pos.x - 2)
        {
            ChessPiece rook = board.GetPiece(new Position(0, pos.y));
            if (rook != null)
            {
                ChessBoardgame c = GameObject.FindObjectOfType<ChessBoardgame>();
                if (c != null)
                    c.MovePieceObject(new Move(rook.pos, new Position(pos.x - 1, rook.pos.y)));
                rook.MoveToPos(new Move(rook.pos, new Position(pos.x - 1, rook.pos.y)));

            }
        }

        base.MoveToPos(move);
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
        /*Castling Conditions:
         * The king and the chosen rook are on the player's first rank.
         * Neither the king nor the chosen rook has previously moved.
         * There are no pieces between the king and the chosen rook.
         * The king is not currently in check.
         * The king does not pass through a square that is attacked by an enemy piece.
         * The king does not end up in check. (True of any legal move.)
         */
        if (!hasMoved)
        {
            ChessPlayer chessPlayer = player as ChessPlayer;
            if (chessPlayer != null && !chessPlayer.inCheck)
            {
                //Left
                ChessPiece cp = board.GetPiece(new Position(0, pos.y));
                if (cp != null)
                {
                    if (cp.type == ChessPieceType.ROOK && !cp.hasMoved && cp.player == player)
                    {
                        bool piecesInBetween = false;

                        for (int i = 1; i < pos.x; i++)
                        {
                            if (board.GetPiece(new Position(i, pos.y)) != null)
                            {
                                piecesInBetween = true;
                                break;
                            }
                        }

                        if (!piecesInBetween)
                        {
                            bool kingSafePass = true;
                            List<Move> otherPlayerMoves = board.GetPossibleMoves(board.OtherPlayer(this.player as ChessPlayer));
                            foreach (Move move in otherPlayerMoves)
                            {
                                if (move.end == new Position(pos.x - 1, pos.y) || move.end == new Position(pos.x - 2, pos.y))
                                {
                                    kingSafePass = false;
                                    break;
                                }
                            }

                            if (kingSafePass)
                            {
                                moves.Add(new Move(pos, new Position(pos.x - 2, pos.y)));
                            }
                        }
                    }
                }

                //Right
                cp = board.GetPiece(new Position(board.nodes.GetLength(0) - 1, pos.y));
                if (cp != null)
                {
                    if (cp.type == ChessPieceType.ROOK && !cp.hasMoved && cp.player == player)
                    {
                        bool piecesInBetween = false;

                        for (int i = pos.x + 1; i < board.nodes.GetLength(0) - 1; i++)
                        {
                            if (board.GetPiece(new Position(i, pos.y)) != null)
                            {
                                piecesInBetween = true;
                                break;
                            }
                        }

                        if (!piecesInBetween)
                        {
                            bool kingSafePass = true;
                            List<Move> otherPlayerMoves = board.GetPossibleMoves(board.OtherPlayer(this.player as ChessPlayer));
                            foreach (Move move in otherPlayerMoves)
                            {
                                if (move.end == new Position(pos.x + 1, pos.y) || move.end == new Position(pos.x + 2, pos.y))
                                {
                                    kingSafePass = false;
                                    break;
                                }
                            }

                            if (kingSafePass)
                            {
                                moves.Add(new Move(pos, new Position(pos.x + 2, pos.y)));
                            }
                        }
                    }
                }
            }
        }
        return moves;
    }

    public List<Move> GetDefaultMovements()
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

        return moves;
    }

    public override bool ValidPosition(Position pos)
    {
        return base.ValidPosition(pos);
    }
}
