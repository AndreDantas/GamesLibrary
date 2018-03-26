﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum MapNavigation
{
    /// <summary>
    /// Vertical and horizontal movement.
    /// </summary>
    Cross,
    /// <summary>
    /// 8 directions movement.
    /// </summary>
    Diagonals
}
[System.Serializable]
public enum NodeDistance
{
    Manhattan,
    Euclidean
}
[System.Serializable]
public class Board
{
    /// <summary>
    /// Type of navigation.
    /// </summary>
    public MapNavigation mapNavigation = MapNavigation.Cross;
    /// <summary>
    /// Number of rows. Can't be less than 1.
    /// </summary>
    public int rows = 10;
    /// <summary>
    /// Number of columns. Can't be less than 1.
    /// </summary>
    public int columns = 10;
    /// <summary>
    ///The X offset of each node.
    /// </summary>
    public float nodeOffsetX = 0.5f;
    /// <summary>
    ///The Y offset of each node.
    /// </summary>
    public float nodeOffsetY = 0.5f;
    /// <summary>
    /// The 2D array of nodes.
    /// </summary>
    public Node[,] nodes;

    private void OnValidate()
    {
        rows = MathOperations.ClampMin(rows, 1);
        columns = MathOperations.ClampMin(columns, 1);
    }

    public Board(int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
    }

    public Board()
    {

    }

