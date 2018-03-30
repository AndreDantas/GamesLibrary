using System;
using System.Collections;
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
public class ChessBoard
{    /// <summary>
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
    public ChessNode[,] nodes;

    public ChessPlayer player1;
    public ChessPlayer player2;
    /// <summary>
    /// If the board has been initiated.
    /// </summary>
    public bool isInit;
    public ChessBoard()
    {

    }
    public ChessBoard(int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
    }
    public ChessBoard(ChessBoard oldBoard)
    {
        if (oldBoard == null)
            return;
        columns = oldBoard.columns;
        rows = oldBoard.rows;
        nodeOffsetX = oldBoard.nodeOffsetX;
        nodeOffsetY = oldBoard.nodeOffsetY;
        ChessNode[,] n = new ChessNode[oldBoard.columns, oldBoard.rows];
        for (int i = 0; i < oldBoard.nodes.GetLength(0); i++)
        {
            for (int j = 0; j < oldBoard.nodes.GetLength(1); j++)
            {
                if (oldBoard.nodes[i, j] != null)
                    n[i, j] = new ChessNode(oldBoard.nodes[i, j], this);
            }
        }
        this.nodes = n;
        player1 = oldBoard.player1;
        player2 = oldBoard.player2;
        isInit = oldBoard.isInit;

    }


    private void OnValidate()
    {
        rows = MathOperations.ClampMin(rows, 1);
        columns = MathOperations.ClampMin(columns, 1);
    }


