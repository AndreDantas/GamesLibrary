using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;
[System.Serializable]
public class Connect4Board : Board
{
    [TableMatrix(DrawElementMethod = "DrawColoredEnumElement", ResizableColumns = false)]
    public Connect4Node[,] nodes;

    public Player player1;
    public Player player2;
    [SerializeField, HideInInspector]
    private int connectTarget = 4;
    [ShowInInspector]
    public int ConnectTarget { get { return connectTarget; } set { value = Mathf.Max(3, value); connectTarget = value; } }
    [SerializeField, HideInInspector]
    protected bool finished = false;
    public static List<Position> ConnectDirections
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

    public Connect4Board()
    {

    }
    public Connect4Board(int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
    }

    public Connect4Board(Connect4Board oldBoard)
    {
        if (oldBoard == null)
            return;
        columns = oldBoard.columns;
        rows = oldBoard.rows;
        nodeOffsetX = oldBoard.nodeOffsetX;
        nodeOffsetY = oldBoard.nodeOffsetY;
        Connect4Node[,] n = new Connect4Node[oldBoard.columns, oldBoard.rows];
        for (int i = 0; i < oldBoard.nodes.GetLength(0); i++)
        {
            for (int j = 0; j < oldBoard.nodes.GetLength(1); j++)
            {
                if (oldBoard.nodes[i, j] != null)
                    n[i, j] = new Connect4Node(oldBoard.nodes[i, j]);
            }
        }
        this.nodes = n;
        player1 = oldBoard.player1;
        player2 = oldBoard.player2;
        isInit = oldBoard.isInit;
        finished = oldBoard.finished;

    }

    private void OnValidate()
    {
        rows = UtilityFunctions.ClampMin(rows, 2);
        columns = UtilityFunctions.ClampMin(columns, 2);
    }

    public override void InitBoard()
    {

        if (columns <= 0 || rows <= 0)
            return;
        nodes = new Connect4Node[columns, rows];
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Connect4Node node = new Connect4Node(new Position(i, j));

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
    public Piece AddPiece(Player player, Position pos)
    {
        if (player == null || !ValidCoordinate(pos))
            return null;

        return (nodes[pos.x, pos.y].pieceOnNode = new Piece(pos, player));

    }

    /// <summary>
    /// Returns a list with all the nodes.
    /// </summary>
    public List<Connect4Node> GetNodes()
    {
        if (nodes == null)
            return null;
        List<Connect4Node> result = new List<Connect4Node>();
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {

                result.Add(nodes[i, j]);
            }
        }
        return result;
    }

    public void Move(Player player, int columnIndex)
    {
        if (!isInit || !ValidColumn(columnIndex) || player == null)
            return;

        for (int i = 0; i < rows; i++)
        {
            if (nodes[columnIndex, i].pieceOnNode == null)
            {
                nodes[columnIndex, i].pieceOnNode = new Piece(new Position(columnIndex, i), player);
                return;
            }
        }
    }

    public Connect4Board BoardAfterMove(Player player, int columnIndex)
    {
        if (!isInit || !ValidColumn(columnIndex) || player == null)
            return null;
        Connect4Board b = new Connect4Board(this);
        b.Move(player, columnIndex);
        return b;

    }


    public Position CheckForConnect(Player player)
    {
        if (!isInit || player == null)
            return new Position(-1, -1);

        int maxCon = 0;
        Position pos = new Position(-1, -1);
        foreach (var item in GetNodes())
        {

            int pieceCount = GetPieceConnectionsCount(item.pos, player);
            if (pieceCount > maxCon)
            {
                pos = item.pos;
                maxCon = pieceCount;
            }
        }

        if (maxCon >= connectTarget)
            return pos;
        else
            return new Position(-1, -1);
    }

    public int EvaluateBoard()
    {
        if (!isInit)
            return 0;
        int totalValue = 0;
        foreach (var item in GetNodes())
        {
            if (item.pieceOnNode != null)
            {
                if (item.pieceOnNode.player == player1)
                    totalValue += EvaluatePosition(item.pos, player1);
                else
                    totalValue -= EvaluatePosition(item.pos, player2);
            }
        }

        return totalValue;
    }

    public int EvaluatePosition(Position pos, Player player)
    {
        if (!ValidCoordinate(pos) || !isInit || player == null || finished)
            return 0;

        Piece piece = nodes[pos.x, pos.y].pieceOnNode;
        if (piece == null)
            return 0;

        int pts = 0;
        if (piece.player == player)
        {
            int connections = GetPieceConnectionsCount(pos, player);

            if (connections >= connectTarget)
                pts = 1000;
            else
                pts = connections * (player == player1 ? 10 : 1);

        }


        return pts;
    }

    public int GetPieceConnectionsCount(Position pos, Player player)
    {
        if (!ValidCoordinate(pos) || !isInit || player == null)
            return 0;

        int maxConnections = 0;
        Piece piece = nodes[pos.x, pos.y].pieceOnNode;
        if (piece?.player == player)
        {
            int x, y;
            //Debug.Log(pos);

            foreach (var item in ConnectDirections)
            {

                x = pos.x;
                y = pos.y;


                //Debug.Log("Checking at : " + new Position(x, y));
                piece = nodes[x, y].pieceOnNode;
                //Debug.Log(piece);
                if (piece?.player == player)
                {


                    int connections = 0;
                    while (piece?.player == player)
                    {
                        connections++;
                        x += item.x;
                        y += item.y;

                        if (!ValidCoordinate(x, y))
                            break;
                        piece = nodes[x, y].pieceOnNode;


                    }
                    if (connections > maxConnections)
                        maxConnections = connections;


                }
                else
                    break;

            }
        }
        else
        {
            return 0;
        }

        return maxConnections;

    }

