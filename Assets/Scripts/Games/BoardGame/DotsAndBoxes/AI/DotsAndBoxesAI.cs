using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class DotsAndBoxesAI : Player
{

    public bool havingTurn { get; internal set; }
    public DotsAndBoxesBoard board;
    public Edge bestMove { get; internal set; }
    public float turnProgress { get; internal set; }
    public DotsAndBoxesAI(DotsAndBoxesBoard board)
    {
        this.board = board;
    }

    public virtual IEnumerator CalculateBestMove()
    {

        yield return new WaitForSeconds(0.1f);
        turnProgress = 0f;
        havingTurn = true;

        ///////////////////////////
        //float elapsedTime = 0f;
        //yield return Ninja.JumpToUnity;
        //float startTime = Time.time;
        //yield return Ninja.JumpBack;
        ///////////////////////////////

        if (board == null ? true : !board.isInit)
            yield break;
        Edge bestMove = null;

        List<Edge> allMoves = board.GetValidEdges();

        if (!allMoves.IsEmpty())
        {

            float bestValue = int.MinValue;
            Edge currentMove;
            DotsAndBoxesBoard boardAfterMove;
            for (int i = 0; i < allMoves.Count; i++)
            {
                currentMove = allMoves[i];
                if (currentMove.active == true)
                    continue;
                boardAfterMove = board.BoardAfterMove(this, currentMove);
                float boardValue = 0f;
                boardValue = board.alphaBeta(2, boardAfterMove, false);

                //yield return boardAfterMove.alphaBeta(4, boardAfterMove, false, v => boardValue = v);
                //Debug.Log(boardValue);

                if (boardValue >= bestValue)
                {
                    bestValue = boardValue;
                    bestMove = currentMove;
                }
                turnProgress = UtilityFunctions.Map(0, allMoves.Count - 1, 0f, 1f, i);
            }
        }

        ////////////////
        //yield return Ninja.JumpToUnity;
        //elapsedTime = Time.time - startTime;
        //yield return Ninja.JumpBack;
        //Debug.Log(elapsedTime);
        ///////////////////

        yield return null;
        this.bestMove = bestMove;

        havingTurn = false;
    }
}
