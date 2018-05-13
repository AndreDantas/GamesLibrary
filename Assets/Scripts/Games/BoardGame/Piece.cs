using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Piece
{
    /// <summary>
    /// The piece's position.
    /// </summary>
    public Position pos;
    /// <summary>
    /// The piece's associated player.
    /// </summary>
    public Player player;

    /// <summary>
    /// If the piece has moved once.
    /// </summary>
    public bool hasMoved = false;

    public Piece()
    {

    }
    public Piece(Position position)
    {
        hasMoved = false;
        pos = position;
    }
    public Piece(Position position, Player player)
    {
        pos = position;
        hasMoved = false;
        this.player = player;
    }
    public Piece(Piece oldPiece)
    {
        if (oldPiece == null)
            return;
        pos = new Position(oldPiece.pos);
        player = oldPiece.player;
    }
    /// <summary>
    /// Checks if the position is valid based on the piece type.
    /// </summary>
    /// <returns></returns>
    public virtual bool ValidPosition(Position pos)
    {
        return false;
    }


}
