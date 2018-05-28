﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

[Serializable]
public struct CheckersMoveInfo
{
    public Checker piece;
    public List<CheckerMove> moves;

}
[Serializable]
class CheckerBoardSaveData
{
    public CheckersBoard board;
    public List<CheckersMoveInfo> movesLog;
    public CheckersPlayer turnPlayer;
    public bool capturing;
    public Checker selectedPiece;
    public CheckerMove previousCaptureMove;
    public CheckersSettingsData settings;
}
public class CheckersBoardgame : Boardgame
{
    protected CheckersSettingsData gameSettings;
    [Header("Tile Settings")]
    public GameObject tilePrefab;
    public Color lightTile = Color.white;
    public Color darkTile = Color.cyan;
    [Space(10)]
    [Header("Pieces")]
    public GameObject checkerPrefab;
    public GameObject checkerKingPrefab;
    public Color topPlayerColor = Color.red;
    public Color bottomPlayerColor = Color.black;
    [Space(10)]

    public CheckersBoard board;
    public bool vsAI;
    public CheckersPlayer turnPlayer { get; internal set; }
    protected Checker selectedPiece;
    [Header("Renders")]
    public ProceduralMeshRenderer movementsRender;
    public ProceduralMeshRenderer captureRender;
    public ProceduralMeshRenderer lastMoveRender;
    public ProceduralMeshRenderer selectedPieceRender;
    [Space(10)]
    public AudioClip pieceMovement;
    [Space(10)]
    public TextMeshProUGUI victoryMsg;
    public GameObject aiTurnTimeIndicator;
    public bool canClick = true;
    [ListDrawerSettings(NumberOfItemsPerPage = 1)]
    public List<CheckersMoveInfo> movesLog;
    private CheckersMoveInfo currentMoveInfo;
    private float tileRenderScale = 0.89f;
    private GameObject tilesParentObj;
    private GameObject piecesParentObj;
    private GameObject player1PiecesParent;
    private GameObject player2PiecesParent;
    private GameObject indicatorParent;
    public CheckerTile[,] tiles { get; internal set; }
    private CheckerMove previousCaptureMove;
    private bool capturing;
    protected override void Start()
    {
        base.Start();
        gameObject.AddAudio(pieceMovement);
        //PrepareGame();
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
    public void PrepareGame()
    {
        StopAllCoroutines();
        gameSettings = new CheckersSettingsData(CheckersSettings.instance.settings);
        // Board settings
        lightTile = gameSettings.lightTileColor;
        darkTile = gameSettings.darkTileColor;
        topPlayerColor = gameSettings.topPieceColor;
        bottomPlayerColor = gameSettings.bottomPieceColor;
        columns = gameSettings.columns;
        rows = gameSettings.rows;
        board = new CheckersBoard();
        board.settings = new CheckersSettingsData(gameSettings);
        board.columns = columns;
        board.rows = rows;
        (board.playerTop = new CheckersPlayer()).orientation = Orientation.UP;
        (board.playerBottom = new CheckersPlayer()).orientation = Orientation.DOWN;

        turnPlayer = board.playerBottom;
        board.InitBoard();

        // Map settings
        RenderMap();
        PlacePieces();
        ClearRenders();


        movesLog = new List<CheckersMoveInfo>();
        canClick = true;
        capturing = false;
        selectedPiece = null;

        StartTurn();
    }

    public void PrepareGameAI()
    {

        StopAllCoroutines();
        gameSettings = new CheckersSettingsData(CheckersSettings.instance.settings);
        // Board settings
        lightTile = gameSettings.lightTileColor;
        darkTile = gameSettings.darkTileColor;
        topPlayerColor = gameSettings.topPieceColor;
        bottomPlayerColor = gameSettings.bottomPieceColor;
        columns = gameSettings.columns;
        rows = gameSettings.rows;
        board = new CheckersBoard();
        board.settings = new CheckersSettingsData(gameSettings);
        board.columns = columns;
        board.rows = rows;
        board.playerTop = new CheckersAI(Orientation.UP);
        (board.playerBottom = new CheckersPlayer()).orientation = Orientation.DOWN;
        ((CheckersAI)board.playerTop).board = board;
        turnPlayer = board.playerBottom;
        board.InitBoard();

        // Map settings
        RenderMap();
        PlacePieces();
        ClearRenders();


        movesLog = new List<CheckersMoveInfo>();
        canClick = true;
        capturing = false;
        selectedPiece = null;

        StartTurn();
    }



    public void PlacePieces()
    {
        if (board == null ? true : board.nodes == null)
            return;
        int rowsWithPieces = gameSettings.piecesByRow;
        if (rowsWithPieces >= rows / 2)
            rowsWithPieces = (rows / 2) - 1;
        if (rowsWithPieces <= 0)
            rowsWithPieces = 1;


        for (int i = 0; i < rowsWithPieces; i++)
        {
            bool oddRow = i % 2 == 0;
            for (int j = 0; j < columns; j += 2)
            {
                Position pos = new Position(oddRow ? j : j + 1, i);
                Checker c = new Checker(pos);
                c.player = board.playerBottom;
                c.moveDistance = gameSettings.pieceMoveDistance;
                c.jumpMovement = new DiagonalMovement(true, true, gameSettings.multiDirectionalCapture, gameSettings.multiDirectionalCapture);
                GeneratePiece(c, pos);
            }
        }


        for (int i = rows - 1; i > rows - 1 - rowsWithPieces; i--)
        {
            bool oddRow = i % 2 == 0;
            for (int j = 0; j < columns; j += 2)
            {
                Position pos = new Position(oddRow ? j : j + 1, i);
                Checker c = new Checker(pos);
                c.normalMovement = new DiagonalMovement(false, false, true, true);
                c.jumpMovement = new DiagonalMovement(gameSettings.multiDirectionalCapture, gameSettings.multiDirectionalCapture, true, true);
                c.player = board.playerTop;
                c.moveDistance = gameSettings.pieceMoveDistance;
                GeneratePiece(c, pos, true);
            }
        }
    }

    public void GeneratePiece(Checker piece, Position pos, bool isTopPlayer = false)
    {
        if (piece == null || board == null || tiles == null)
            return;
        if (!ValidCoordinate(pos))
            return;
        piece.board = board;
        piece.startPosition = pos;
        board.SetPiece(pos, piece);
        Destroy(tiles[pos.x, pos.y].piece);
        GameObject pieceObj = Instantiate(piece.isKing ? checkerKingPrefab : checkerPrefab);

        SpriteRenderer sr = pieceObj.GetComponent<SpriteRenderer>();
        if (sr)
        {
            sr.color = isTopPlayer ? topPlayerColor : bottomPlayerColor;
        }

        pieceObj.transform.SetParent(isTopPlayer ? player2PiecesParent.transform : player1PiecesParent.transform);
        pieceObj.transform.localScale = Vector3.one;
        pieceObj.transform.localPosition = tiles[pos.x, pos.y].transform.localPosition;
        tiles[pos.x, pos.y].piece = pieceObj;
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

            if (!indicatorParent)
                indicatorParent = new GameObject("Indicator");
            indicatorParent.transform.SetParent(transform.parent);
            indicatorParent.transform.localScale = Vector3.one;
            indicatorParent.transform.localPosition = Vector3.zero;
            bool tileColor = false;
            SpriteRenderer sr;
            GameObject tile;

            float columns = this.columns;
            float rows = this.rows;
            float width = UtilityFunctions.ScreenWidth;
            boardWidth = boardHeight = width;
            tileRenderScale = (width * 1.0f) / (columns * 1.0f);

            transform.localScale = Vector3.one * ((width * 1.0f) / (columns * 1.0f));

            tiles = new CheckerTile[this.columns, this.rows];
            for (int i = 0; i < this.columns; i++)
            {
                for (int j = 0; j < this.rows; j++)
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
                    tile.transform.localPosition = new Vector3(i + 0.5f - columns / 2f, j + 0.5f - rows / 2f, tilesParentObj.transform.localPosition.z);

                    if (j < this.rows - 1)
                        tileColor = !tileColor;
                    else if (this.rows % 2 != 0)
                        tileColor = !tileColor;
                }
            }
        }
    }
    #region Save and Load

    public void ConfirmRestartMatch()
    {
        if (vsAI)
            ModalWindow.Choice(GameTranslations.RESTART_MATCH_CONFIRM.Get(), PrepareGameAI);
        else
            ModalWindow.Choice(GameTranslations.RESTART_MATCH_CONFIRM.Get(), PrepareGame);
    }

    public void SaveBoardState()
    {
        if (board == null)
            return;

        CheckerBoardSaveData save = new CheckerBoardSaveData();
        save.board = board;
        save.movesLog = movesLog;
        save.turnPlayer = turnPlayer;
        save.capturing = capturing;
        save.selectedPiece = selectedPiece;
        save.previousCaptureMove = previousCaptureMove;
        save.settings = new CheckersSettingsData(gameSettings);
        string saveName = "";
        if (!vsAI)
            saveName = "1v1";
        else
            saveName = "AI";

        SaveLoad.SaveFile("/checkers_game_" + saveName + "_data.dat", save);
        ModalWindow.Message(GameTranslations.GAME_SAVED.Get());
    }

    public void ConfirmBoardLoad()
    {
        ModalWindow.Choice(GameTranslations.LOAD_GAME_CONFIRM.Get(), LoadBoardState);
    }


    public void LoadBoardState()
    {
        string saveName = "";
        if (!vsAI)
            saveName = "1v1";
        else
            saveName = "AI";
        CheckerBoardSaveData load = SaveLoad.LoadFile<CheckerBoardSaveData>("/checkers_game_" + saveName + "_data.dat");
        if (load != null ? load.board != null : false)
        {
            ReconstructBoard(load);
        }
        else
            ModalWindow.Message(GameTranslations.NO_GAME_SAVED.Get());

    }


    void ReconstructBoard(CheckerBoardSaveData data, bool playerVsplayer = true)
    {
        StopAllCoroutines();
        ClearRenders();
        if (data.board != null)
        {
            board = data.board;

            lightTile = gameSettings.lightTileColor;
            darkTile = gameSettings.darkTileColor;
            topPlayerColor = gameSettings.topPieceColor;
            bottomPlayerColor = gameSettings.bottomPieceColor;
            gameSettings = data.settings;
            columns = gameSettings.columns;
            rows = gameSettings.rows;
            movesLog = data.movesLog;
            turnPlayer = data.turnPlayer;
            capturing = data.capturing;
            selectedPiece = data.selectedPiece;
            previousCaptureMove = data.previousCaptureMove;
            RenderMap();

            foreach (CheckersNode node in data.board.GetNodes())
            {
                if (node.checkerOnNode == null)
                    continue;
                GameObject obj = Instantiate(node.checkerOnNode.isKing ? checkerKingPrefab : checkerPrefab);
                obj.transform.SetParent(node.checkerOnNode.player == board.playerTop ? player2PiecesParent.transform : player1PiecesParent.transform);
                obj.transform.localPosition = tiles[node.pos.x, node.pos.y].transform.localPosition;
                obj.transform.localScale = Vector3.one;
                tiles[node.pos.x, node.pos.y].piece = obj;

                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    if (node.checkerOnNode.player == board.playerTop)
                    {
                        sr.color = topPlayerColor;

                    }
                    else
                    {
                        sr.color = bottomPlayerColor;
                    }

                }

            }
            if (capturing)
            {
                List<CheckerMove> captureMoves = selectedPiece.GetPossibleJumps(new List<CheckerMove> { previousCaptureMove });
                captureMoves.Remove(captureMoves.Find(x => x.end == selectedPiece.pos));
                if (captureMoves.Count > 0)
                {
                    capturing = true;
                    RenderMoves(captureMoves);
                    RenderSelectedPiece(selectedPiece.pos);
                }
                else
                {
                    ClearRenders();
                    selectedPiece = null;
                    ChangeTurn();
                }
            }
            else
                StartTurn();
            canClick = true;
        }
    }
    #endregion


    /// <summary>
    /// Changes the color of the pieces at runtime.
    /// </summary>
    /// <param name="topPlayer"></param>
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
        currentMoveInfo.piece = piece;
        ClearRenders();
        capturing = false;
        if (!moves[0].isCapture)
        {
            CheckerMove move = moves.Find(x => x.end == positionClicked);
            board.Move(move);
            currentMoveInfo.moves.Add(move);
            MovePieceObject(move);

        }
        else
        {
            List<CheckerMove> movements = new List<CheckerMove>();
            CheckerMove current = moves.Find(x => x.end == positionClicked);
            movements.Add(current);
            previousCaptureMove = current;


            while (current.start != piece.pos)
            {
                current = current.previous;
                movements.Add(current);
            }
            movements.Reverse();
            board.CaptureMovement(movements);
            currentMoveInfo.moves.AddRange(movements);
            MovePieceObjectCapture(movements);
        }

    }

    IEnumerator MakeAMove(CheckerMove m)
    {

        if (m == null)
        {
            ChangeTurn();
            yield break;
        }


        List<CheckerMove> moves = new List<CheckerMove>();
        CheckerMove current = m;

        while (current != null)
        {

            moves.Add(current);
            current = current.next;
        }

        if (!moves[0].isCapture)
        {

            board.Move(m);
            currentMoveInfo.moves.Add(m);
            yield return MovePieceObj(m);

        }
        else
        {
            previousCaptureMove = moves[moves.Count - 1];

            board.CaptureMovement(moves);
            currentMoveInfo.moves.AddRange(moves);
            yield return MovePieceObjCapture(moves);
        }


        if (aiTurnTimeIndicator != null)
            aiTurnTimeIndicator.SetActive(false);

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

            GameObject temp = tiles[move.start.x, move.start.y].piece;
            if (temp != null)
            {
                canClick = false;
                tiles[move.start.x, move.start.y].piece.transform.MoveTo(tiles[move.end.x, move.end.y].transform.position, 0.1f);

                yield return new WaitForSeconds(0.1f);
                gameObject.PlayAudio(pieceMovement);
                tiles[move.start.x, move.start.y].piece = null;
                tiles[move.end.x, move.end.y].piece = temp;
                canClick = true;
            }
            selectedPiece = null;
            ChangeTurn();
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

                GameObject temp = tiles[move.start.x, move.start.y].piece;
                if (temp != null)
                {

                    tiles[move.start.x, move.start.y].piece.transform.MoveTo(tiles[move.end.x, move.end.y].transform.position, 0.1f);

                    yield return new WaitForSeconds(0.1f);
                    gameObject.PlayAudio(pieceMovement);
                    tiles[move.start.x, move.start.y].piece = null;
                    Position delta = new Position(UtilityFunctions.Sign(move.end.x - move.start.x), UtilityFunctions.Sign(move.end.y - move.start.y));
                    Position captured = move.end - delta;
                    if (tiles[captured.x, captured.y].piece != null)
                    {
                        Destroy(tiles[captured.x, captured.y].piece);
                        tiles[captured.x, captured.y].piece = null;
                    }
                    tiles[move.end.x, move.end.y].piece = temp;
                    if ((moves.IndexOf(move) != moves.Count - 1))
                        yield return new WaitForSeconds(0.2f);

                }
            }
        canClick = true;
        List<CheckerMove> captureMoves = new List<CheckerMove>();

        if (selectedPiece != null)
        {
            captureMoves = selectedPiece.GetPossibleJumps(new List<CheckerMove> { previousCaptureMove });
            captureMoves.Remove(captureMoves.Find(x => x.end == selectedPiece.pos));

        }
        if (captureMoves.Count > 0)
        {
            capturing = true;
            RenderMoves(captureMoves);
            RenderSelectedPiece(selectedPiece.pos);
        }
        else
        {
            ClearRenders();
            selectedPiece = null;
            ChangeTurn();
        }
    }
    public bool CheckForDefeat()
    {

        return board.GetPossibleMovements(turnPlayer).Count == 0;

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
            sr.color = Mathf.Sign(orientation) >= 1 ? topPlayerColor : bottomPlayerColor;
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
                sr.color = Mathf.Sign(orientation) >= 1 ? topPlayerColor : bottomPlayerColor;
            }
        }



    }
    public void StartTurn()
    {
        RenderLastTurn();
        if (selectedPieceRender && !capturing)
            selectedPieceRender.Clear();
        List<Checker> attackPieces = board.GetPiecesWithCapture(turnPlayer);
        if (attackPieces != null ? attackPieces.Count > 0 : false)
        {
            List<GameObject> objects = new List<GameObject>();
            foreach (var item in attackPieces)
            {
                objects.Add(tiles[item.pos.x, item.pos.y].piece);
            }
            RenderAttackPieces(objects);
        }
        if (victoryMsg)
            victoryMsg.gameObject.SetActive(false);
        if (aiTurnTimeIndicator)
            aiTurnTimeIndicator.SetActive(false);
        IndicateTurnPlayer(turnPlayer.orientation == Orientation.DOWN ? -1 : 1);
        if (CheckForDefeat())
        {
            EndGame();
            return;
        }
        if (turnPlayer is CheckersAI)
        {
            playerTurnIndicator?.SetActive(false);
            playerTurnBorder?.SetActive(false);
            StartCoroutine(AITurn());
        }
        currentMoveInfo = new CheckersMoveInfo();
        currentMoveInfo.moves = new List<CheckerMove>();

    }
    IEnumerator AITurn()
    {

        if (aiTurnTimeIndicator != null)
            aiTurnTimeIndicator.SetActive(true);

        canClick = false;
        CheckersAI ai = turnPlayer as CheckersAI;
        if (ai != null)
        {

            yield return ai.CalculateBestMove();

            if (board.GetPiece(ai.bestMove?.start) != null)
            {

                selectedPiece = board.GetPiece(ai.bestMove?.start);
                yield return MakeAMove(ai.bestMove);
                yield break;
            }
        }
        if (aiTurnTimeIndicator != null)
            aiTurnTimeIndicator.SetActive(false);
        ChangeTurn();
    }
    public void EndGame()
    {
        canClick = false;
        if (playerTurnIndicator)
            playerTurnIndicator.SetActive(false);
        if (playerTurnBorder)
            playerTurnBorder.SetActive(false);
        if (victoryMsg)
        {
            string winner;
            if (vsAI)
            {
                winner = turnPlayer == board.playerTop ? GameTranslations.PLAYER_NAME.Get() + " 1" : GameTranslations.AI_NAME.Get();
            }
            else
            {
                winner = turnPlayer == board.playerTop ? GameTranslations.PLAYER_NAME.Get() + " 1" : GameTranslations.PLAYER_NAME.Get() + " 2";
            }
            victoryMsg.text = winner + " " + GameTranslations.WON.Get();
            victoryMsg.gameObject.SetActive(true);
        }

    }
    public void ChangeTurn()
    {
        Position pos = CheckForPromotion();
        if (ValidCoordinate(pos))
        {
            Checker c = board.nodes[pos.x, pos.y].checkerOnNode;
            BecomeKing(c);
            GeneratePiece(c, pos, turnPlayer == board.playerTop ? true : false);
        }
        movesLog.Add(currentMoveInfo);
        turnPlayer = turnPlayer == board.playerTop ? board.playerBottom : board.playerTop;
        StartTurn();
    }
    /// <summary>
    /// Used to change checker piece to a king piece
    /// </summary>
    public virtual void BecomeKing(Checker c)
    {
        if (c == null)
            return;
        c.isKing = true;
        c.becameKing = true;
        c.moveDistance = gameSettings.kingInfiniteMoveDistance ? 99 : gameSettings.pieceMoveDistance;
        c.normalMovement = new DiagonalMovement(true, true, true, true);
        c.jumpMovement = new DiagonalMovement(true, true, true, true);
    }

    /// <summary>
    /// Checks for a checker's promotion.
    /// </summary>
    /// <returns></returns>
    public Position CheckForPromotion()
    {
        for (int i = 0; i < board.nodes.GetLength(0); i++)
        {
            Checker piece = board.nodes[i, board.nodes.GetLength(1) - 1].checkerOnNode;
            if (piece != null)
            {
                if (piece.player == board.playerBottom && !piece.becameKing)
                {
                    return piece.pos;
                }
            }

            piece = board.nodes[i, 0].checkerOnNode;
            if (piece != null)
            {
                if (piece.player == board.playerTop && !piece.becameKing)
                {
                    return piece.pos;
                }
            }
        }
        return new Position(int.MinValue, int.MinValue);
    }
    /// <summary>
    /// Changes the color of the tiles at runtime. 
    /// </summary>
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

    /// <summary>
    ///  Clears all renders.
    /// </summary>
    public void ClearRenders()
    {
        if (movementsRender != null)
            movementsRender.Clear();
        if (lastMoveRender != null)
            lastMoveRender.Clear();
        if (captureRender != null)
            captureRender.Clear();
        if (selectedPieceRender != null)
            selectedPieceRender.Clear();
    }
    public void RenderMoves(List<CheckerMove> moves)
    {
        if (movementsRender == null)
            return;

        List<Vector3> pos = new List<Vector3>();

        if (moves != null ? moves.Count != 0 : false)
        {
            foreach (Move m in moves)
            {
                if (ValidCoordinate(m.end))
                {
                    pos.Add(tiles[m.end.x, m.end.y].transform.position);
                }
            }
            movementsRender.RenderSquaresArea(pos, tileRenderScale, tileRenderScale);
        }
    }

    public void RenderSelectedPiece(Position pos)
    {
        if (selectedPieceRender == null)
            return;
        if (ValidCoordinate(pos))
            selectedPieceRender.RenderSquaresArea(new List<Vector3> { tiles[pos.x, pos.y].transform.position }, tileRenderScale, tileRenderScale);


    }

    public void RenderLastTurn()
    {
        if (lastMoveRender)
        {
            if (movesLog != null ? movesLog.Count > 0 : false)
            {
                List<Vector3> positions = new List<Vector3>();
                List<CheckerMove> moves = movesLog[movesLog.Count - 1].moves;
                if (!ValidCoordinate(moves[0].start))
                    return;
                positions.Add(tiles[moves[0].start.x, moves[0].start.y].transform.position);
                foreach (CheckerMove m in moves)
                {
                    Vector3 pos = tiles[m.end.x, m.end.y].transform.position;
                    if (!positions.Exists(x => x.x == pos.x && x.y == pos.y))
                        positions.Add(pos);
                }

                lastMoveRender.RenderSquaresArea(positions, tileRenderScale, tileRenderScale);
            }

        }
    }


    public void RenderAttackPieces(List<GameObject> pieceObjs)
    {
        if (pieceObjs == null || captureRender == null)
            return;
        foreach (var obj in pieceObjs)
        {
            if (obj == null)
                continue;

            List<Vector3> positions = new List<Vector3>();
            foreach (var item in pieceObjs)
            {
                positions.Add(item.transform.position);
            }
            captureRender.RenderSquaresArea(positions, tileRenderScale, tileRenderScale);

        }
    }

    public override void OnClick(Position pos)
    {
        if (!canClick)
        {
            movementsRender.Clear();
            return;
        }
        if (!board.ValidCoordinate(pos))
            return;

        if (capturing)
        {
            List<CheckerMove> moves = selectedPiece.GetPossibleJumps(new List<CheckerMove> { previousCaptureMove });
            moves.Remove(moves.Find(x => x.end == selectedPiece.pos));
            foreach (var item in moves)
            {
                if (pos == item.end)
                {
                    MovePiece(selectedPiece, moves, pos);
                    movementsRender.Clear();
                    selectedPieceRender.Clear();
                }
            }

            return;
        }
        if (selectedPiece != null)
        {
            List<CheckerMove> moves = selectedPiece.GetMovements();
            foreach (var item in moves)
            {
                if (pos == item.end)
                {
                    MovePiece(selectedPiece, moves, pos);
                    return;
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
                selectedPieceRender.Clear();
            }
            else // Player piece
            {
                List<Checker> attackPieces = board.GetPiecesWithCapture(turnPlayer);

                // If there are pieces that can attack.
                if (attackPieces != null ? attackPieces.Count > 0 : false)
                {
                    if (attackPieces.Contains(piece))
                    {
                        List<CheckerMove> possibleMoves = piece.GetMovements();
                        RenderMoves(possibleMoves);
                        selectedPiece = piece;
                        RenderSelectedPiece(selectedPiece.pos);

                    }
                    else
                    {
                        //Indicate invalid move.
                        movementsRender.Clear();
                        selectedPieceRender.Clear();
                    }
                }
                else // No pieces that can attack
                {

                    List<CheckerMove> possibleMoves = piece.GetMovements();
                    if (possibleMoves != null ? possibleMoves.Count != 0 : false)
                    {
                        RenderMoves(possibleMoves);
                    }
                    else
                    {
                        movementsRender.Clear();
                    }

                    selectedPiece = piece;
                    RenderSelectedPiece(selectedPiece.pos);
                }
            }


        }
        else
        {
            selectedPiece = null;
            movementsRender.Clear();
            selectedPieceRender.Clear();
        }
    }
}
