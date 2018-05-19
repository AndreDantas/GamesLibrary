using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class BoxTile : MonoBehaviour
{
    public Position pos;
    public BoxStruct<EdgeObject> Edges;

    public void SetEdges(BoxStruct<EdgeObject> newEdges)
    {
        if (newEdges == null)
            return;
        Edges = new BoxStruct<EdgeObject>();
        Edges.left = newEdges.left;
        Edges.right = newEdges.right;
        Edges.top = newEdges.top;
        Edges.bottom = newEdges.bottom;
    }


}
