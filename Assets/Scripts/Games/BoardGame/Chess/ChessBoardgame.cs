using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
[Serializable]
public struct ChessMoveInfo
{
    public ChessPiece piece;
    public Move move;
    public ChessBoard boardAfterMove;
}

[Serializable]
class ChessBoardSaveData
{
    public ChessBoard board;
    public List<ChessMoveInfo> movesLog;
    public ChessPlayer turnPlayer;
}
public class ChessBoardgame : Boardgame
{
    public ChessBoard board;
    [Space]
    [Header("Tiles")]
    public GameObject tilePrefab;
    public Color lightTileColor = Color.white;
    public Color darkTileColor = Color.black;
    [SerializeField]
    private float tileRenderScale = 0.89f;
    public ChessPlayer turnPlayer { get; internal set; }
    protected ChessPiece selectedPiece;
    [Space(10)]
    [Header("Pieces")]
    public GameObject pawnPrefab;
    public GameObject rookPrefab;
    public GameObject bishopPrefab;
    public GameObject knightPrefab;
    public GameObject kingPrefab;
    public GameObject queenPrefab;
    public Color lightPieceColor = Color.white;
    public Color darkPieceColor = Color.black;
    public bool darkPiecesFlipped;
    [Space(10)]
    public AudioClip pieceMovement;
    [Space(10)]
    [Header("Renders")]
    public AreaRangeRenderer movementsRender;
    public AreaRangeRenderer checkRender;
    public AreaRangeRenderer lastMoveRender;
    [Space(10)]
    public TextMeshProUGUI victoryMsg;
    public GameObject promotionObj;

    [ReadOnly]
    public List<ChessMoveInfo> movesLog;
    public ChessTile[,] tiles { get; internal set; }
    private List<ChessPiece> pieces = new List<ChessPiece>();

    private GameObject tilesParentObj;
    private GameObject piecesParentObj;
    private GameObject darkPiecesParent;
    private GameObject lightPiecesParent;
    private GameObject indicatorParent;
    public bool canClick = true;
    private bool waitingPromotion = false;
    private Position promotionPos;

    public delegate void OnGameEnd();
    public OnGameEnd OnEnd;

    protected void Start()
    {
        gameObject.AddAudio(pieceMovement);
        //PrepareGame();
    }

    private void OnEnable()
    {
        if (promotionObj != null)
            promotionObj.SetActive(false);
    }

    private void OnValidate()
    {
        ChangePiecesColor(true);
        ChangePiecesColor(false);
        ChangeTileColor();
    }

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

    public virtual void PrepareGame1vs1()
    {
        waitingPromotion = false;
        board = new ChessBoard(columns, rows);
        board.InitBoard();
        board.player1 = new ChessPlayer(Orientation.DOWN);
        board.player2 = new ChessPlayer(Orientation.UP);
        turnPlayer = board.player1;
        RenderMap();
        PlacePieces();
        ClearRenders();
        movesLog = new List<ChessMoveInfo>();
        canClick = true;
        FlipDarkSidePieces(darkPiecesFlipped);
        StartTurn();
    }
    public void ConfirmRestartMatch()
    {
        ModalWindow.Choice("Reiniciar jogo?", PrepareGame1vs1);
    }

    public void ToggleMuteGame()
    {
        gameObject.ToggleMute();
    }

    public void ToggleFlipDarkSidesPieces()
    {
        FlipDarkSidePieces(!darkPiecesFlipped);
    }
    public void FlipDarkSidePieces(bool flip)
    {
        if (darkPiecesParent == null)
            return;
        foreach (Transform g in darkPiecesParent.transform)
        {
            SpriteRenderer sr = g.gameObject.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.flipX = flip;
                sr.flipY = flip;
            }
        }

        darkPiecesFlipped = flip;
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


            darkPiecesParent = new GameObject("Dark Pieces");
            darkPiecesParent.transform.SetParent(piecesParentObj.transform);
            darkPiecesParent.transform.localScale = Vector3.one;
            darkPiecesParent.transform.localPosition = Vector3.zero;
            lightPiecesParent = new GameObject("Light Pieces");
            lightPiecesParent.transform.SetParent(piecesParentObj.transform);
            lightPiecesParent.transform.localScale = Vector3.one;
            lightPiecesParent.transform.localPosition = Vector3.zero;


            if (!indicatorParent)
                indicatorParent = new GameObject("Indicator");
            indicatorParent.transform.SetParent(transform.parent);
            indicatorParent.transform.localScale = Vector3.one;
            indicatorParent.transform.localPosition = Vector3.zero;

            bool tileColor = false;
            SpriteRenderer sr;
            GameObject tile;

            float width = UtilityFunctions.ScreenWidth;
            tileRenderScale = width / columns;
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

    public void SaveBoardState()
    {
        if (board == null)
            return;

        ChessBoardSaveData save = new ChessBoardSaveData();
        save.board = board;
        save.movesLog = movesLog;
        save.turnPlayer = turnPlayer;
        SaveLoad.SaveFile("/chess_game1v1_data.dat", save);
    }

