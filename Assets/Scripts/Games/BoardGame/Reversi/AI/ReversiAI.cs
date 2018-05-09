using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class ReversiAI : Player
{

    public bool havingTurn { get; internal set; }
    public ReversiBoard board;
    public Position bestMove { get; internal set; }

    public ReversiAI(ReversiBoard board)
    {
        this.board = board;
    }

    public virtual IEnumerator CalculateBestMove()
    {

        yield return new WaitForSeconds(0.2f);
        havingTurn = true;
        if (board == null ? true : !board.isInit)
            yield break;
        Position bestMove = null;

        List<Position> allMoves = board.GetValidMoves(this);
        //Debug.Log(allMoves.Count);
        //foreach (CheckerMove c in allMoves)
        //    Debug.Log(c);

        if (!allMoves.IsEmpty())
        {

            float bestValue = int.MinValue;
            Position currentMove;
            ReversiBoard boardAfterMove;
            for (int i = 0; i < allMoves.Count; i++)
            {
                currentMove = allMoves[i];
                boardAfterMove = board.BoardAfterMove(this, currentMove);
                float boardValue = 0f;

                yield return boardAfterMove.alphaBeta(Random.Range(1, 4), boardAfterMove, false, v => boardValue = v);
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
