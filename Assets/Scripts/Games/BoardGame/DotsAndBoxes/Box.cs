using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class Box
{
    public bool filled = false;
    public Player owner;
    public Position pos;

    public Box(Position pos)
    {
        this.pos = pos;
    }

    public Box(Box other)
    {
        this.filled = other.filled;
        owner = other.owner;
        pos = other.pos;
        Edges = new BoxStruct<Edge>
        {
            left = new Edge(other.Edges.left),
            right = new Edge(other.Edges.right),
            top = new Edge(other.Edges.top),
            bottom = new Edge(other.Edges.bottom)
        };
    }

    public BoxStruct<Edge> Edges;

    public void SetEdges(BoxStruct<Edge> newEdges)
    {

        Edges = new BoxStruct<Edge>
        {
            left = new Edge(newEdges.left),
            right = new Edge(newEdges.right),
            top = new Edge(newEdges.top),
            bottom = new Edge(newEdges.bottom)
        };
    }

    public int CompletedEdgesCount()
    {
        int value = 0;
        if (Edges.left.active)
            value++;
        if (Edges.right.active)
            value++;
        if (Edges.top.active)
            value++;
        if (Edges.bottom.active)
            value++;

        return value;
    }

    public bool UpdateFilled()
    {

        bool filled = true;

        Edge[] edges = Edges.GetValues();

        for (int i = 0; i < edges.Length; i++)
        {
            if (edges[i] != null ? !edges[i].active : true)
                filled = false;

        }

        this.filled = filled;
        return filled;
    }

    public override string ToString()
    {
        return "Filled: " + filled + " - Position: " + pos + " - Owner: " + owner?.name + "\n" + Edges;
    }
}