    public void LoadBoardState()
    {
        ChessBoardSaveData load = SaveLoad.LoadFile<ChessBoardSaveData>("/chess_game1v1_data.dat");
        if (load != null ? load.board != null : false)
        {
            ReconstructBoard(load);
        }
        else
            ModalWindow.Message("Sem jogos salvos.");
    }
    public void ConfirmBoardLoad()
    {
        ModalWindow.Choice("Carregar jogo salvo?", LoadBoardState);
    }

    void ReconstructBoard(ChessBoardSaveData data, bool playerVsplayer = true)
    {
        ClearRenders();
        if (data.board != null)
        {
            board = data.board;
            movesLog = data.movesLog;
            turnPlayer = data.turnPlayer;
            RenderMap();

            foreach (ChessNode node in data.board.GetNodes())
            {
                if (node.pieceOnNode == null)
                    continue;
                GameObject obj = Instantiate(GetPieceObjectFromType(node.pieceOnNode.type));
                obj.transform.SetParent(node.pieceOnNode.player == board.player1 ? lightPiecesParent.transform : darkPiecesParent.transform);
                obj.transform.localPosition = tiles[node.pos.x, node.pos.y].transform.localPosition;
                //obj.transform.localScale = Vector3.one;
                tiles[node.pos.x, node.pos.y].chessPiece = obj;

                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    if (node.pieceOnNode.player == board.player1)
                    {
                        sr.color = lightPieceColor;

                    }
                    else
                    {
                        sr.color = darkPieceColor;
                    }

                }

            }
            FlipDarkSidePieces(darkPiecesFlipped);
            StartTurn();
            canClick = true;
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
        obj.gameObject.transform.SetParent(lightPiece ? lightPiecesParent.transform : darkPiecesParent.transform);
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

    public void ChangePiecesColor(bool lightPiece = true)
    {
        if (lightPiecesParent != null && darkPiecesParent != null)
            foreach (Transform t in lightPiece ? lightPiecesParent.transform : darkPiecesParent.transform)
            {
                SpriteRenderer sr = t.gameObject.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = lightPiece ? lightPieceColor : darkPieceColor;
                }
            }
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
                        sr.color = tileColor ? lightTileColor : darkTileColor;
                    }

