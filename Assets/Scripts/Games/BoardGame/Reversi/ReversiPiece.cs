using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class ReversiPiece : Piece
{
    public ReversiBoard board;
    public ReversiPiece()
    {

    }
    public ReversiPiece(Position position) : base(position)
    {
    }
    public ReversiPiece(Position position, Player player) : base(position)
    {
        this.player = player;
    }
    public ReversiPiece(Position position, Player player, ReversiBoard board) : this(position, player)
    {

        this.board = board;
    }
    public ReversiPiece(ReversiPiece other) : base(other)
    {
        if (other != null)
        {
            hasMoved = other.hasMoved;
            player = other.player;
            board = other.board;
        }
    }
}
