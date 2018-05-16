using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class Connect4Node : Node
{

    public Piece pieceOnNode;
    public Connect4Node(Position position) : base(position)
    {
    }

    public Connect4Node(Connect4Node other) : base(other)
    {
        if (other.pieceOnNode != null)
            pieceOnNode = new Piece(other.pieceOnNode);
        else
            pieceOnNode = null;
    }


}
