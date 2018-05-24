using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class Edge
{
    public bool active;
    public EdgePosition orientation;
    public Position pos;
    public Player owner;
    public Edge()
    {
        active = false;
    }

    public Edge(bool active)
    {
        this.active = active;
    }
    public Edge(bool active, EdgePosition orientation, Position pos)
    {
        this.active = active;
        this.orientation = orientation;
        this.pos = pos;
    }
    public Edge(EdgePosition orientation, Position pos)
    {
        this.active = false;
        this.orientation = orientation;
        this.pos = pos;
    }
    public Edge(bool active, EdgePosition orientation)
    {
        this.active = active;
        this.orientation = orientation;
    }
    public Edge(EdgePosition orientation)
    {
        this.active = false;
        this.orientation = orientation;
    }
    public Edge(Edge other)
    {
        active = other.active;
        orientation = other.orientation;
        pos = other.pos;
        owner = other.owner;
    }

    public override string ToString()
    {
        return "Orientation: " + orientation + " - Pos: " + pos + " - Active: " + active + " - Owner: " + owner?.name;
    }
}
