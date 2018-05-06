using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ChessAI : ChessPlayer
{

    public bool havingTurn { get; internal set; }
    public ChessBoard board;

    public ChessAI(Orientation orientation) : base(orientation)
    {
    }

    public virtual Move CalculateBestMove()
    {
        if (board == null ? true : !board.isInit)
            return null;
        Move bestMove = null;
        List<Move> allMoves = ChessPiece.RemoveMovesPlayerInCheck(board.GetPossibleMoves(this), board, this);

        if (!allMoves.IsEmpty())
        {

            float bestValue = -9999f;
            Move currentMove;
            ChessBoard boardAfterMove;
            for (int i = 0; i < allMoves.Count; i++)
            {
                currentMove = allMoves[i];
                boardAfterMove = board.BoardAfterMove(currentMove);
                float boardValue = boardAfterMove.alphaBeta(2, boardAfterMove, false);
                //Debug.Log(boardValue);
                if (boardValue >= bestValue)
                {
                    bestValue = boardValue;
                    bestMove = currentMove;
                }
            }
        }
        return bestMove;
    }


}
