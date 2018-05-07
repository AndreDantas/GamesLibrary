using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class CheckersAI : CheckersPlayer
{


    public bool havingTurn { get; internal set; }
    public CheckersBoard board;
    public CheckerMove bestMove { get; internal set; }
    public CheckersAI(Orientation orientation) : base(orientation)
    {
    }

    public virtual IEnumerator CalculateBestMove()
    {

        yield return new WaitForSeconds(0.2f);
        havingTurn = true;
        if (board == null ? true : !board.isInit)
            yield break;
        CheckerMove bestMove = null;

        List<CheckerMove> allMoves = board.GetPossibleMovementsAI(this);
        //Debug.Log(allMoves.Count);
        //foreach (CheckerMove c in allMoves)
        //    Debug.Log(c);

        if (!allMoves.IsEmpty())
        {
            board.ResetMovesEval();
            float bestValue = int.MinValue;
            CheckerMove currentMove;
            CheckersBoard boardAfterMove;
            for (int i = 0; i < allMoves.Count; i++)
            {
                currentMove = allMoves[i];
                boardAfterMove = board.BoardAfterMove(currentMove);
                float boardValue = 0f;

                yield return boardAfterMove.alphaBeta(4, boardAfterMove, false, v => boardValue = v);
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
