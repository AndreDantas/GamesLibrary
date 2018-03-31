using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DiagonalMovement
{
    public bool canChangeDirection;
    public bool topRight;
    public bool topLeft;
    public bool bottomRight;
    public bool bottomLeft;

    public DiagonalMovement(bool topRight, bool topLeft, bool bottomRight, bool bottomLeft)
    {
        canChangeDirection = false;
        this.topRight = topRight;
        this.topLeft = topLeft;
        this.bottomLeft = bottomLeft;
        this.bottomRight = bottomRight;
    }

    public DiagonalMovement(bool topRight, bool topLeft, bool bottomRight, bool bottomLeft, bool canChangeDirection)
    {
        this.canChangeDirection = canChangeDirection;
        this.topRight = topRight;
        this.topLeft = topLeft;
        this.bottomLeft = bottomLeft;
        this.bottomRight = bottomRight;
    }

    public DiagonalMovement(DiagonalMovement other)
    {
        canChangeDirection = other.canChangeDirection;
        topRight = other.topRight;
        topLeft = other.topLeft;
        bottomRight = other.bottomRight;
        bottomLeft = other.bottomLeft;
    }
}
[System.Serializable]
public class Checker : Piece
{
    public Position startPosition;
    public CheckersBoard board;
    public DiagonalMovement normalMovement = new DiagonalMovement(true, true, false, false, false);
    public DiagonalMovement jumpMovement = new DiagonalMovement(true, true, false, false, true);
    public int moveDistance = 1;
    public bool isKing = false;

    public Checker(Position pos) : base(pos)
    {

    }

    public Checker(Checker other) : base(other)
    {
        if (other != null)
        {
            startPosition = other.startPosition;
            board = other.board;
            hasMoved = other.hasMoved;
            isKing = other.isKing;
            normalMovement = new DiagonalMovement(other.normalMovement);
            jumpMovement = new DiagonalMovement(other.jumpMovement);
        }
    }

    public virtual void BecomeKing()
    {
        isKing = true;
        moveDistance = 99;
        normalMovement = new DiagonalMovement(true, true, true, true, false);
        jumpMovement = new DiagonalMovement(true, true, true, true, true);
    }

    public virtual List<CheckerMove> GetMovements()
    {
        if (isKing)
            return GetKingMovements();

        List<CheckerMove> moves = new List<CheckerMove>();
        List<Position> posCheck = new List<Position>();

        posCheck.Add(pos);

        while (posCheck.Count > 0)
        {

        }

        return moves;
    }

    public virtual List<CheckerMove> GetKingMovements()
    {
        List<CheckerMove> moves = new List<CheckerMove>();


        return moves;
    }
}
