﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckersBoard : Board
{

    public CheckersNode[,] nodes;

    public CheckerPlayer playerTop;
    public CheckerPlayer playerBottom;

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
        rows = MathOperations.ClampMin(rows, 1);
        columns = MathOperations.ClampMin(columns, 1);
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

    public bool IsPositionEmpty(Position pos)
    {
        if (!ValidCoordinate(pos))
            return false;

        return nodes[pos.x, pos.y].checkerOnNode == null;
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

    public int GetDiagonalDistance(Position start, Position end)
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
