using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct MoveInfo
{
    public ChessPiece piece;
    public Move move;
}
public class ChessBoardgame : Boardgame
{
    public ChessBoard board;
    public GameObject tilePrefab;

    public Color lightTileColor = Color.white;
    public Color darkTileColor = Color.black;
    protected ChessPlayer turnPlayer;
    protected ChessPiece selectedPiece;
    public GameObject pawnPrefab;
    public GameObject rookPrefab;
    public GameObject bishopPrefab;
    public GameObject knightPrefab;
    public GameObject kingPrefab;
    public GameObject queenPrefab;
    public Color lightPieceColor = Color.white;
    public Color darkPieceColor = Color.black;
    public AreaRangeRenderer movementsRender;
    public AreaRangeRenderer checkRender;
    public AreaRangeRenderer lastMoveRender;
    public List<MoveInfo> movesLog;
    public ChessTile[,] tiles { get; internal set; }
    private List<ChessPiece> pieces = new List<ChessPiece>();
    private GameObject tilesParentObj;
    private GameObject piecesParentObj;
    [SerializeField]
    private float tileScale;
    [SerializeField]
    private bool canClick = true;

    public delegate void OnGameEnd();
    public OnGameEnd OnEnd;

    protected void Start()
    {
        //PrepareGame();
    }

    /*
     * Finish extra rules for each piece.
     * Recover lost pieces.
     * Player score.
     * Fix check bug
     */
#if UNITY_EDITOR
    protected void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;


        Vector3 scale = Vector3.one * (6.25f / columns);

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Gizmos.DrawWireCube(new Vector2(i + 0.5f + transform.position.x - columns / 2, j + 0.5f + transform.position.y - rows / 2) * scale.x, scale);
            }

        }

    }
