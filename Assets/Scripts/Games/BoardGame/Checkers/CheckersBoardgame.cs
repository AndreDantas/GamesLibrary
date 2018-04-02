using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make continuation of capture if there are more jumps. Check for more after moving the piece object
// Check for out of moves
// Make promotion
// 
public class CheckersBoardgame : Boardgame
{
    public GameObject tilePrefab;
    public Color lightTile = Color.white;
    public Color darkTile = Color.cyan;
    public GameObject checkerPrefab;
    public GameObject checkerKingPrefab;
    public Color topPlayerColor = Color.red;
    public Color bottomPlayerColor = Color.black;
    public CheckersBoard board;
    public CheckerPlayer turnPlayer { get; internal set; }
    protected Checker selectedPiece;
    public AreaRangeRenderer movementsRender;
    public bool canClick = true;
    private float tileRenderScale = 0.89f;
    private GameObject tilesParentObj;
    private GameObject piecesParentObj;
    private GameObject player1PiecesParent;
    private GameObject player2PiecesParent;
    public CheckerTile[,] tiles { get; internal set; }
    private bool capturing;
    private void Start()
    {
        PrepareGame();
    }
    private void OnValidate()
    {
        ChangePiecesColor(true);
        ChangePiecesColor(false);
        ChangeTileColor();
    }
    public void PrepareGame()
    {
        board = new CheckersBoard();
        board.columns = board.rows = 8;
        (board.playerTop = new CheckerPlayer()).orientation = Orientation.UP;
        (board.playerBottom = new CheckerPlayer()).orientation = Orientation.DOWN;
        turnPlayer = board.playerBottom;
        board.InitBoard();
        RenderMap();
        PlacePieces();
        canClick = true;

    }

    public void PlacePieces()
    {
        for (int i = 0; i < 3; i++)
        {
            bool oddRow = i % 2 == 0;
            for (int j = 0; j < 8; j += 2)
            {
                Position pos = new Position(oddRow ? j : j + 1, i);
                Checker c = new Checker(pos);
                c.player = board.playerBottom;
                GeneratePiece(c, pos);
            }
        }

        for (int i = 7; i > 4; i--)
        {
            bool oddRow = i % 2 == 0;
            for (int j = 0; j < 8; j += 2)
            {
                Position pos = new Position(oddRow ? j : j + 1, i);
                Checker c = new Checker(pos);
                c.normalMovement = new DiagonalMovement(false, false, true, true);
                c.jumpMovement = new DiagonalMovement(false, false, true, true);
                c.player = board.playerTop;
                GeneratePiece(c, pos, true);
            }
        }
    }

    public void GeneratePiece(Checker piece, Position pos, bool isTopPlayer = false)
    {
        if (piece == null || board == null || tiles == null)
            return;
        piece.board = board;
        board.SetPiece(pos, piece);
        GameObject pieceObj = Instantiate(piece.isKing ? checkerKingPrefab : checkerPrefab);

        SpriteRenderer sr = pieceObj.GetComponent<SpriteRenderer>();
        if (sr)
        {
            sr.color = isTopPlayer ? topPlayerColor : bottomPlayerColor;
        }

        pieceObj.transform.SetParent(isTopPlayer ? player2PiecesParent.transform : player1PiecesParent.transform);
        pieceObj.transform.localScale = Vector3.one;
        pieceObj.transform.localPosition = tiles[pos.x, pos.y].transform.localPosition;
        tiles[pos.x, pos.y].checkerPiece = pieceObj;
    }

