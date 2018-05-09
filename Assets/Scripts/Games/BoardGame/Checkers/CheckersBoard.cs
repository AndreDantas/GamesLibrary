using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.UI;

[System.Serializable]
public class CheckersBoard : Board
{

    public CheckersNode[,] nodes;
    public CheckersSettingsData settings = new CheckersSettingsData();
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
                        pos[j, i] = nodes[x, y].checkerOnNode != null;
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
    public CheckersPlayer playerTop;
    public CheckersPlayer playerBottom;
    public CheckersBoard()
    {

    }
    public CheckersBoard(CheckersBoard oldBoard)
    {
        if (oldBoard == null)
            return;
        columns = oldBoard.columns;
        rows = oldBoard.rows;
        nodeOffsetX = oldBoard.nodeOffsetX;
        nodeOffsetY = oldBoard.nodeOffsetY;
        CheckersNode[,] n = new CheckersNode[oldBoard.columns, oldBoard.rows];
        for (int i = 0; i < oldBoard.nodes.GetLength(0); i++)
        {
            for (int j = 0; j < oldBoard.nodes.GetLength(1); j++)
            {
                if (oldBoard.nodes[i, j] != null)
                    n[i, j] = new CheckersNode(oldBoard.nodes[i, j], this);
            }
        }
        this.nodes = n;
        playerTop = oldBoard.playerTop;
        playerBottom = oldBoard.playerBottom;
        isInit = oldBoard.isInit;
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
        nodes = new CheckersNode[columns, rows];
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                CheckersNode node = new CheckersNode(new Position(i, j));
                nodes[i, j] = node;

            }
        }
        isInit = true;
    }

    /// <summary>
    /// Returns a list with all the nodes.
    /// </summary>
    public List<CheckersNode> GetNodes()
    {
        if (nodes == null)
            return null;
        List<CheckersNode> result = new List<CheckersNode>();
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
    /// Checks if the coordinate is valid in this map.
    /// </summary>
    public bool ValidCoordinate(Vector2 worldPos)
    {

        int tilePosX = (int)Mathf.Floor(worldPos.x);
        int tilePosY = (int)Mathf.Floor(worldPos.y);

        return ValidCoordinate(tilePosX, tilePosY);
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
    /// <summary>
    /// Checks if the coordinate is valid in this map.
    /// </summary>
    public bool ValidCoordinate(Node node)
    {
        if (node == null)
            return false;

        return ValidCoordinate(node.pos);
    }

    public List<CheckersNode> GetNeighbors(Position pos)
    {
        if (ValidCoordinate(pos))
        {
            List<CheckersNode> result = new List<CheckersNode>();
            if (ValidCoordinate(pos.x + 1, pos.y + 1))
            {
                result.Add(nodes[pos.x + 1, pos.y + 1]);
            }
            if (ValidCoordinate(pos.x + 1, pos.y - 1))
            {
                result.Add(nodes[pos.x + 1, pos.y - 1]);
            }
            if (ValidCoordinate(pos.x - 1, pos.y + 1))
            {
                result.Add(nodes[pos.x - 1, pos.y + 1]);
            }
            if (ValidCoordinate(pos.x - 1, pos.y - 1))
            {
                result.Add(nodes[pos.x - 1, pos.y - 1]);
            }
            return result;
        }
        return null;
    }

    public bool IsPositionEmpty(Position pos)
    {
        if (!ValidCoordinate(pos))
            return false;

        return nodes[pos.x, pos.y].checkerOnNode == null;
    }

    /// <summary>
    /// Used to move a piece.
    /// </summary>
    /// <param name="move"></param>
    public CheckersBoard Move(CheckerMove move)
    {
        if (move == null)
            return null;

        Checker piece = GetPiece(move.start);
        if (piece == null)
            Debug.Log(move);
        SetPiece(move.end, piece);
        piece.hasMoved = true;
        SetPiece(move.start, null);
        CheckersPlayer player = piece.player as CheckersPlayer;
        if (!piece.isKing)
        {
            if (move.isCapture ? move.next == null : true)
            {
                if (player.orientation == Orientation.DOWN)
                {

                    if (piece.pos.y == rows - 1)
                        BecomeKing(piece);
                }
                else
                {
                    if (piece.pos.y == 0)
                        BecomeKing(piece);
                }
            }
        }
        return this;

    }
    /// <summary>
    /// Used to change checker piece to a king piece
    /// </summary>
    public virtual void BecomeKing(Checker c)
    {
        if (c == null || c.isKing)
            return;
        c.isKing = true;
        c.moveDistance = settings.kingInfiniteMoveDistance ? 99 : settings.pieceMoveDistance;
        c.normalMovement = new DiagonalMovement(true, true, true, true);
        c.jumpMovement = new DiagonalMovement(true, true, true, true);
    }

    public CheckersBoard MoveCapture(CheckerMove move)
    {
        if (move == null)
            return null;
        if (!move.isCapture)
            return null;
        CheckerMove current = move;


        while (current != null)
        {
            Move(current);
            Position delta = new Position(UtilityFunctions.Sign(current.end.x - current.start.x), UtilityFunctions.Sign(current.end.y - current.start.y));
            Position capturePos = current.end - delta;
            nodes[capturePos.x, capturePos.y].checkerOnNode = null;
            current = current.next;
        }
        return this;
    }

    /// <summary>
    /// Returns all possible moves of the given player.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public List<CheckerMove> GetPossibleMovements(CheckersPlayer player)
    {

        List<CheckerMove> result = new List<CheckerMove>();
        List<CheckerMove> captures = new List<CheckerMove>();
        List<CheckerMove> temp = new List<CheckerMove>();
        if (nodes == null)
            return result;

        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                Checker c = GetPiece(new Position(i, j));
                if (c != null ? c.player == player : false)
                {

                    temp = c.GetMovements();
                    if (temp.Count > 0)
                    {
                        if (temp[0].isCapture)
                            captures.AddRange(temp);
                        result.AddRange(temp);
                    }
                }
            }
        }
        return captures.Count > 0 ? captures : result;

    }
    /// <summary>
    /// Returns all possible moves of the given AI.
    /// </summary>
    /// <param name="ai"></param>
    /// <returns></returns>
    public List<CheckerMove> GetPossibleMovementsAI(CheckersPlayer ai)
    {

        List<CheckerMove> result = new List<CheckerMove>();
        List<CheckerMove> captures = new List<CheckerMove>();
        List<CheckerMove> temp = new List<CheckerMove>();
        if (nodes == null)
            return result;

        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                Checker c = GetPiece(new Position(i, j));
                if (c != null ? c.player == ai : false)
                {

                    temp = c.GetMovements();
                    if (temp.Count > 0)
                    {
                        if (temp[0].isCapture)
                        {
                            foreach (CheckerMove cm in temp)
                            {
                                if (!captures.Contains(cm) && cm.start == c.pos)
                                    captures.Add(cm);
                            }
                        }
                        result.AddRange(temp);
                    }
                }
            }
        }
        return captures.Count > 0 ? captures : result;

    }
    /// <summary>
    /// Returns all pieces of the given player that can capture.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public List<Checker> GetPiecesWithCapture(CheckersPlayer player)
    {
        List<Checker> result = new List<Checker>();
        if (nodes == null)
            return result;

        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                Checker c = GetPiece(new Position(i, j));
                if (c != null ? c.player == player : false)
                {
                    if (c.HasCapture())
                        result.Add(c);
                }

            }
        }
        return result;
    }

    /// <summary>
    /// Used to make "Capture" movements. Returns the captured pieces.
    /// </summary>
    /// <param name="moves"></param>
    /// <returns></returns>
    public List<Checker> CaptureMovement(List<CheckerMove> moves)
    {
        if (moves == null)
            return null;
        List<Checker> captured = new List<Checker>();
        foreach (CheckerMove m in moves)
        {
            Position delta = new Position(UtilityFunctions.Sign(m.end.x - m.start.x), UtilityFunctions.Sign(m.end.y - m.start.y));
            Position capturedPiece = m.end - delta;
            Move(m);
            captured.Add(GetPiece(capturedPiece));
            SetPiece(capturedPiece, null);
        }
        return captured;
    }

    public float EvaluateBoard()
    {
        if (!isInit)
            return 0;
        float totalValue = 0;
        foreach (CheckersNode n in GetNodes())
        {
            if (n.checkerOnNode != null)
            {
                if (n.checkerOnNode.player == playerBottom)
                    totalValue += n.checkerOnNode.GetPieceValue();
                else
                    totalValue -= n.checkerOnNode.GetPieceValue();
            }
        }
        return totalValue;
    }

    public CheckersBoard BoardAfterMove(CheckerMove move)
    {
        CheckersBoard boardAfterMove = new CheckersBoard(this);
        if (move.isCapture)
            return boardAfterMove.MoveCapture(move);
        else
            return boardAfterMove.Move(move);

    }



    public int movesEval { get; internal set; }
    public int movesEvalPerFrame = 200;
    public void ResetMovesEval()
    {
        movesEval = 0;
    }
    public IEnumerator alphaBeta(float depth, CheckersBoard board, bool maximisingPlayer, Action<float> result, float alpha = -10000, float beta = 10000)
    {

        float bestValue;

        if (depth <= 0)
        {
            bestValue = -board.EvaluateBoard();

        }
        else if (maximisingPlayer)
        {
            bestValue = alpha;
            List<CheckerMove> moves = board.GetPossibleMovementsAI(playerTop);
            if (moves.Count > 0)
                for (var i = 0; i < moves.Count; i++)
                {
                    CheckersBoard b = board.BoardAfterMove(moves[i]);
                    var childValue = 0f;
                    if (movesEval % movesEvalPerFrame == 0)
                    {

                        yield return alphaBeta(depth - 1, b, false, v => childValue = v, bestValue, beta);
                    }
                    else
                    {
                        childValue = alphaBeta(depth - 1, b, false, bestValue, beta);
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
            List<CheckerMove> moves = board.GetPossibleMovementsAI(playerBottom);
            // Recurse for all children of node.
            if (moves.Count > 0)
                for (var i = 0; i < moves.Count; i++)
                {
                    CheckersBoard b = board.BoardAfterMove(moves[i]);
                    var childValue = 0f;
                    if (movesEval % movesEvalPerFrame == 0)
                    {

                        yield return alphaBeta(depth - 1, b, true, v => childValue = v, alpha, bestValue);
                    }
                    else
                    {
                        //var e = alphaBeta(depth - 1, b, true, v => childValue = v, alpha, bestValue);
                        //while (e.MoveNext())
                        //    yield return e.Current;
                        childValue = alphaBeta(depth - 1, b, true, alpha, bestValue);
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
    public float alphaBeta(float depth, CheckersBoard board, bool maximisingPlayer, float alpha = -10000, float beta = 10000)
    {

        float bestValue;

        if (depth <= 0)
        {
            bestValue = -board.EvaluateBoard();

        }
        else if (maximisingPlayer)
        {
            bestValue = alpha;
            List<CheckerMove> moves = board.GetPossibleMovementsAI(playerTop);
            for (var i = 0; i < moves.Count; i++)
            {
                CheckersBoard b = board.BoardAfterMove(moves[i]);
                var childValue = 0f;
                childValue = alphaBeta(depth - 1, b, false, bestValue, beta);

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
            List<CheckerMove> moves = board.GetPossibleMovementsAI(playerBottom);
            // Recurse for all children of node.
            for (var i = 0; i < moves.Count; i++)
            {
                CheckersBoard b = board.BoardAfterMove(moves[i]);
                var childValue = 0f;
                childValue = alphaBeta(depth - 1, b, true, alpha, bestValue);


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

    /// <summary>
    /// Returns a piece from a position.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public virtual Checker GetPiece(Position pos)
    {
        if (ValidCoordinate(pos))
        {

            if (nodes[pos.x, pos.y].checkerOnNode != null)
            {
                Checker p = nodes[pos.x, pos.y].checkerOnNode;

                return p;
            }
        }
        return null;
    }



    /// <summary>
    /// Sets a piece on a position.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="piece"></param>
    /// <returns></returns>
    public virtual CheckersBoard SetPiece(Position pos, Checker piece)
    {
        if (ValidCoordinate(pos))
        {
            nodes[pos.x, pos.y].checkerOnNode = piece;
            if (piece != null)
                piece.pos = pos;
        }

        return this;
    }

    /// <summary>
    /// Returns the distance between two points by diagonal movements only.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static int GetDiagonalDistance(Position start, Position end)
    {
        return Mathf.Max(Mathf.Abs(start.x - end.x), Mathf.Abs(start.y - end.y));
    }

    /// <summary>
    /// Removes a piece from a position on the map.
    /// </summary>
    /// <param name="pos"></param>
    public virtual void RemovePiece(Position pos)
    {
        if (ValidCoordinate(pos))
        {
            Checker p = GetPiece(pos);
            if (p != null)
            {
                //removed.Add(p);
                nodes[pos.x, pos.y].checkerOnNode = null;
            }
        }
    }
}