#endif
    public virtual void PrepareGame()
    {

        board = new ChessBoard(columns, rows);
        board.InitBoard();
        board.player1 = new ChessPlayer(Orientation.DOWN);
        board.player2 = new ChessPlayer(Orientation.UP);
        turnPlayer = board.player1;
        RenderMap();
        PlacePieces();
        ClearRenders();
        movesLog = new List<MoveInfo>();
    }
    public override void RenderMap()
    {
        if (columns > 0 && rows > 0)
        {
            if (tilesParentObj != null)
                tilesParentObj.transform.DestroyChildren();
            tilesParentObj = new GameObject("Tiles");
            tilesParentObj.transform.SetParent(transform);
            tilesParentObj.transform.localScale = Vector3.one;
            tilesParentObj.transform.localPosition = Vector3.zero;

            if (piecesParentObj != null)
                piecesParentObj.transform.DestroyChildren();
            piecesParentObj = new GameObject("Pieces");
            piecesParentObj.transform.SetParent(transform);
            piecesParentObj.transform.localScale = Vector3.one;
            piecesParentObj.transform.localPosition = Vector3.zero;

            bool tileColor = false;
            SpriteRenderer sr;
            GameObject tile;

            float width = MathOperations.ScreenWidth;
            transform.localScale = Vector3.one * (width / columns);
            tiles = new ChessTile[columns, rows];
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    // Create tile object.
                    tile = Instantiate(tilePrefab);
                    tile.name = "Tile(" + i + "," + j + ")";

                    // Add Tile component 
                    ChessTile t = tile.AddComponent<ChessTile>();
                    t.pos = new Position(i, j);
                    t.boardGame = this;
                    tiles[i, j] = t;

                    // Change sprite color
                    sr = tile.GetComponent<SpriteRenderer>();
                    if (sr)
                    {
                        sr.color = tileColor ? lightTileColor : darkTileColor;
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

    /// <summary>
    /// Initializes the piece on the board.
    /// </summary>
    /// <param name="obj">The piece's GameObject (The prefab)</param>
    /// <param name="pos">The position on the board</param>
    /// <param name="p">The correspondent piece</param>
    /// <param name="player">The piece's player</param>
    /// <param name="lightPiece">If it's light piece</param>
    public void PlacePiece(GameObject obj, Position pos, ChessPiece p, ChessPlayer player, bool lightPiece = true)
    {
        if (!board.ValidCoordinate(pos))
            return;
        obj.gameObject.transform.SetParent(piecesParentObj.transform);
        tiles[pos.x, pos.y].chessPiece = obj;
        ChessPiece cp = p;
        cp.startPosition = cp.pos = pos;
        cp.board = board;
        cp.player = player;
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = lightPiece ? lightPieceColor : darkPieceColor;
        }
        obj.transform.localPosition = tiles[pos.x, pos.y].transform.localPosition;
        board.nodes[pos.x, pos.y].pieceOnNode = cp;
        pieces.Add(cp);
    }


    public virtual void PlacePieces()
    {
        pieces = new List<ChessPiece>();
        // First Player
        for (int i = 0; i < columns; i++)
        {
            Position pos = new Position(i, 1);
            PlacePiece(Instantiate(pawnPrefab), pos, new Pawn(pos), board.player1);
        }

        PlacePiece(Instantiate(rookPrefab), new Position(0, 0), new Rook(new Position(0, 0)), board.player1);
        PlacePiece(Instantiate(rookPrefab), new Position(7, 0), new Rook(new Position(7, 0)), board.player1);

        PlacePiece(Instantiate(knightPrefab), new Position(1, 0), new Knight(new Position(1, 0)), board.player1);
        PlacePiece(Instantiate(knightPrefab), new Position(6, 0), new Knight(new Position(6, 0)), board.player1);

        PlacePiece(Instantiate(bishopPrefab), new Position(2, 0), new Bishop(new Position(2, 0)), board.player1);
        PlacePiece(Instantiate(bishopPrefab), new Position(5, 0), new Bishop(new Position(5, 0)), board.player1);

        PlacePiece(Instantiate(queenPrefab), new Position(3, 0), new Queen(new Position(3, 0)), board.player1);
        PlacePiece(Instantiate(kingPrefab), new Position(4, 0), new King(new Position(4, 0)), board.player1);


        // Second Player
        for (int i = 0; i < columns; i++)
        {
            Position pos = new Position(i, 6);
            PlacePiece(Instantiate(pawnPrefab), pos, new Pawn(pos), board.player2, false);
        }

        PlacePiece(Instantiate(rookPrefab), new Position(0, 7), new Rook(new Position(0, 7)), board.player2, false);
        PlacePiece(Instantiate(rookPrefab), new Position(7, 7), new Rook(new Position(7, 7)), board.player2, false);

        PlacePiece(Instantiate(knightPrefab), new Position(1, 7), new Knight(new Position(1, 7)), board.player2, false);
        PlacePiece(Instantiate(knightPrefab), new Position(6, 7), new Knight(new Position(6, 7)), board.player2, false);

        PlacePiece(Instantiate(bishopPrefab), new Position(2, 7), new Bishop(new Position(2, 7)), board.player2, false);
        PlacePiece(Instantiate(bishopPrefab), new Position(5, 7), new Bishop(new Position(5, 7)), board.player2, false);

        PlacePiece(Instantiate(queenPrefab), new Position(3, 7), new Queen(new Position(3, 7)), board.player2, false);
        PlacePiece(Instantiate(kingPrefab), new Position(4, 7), new King(new Position(4, 7)), board.player2, false);

    }

    /// <summary>
    /// Moves the piece's gameobject to a position
    /// </summary>
    /// <param name="move"></param>
    public void MovePieceObject(Move move)
    {
        StartCoroutine(MovePieceObj(move));
    }

    IEnumerator MovePieceObj(Move move)
    {
        if (ValidCoordinate(move.end))
        {

            GameObject temp = tiles[move.start.x, move.start.y].chessPiece;
            if (temp != null)
            {
                canClick = false;
                tiles[move.start.x, move.start.y].chessPiece.transform.MoveTo(tiles[move.end.x, move.end.y].transform.position, 0.1f);

                yield return new WaitForSeconds(0.1f);
                tiles[move.start.x, move.start.y].chessPiece = null;
                if (tiles[move.end.x, move.end.y].chessPiece != null)
                    tiles[move.end.x, move.end.y].chessPiece.gameObject.SetActive(false);
                tiles[move.end.x, move.end.y].chessPiece = temp;
                canClick = true;
            }
        }
    }

    IEnumerator MakeAMove(Move move)
    {
        selectedPiece.MoveToPos(move); // Move the piece internally
        yield return MovePieceObj(move); // Move piece object on scene
        MoveInfo m = new MoveInfo();
        m.piece = selectedPiece;
        m.move = move;
        movesLog.Add(m);
        selectedPiece = null;
        movementsRender.Clear();
        ChangeTurn();

    }

    public void StartTurn()
    {
        RenderCheck();
        RenderLastMove();
        if (CheckForCheckmate())
        {

            EndGame();
            return;
        }

    }

    public void ChangeTurn()
    {
        turnPlayer = turnPlayer == board.player1 ? board.player2 : board.player1;
        StartTurn();
    }

    public void EndGame()
    {
        canClick = false;
        if (OnEnd != null)
            OnEnd();
    }

    /// <summary>
    /// Checks for a checkmate.
    /// </summary>
    /// <returns></returns>
    public bool CheckForCheckmate()
    {
        return (ChessPiece.RemoveMovesPlayerInCheck(board.GetPossibleMoves(turnPlayer), board, turnPlayer).Count == 0);
    }

    public void RenderCheck()
    {
        if (checkRender != null)
        {
            if (board.IsPlayerInCheck(turnPlayer))
            {
                Position pos = board.GetKingPos(turnPlayer);
                if (ValidCoordinate(pos))
                {
                    List<Vector3> positions = new List<Vector3>();
                    positions.Add(tiles[pos.x, pos.y].transform.position);
                    checkRender.RenderSquaresArea(positions, tileScale);
                }
            }
            else
            {
                checkRender.Clear();
            }
        }
    }

    public void RenderLastMove()
    {
        if (lastMoveRender)
        {
            if (movesLog != null ? movesLog.Count > 0 : false)
            {
                Move move = movesLog[movesLog.Count - 1].move;
                List<Vector3> pos = new List<Vector3>();
                if (ValidCoordinate(move.start))
                    pos.Add(tiles[move.start.x, move.start.y].transform.position);
                if (ValidCoordinate(move.end))
                    pos.Add(tiles[move.end.x, move.end.y].transform.position);

                lastMoveRender.RenderSquaresArea(pos, tileScale);
            }
            else
                lastMoveRender.Clear();
        }
    }

    public void ClearRenders()
    {
        if (movementsRender)
            movementsRender.Clear();
        if (lastMoveRender)
            lastMoveRender.Clear();
        if (checkRender)
            checkRender.Clear();


    }

    /// <summary>
    /// When a tile is clicked.
    /// </summary>
    /// <param name="pos"></param>
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
            List<Move> moves = ChessPiece.RemoveMovesPlayerInCheck(selectedPiece.GetPossibleMovement(), board, selectedPiece.player as ChessPlayer);
            foreach (var item in moves)
            {
                if (pos == item.end)
                {
                    StartCoroutine(MakeAMove(item));
                    return;
                }
            }


        }

        ChessPiece piece = board.GetPiece(pos);


        if (piece != null) // Piece on tile
        {
            if (piece.player != turnPlayer) // Opponent piece
            {
                movementsRender.Clear();
            }
            else // Player piece
            {

                List<Vector3> moves = new List<Vector3>();
                List<Move> possibleMoves = ChessPiece.RemoveMovesPlayerInCheck(piece.GetPossibleMovement(), board, piece.player as ChessPlayer);
                if (possibleMoves != null ? possibleMoves.Count != 0 : false)
                {
                    foreach (Move m in possibleMoves)
                    {
                        if (ValidCoordinate(m.end))
                        {
                            moves.Add(tiles[m.end.x, m.end.y].transform.position);
                        }
                    }
                    movementsRender.RenderSquaresArea(moves, tileScale);
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
            movementsRender.Clear();
        }
    }
}
