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
        SetEdges(other.Edges);
    }

    public BoxStruct<Edge> Edges { get; internal set; }

    public void SetEdges(BoxStruct<Edge> newEdges)
    {
        if (newEdges == null)
            return;
        Edges = new BoxStruct<Edge>
        {
            left = new Edge(newEdges.left),
            right = new Edge(newEdges.right),
            top = new Edge(newEdges.top),
            bottom = new Edge(newEdges.bottom)
        };
    }

    public bool UpdateFilled()
    {
        if (Edges == null)
            return (this.filled = false);

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
}
