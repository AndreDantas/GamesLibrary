using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using CielaSpike;
[System.Serializable]
public class ChessAI : ChessPlayer
{

    public bool havingTurn { get; internal set; }
    public ChessBoard board;
    public Move bestMove { get; internal set; }
    public ChessAI(Orientation orientation) : base(orientation)
    {
    }
    public float turnProgress { get; internal set; }
    public virtual IEnumerator CalculateBestMove()
    {
        turnProgress = 0f;
        havingTurn = true;

        ///////////////////////////
        //float elapsedTime = 0f;
        //yield return Ninja.JumpToUnity;
        //float startTime = Time.time;
        //yield return Ninja.JumpBack;
        ///////////////////////////////

        yield return new WaitForSeconds(0.1f);
        if (board == null ? true : !board.isInit)
            yield break;
        Move bestMove = null;
        List<Move> allMoves = ChessPiece.RemoveMovesPlayerInCheck(board.GetPossibleMoves(this), board, this);
        allMoves = OrderMoves(allMoves);
        allMoves.Reverse();
        if (!allMoves.IsEmpty())
        {
            board.ResetMovesEval();
            float bestValue = int.MinValue;
            Move currentMove;
            ChessBoard boardAfterMove;
            int moveCount = 0;
            for (int i = 0; i < allMoves.Count; i++)
            {
                currentMove = allMoves[i];
                boardAfterMove = board.BoardAfterMove(currentMove);
                float boardValue = 0f;
                //yield return boardAfterMove.alphaBeta(2, boardAfterMove, false, v => boardValue = v);
                //boardValue = boardAfterMove.NegaMax(2, boardAfterMove, board.OtherPlayer(this));
                boardValue = boardAfterMove.alphaBeta(2, boardAfterMove, false);
                //Debug.Log(boardValue);
                if (boardValue >= bestValue)
                {
                    bestValue = boardValue;
                    bestMove = currentMove;
                }
                //yield return null;
                moveCount++;
                turnProgress = UtilityFunctions.Map(0, allMoves.Count - 1, 0f, 1f, moveCount);
            }
        }
        // yield return null;
        this.bestMove = bestMove;
        havingTurn = false;

        ////////////////
        //yield return Ninja.JumpToUnity;
        //elapsedTime = Time.time - startTime;
        //yield return Ninja.JumpBack;
        //Debug.Log(elapsedTime);
        ///////////////////

        yield break;
    }

    public List<Move> OrderMoves(List<Move> moves)
    {


        List<Move> newMoves = new List<Move>();
        foreach (var item in moves.OrderBy(x => board.BoardAfterMove(x).EvaluateBoard()))
        {
            newMoves.Add(item);
        }
        return newMoves;
    }

    //public virtual void CalculateBestMove()
    //{
    //    //yield return new WaitForSeconds(0.2f);
    //    havingTurn = true;
    //    if (board == null ? true : !board.isInit)
    //        return;
    //    Move bestMove = null;
    //    List<Move> allMoves = ChessPiece.RemoveMovesPlayerInCheck(board.GetPossibleMoves(this), board, this);

    //    if (!allMoves.IsEmpty())
    //    {
    //        board.ResetMovesEval();
    //        float bestValue = int.MinValue;
    //        Move currentMove;
    //        ChessBoard boardAfterMove;
    //        for (int i = 0; i < allMoves.Count; i++)
    //        {
    //            currentMove = allMoves[i];
    //            boardAfterMove = board.BoardAfterMove(currentMove);
    //            float boardValue = 0f;

    //            boardValue = boardAfterMove.alphaBeta(2, boardAfterMove, false);
    //            //Debug.Log(boardValue);
    //            if (boardValue >= bestValue)
    //            {
    //                bestValue = boardValue;
    //                bestMove = currentMove;
    //            }
    //        }
    //    }
    //    //yield return null;
    //    this.bestMove = bestMove;
    //    havingTurn = false;
    //}

}