    public List<Position> GetConnectPositions(Position pos, Player player)
    {
        if (!ValidCoordinate(pos) || !isInit || player == null)
            return null;

        int maxConnections = 0;
        List<Position> temp = new List<Position>();
        bool validConnect = false;
        List<Position> result = new List<Position>();
        Piece piece = nodes[pos.x, pos.y].pieceOnNode;
        if (piece?.player == player)
        {
            int x, y;
            //Debug.Log(pos);

            foreach (var item in ConnectDirections)
            {

                x = pos.x;
                y = pos.y;

                temp.Clear();
                //Debug.Log("Checking at : " + new Position(x, y));
                piece = nodes[x, y].pieceOnNode;
                //Debug.Log(piece);
                if (piece?.player == player)
                {


                    int connections = 0;
                    while (piece?.player == player)
                    {
                        temp.Add(piece.pos);
                        connections++;
                        x += item.x;
                        y += item.y;

                        if (!ValidCoordinate(x, y))
                            break;
                        piece = nodes[x, y].pieceOnNode;


                    }
                    if (connections > maxConnections)
                    {
                        maxConnections = connections;
                        if (maxConnections >= connectTarget)
                        {
                            //Debug.Log(maxConnections);
                            result.Clear();
                            result.AddRange(temp);
                            validConnect = true;
                        }


                    }


                }
                else
                    break;

            }
        }
        else
        {
            return null;
        }
        if (validConnect)
            return result;
        else return null;

    }

    public bool ValidColumnIndex(int columnIndex)
    {
        return !(columnIndex < 0 || columnIndex >= columns);

    }

    public bool ValidColumn(int columnIndex)
    {
        if (!isInit || !ValidColumnIndex(columnIndex) || finished)
            return false;

        bool valid = false;
        for (int i = 0; i < rows; i++)
        {
            if (nodes[columnIndex, i].pieceOnNode == null)
                valid = true;
        }

        return valid;
    }

    public int GetRowEmptyPosition(int columnIndex)
    {
        if (!isInit || !ValidColumnIndex(columnIndex))
            return -1;
        for (int i = 0; i < rows; i++)
        {
            if (nodes[columnIndex, i].pieceOnNode == null)
                return i;
        }
        return -1; ;
    }

    public List<int> GetValidColumns()
    {
        if (!isInit)
            return null;
        List<int> result = new List<int>();
        for (int i = 0; i < columns; i++)
        {
            if (ValidColumn(i))
                result.Add(i);
        }

        return result;
    }

    [ShowInInspector]
    public int movesEval { get; internal set; }
    [ShowInInspector]
    public int movesEvalPerFrame { get { return 300; } }
    public void ResetMovesEval()
    {
        movesEval = 0;
    }

    public IEnumerator alphaBeta(float depth, Connect4Board board, bool maximisingPlayer, Action<float> result, float alpha = -10000, float beta = 10000)
    {

        float bestValue;

        if (depth <= 0)
        {
            bestValue = -board.EvaluateBoard();

        }
        else if (maximisingPlayer)
        {
            bestValue = alpha;


            for (var i = 0; i < rows; i++)
            {
                if (!board.ValidColumn(i))
                    continue;
                var childValue = 0f;
                if (movesEval % movesEvalPerFrame == 0)
                {

                    yield return alphaBeta(depth - 1, board.BoardAfterMove(player2, i), false, v => childValue = v, bestValue, beta);
                }
                else
                {
                    childValue = alphaBeta(depth - 1, board.BoardAfterMove(player2, i), false, bestValue, beta);
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

        }
        else
        {
            bestValue = beta;

            for (var i = 0; i < rows; i++)
            {
                if (!board.ValidColumn(i))
                    continue;
                var childValue = 0f;
                if (movesEval % movesEvalPerFrame == 0)
                {

                    yield return alphaBeta(depth - 1, board.BoardAfterMove(player1, i), true, v => childValue = v, alpha, bestValue);
                }
                else
                {
                    //var e = alphaBeta(depth - 1, b, true, v => childValue = v, alpha, bestValue);
                    //while (e.MoveNext())
                    //    yield return e.Current;
                    childValue = alphaBeta(depth - 1, board.BoardAfterMove(player1, i), true, alpha, bestValue);
                }
                bestValue = Mathf.Min(bestValue, childValue);
                if (bestValue <= alpha)
                {
                    break;
                }
            }

        }
        movesEval++;
        result(bestValue);
    }
    public float alphaBeta(float depth, Connect4Board board, bool maximisingPlayer, float alpha = -10000, float beta = 10000)
    {

        float bestValue;

        if (depth <= 0)
        {
            bestValue = -board.EvaluateBoard();

        }
        else if (maximisingPlayer)
        {
            bestValue = alpha;

            for (var i = 0; i < rows; i++)
            {
                if (!board.ValidColumn(i))
                    continue;
                var childValue = 0f;
                childValue = alphaBeta(depth - 1, board.BoardAfterMove(player2, i), false, bestValue, beta);

                bestValue = Mathf.Max(bestValue, childValue);
                if (beta <= bestValue)
                {
                    break;
                }
            }

        }
        else
        {
            bestValue = beta;


            for (var i = 0; i < rows; i++)
            {
                if (!board.ValidColumn(i))
                    continue;
                var childValue = 0f;
                childValue = alphaBeta(depth - 1, board.BoardAfterMove(player1, i), true, alpha, bestValue);

                bestValue = Mathf.Min(bestValue, childValue);
                if (bestValue <= alpha)
                {
                    break;
                }
            }

        }
        movesEval++;
        return bestValue;
    }
}