    public virtual void InitBoard()
    {
        //removed = new List<Piece>();
        if (columns <= 0 || rows <= 0)
            return;
        nodes = new ChessNode[columns, rows];
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                ChessNode node = new ChessNode(new Position(i, j));
                nodes[i, j] = node;

            }
        }
        isInit = true;
    }

    /// <summary>
    /// Returns a list with all the nodes.
    /// </summary>
    public List<ChessNode> GetNodes()
    {
        if (nodes == null)
            return null;
        List<ChessNode> result = new List<ChessNode>();
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {

                result.Add(nodes[i, j]);
            }
        }
        return result;
    }

    public static ChessPiece GetPieceFromType(ChessPieceType type, Position pos)
    {
        switch (type)
        {
            case ChessPieceType.PAWN:
                return new Pawn(pos);
            case ChessPieceType.ROOK:
                return new Rook(pos);
            case ChessPieceType.BISHOP:
                return new Bishop(pos);
            case ChessPieceType.KNIGHT:
                return new Knight(pos);
            case ChessPieceType.QUEEN:
                return new Queen(pos);
            case ChessPieceType.KING:
                return new King(pos);
        }
        return null;
    }

    /// <summary>
    /// Returns node's neighbors.
    /// </summary>
    public List<ChessNode> GetNeighbors(ChessNode node)
    {
        if (ValidCoordinate(node.pos))
        {
            List<ChessNode> result = new List<ChessNode>();
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
    public List<ChessNode> GetNeighbors(int x, int y)
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

    public bool ValidCoordinate(ChessNode node)
    {
        if (node == null)
            return false;

        return ValidCoordinate(node.pos);
    }

    /// <summary>
    /// Returns the node located on that position.
    /// </summary>
    /// <param name="worldPos"></param>

    public ChessNode GetNodeFromWorldPosition(Vector2 worldPos)
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
    public List<Vector3> GetWorldPositionsFromNodes(List<ChessNode> nodeList)
    {
        if (nodeList == null ? true : nodeList.Count == 0)
            return null;
        List<Vector3> posList = new List<Vector3>();
        foreach (ChessNode n in nodeList)
        {
            posList.Add(GetWorldPositionFromNode(n.pos.x, n.pos.y));
        }
        return posList;
    }

    /// <summary>
    /// Returns the world position of the node.
    /// </summary>

    public Vector3 GetWorldPositionFromNode(ChessNode node)
    {
        return GetWorldPositionFromNode(node.pos.x, node.pos.y);
    }
    /// <summary>
    /// Distance between two nodes.
    /// </summary>
    public static float Distance(ChessNode a, ChessNode b, NodeDistance distance = NodeDistance.Manhattan)
    {
        if (distance == NodeDistance.Manhattan)
        {
            return (Mathf.Abs(a.pos.x - b.pos.x) + Mathf.Abs(a.pos.y - b.pos.y)) * ChessNode.MinCost;
        }
        else
            return Mathf.Sqrt(Mathf.Pow(a.pos.x - b.pos.x, 2) + Mathf.Pow(a.pos.y - b.pos.y, 2));
    }

    public bool IsPositionEmpty(Position pos)
    {
        if (!ValidCoordinate(pos))
            return false;

        return nodes[pos.x, pos.y].pieceOnNode == null;
    }




    /// <summary>
    /// Checks if a player is in check.
    /// </summary>
    /// <param name="playerToCheck">The player to check.</param>
    /// <returns></returns>
    public bool IsPlayerInCheck(Player playerToCheck)
    {

        List<Move> moves;

        if (playerToCheck == player1)
            moves = GetPossibleMoves(player2);
        else if (playerToCheck == player2)
            moves = GetPossibleMoves(player1);
        else
            return false;

        for (int i = 0; i < moves.Count; i++)
        {

            Position pos = moves[i].end;
            ChessPiece piece = GetPiece(pos);

            if (piece != null)
            {
                if (piece.player == playerToCheck)
                {
                    if (piece.type == ChessPieceType.KING)
                    {

                        return true;
                    }
                }
            }
        }
        return false;
    }

    public Position GetKingPos(ChessPlayer player)
    {
        foreach (ChessNode n in GetNodes())
        {
            if (n.pieceOnNode != null)
                if (n.pieceOnNode.type == ChessPieceType.KING && n.pieceOnNode.player == player)
                    return n.pos;
        }
        return new Position(-1, -1);
    }
    /// <summary>
    /// Returns a piece from a position.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public virtual ChessPiece GetPiece(Position pos)
    {
        if (ValidCoordinate(pos))
        {

            if (nodes[pos.x, pos.y].pieceOnNode != null)
            {
                ChessPiece p = nodes[pos.x, pos.y].pieceOnNode;

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
    public virtual ChessBoard SetPiece(Position pos, ChessPiece piece)
    {
        if (ValidCoordinate(pos))
        {
            nodes[pos.x, pos.y].pieceOnNode = piece;
            if (piece != null)
                piece.pos = pos;
        }

        return this;
    }

    /// <summary>
    /// Removes a piece from a position on the map.
    /// </summary>
    /// <param name="pos"></param>
    public virtual void RemovePiece(Position pos)
    {
        if (ValidCoordinate(pos))
        {
            ChessPiece p = GetPiece(pos);
            if (p != null)
            {
                //removed.Add(p);
                nodes[pos.x, pos.y].pieceOnNode = null;
            }
        }
    }
    /// <summary>
    /// Moves a piece on the board.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public ChessBoard Move(Position start, Position end)
    {
        ChessPiece piece = GetPiece(start);
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
    public ChessBoard Move(Move move)
    {
        return Move(move.start, move.end);
    }

    /// <summary>
    /// Returns the state of the board after a move.
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    public virtual ChessBoard BoardAfterMove(Move move)
    {
        ChessBoard board = new ChessBoard(this);
        board.Move(move);
        return board;
    }

    /// <summary>
    /// Returns the other player.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public ChessPlayer OtherPlayer(ChessPlayer player)
    {
        if (player == player1)
            return player2;
        else
            return player1;
    }

    /// <summary>
    /// Returns all possible movements from the pieces of the given player.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public List<Move> GetPossibleMoves(ChessPlayer player)
    {
        List<Move> possibleMoves = new List<Move>();
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Position pos = new Position(i, j);
                ChessPiece current = GetPiece(pos);

                if (current != null)
                {


                    if (current.player == player)
                    {
                        List<Move> moves = null;
                        if (current.type != ChessPieceType.KING)
                            moves = current.GetPossibleMovement();
                        else
                        {
                            King k = current as King;
                            if (k != null)
                                moves = k.GetDefaultMovements();
                        }

                        if (moves != null)
                        {

                            possibleMoves.AddRange(moves);
                        }
                    }
                }
            }
        }
        return possibleMoves;
    }



}
