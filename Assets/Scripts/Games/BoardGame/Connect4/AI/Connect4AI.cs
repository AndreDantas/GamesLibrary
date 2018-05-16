using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class Connect4AI : Player
{
    public bool havingTurn { get; internal set; }
    public Connect4Board board;
    public int bestMove { get; internal set; }

    public Connect4AI(Connect4Board board)
    {
        this.board = board;
    }

    public virtual IEnumerator CalculateBestMove()
    {

        yield return new WaitForSeconds(0.5f);
        havingTurn = true;
        if (board == null ? true : !board.isInit)
            yield break;
        int bestMove = -1;

        List<int> allMoves = board.GetValidColumns();
        //Debug.Log(allMoves.Count);
        //foreach (CheckerMove c in allMoves)
        //    Debug.Log(c);

        if (!allMoves.IsEmpty())
        {

            float bestValue = int.MinValue;
            int currentMove;
            Connect4Board boardAfterMove;
            for (int i = 0; i < allMoves.Count; i++)
            {
                currentMove = allMoves[i];
                boardAfterMove = board.BoardAfterMove(this, currentMove);
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
