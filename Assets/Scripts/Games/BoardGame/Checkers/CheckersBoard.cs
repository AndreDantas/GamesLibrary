using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CheckersBoard : Board
{

    public CheckersNode[,] nodes;

    public CheckerPlayer playerTop;
    public CheckerPlayer playerBottom;
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
    public void Move(CheckerMove move)
    {
        if (move == null)
            return;

        Checker piece = GetPiece(move.start);
        SetPiece(move.end, piece);
        SetPiece(move.start, null);
        piece.hasMoved = true;

    }

    /// <summary>
    /// Returns all possible moves of the given player.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public List<CheckerMove> GetPossibleMovements(CheckerPlayer player)
    {

        List<CheckerMove> result = new List<CheckerMove>();
        if (nodes == null)
            return result;

        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                Checker c = GetPiece(new Position(i, j));
                if (c != null ? c.player == player : false)
                    result.AddRange(c.GetMovements());
            }
        }
        return result;
    }

    /// <summary>
    /// Returns all pieces of the given player that can capture.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public List<Checker> GetPiecesWithCapture(CheckerPlayer player)
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