    public virtual void InitBoard()
    {
        if (columns <= 0 || rows <= 0)
            return;
        nodes = new Node[columns, rows];
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Node node = new Node(new Position(i, j));
                nodes[i, j] = node;

            }
        }
    }
    public Board(Board oldBoard)
    {
        columns = oldBoard.columns;
        rows = oldBoard.rows;
        nodeOffsetX = oldBoard.nodeOffsetX;
        nodeOffsetY = oldBoard.nodeOffsetY;
        nodes = (Node[,])oldBoard.nodes.Clone();
    }

    /// <summary>
    /// Returns a list with all the nodes.
    /// </summary>
    public List<Node> GetNodes()
    {
        if (nodes == null)
            return null;
        List<Node> result = new List<Node>();
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
    /// Returns node's neighbors.
    /// </summary>
    public List<Node> GetNeighbors(Node node)
    {
        if (ValidCoordinate(node.pos))
        {
            List<Node> result = new List<Node>();
            if (mapNavigation == MapNavigation.Cross)
            {
                //Left neighbor
                if (ValidCoordinate(node.pos.x - 1, node.pos.y))
                {
                    result.Add(nodes[node.pos.x - 1, node.pos.y]);
                }
                //Right neighbor
                if (ValidCoordinate(node.pos.x + 1, node.pos.y))
                {
                    result.Add(nodes[node.pos.x + 1, node.pos.y]);
                }
                //Bottom neighbor
                if (ValidCoordinate(node.pos.x, node.pos.y - 1))
                {
                    result.Add(nodes[node.pos.x, node.pos.y - 1]);
                }
                //Top neighbor
                if (ValidCoordinate(node.pos.x, node.pos.y + 1))
                {
                    result.Add(nodes[node.pos.x, node.pos.y + 1]);
                }
            }
            else
            {
                for (int i = node.pos.x - 1; i <= node.pos.x + 1; i++)
                {
                    for (int j = node.pos.y - 1; j <= node.pos.y + 1; j++)
                    {
                        if (i == node.pos.x && j == node.pos.y)
                            continue;
                        else
                        {
                            if (ValidCoordinate(i, j))
                            {
                                result.Add(nodes[i, j]);
                            }
                        }
                    }
                }
            }
            return result;
        }
        else
            return null;
    }

    /// <summary>
    /// Returns node's neighbors.
    /// </summary>
    public List<Node> GetNeighbors(int x, int y)
    {
        return GetNeighbors(nodes[x, y]);
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

    /// <summary>
    /// Returns the node located on that position.
    /// </summary>
    /// <param name="worldPos"></param>

    public Node GetNodeFromWorldPosition(Vector2 worldPos)
    {
        if (ValidCoordinate(worldPos))
        {
            int tilePosX = (int)Mathf.Floor(worldPos.x);
            int tilePosY = (int)Mathf.Floor(worldPos.y);
            return nodes[tilePosX, tilePosY];
        }
        return null;
    }
    /// <summary>
    /// Returns the world position of the node.
    /// </summary>
    public Vector3 GetWorldPositionFromNode(int x, int y)
    {
        if (ValidCoordinate(x, y))
        {
            return new Vector3(nodes[x, y].pos.x + nodeOffsetX, nodes[x, y].pos.y + nodeOffsetY, 0);
        }
        return Vector3.zero;
    }
    public List<Vector3> GetWorldPositionsFromNodes(List<Node> nodeList)
    {
        if (nodeList == null ? true : nodeList.Count == 0)
            return null;
        List<Vector3> posList = new List<Vector3>();
        foreach (Node n in nodeList)
        {
            posList.Add(GetWorldPositionFromNode(n.pos.x, n.pos.y));
        }
        return posList;
    }

    /// <summary>
    /// Returns the world position of the node.
    /// </summary>

    public Vector3 GetWorldPositionFromNode(Node node)
    {
        return GetWorldPositionFromNode(node.pos.x, node.pos.y);
    }
    /// <summary>
    /// Distance between two nodes.
    /// </summary>
    public static float Distance(Node a, Node b, NodeDistance distance = NodeDistance.Manhattan)
    {
        if (distance == NodeDistance.Manhattan)
        {
            return (Mathf.Abs(a.pos.x - b.pos.x) + Mathf.Abs(a.pos.y - b.pos.y)) * Node.MinCost;
        }
        else
            return Mathf.Sqrt(Mathf.Pow(a.pos.x - b.pos.x, 2) + Mathf.Pow(a.pos.y - b.pos.y, 2));
    }

    /// <summary>
    /// Returns a list of the nodes that are closer to te target.
    /// </summary>
    /// <param name="availableArea">The area to check.</param>
    /// <param name="target">The target node</param>
    /// <param name="optimalDistance">The best possible distance.</param>
    /// <returns></returns>
    public static List<Node> GetClosestNode(List<Node> availableArea, Node target, int optimalDistance)
    {
        List<Node> result = new List<Node>();
        foreach (Node n in availableArea)
        {
            if (n.pos == target.pos)
                continue;
            int distance = Mathf.Abs(n.pos.x - target.pos.x) + Mathf.Abs(n.pos.y - target.pos.y);
            if (distance <= optimalDistance)
            {
                result.Add(n);
            }
        }
        return result;
    }

    /// <summary>
    /// O(n^2) solution to find the Manhattan distance to "on" nodes in a two dimension array
    /// </summary>
    /// <param name="area">The "on" nodes.</param>
    /// <returns></returns>
    public int[,] Manhattan(List<Node> area)
    {
        //Algorithm used: http://blog.ostermiller.org/dilate-and-erode

        if (nodes == null) // The map can't be null.
            return null;
        int[,] rangeMap = new int[nodes.GetLength(0), nodes.GetLength(1)];

        foreach (Node n in area)
        {
            if (ValidCoordinate(n))
            {
                rangeMap[n.pos.x, n.pos.y] = 1; // Marking the nodes on the map.
            }
        }

        // Traverse from top left to bottom right
        for (int i = 0; i < rangeMap.GetLength(0); i++)
        {
            for (int j = 0; j < rangeMap.GetLength(1); j++)
            {
                if (rangeMap[i, j] == 1)
                {
                    rangeMap[i, j] = 0;
                }
                else
                {
                    rangeMap[i, j] = rangeMap.GetLength(0) + rangeMap.GetLength(1);
                    if (i > 0)
                        rangeMap[i, j] = Mathf.Min(rangeMap[i, j], rangeMap[i - 1, j] + 1);
                    if (j > 0)
                        rangeMap[i, j] = Mathf.Min(rangeMap[i, j], rangeMap[i, j - 1] + 1);
                }
            }
        }

        // Traverse from bottom right to top left
        for (int i = rangeMap.GetLength(0) - 1; i >= 0; i--)
        {
            for (int j = rangeMap.GetLength(1) - 1; j >= 0; j--)
            {
                if (i + 1 < rangeMap.GetLength(0))
                    rangeMap[i, j] = Mathf.Min(rangeMap[i, j], rangeMap[i + 1, j] + 1);
                if (j + 1 < rangeMap.GetLength(1))
                    rangeMap[i, j] = Mathf.Min(rangeMap[i, j], rangeMap[i, j + 1] + 1);
            }
        }
        return rangeMap;
    }

    public bool IsPositionEmpty(Position pos)
    {
        if (!ValidCoordinate(pos))
            return false;

        return nodes[pos.x, pos.y].pieceOnNode == null;
    }

    /// <summary>
    /// Returns a piece from a position.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public virtual Piece GetPiece(Position pos)
    {
        if (ValidCoordinate(pos))
        {
            if (nodes[pos.x, pos.y].pieceOnNode != null)
                return nodes[pos.x, pos.y].pieceOnNode;
        }
        return null;
    }

    /// <summary>
    /// Sets a piece on a position.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="piece"></param>
    /// <returns></returns>
    public virtual Board SetPiece(Position pos, Piece piece)
    {
        if (ValidCoordinate(pos))
        {
            nodes[pos.x, pos.y].pieceOnNode = piece;

        }

        return this;
    }

    /// <summary>
    /// Moves a piece on the board.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public Board Move(Position start, Position end)
    {
        Piece piece = GetPiece(start);
        SetPiece(end, piece);
        SetPiece(start, null);
        return this;
    }
    /// <summary>
    /// Moves a piece on the board.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public Board Move(Move move)
    {
        return Move(move.start, move.end);
    }

    /// <summary>
    /// Returns the state of the board after a move.
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    public virtual Board BoardAfterMove(Move move)
    {
        Board board = new Board(this);
        board.Move(move);
        return board;
    }



    public static int DefaultManhattanDistance(Node a, Node b)
    {
        return DefaultManhattanDistance(a.pos.x, a.pos.y, b.pos.x, b.pos.y);
    }

    public static int DefaultManhattanDistance(int Ax, int Ay, int Bx, int By)
    {
        return Mathf.Abs(Ax - Bx) + Mathf.Abs(Ay - By);
    }

}
