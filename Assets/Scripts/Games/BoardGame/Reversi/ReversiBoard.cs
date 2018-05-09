using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;

[System.Serializable]
public class ReversiBoard : Board
{

    public ReversiNode[,] nodes;

    public Player player1;
    public Player player2;
#if UNITY_EDITOR
    [ShowInInspector, TableMatrix(DrawElementMethod = "DrawColoredEnumElement", ResizableColumns = false)]
    private bool[,] piecesPos
    {
        get
        {
            if (!isInit)
                return null;
            else
            {
                bool[,] pos = new bool[columns, rows];
                int x = 0, y = 0;
                for (int i = columns - 1; i >= 0; i--)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        pos[j, i] = nodes[x, y].pieceOnNode != null;
                        x++;
                    }
                    x = 0;
                    y++;
                }
                return pos;
            }
        }
    }



    private static bool DrawColoredEnumElement(Rect rect, bool value)
    {
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            value = !value;
            GUI.changed = true;
            Event.current.Use();
        }

        UnityEditor.EditorGUI.DrawRect(rect, value ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));

        return value;
    }

#endif

    public List<Position> FlipDirections
    {
        get
        {
            List<Position> result = new List<Position>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    else
                    {

                        result.Add(new Position(i, j));

                    }
                }
            }
            return result;
        }
    }

    public ReversiBoard()
    {

    }
    public ReversiBoard(int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
    }

    public ReversiBoard(ReversiBoard oldBoard)
    {
        if (oldBoard == null)
            return;
        columns = oldBoard.columns;
        rows = oldBoard.rows;
        nodeOffsetX = oldBoard.nodeOffsetX;
        nodeOffsetY = oldBoard.nodeOffsetY;
        ReversiNode[,] n = new ReversiNode[oldBoard.columns, oldBoard.rows];
        for (int i = 0; i < oldBoard.nodes.GetLength(0); i++)
        {
            for (int j = 0; j < oldBoard.nodes.GetLength(1); j++)
            {
                if (oldBoard.nodes[i, j] != null)
                    n[i, j] = new ReversiNode(oldBoard.nodes[i, j], this);
            }
        }
        this.nodes = n;
        player1 = oldBoard.player1;
        player2 = oldBoard.player2;
        isInit = oldBoard.isInit;

    }


    private void OnValidate()
    {
        rows = UtilityFunctions.ClampMin(rows, 2);
        columns = UtilityFunctions.ClampMin(columns, 2);
    }
    public bool ValidCoordinate(Position pos)
    {
        if (pos != null)
            return ValidCoordinate(pos.x, pos.y);
        else
            return false;
    }

    /// <summary>
    /// Checks if the coordinate is valid in this map.
    /// </summary>
    public bool ValidCoordinate(int x, int y)
    {
        if (nodes == null)
            return false;
        if (x < 0 || x >= nodes.GetLength(0))
            return false;
        if (y < 0 || y >= nodes.GetLength(1))
            return false;

        return true;
    }
    public override void InitBoard()
    {

        if (columns <= 0 || rows <= 0)
            return;
        nodes = new ReversiNode[columns, rows];
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                ReversiNode node = new ReversiNode(new Position(i, j));

                nodes[i, j] = node;

            }
        }
        isInit = true;
    }
    /// <summary>
    /// Returns the other player.
    /// </summary>
    /// <returns></returns>
    public Player OtherPlayer(Player thisPlayer)
    {
        if ((thisPlayer != player1 && thisPlayer != player2) || thisPlayer == null)
            return null;

        return thisPlayer == player1 ? player2 : player1;
    }

    /// <summary>
    /// Returns a list with all the nodes.
    /// </summary>
    public List<ReversiNode> GetNodes()
    {
        if (nodes == null)
            return null;
        List<ReversiNode> result = new List<ReversiNode>();
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {

                result.Add(nodes[i, j]);
            }
        }
        return result;
    }

    public ReversiPiece AddPiece(Player player, Position pos)
    {
        if (player == null || !ValidCoordinate(pos))
            return null;

        return (nodes[pos.x, pos.y].pieceOnNode = new ReversiPiece(pos, player, this));

    }

    public int EvaluateBoard()
    {
        if (!isInit)
            return 0;
        int totalValue = 0;
        foreach (ReversiNode n in GetNodes())
        {
            if (n.pieceOnNode != null)
            {
                if (n.pieceOnNode.player == player1)
                    totalValue++;
                else
                    totalValue--;
            }
        }
        return totalValue;
    }

    public int GetScore(Player player)
    {

        if (!isInit)
            return 0;
        int totalValue = 0;
        foreach (ReversiNode n in GetNodes())
        {
            if (n.pieceOnNode != null)
            {
                if (n.pieceOnNode.player == player)
                    totalValue++;
            }
        }
        return totalValue;
    }

    public bool IsCorner(Position pos)
    {
        if (!ValidCoordinate(pos))
            return false;
        int x = pos.x;
        int y = pos.y;
        return (x == 0 && y == 0) || (x == columns - 1 && y == 0) || (x == 0 && y == rows - 1) || (x == columns - 1 && y == rows - 1);
    }

    public List<Position> IsValidMove(Player player, Position pos)
    {
        if (ValidCoordinate(pos) ? nodes[pos.x, pos.y].pieceOnNode != null : true)
            return null;

        nodes[pos.x, pos.y].pieceOnNode = new ReversiPiece(pos, player, this);
        List<Position> nodesToFlip = new List<Position>();
        int x, y;
        //Debug.Log(pos);
        foreach (var item in FlipDirections)
        {

            x = pos.x;
            y = pos.y;
            x += item.x;
            y += item.y;
            //Debug.Log("Direction: " + item);
            if (ValidCoordinate(x, y))
            {
                //Debug.Log("Checking at : " + new Position(x, y));
                ReversiPiece piece = nodes[x, y].pieceOnNode;
                //Debug.Log(piece);
                if (piece?.player == OtherPlayer(player))
                {
                    //Debug.Log("Other player's piece");
                    while (piece?.player == OtherPlayer(player))
                    {
                        x += item.x;
                        y += item.y;

                        if (!ValidCoordinate(x, y))
                            break;
                        piece = nodes[x, y].pieceOnNode;
                    }

                    if (!ValidCoordinate(x, y))
                        continue;
                    if (piece?.player == player)
                    {
                        while (true)
                        {
                            x -= item.x;
                            y -= item.y;
                            if (x == pos.x && y == pos.y)
                                break;
                            nodesToFlip.Add(new Position(x, y));
                        }
                    }
                }
            }
        }

        nodes[pos.x, pos.y].pieceOnNode = null;
        if (nodesToFlip.Count > 0)
        {
            return nodesToFlip;
        }

        return null;
    }

    /// <summary>
    /// Returns a list of valid moves for the given player.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public List<Position> GetValidMoves(Player player)
    {
        if (player == null)
            return null;

        List<Position> moves = new List<Position>();
        foreach (ReversiNode node in GetNodes())
        {
            if (node != null)
            {

                if (IsValidMove(player, node.pos) != null)
                {
                    moves.Add(node.pos);
                }
            }
        }

        return moves;
    }


    public bool MakeAMove(Player player, Position move)
    {
        var moves = IsValidMove(player, move);
        if (moves != null)
        {
            foreach (var item in moves)
            {
                AddPiece(player, item);
            }
            AddPiece(player, move);
            return true;
        }
        else
            return false;

    }

    public ReversiBoard BoardAfterMove(Player player, Position move)
    {
        var b = new ReversiBoard(this);
        b.MakeAMove(player, move);
        return b;
    }
    [ShowInInspector]
    public int movesEval { get; internal set; }
    [ShowInInspector]
    public int movesEvalPerFrame { get { return 300; } }
    public void ResetMovesEval()
    {
        movesEval = 0;
    }

    public IEnumerator alphaBeta(float depth, ReversiBoard board, bool maximisingPlayer, Action<float> result, float alpha = -10000, float beta = 10000)
    {

        float bestValue;

        if (depth <= 0)
        {
            bestValue = -board.EvaluateBoard();

        }
        else if (maximisingPlayer)
        {
            bestValue = alpha;
            List<Position> moves = board.GetValidMoves(player2);
            if (moves.Count > 0)
                for (var i = 0; i < moves.Count; i++)
                {

                    var childValue = 0f;
                    if (movesEval % movesEvalPerFrame == 0)
                    {

                        yield return alphaBeta(depth - 1, board.BoardAfterMove(player2, moves[i]), false, v => childValue = v, bestValue, beta);
                    }
                    else
                    {
                        childValue = alphaBeta(depth - 1, board.BoardAfterMove(player2, moves[i]), false, bestValue, beta);
                        //var e = alphaBeta(depth - 1, b, false, v => childValue = v, bestValue, beta);
                        //while (e.MoveNext())
                        //    yield return e.Current;
                    }
                    bestValue = Mathf.Max(bestValue, childValue);
                    if (beta <= bestValue)
                    {
                        break;
                    }
                }
            else
                bestValue = -10000;
        }
        else
        {
            bestValue = beta;
            List<Position> moves = board.GetValidMoves(player1);
            // Recurse for all children of node.
            if (moves.Count > 0)
                for (var i = 0; i < moves.Count; i++)
                {

                    var childValue = 0f;
                    if (movesEval % movesEvalPerFrame == 0)
                    {

                        yield return alphaBeta(depth - 1, board.BoardAfterMove(player1, moves[i]), true, v => childValue = v, alpha, bestValue);
                    }
                    else
                    {
                        //var e = alphaBeta(depth - 1, b, true, v => childValue = v, alpha, bestValue);
                        //while (e.MoveNext())
                        //    yield return e.Current;
                        childValue = alphaBeta(depth - 1, board.BoardAfterMove(player1, moves[i]), true, alpha, bestValue);
                    }
                    bestValue = Mathf.Min(bestValue, childValue);
                    if (bestValue <= alpha)
                    {
                        break;
                    }
                }
            else
                bestValue = 10000;
        }
        movesEval++;
        result(bestValue);
    }
    public float alphaBeta(float depth, ReversiBoard board, bool maximisingPlayer, float alpha = -10000, float beta = 10000)
    {

        float bestValue;

        if (depth <= 0)
        {
            bestValue = -board.EvaluateBoard();

        }
        else if (maximisingPlayer)
        {
            bestValue = alpha;
            List<Position> moves = board.GetValidMoves(player2);
            if (moves.Count > 0)
                for (var i = 0; i < moves.Count; i++)
                {

                    var childValue = 0f;
                    childValue = alphaBeta(depth - 1, board.BoardAfterMove(player2, moves[i]), false, bestValue, beta);

                    bestValue = Mathf.Max(bestValue, childValue);
                    if (beta <= bestValue)
                    {
                        break;
                    }
                }
            else
                bestValue = -10000;
        }
        else
        {
            bestValue = beta;
            List<Position> moves = board.GetValidMoves(player1);
            // Recurse for all children of node.
            if (moves.Count > 0)
                for (var i = 0; i < moves.Count; i++)
                {

                    var childValue = 0f;
                    childValue = alphaBeta(depth - 1, board.BoardAfterMove(player1, moves[i]), true, alpha, bestValue);

                    bestValue = Mathf.Min(bestValue, childValue);
                    if (bestValue <= alpha)
                    {
                        break;
                    }
                }
            else
                bestValue = 10000;
        }
        movesEval++;
        return bestValue;
    }
}