                    if (j < rows - 1)
                        tileColor = !tileColor;
                }
            }
        }
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

    public GameObject GetPieceObjectFromType(ChessPieceType type)
    {
        switch (type)
        {
            case ChessPieceType.PAWN:
                return pawnPrefab;
            case ChessPieceType.ROOK:
                return rookPrefab;
            case ChessPieceType.BISHOP:
                return bishopPrefab;
            case ChessPieceType.KNIGHT:
                return knightPrefab;
            case ChessPieceType.QUEEN:
                return queenPrefab;
            case ChessPieceType.KING:
                return kingPrefab;
        }
        return null;
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
                gameObject.PlayAudio(pieceMovement);
                tiles[move.start.x, move.start.y].chessPiece = null;
                if (tiles[move.end.x, move.end.y].chessPiece != null)
                {
                    Destroy(tiles[move.end.x, move.end.y].chessPiece);
                }
                tiles[move.end.x, move.end.y].chessPiece = temp;
                canClick = true;
            }
        }
    }

    IEnumerator MakeAMove(Move move)
    {
        selectedPiece.MoveToPos(move); // Move the piece internally
        yield return MovePieceObj(move); // Move piece object on scene
        ChessMoveInfo m = new ChessMoveInfo();
        m.piece = selectedPiece;
        m.move = move;
        m.boardAfterMove = new ChessBoard(board);
        movesLog.Add(m);
        selectedPiece = null;
        movementsRender.Clear();
        ChangeTurn();

    }

    void IndicateTurnPlayer(int orientation)
    {
        if (!playerTurnIndicator)
            return;
        playerTurnIndicator.SetActive(true);
        playerTurnIndicator.transform.SetParent(indicatorParent.transform);
        playerTurnIndicator.transform.localScale = new Vector3(indicatorScale, indicatorScale, 1f);

        SpriteRenderer sr = playerTurnIndicator.GetComponent<SpriteRenderer>();
        if (sr)
        {
            sr.color = Mathf.Sign(orientation) >= 1 ? darkPieceColor : lightPieceColor;
        }
        playerTurnIndicator.transform.localPosition = new Vector3(0f, (rows * tileRenderScale / 2f + playerTurnIndicator.transform.localScale.y / 2f) * Mathf.Sign(orientation) + transform.position.y, 0f);
        playerTurnIndicator.transform.eulerAngles = new Vector3(0, 0, 180 * (Mathf.Sign(orientation) >= 1 ? 0 : 1));

        if (playerTurnBorder)
        {
            playerTurnBorder.SetActive(true);
            playerTurnBorder.transform.SetParent(indicatorParent.transform);
            playerTurnBorder.transform.localScale = new Vector3(columns * tileRenderScale, indicatorScale, 1f);
            playerTurnBorder.transform.localPosition = new Vector3(0f, (rows * tileRenderScale / 2f + playerTurnBorder.transform.localScale.y / 2f) * Mathf.Sign(orientation) + transform.position.y, 0f);
            playerTurnBorder.transform.eulerAngles = new Vector3(0, 0, 180 * (Mathf.Sign(orientation) >= 1 ? 0 : 1));

            sr = playerTurnBorder.GetComponent<SpriteRenderer>();

            if (sr)
            {
                sr.color = Mathf.Sign(orientation) >= 1 ? darkPieceColor : lightPieceColor;
            }
        }


    }

    public void StartTurn()
    {
        RenderCheck();
        RenderLastMove();

        if (victoryMsg)
            victoryMsg.gameObject.SetActive(false);
        IndicateTurnPlayer(turnPlayer.orientation == Orientation.DOWN ? -1 : 1);
        if (CheckForCheckmate())
        {
            EndGame();
            return;
        }

    }

    public void ChangeTurn()
    {
        promotionPos = CheckForPromotion();
        if (ValidCoordinate(promotionPos) && promotionObj != null)
        {
            waitingPromotion = true;
            List<GameObject> focus = new List<GameObject>();
            promotionObj.SetActive(true);
            focus.Add(promotionObj);
            ObjectFocus.instance.SetFocusObjects(focus);
            ObjectFocus.instance.EnableFocus(true);
            return;
        }
        turnPlayer = turnPlayer == board.player1 ? board.player2 : board.player1;
        StartTurn();
    }

    public void SetPromotion(int pieceType)
    {
        if (!waitingPromotion || (ChessPieceType)pieceType == ChessPieceType.KING || (ChessPieceType)pieceType == ChessPieceType.PAWN)
            return;

        ChessPieceType type = (ChessPieceType)pieceType;
        ChessPiece piece;
        board.nodes[promotionPos.x, promotionPos.y].pieceOnNode = piece = ChessBoard.GetPieceFromType(type, promotionPos);
        piece.hasMoved = true;
        piece.board = board;
        piece.player = turnPlayer;

        Destroy(tiles[promotionPos.x, promotionPos.y].chessPiece);
        GameObject obj = Instantiate(GetPieceObjectFromType(type));
        obj.transform.SetParent(piece.player == board.player1 ? lightPiecesParent.transform : darkPiecesParent.transform);
        obj.transform.localPosition = tiles[promotionPos.x, promotionPos.y].transform.localPosition;
        tiles[promotionPos.x, promotionPos.y].chessPiece = obj;

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (piece.player == board.player1)
            {
                sr.color = lightPieceColor;

            }
            else
            {
                sr.color = darkPieceColor;
                if (darkPiecesFlipped)
                {
                    sr.flipX = true;
                    sr.flipY = true;
                }
            }

        }

        waitingPromotion = false;
        promotionPos = new Position(-1, -1);
        if (promotionObj != null)
        {
            promotionObj.SetActive(false);
            ObjectFocus.instance.DisableFocus();

        }
        ChangeTurn();
    }

    public void EndGame()
    {
        canClick = false;
        if (victoryMsg)
        {
            string winner = turnPlayer == board.player1 ? "Preto" : "Branco";
            victoryMsg.text = winner + " venceu!";
            victoryMsg.gameObject.SetActive(true);
        }
        if (playerTurnIndicator)
            playerTurnIndicator.SetActive(false);
        if (playerTurnBorder)
            playerTurnBorder.SetActive(false);
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

    public Position CheckForPromotion()
    {
        for (int i = 0; i < board.nodes.GetLength(0); i++)
        {
            ChessPiece piece = board.nodes[i, board.nodes.GetLength(1) - 1].pieceOnNode;
            if (piece != null)
            {
                if (piece.type == ChessPieceType.PAWN)
                {
                    return piece.pos;
                }
            }

            piece = board.nodes[i, 0].pieceOnNode;
            if (piece != null)
            {
                if (piece.type == ChessPieceType.PAWN)
                {
                    return piece.pos;
                }
            }
        }
        return new Position(int.MinValue, int.MinValue);
    }

    public void RenderCheck()
    {
        if (checkRender != null)
        {
            if (board.IsPlayerInCheck(turnPlayer))
            {
                turnPlayer.inCheck = true;
                Position pos = board.GetKingPos(turnPlayer);
                if (ValidCoordinate(pos))
                {
                    List<Vector3> positions = new List<Vector3>();
                    positions.Add(tiles[pos.x, pos.y].transform.position);
                    checkRender.RenderSquaresArea(positions, tileRenderScale, tileRenderScale);
                }
            }
            else
            {
                turnPlayer.inCheck = false;
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

                lastMoveRender.RenderSquaresArea(pos, tileRenderScale, tileRenderScale);
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
                selectedPiece = null;
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
                    movementsRender.RenderSquaresArea(moves, tileRenderScale, tileRenderScale);
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
