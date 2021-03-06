﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class DotsAndBoxesBoard : Board
{
    public DotsAndBoxesNode[,] nodes;
    public Edge[,] edgesX;
    public Edge[,] edgesY;
    public Player player1;
    public Player player2;
    public float score { get; internal set; }
    public DotsAndBoxesBoard(int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
    }
    public DotsAndBoxesBoard(DotsAndBoxesBoard oldBoard)
    {
        if (oldBoard == null)
            return;
        columns = oldBoard.columns;
        rows = oldBoard.rows;
        nodeOffsetX = oldBoard.nodeOffsetX;
        nodeOffsetY = oldBoard.nodeOffsetY;
        DotsAndBoxesNode[,] n = new DotsAndBoxesNode[oldBoard.columns, oldBoard.rows];
        edgesX = new Edge[oldBoard.columns + 1, oldBoard.rows];
        edgesY = new Edge[oldBoard.columns, oldBoard.rows + 1];

        for (int i = 0; i < oldBoard.edgesX.GetLength(0); i++)
        {
            for (int j = 0; j < oldBoard.edgesX.GetLength(1); j++)
            {
                if (oldBoard.edgesX[i, j] != null)
                    edgesX[i, j] = new Edge(oldBoard.edgesX[i, j]);
            }
        }
        for (int i = 0; i < oldBoard.edgesY.GetLength(0); i++)
        {
            for (int j = 0; j < oldBoard.edgesY.GetLength(1); j++)
            {
                if (oldBoard.edgesY[i, j] != null)
                    edgesY[i, j] = new Edge(oldBoard.edgesY[i, j]);
            }
        }


        for (int i = 0; i < oldBoard.nodes.GetLength(0); i++)
        {
            for (int j = 0; j < oldBoard.nodes.GetLength(1); j++)
            {
                if (oldBoard.nodes[i, j] != null)
                {
                    n[i, j] = new DotsAndBoxesNode(oldBoard.nodes[i, j]);
                    SetBoxEdges(ref n[i, j].box, ref edgesX, ref edgesY);
                }
            }
        }



        this.nodes = n;
        player1 = oldBoard.player1;
        player2 = oldBoard.player2;
        isInit = oldBoard.isInit;
        score = oldBoard.score;
    }

    public List<Edge> GetEdges()
    {
        if (!isInit)
            return null;

        List<Edge> result = new List<Edge>();
        result.AddRange(edgesX.GetItems());
        result.AddRange(edgesY.GetItems());

        return result;
    }

    public int GetFilledSquaresCount(Player player)
    {
        if (!isInit)
            return 0;
        int count = 0;
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                //Debug.Log(nodes[i, j].box);
                if (nodes[i, j].box.filled && nodes[i, j].box.owner == player)
                    count++;
            }
        }
        return count;
    }

    public List<Edge> GetValidEdges()
    {
        if (!isInit)
            return null;

        List<Edge> result = new List<Edge>();

        for (int i = 0; i < edgesX.GetLength(0); i++)
        {
            for (int j = 0; j < edgesX.GetLength(1); j++)
            {
                if (!edgesX[i, j].active)
                    result.Add(edgesX[i, j]);
            }
        }
        for (int i = 0; i < edgesY.GetLength(0); i++)
        {
            for (int j = 0; j < edgesY.GetLength(1); j++)
            {
                if (!edgesY[i, j].active)
                    result.Add(edgesY[i, j]);
            }
        }

        return result;
    }
    public List<Edge> GetFilledEdges()
    {
        if (!isInit)
            return null;

        List<Edge> result = new List<Edge>();

        for (int i = 0; i < edgesX.GetLength(0); i++)
        {
            for (int j = 0; j < edgesX.GetLength(1); j++)
            {
                if (edgesX[i, j].active)
                    result.Add(edgesX[i, j]);
            }
        }
        for (int i = 0; i < edgesY.GetLength(0); i++)
        {
            for (int j = 0; j < edgesY.GetLength(1); j++)
            {
                if (edgesY[i, j].active)
                    result.Add(edgesY[i, j]);
            }
        }

        return result;
    }

    public List<Edge> GetEdgesThatCanCompleteSquares(Player player)
    {
        List<Edge> result = new List<Edge>();
        DotsAndBoxesBoard temp;
        foreach (var item in GetValidEdges())
        {
            temp = BoardAfterMove(player, item);
            if (temp.CheckForFill(item.pos) || temp.CheckForFill(item.orientation == EdgePosition.Horizontal ? item.pos - Position.Right : item.pos - Position.Up))
                result.Add(item);
        }

        if (result.Count > 0)
            return result;

        return null;
    }

    public List<Edge> GetEdgesThatDontEnableCapture(Player player)
    {
        List<Edge> result = new List<Edge>();

        foreach (var item in GetValidEdges())
        {

            if (nodes.ValidCoordinates(item.pos) ? nodes[item.pos.x, item.pos.y].box.CompletedEdgesCount() >= 3 : false)
                continue;
            Position pos = item.orientation == EdgePosition.Horizontal ? item.pos - Position.Right : item.pos - Position.Up;
            if (nodes.ValidCoordinates(pos))
            {
                if (nodes[pos.x, pos.y].box.CompletedEdgesCount() >= 3)
                    continue;
            }

            result.Add(item);

        }

        return result;
    }

    private void OnValidate()
    {
        rows = UtilityFunctions.ClampMin(rows, 1);
        columns = UtilityFunctions.ClampMin(columns, 1);
    }

    public override void InitBoard()
    {

        if (columns <= 0 || rows <= 0)
            return;
        score = 0;
        edgesX = new Edge[columns + 1, rows];
        edgesY = new Edge[columns, rows + 1];
        nodes = new DotsAndBoxesNode[columns, rows];

        for (int i = 0; i < edgesX.GetLength(0); i++)
        {
            for (int j = 0; j < edgesX.GetLength(1); j++)
            {

                edgesX[i, j] = new Edge(EdgePosition.Horizontal, new Position(i, j));
            }
        }
        for (int i = 0; i < edgesY.GetLength(0); i++)
        {
            for (int j = 0; j < edgesY.GetLength(1); j++)
            {

                edgesY[i, j] = new Edge(EdgePosition.Vertical, new Position(i, j));
            }
        }

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Box b = new Box(new Position(i, j));

                SetBoxEdges(ref b, ref edgesX, ref edgesY);

                DotsAndBoxesNode node = new DotsAndBoxesNode(new Position(i, j));
                node.box = b;

                nodes[i, j] = node;

            }
        }
        isInit = true;

    }

    public float EvaluateBoard()
    {
        List<Edge> edges = GetEdges();
        float score = 0;
        for (int i = 0; i < edges.Count; i++)
        {
            if (edges[i].active)
            {
                if (edges[i].owner == player1)
                    score++;
                else
                    score--;
            }
        }
        List<DotsAndBoxesNode> nodes = this.nodes.GetItems();
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].box.filled)
            {
                if (nodes[i].box.owner == player1)
                    score += 10;
                else
                    score -= 10;
            }
        }

        return score;
    }

    public DotsAndBoxesBoard BoardAfterMove(Player player, Edge edge)
    {
        DotsAndBoxesBoard b = new DotsAndBoxesBoard(this);

        b.TraceEdge(edge, player);
        return b;
    }

    public void PrintNodes()
    {
        foreach (var item in nodes.GetItems())
        {
            Debug.Log(item.box);
        }
    }

    public void TraceEdge(Edge edge, Player player)
    {
        if (!isInit || edge == null)
            return;

        if (edge.orientation == EdgePosition.Horizontal && edgesX.ValidCoordinates(edge.pos.x, edge.pos.y))
        {

            edgesX[edge.pos.x, edge.pos.y].active = true;
            edgesX[edge.pos.x, edge.pos.y].owner = player;


            if (CheckForFill(edge.pos))
                FillBox(edge.pos, player);
            if (CheckForFill(edge.pos - Position.Right))
                FillBox(edge.pos - Position.Right, player);

        }
        else if (edge.orientation == EdgePosition.Vertical && edgesY.ValidCoordinates(edge.pos.x, edge.pos.y))
        {

            edgesY[edge.pos.x, edge.pos.y].active = true;
            edgesY[edge.pos.x, edge.pos.y].owner = player;

            //Debug.Log(edgesY[edge.pos.x, edge.pos.y]);
            if (CheckForFill(edge.pos))
                FillBox(edge.pos, player);
            if (CheckForFill(edge.pos - Position.Up))
                FillBox(edge.pos - Position.Up, player);
        }
        //if (player == player1)
        //    score++;
        //else
        //    score--;

    }

    public bool CheckForFill(Position pos)
    {
        //Debug.Log("Checking at " + pos);
        if (!nodes.ValidCoordinates(pos.x, pos.y))
            return false;


        foreach (Edge item in nodes[pos.x, pos.y].box.Edges.GetValues())
        {
            //Debug.Log(item);

            if (item.active == false)
                return false;
        }

        return true;
    }

    public void FillBox(Position pos, Player player)
    {
        if (!nodes.ValidCoordinates(pos.x, pos.y))
            return;

        Box box = nodes[pos.x, pos.y].box;
        foreach (Edge item in box.Edges.GetValues())
        {
            if (item.active == false)
                return;

        }

        box.filled = true;
        box.owner = player;
        if (player == player1)
            score += 10;
        else
            score -= 10;

    }

    void SetBoxEdges(ref Box box, ref Edge[,] edgesX, ref Edge[,] edgesY)
    {
        int x = box.pos.x;
        int y = box.pos.y;

        box.Edges = new BoxStruct<Edge>();
        box.Edges.left = edgesX[x, y];
        box.Edges.right = edgesX[x + 1, y];
        box.Edges.top = edgesY[x, y + 1];
        box.Edges.bottom = edgesY[x, y];
    }

    public bool IsMoveCapture(Edge edge, Player p)
    {
        var b = BoardAfterMove(p, edge);
        if (b.CheckForFill(edge.pos) || b.CheckForFill(edge.orientation == EdgePosition.Horizontal ? edge.pos - Position.Right : edge.pos - Position.Up))
            return true;
        return false;
    }

    void CreateBoxEdges(ref Box box)
    {

        int x = box.pos.x;
        int y = box.pos.y;

        //Left Edge

        box.Edges = new BoxStruct<Edge>();

        if (ValidCoordinate(x - 1, y) ? nodes[x - 1, y] == null : true)
        {
            box.Edges.left = new Edge(EdgePosition.Horizontal, new Position(x, y));
            edgesX[x, y] = box.Edges.left;
        }
        else
        {
            box.Edges.left = nodes[x - 1, y].box?.Edges.right;
        }

        //Right Edge
        if (ValidCoordinate(x + 1, y) ? nodes[x + 1, y] == null : true)
        {
            box.Edges.right = new Edge(EdgePosition.Horizontal, new Position(x + 1, y));
            edgesX[x + 1, y] = box.Edges.right;
        }
        else
        {
            box.Edges.right = nodes[x + 1, y].box?.Edges.left;
        }

        //Top Edge
        if (ValidCoordinate(x, y + 1) ? nodes[x, y + 1] == null : true)
        {
            box.Edges.top = new Edge(EdgePosition.Vertical, new Position(x, y + 1));
            edgesY[x, y + 1] = box.Edges.top;
        }
        else
        {
            box.Edges.top = nodes[x, y + 1].box?.Edges.bottom;
        }

        //Bottom Edge
        if (ValidCoordinate(x, y - 1) ? nodes[x, y - 1] == null : true)
        {
            box.Edges.bottom = new Edge(EdgePosition.Vertical, new Position(x, y));
            edgesY[x, y] = box.Edges.bottom;
        }
        else
        {
            box.Edges.bottom = nodes[x, y - 1].box?.Edges.top;
        }
    }
    /// <summary>
    /// Returns a list with all the nodes.
    /// </summary>
    public List<DotsAndBoxesNode> GetNodes()
    {
        if (nodes == null)
            return null;
        List<DotsAndBoxesNode> result = new List<DotsAndBoxesNode>();
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {

                result.Add(nodes[i, j]);
            }
        }
        return result;
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

    public float alphaBeta(float depth, DotsAndBoxesBoard board, bool maximisingPlayer, float alpha = -10000, float beta = 10000, bool capture = false)
    {

        float bestValue;

        if (depth <= 0)
        {
            //if (capture)
            //    bestValue = board.score;
            //else
            bestValue = -board.score;

        }
        else if (maximisingPlayer)
        {
            bestValue = alpha;
            List<Edge> moves = board.GetValidEdges();
            if (moves.Count > 0)
                for (var i = 0; i < moves.Count; i++)
                {

                    var childValue = 0f;
                    if (board.IsMoveCapture(moves[i], board.player2))
                    {

                        childValue = alphaBeta(depth - 1, board.BoardAfterMove(player2, moves[i]), true, bestValue, beta, true);
                    }
                    else
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
            List<Edge> moves = board.GetValidEdges();
            // Recurse for all children of node.
            if (moves.Count > 0)
                for (var i = 0; i < moves.Count; i++)
                {

                    var childValue = 0f;
                    if (board.IsMoveCapture(moves[i], board.player1))
                    {

                        childValue = alphaBeta(depth - 1, board.BoardAfterMove(player1, moves[i]), false, alpha, bestValue);
                    }
                    else
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

        return bestValue;
    }
}
