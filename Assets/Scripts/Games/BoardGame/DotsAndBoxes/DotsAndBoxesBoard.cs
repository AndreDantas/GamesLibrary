using System.Collections;
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
        for (int i = 0; i < oldBoard.nodes.GetLength(0); i++)
        {
            for (int j = 0; j < oldBoard.nodes.GetLength(1); j++)
            {
                if (oldBoard.nodes[i, j] != null)
                    n[i, j] = new DotsAndBoxesNode(oldBoard.nodes[i, j]);
            }
        }
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

        this.nodes = n;
        player1 = oldBoard.player1;
        player2 = oldBoard.player2;
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
        edgesX = new Edge[columns + 1, rows];
        edgesY = new Edge[columns, rows + 1];
        nodes = new DotsAndBoxesNode[columns, rows];
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Box b = new Box(new Position(i, j));

                SetBoxEdges(ref b);

                DotsAndBoxesNode node = new DotsAndBoxesNode(new Position(i, j), b);

                nodes[i, j] = node;

            }
        }
        isInit = true;
    }

    void SetBoxEdges(ref Box box)
    {
        if (!isInit)
            return;

        int x = box.pos.x;
        int y = box.pos.y;

        //Left Edge

        if (ValidCoordinate(x - 1, y) ? nodes[x - 1, y] == null : true)
        {
            box.Edges.left = new Edge(EdgePosition.Horizontal, new Position(x, y));
            edgesX[x, y] = box.Edges.left;
        }
        else
        {
            box.Edges.left = new Edge(nodes[x - 1, y].box?.Edges?.right);
        }

        //Right Edge
        if (ValidCoordinate(x + 1, y) ? nodes[x + 1, y] == null : true)
        {
            box.Edges.right = new Edge(EdgePosition.Horizontal, new Position(x + 1, y));
            edgesX[x + 1, y] = box.Edges.right;
        }
        else
        {
            box.Edges.right = new Edge(nodes[x + 1, y].box?.Edges?.left);
        }

        //Top Edge
        if (ValidCoordinate(x, y + 1) ? nodes[x, y + 1] == null : true)
        {
            box.Edges.top = new Edge(EdgePosition.Vertical, new Position(x, y + 1));
            edgesY[x, y + 1] = box.Edges.top;
        }
        else
        {
            box.Edges.top = new Edge(nodes[x, y + 1].box?.Edges?.bottom);
        }

        //Bottom Edge
        if (ValidCoordinate(x, y - 1) ? nodes[x, y - 1] == null : true)
        {
            box.Edges.bottom = new Edge(EdgePosition.Vertical, new Position(x, y));
            edgesY[x, y] = box.Edges.bottom;
        }
        else
        {
            box.Edges.bottom = new Edge(nodes[x, y - 1].box?.Edges?.top);
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
}