    public override void RenderMap()
    {
        if (columns > 0 && rows > 0)
        {
            if (tilesParentObj != null)
                tilesParentObj.transform.DestroyChildren();
            Destroy(tilesParentObj);
            tilesParentObj = new GameObject("Tiles");
            tilesParentObj.transform.SetParent(transform);
            tilesParentObj.transform.localScale = Vector3.one;
            tilesParentObj.transform.localPosition = Vector3.zero;

            if (piecesParentObj != null)
                piecesParentObj.transform.DestroyChildren();
            Destroy(piecesParentObj);
            piecesParentObj = new GameObject("Pieces");
            piecesParentObj.transform.SetParent(transform);
            piecesParentObj.transform.localScale = Vector3.one;
            piecesParentObj.transform.localPosition = Vector3.zero;


            player1PiecesParent = new GameObject("Player1 Pieces");
            player1PiecesParent.transform.SetParent(piecesParentObj.transform);
            player1PiecesParent.transform.localScale = Vector3.one;
            player1PiecesParent.transform.localPosition = Vector3.zero;
            player2PiecesParent = new GameObject("Player2 Pieces");
            player2PiecesParent.transform.SetParent(piecesParentObj.transform);
            player2PiecesParent.transform.localScale = Vector3.one;
            player2PiecesParent.transform.localPosition = Vector3.zero;

            bool tileColor = false;
            SpriteRenderer sr;
            GameObject tile;

            float width = MathOperations.ScreenWidth;
            tileRenderScale = width / columns;
            transform.localScale = Vector3.one * (width / columns);
            tiles = new CheckerTile[columns, rows];
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    // Create tile object.
                    tile = Instantiate(tilePrefab);
                    tile.name = "Tile(" + i + "," + j + ")";

                    // Add Tile component 
                    CheckerTile t = tile.AddComponent<CheckerTile>();
                    t.pos = new Position(i, j);
                    t.boardGame = this;
                    tiles[i, j] = t;

                    // Change sprite color
                    sr = tile.GetComponent<SpriteRenderer>();
                    if (sr)
                    {
                        sr.color = tileColor ? lightTile : darkTile;
                    }

                    // Set tile's position
                    tile.transform.SetParent(tilesParentObj.transform);
                    tile.transform.localScale = Vector3.one;
                    tile.transform.localPosition = new Vector3(i + 0.5f - columns / 2, j + 0.5f - rows / 2, tilesParentObj.transform.localPosition.z);

                    if (j < rows - 1)
                        tileColor = !tileColor;
                }
            }
        }
    }

    public bool ValidCoordinate(Position pos)
    {
        int x = pos.x;
        int y = pos.y;

        if (x < 0 || x >= columns)
            return false;
        if (y < 0 || y >= rows)
            return false;

        return true;
    }

    public void ChangePiecesColor(bool topPlayer = true)
    {
        if (player1PiecesParent != null && player2PiecesParent != null)
            foreach (Transform t in topPlayer ? player2PiecesParent.transform : player1PiecesParent.transform)
            {
                SpriteRenderer sr = t.gameObject.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = topPlayer ? topPlayerColor : bottomPlayerColor;
                }
            }
    }

    public void MovePiece(Checker piece, List<CheckerMove> moves, Position positionClicked)
    {
        if (piece == null)
            return;
        if (moves == null ? true : moves.Count == 0)
            return;


        if (!moves[0].isCapture)
        {
            CheckerMove move = moves.Find(x => x.end == positionClicked);
            board.Move(move);
            MovePieceObject(move);
            selectedPiece = null;
            ChangeTurn();
        }
        else
        {
            List<CheckerMove> movements = new List<CheckerMove>();
            CheckerMove current = moves.Find(x => x.end == positionClicked);
            movements.Add(current);
            while (current.start != piece.pos)
            {
                current = current.previous;
                movements.Add(current);
            }
            movements.Reverse();
            board.CaptureMovement(movements);
            MovePieceObjectCapture(movements);

            // REMOVE AFTER CAPTURE IS FULLY IMPLEMENTED.
            selectedPiece = null;
            ChangeTurn();
            ////////////////////////////////////////////
        }

    }
    /// <summary>
    /// Moves the piece's gameobject to a position
    /// </summary>
    /// <param name="move"></param>
    public void MovePieceObject(CheckerMove move)
    {
        StartCoroutine(MovePieceObj(move));
    }
    IEnumerator MovePieceObj(CheckerMove move)
    {
        if (ValidCoordinate(move.end))
        {

            GameObject temp = tiles[move.start.x, move.start.y].checkerPiece;
            if (temp != null)
            {
                canClick = false;
                tiles[move.start.x, move.start.y].checkerPiece.transform.MoveTo(tiles[move.end.x, move.end.y].transform.position, 0.1f);

                yield return new WaitForSeconds(0.1f);
                tiles[move.start.x, move.start.y].checkerPiece = null;
                tiles[move.end.x, move.end.y].checkerPiece = temp;
                canClick = true;
            }
        }
    }

    public void MovePieceObjectCapture(List<CheckerMove> moves)
    {
        StartCoroutine(MovePieceObjCapture(moves));
    }

    IEnumerator MovePieceObjCapture(List<CheckerMove> moves)
    {
        canClick = false;
        foreach (CheckerMove move in moves)
            if (ValidCoordinate(move.end))
            {

                GameObject temp = tiles[move.start.x, move.start.y].checkerPiece;
                if (temp != null)
                {

                    tiles[move.start.x, move.start.y].checkerPiece.transform.MoveTo(tiles[move.end.x, move.end.y].transform.position, 0.1f);

                    yield return new WaitForSeconds(0.1f);
                    tiles[move.start.x, move.start.y].checkerPiece = null;
                    Position delta = new Position(MathOperations.Sign(move.end.x - move.start.x), MathOperations.Sign(move.end.y - move.start.y));
                    Position captured = move.end - delta;
                    if (tiles[captured.x, captured.y].checkerPiece != null)
                    {
                        Destroy(tiles[captured.x, captured.y].checkerPiece);
                        tiles[captured.x, captured.y].checkerPiece = null;
                    }
                    tiles[move.end.x, move.end.y].checkerPiece = temp;
                    yield return new WaitForSeconds(0.2f);

                }
            }
        canClick = true;
    }
    public bool CheckForDefeat()
    {
        return false;

    }
    public void StartTurn()
    {

        if (CheckForDefeat())
        {
            EndGame();
            return;
        }

    }
    public void EndGame()
    {
        canClick = false;

    }
    public void ChangeTurn()
    {

        turnPlayer = turnPlayer == board.playerTop ? board.playerBottom : board.playerTop;
        StartTurn();
    }
    public void ChangeTileColor()
    {
        if (tiles != null ? tiles.GetLength(0) > 0 && tiles.GetLength(1) > 0 : false)
        {
            bool tileColor = false;
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    SpriteRenderer sr = tiles[i, j].GetComponent<SpriteRenderer>();
                    if (sr)
                    {
                        sr.color = tileColor ? lightTile : darkTile;
                    }

                    if (j < rows - 1)
                        tileColor = !tileColor;
                }
            }
        }
    }
    public void OnClick(Position pos)
    {
        if (!canClick)
        {
            movementsRender.Clear();
            return;
        }
        if (!board.ValidCoordinate(pos))
            return;

        if (selectedPiece != null)
        {
            List<CheckerMove> moves = selectedPiece.GetMovements();
            foreach (var item in moves)
            {
                if (pos == item.end)
                {
                    MovePiece(selectedPiece, moves, pos);
                }
            }


        }

        Checker piece = board.GetPiece(pos);


        if (piece != null) // Piece on tile
        {
            if (piece.player != turnPlayer) // Opponent piece
            {
                selectedPiece = null;
                movementsRender.Clear();
            }
            else // Player piece
            {

                List<Vector3> moves = new List<Vector3>();
                List<CheckerMove> possibleMoves = piece.GetMovements();
                if (possibleMoves != null ? possibleMoves.Count != 0 : false)
                {
                    foreach (Move m in possibleMoves)
                    {
                        if (ValidCoordinate(m.end))
                        {
                            moves.Add(tiles[m.end.x, m.end.y].transform.position);
                        }
                    }
                    movementsRender.RenderSquaresArea(moves, tileRenderScale);
                }
                else
                {
                    movementsRender.Clear();
                }

                selectedPiece = piece;
            }


        }
        else
        {
            selectedPiece = null;
            movementsRender.Clear();
        }
    }
}
