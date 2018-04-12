using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DiagonalMovement
{
    public bool topRight;
    public bool topLeft;
    public bool bottomRight;
    public bool bottomLeft;

    public DiagonalMovement(bool topRight, bool topLeft, bool bottomRight, bool bottomLeft)
    {
        this.topRight = topRight;
        this.topLeft = topLeft;
        this.bottomLeft = bottomLeft;
        this.bottomRight = bottomRight;
    }


    public DiagonalMovement(DiagonalMovement other)
    {

        topRight = other.topRight;
        topLeft = other.topLeft;
        bottomRight = other.bottomRight;
        bottomLeft = other.bottomLeft;
    }

    public DiagonalMovement(bool value)
    {
        topLeft = topRight = bottomLeft = bottomRight = value;
    }
}
[System.Serializable]
public class Checker : Piece
{
    public Position startPosition;
    public CheckersBoard board;
    public DiagonalMovement normalMovement = new DiagonalMovement(true, true, false, false);
    public DiagonalMovement jumpMovement = new DiagonalMovement(true, true, false, false);
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


    public virtual List<CheckerMove> GetMovements()
    {
        if (board == null)
            return null;
        List<CheckerMove> moves = new List<CheckerMove>();
        List<CheckerMove> jumpMoves = new List<CheckerMove>();

        bool hasJump = false;

        for (int i = 0; i < 4; i++)
        {
            bool normalMovement = false;
            bool jumpMovement = false;
            int dx = 0;
            int dy = 0;
            switch (i)
            {
                case 0: //Top Left
                    normalMovement = this.normalMovement.topLeft;
                    jumpMovement = this.jumpMovement.topLeft;
                    dx = -1;
                    dy = 1;
                    break;
                case 1: //Top Right
                    normalMovement = this.normalMovement.topRight;
                    jumpMovement = this.jumpMovement.topRight;
                    dx = 1;
                    dy = 1;
                    break;
                case 2: //Bottom Left
                    normalMovement = this.normalMovement.bottomLeft;
                    jumpMovement = this.jumpMovement.bottomLeft;
                    dx = -1;
                    dy = -1;
                    break;
                case 3: //Bottom Right
                    normalMovement = this.normalMovement.bottomRight;
                    jumpMovement = this.jumpMovement.bottomRight;
                    dx = 1;
                    dy = -1;
                    break;
            }
            if (normalMovement || jumpMovement)
                for (int j = 0; j < moveDistance; j++)
                {
                    Position checkPos = new Position(pos.x + (j + 1) * UtilityFunctions.Sign(dx), pos.y + (j + 1) * UtilityFunctions.Sign(dy));
                    // Movement blocked
                    if (!board.IsPositionEmpty(checkPos))
                    {

                        // Enemy piece in the way
                        if (board.GetPiece(checkPos) != null ? board.nodes[checkPos.x, checkPos.y].checkerOnNode.player != player : false)
                        {
                            // If is possible to jump on that direction
                            if (jumpMovement)
                            {
                                // Get jump direction
                                Position newPos = new Position(checkPos.x + dx, checkPos.y + dy);

                                // Check if jump is possible
                                if (board.IsPositionEmpty(newPos))
                                {
                                    jumpMoves.Add(new CheckerMove(pos, newPos, true));
                                    hasJump = true;
                                }
                            }

                        }

                        break;
                    }
                    else if (normalMovement)
                        moves.Add(new CheckerMove(pos, checkPos));
                }
        }


        if (hasJump) // If there was a jump
        {
            return GetPossibleJumps(jumpMoves);
        }
        else
        {
            return moves;
        }

    }

    public List<CheckerMove> GetPossibleJumps(List<CheckerMove> jumpMoves)
    {
        List<CheckerMove> movesChecked = new List<CheckerMove>();
        List<Position> jumpedPositions = new List<Position>();

        while (jumpMoves.Count > 0)
        {
            CheckerMove current = jumpMoves[0];
            movesChecked.Add(current);
            Position delta = new Position(UtilityFunctions.Sign(current.end.x - current.start.x), UtilityFunctions.Sign(current.end.y - current.start.y));
            if (!jumpedPositions.Exists(x => x == current.end - delta))
                jumpedPositions.Add(current.end - delta);
            jumpMoves.Remove(current);
            foreach (CheckerMove m in CheckForNextJump(current.end))
            {

                Position checkJump = new Position(((m.end.x + m.start.x) / 2), ((m.end.y + m.start.y) / 2));

                if (movesChecked.Exists(x => x.start == m.start && x.end == m.end) || jumpedPositions.Exists(x => x == checkJump))
                    continue;
                m.previous = current;
                current.next = m;
                jumpMoves.Add(m);
                jumpedPositions.Add(checkJump);

            }
        }
        return movesChecked;
    }

    public bool HasCapture()
    {
        List<CheckerMove> moves = GetMovements();
        if (moves != null ? moves.Count > 0 : false)
            foreach (var item in moves)
            {
                if (item.isCapture)
                {
                    return true;
                }
            }
        return false;
    }

    /// <summary>
    /// Used to find other jumps
    /// </summary>
    /// <param name="currentPosition">The current position to check</param>
    /// <returns></returns>
    public virtual List<CheckerMove> CheckForNextJump(Position currentPosition)
    {
        List<CheckerMove> jumpMoves = new List<CheckerMove>();
        for (int i = 0; i < 4; i++)
        {
            bool jumpMovement = false;
            int dx = 0;
            int dy = 0;
            switch (i)
            {
                case 0: //Top Left
                    jumpMovement = this.jumpMovement.topLeft;
                    dx = -1;
                    dy = 1;
                    break;
                case 1: //Top Right
                    jumpMovement = this.jumpMovement.topRight;
                    dx = 1;
                    dy = 1;
                    break;
                case 2: //Bottom Left
                    jumpMovement = this.jumpMovement.bottomLeft;
                    dx = -1;
                    dy = -1;
                    break;
                case 3: //Bottom Right
                    jumpMovement = this.jumpMovement.bottomRight;
                    dx = 1;
                    dy = -1;
                    break;
            }
            if (jumpMovement)
            {
                Position checkPos = new Position(currentPosition.x + dx, currentPosition.y + dy);


                if (!board.IsPositionEmpty(checkPos))
                {

                    // Enemy piece in the way
                    if (board.GetPiece(checkPos) != null ? board.GetPiece(checkPos).player != player : false)
                    {
                        // Get jump direction
                        Position newPos = new Position(checkPos.x + dx, checkPos.y + dy);

                        // Check if jump is possible
                        if (board.IsPositionEmpty(newPos))
                        {
                            CheckerMove move = new CheckerMove(currentPosition, newPos, true);

                            jumpMoves.Add(move);
                        }
                    }

                }
            }
        }


        return jumpMoves;
    }

    public override string ToString()
    {
        return "Checker: Pos" + pos + " - IsKing: " + isKing;
    }

}
