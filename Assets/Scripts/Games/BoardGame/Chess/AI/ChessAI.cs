using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ChessAI : ChessPlayer
{

    public bool havingTurn { get; internal set; }
    public ChessBoard board;
    public Move bestMove { get; internal set; }
    public ChessAI(Orientation orientation) : base(orientation)
    {
    }

    public virtual IEnumerator CalculateBestMove()
    {
        yield return new WaitForSeconds(0.2f);
        havingTurn = true;
        if (board == null ? true : !board.isInit)
            yield break;
        Move bestMove = null;
        List<Move> allMoves = ChessPiece.RemoveMovesPlayerInCheck(board.GetPossibleMoves(this), board, this);

        if (!allMoves.IsEmpty())
        {
            board.ResetMovesEval();
            float bestValue = int.MinValue;
            Move currentMove;
            ChessBoard boardAfterMove;
            for (int i = 0; i < allMoves.Count; i++)
            {
                currentMove = allMoves[i];
                boardAfterMove = board.BoardAfterMove(currentMove);
                float boardValue = 0f;

                yield return boardAfterMove.alphaBeta(2, boardAfterMove, false, v => boardValue = v);
                //Debug.Log(boardValue);
                if (boardValue >= bestValue)
                {
                    bestValue = boardValue;
                    bestMove = currentMove;
                }
            }
        }
        yield return null;
        this.bestMove = bestMove;
        havingTurn = false;
    }


}
