using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public abstract class Piece : MonoBehaviour
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
    /// The current board.
    /// </summary>
    public Board board;

    public float moveTime = 0.1f;
    /// <summary>
    /// If the piece is moving.
    /// </summary>
    protected bool isMoving;
    public Piece(Position position)
    {
        pos = position;
    }

    /// <summary>
    /// Checks if the position is valid based on the piece type.
    /// </summary>
    /// <returns></returns>
    public abstract bool ValidPosition(Position pos);

    /// <summary>
    /// Returns a list of the piece's possible movements.
    /// </summary>
    /// <returns></returns>
    public abstract List<Move> GetPossibleMovement();


    public virtual void MovePiece(Vector2 position)
    {
        if (board != null)
            StartCoroutine(Move(position));
    }

    protected virtual IEnumerator Move(Vector2 pos)
    {
        if (isMoving)
            yield break;
        isMoving = true;
        EventHandler onComplete = (object sender, EventArgs args) =>
        {
            isMoving = false;
        };
        transform.MoveTo(pos, moveTime).easingControl.completedEvent += onComplete;
    }
}
