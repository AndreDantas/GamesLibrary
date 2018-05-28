using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using CielaSpike;
[System.Serializable]
class ReversiBoardSaveData
{
    public ReversiBoard board;
    public List<ReversiMoveInfo> movesLog;
    public Player turnPlayer;
    public ReversiSettingsData settings;
}
[System.Serializable]
public struct ReversiMoveInfo
{
    public Position movePos;
    public List<Position> flippedPieces;
    public ReversiMoveInfo(Position pos, List<Position> flippedPieces)
    {
        movePos = pos;
        this.flippedPieces = flippedPieces;
    }
}
public class ReversiBoardGame : Boardgame
{
    [Header("Tile Settings")]
    public GameObject tilePrefab;
    public Color lightTile = Colors.EmeraldGreen;
    public Color darkTile = Colors.FernGreen;
    [Space(10)]
    [Header("Pieces")]
    public GameObject piecePrefab;
    public Color topPlayerColor = Colors.BlackChocolate;
    public Color bottomPlayerColor = Colors.GhostWhite;
    [MinValue(0.1f)]
    public float colorChangeTime = 0.3f;
    [Space(10)]
    public ReversiSettingsData gameSettings;
    public ReversiBoard board;
    public bool vsAI;
    public bool showHints = false;
    public Player turnPlayer { get; internal set; }

    [Header("Renders")]
    public ProceduralMeshRenderer renderFlippedPieces;
    public ProceduralMeshRenderer lastMoveRender;
    public ProceduralMeshRenderer hintsRender;
    [Space(10)]
    public AudioClip piecePlacement;
    [Space(10)]
    public TextMeshProUGUI victoryMsg;
    public GameObject aiTurnTimeIndicator;
    public Image player1PieceScore;
    public Image player2PieceScore;
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public GameObject playerPassedObject;
    public TextMeshProUGUI playerPassedText;
    public bool canClick = true;
    [ListDrawerSettings(NumberOfItemsPerPage = 1)]

    private float tileRenderScale = 0.89f;
    private GameObject tilesParentObj;
    private GameObject piecesParentObj;
    private GameObject player1PiecesParent;
    private GameObject player2PiecesParent;
    private GameObject indicatorParent;
    public ReversiTile[,] tiles { get; internal set; }
    public List<ReversiMoveInfo> movesLog;

    protected override void Start()
    {
        base.Start();
        if (victoryMsg)
            victoryMsg.gameObject.SetActive(false);
        if (aiTurnTimeIndicator)
            aiTurnTimeIndicator.SetActive(false);
        if (playerPassedObject)
            playerPassedObject.SetActive(false);
        gameObject.AddAudio(piecePlacement);
        //PrepareGame();
    }
    private void OnValidate()
    {
        ChangePiecesColor(true);
        ChangePiecesColor(false);
        ChangeTileColor();
    }
    public virtual void PrepareGame()
    {
        StopAllCoroutines();
        gameSettings = new ReversiSettingsData(BoardGameSettings.instance.settings as ReversiSettingsData);
        columns = gameSettings.columns;
        rows = gameSettings.rows;
        topPlayerColor = gameSettings.topPieceColor;
        bottomPlayerColor = gameSettings.bottomPieceColor;
        darkTile = gameSettings.darkTileColor;
        lightTile = gameSettings.lightTileColor;

        board = new ReversiBoard(columns, rows);
        board.player1 = new Player(GameTranslations.PLAYER_NAME.Get() + " 1");
        board.player2 = new Player(GameTranslations.PLAYER_NAME.Get() + " 2");
        board.InitBoard();
        turnPlayer = board.player1;
        RenderMap();
        PlacePieces();
        ClearRenders();
        movesLog = new List<ReversiMoveInfo>();
        canClick = true;

        StartTurn();
    }
    public virtual void PrepareGameAI()
    {
        StopAllCoroutines();
        gameSettings = new ReversiSettingsData(BoardGameSettings.instance.settings as ReversiSettingsData);
        columns = gameSettings.columns;
        rows = gameSettings.rows;
        topPlayerColor = gameSettings.topPieceColor;
        bottomPlayerColor = gameSettings.bottomPieceColor;
        darkTile = gameSettings.darkTileColor;
        lightTile = gameSettings.lightTileColor;

        board = new ReversiBoard(columns, rows);
        board.InitBoard();
        board.player1 = new Player(GameTranslations.PLAYER_NAME.Get() + " 1");
        board.player2 = new ReversiAI(board);
        board.player2.name = GameTranslations.AI_NAME.Get();

        turnPlayer = board.player1;
        RenderMap();
        PlacePieces();
        ClearRenders();
        movesLog = new List<ReversiMoveInfo>();
        canClick = true;

        StartTurn();
    }
    public void PlacePieces()
    {

        AddPiece(board.player1, new Position(columns / 2 - 1, rows / 2));
        AddPiece(board.player1, new Position(columns / 2, rows / 2 - 1));
        AddPiece(board.player2, new Position(columns / 2, rows / 2));
        AddPiece(board.player2, new Position(columns / 2 - 1, rows / 2 - 1));

    }

    public void AddPiece(Player player, Position pos)
    {
        if (!ValidCoordinate(pos))
            return;
        board.AddPiece(player, pos);
        GeneratePiece(player, pos);
    }

    public void GeneratePiece(Player player, Position pos)
    {
        if (board == null || tiles == null || player == null || piecePrefab == null)
            return;
        if (!ValidCoordinate(pos))
            return;

        Destroy(tiles[pos.x, pos.y].piece);
        GameObject pieceObj = Instantiate(piecePrefab);

        SpriteRenderer sr = pieceObj.GetComponent<SpriteRenderer>();
        if (sr)
        {
            sr.color = player == board.player2 ? topPlayerColor : bottomPlayerColor;
        }

        pieceObj.transform.SetParent(player == board.player2 ? player2PiecesParent.transform : player1PiecesParent.transform);
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

            tiles = new ReversiTile[this.columns, this.rows];
            for (int i = 0; i < this.columns; i++)
            {
                for (int j = 0; j < this.rows; j++)
                {
                    // Create tile object.
                    tile = Instantiate(tilePrefab);
                    tile.name = "Tile(" + i + "," + j + ")";

                    // Add Tile component 
                    ReversiTile t = tile.AddComponent<ReversiTile>();
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

        ReversiBoardSaveData save = new ReversiBoardSaveData();
        save.board = board;
        save.movesLog = movesLog;
        save.turnPlayer = turnPlayer;
        save.settings = gameSettings;
        string saveName = "";
        if (!vsAI)
            saveName = "1v1";
        else
            saveName = "AI";

        SaveLoad.SaveFile("/reversi_game_" + saveName + "_data.dat", save);
        ModalWindow.Message(GameTranslations.GAME_SAVED.Get());
    }

    public void LoadBoardState()
    {
        string saveName = "";
        if (!vsAI)
            saveName = "1v1";
        else
            saveName = "AI";
        ReversiBoardSaveData load = SaveLoad.LoadFile<ReversiBoardSaveData>("/reversi_game_" + saveName + "_data.dat");
        if (load != null ? load.board != null : false)
        {
            ReconstructBoard(load);
        }
        else
            ModalWindow.Message(GameTranslations.NO_GAME_SAVED.Get());
    }
    public void ConfirmBoardLoad()
    {
        ModalWindow.Choice(GameTranslations.LOAD_GAME_CONFIRM.Get(), LoadBoardState);
    }

    void ReconstructBoard(ReversiBoardSaveData data, bool playerVsplayer = true)
    {
        ClearRenders();
        GameExit();
        StopAllCoroutines();
        if (data.board != null)
        {

            lightTile = gameSettings.lightTileColor;
            darkTile = gameSettings.darkTileColor;
            bottomPlayerColor = gameSettings.bottomPieceColor;
            topPlayerColor = gameSettings.topPieceColor;
            board = data.board;
            columns = board.columns;
            rows = board.rows;
            movesLog = data.movesLog;
            turnPlayer = data.turnPlayer;
            RenderMap();

            foreach (ReversiNode node in data.board.GetNodes())
            {
                if (node.pieceOnNode == null)
                    continue;
                GameObject obj = Instantiate(piecePrefab);
                obj.transform.SetParent(node.pieceOnNode.player == board.player1 ? player1PiecesParent.transform : player2PiecesParent.transform);
                obj.transform.localPosition = tiles[node.pos.x, node.pos.y].transform.localPosition;
                obj.transform.localScale = Vector3.one;
                tiles[node.pos.x, node.pos.y].piece = obj;

                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    if (node.pieceOnNode.player == board.player1)
                    {
                        sr.color = bottomPlayerColor;

                    }
                    else
                    {
                        sr.color = topPlayerColor;
                    }

                }

            }

            StartTurn();
            canClick = true;
        }
        else
            ModalWindow.Message(GameTranslations.NO_GAME_SAVED.Get());
    }

    public IEnumerator MakeAMove(Position pos)
    {
        canClick = false;
        ClearRenders();
        var flipPieces = board.IsValidMove(turnPlayer, pos);
        if (flipPieces != null)
        {
            board.MakeAMove(turnPlayer, pos);
            GeneratePiece(turnPlayer, pos);
            movesLog.Add(new ReversiMoveInfo(pos, flipPieces));
            gameObject.PlayAudio(piecePlacement);
            yield return FlipPieces(flipPieces, turnPlayer == board.player2);

        }

        canClick = true;
        ChangeTurn();
    }

    public IEnumerator FlipPieces(List<Position> flipPieces, bool isTopPlayer = true)
    {
        if (flipPieces == null)
            yield break;
        SpriteRenderer sr;
        foreach (var item in flipPieces)
        {
            if (ValidCoordinate(item) && board.ValidCoordinate(item))
            {
                if (board.nodes[item.x, item.y]?.pieceOnNode != null && tiles[item.x, item.y]?.piece != null)
                {
                    var piece = tiles[item.x, item.y]?.piece;
                    sr = piece.GetComponent<SpriteRenderer>();
                    if (sr)
                    {
                        sr.ChangeColorTo(isTopPlayer ? topPlayerColor : bottomPlayerColor, colorChangeTime);
                    }
                }
            }
        }

        yield return new WaitForSeconds(colorChangeTime);
    }

    public void ToggleShowHints()
    {
        showHints = !showHints;
        if (!showHints)
            hintsRender?.Clear();
        else if (canClick)
        {
            RenderHints();
        }


    }
    public void UpdateScores()
    {
        if (player1PieceScore)
            player1PieceScore.color = bottomPlayerColor;
        if (player2PieceScore)
            player2PieceScore.color = topPlayerColor;
        if (player1ScoreText)
            player1ScoreText.text = board.GetScore(board.player1).ToString();
        if (player2ScoreText)
            player2ScoreText.text = board.GetScore(board.player2).ToString();
    }

    public void StartTurn()
    {
        UpdateScores();
        RenderLastTurn();
        //RenderFlippedPieces();
        if (victoryMsg)
            victoryMsg.gameObject.SetActive(false);
        if (aiTurnTimeIndicator)
            aiTurnTimeIndicator.SetActive(false);
        if (playerPassedObject)
            playerPassedObject.SetActive(false);
        IndicateTurnPlayer(turnPlayer == board.player1 ? -1 : 1);
        if (CheckForEnd())
        {
            EndGame();
            return;
        }
        if (board.GetValidMoves(turnPlayer).Count == 0)
        {
            StartCoroutine(Passed());
            return;
        }
        if (turnPlayer is ReversiAI)
        {
            playerTurnIndicator?.SetActive(false);
            playerTurnBorder?.SetActive(false);
            StartCoroutine(AITurn());
            return;
        }

        if (showHints)
            RenderHints();

    }

    IEnumerator Passed()
    {
        yield return null;
        if (playerPassedObject)
        {
            if (playerPassedText)
            {
                playerPassedObject.SetActive(true);
                playerPassedObject.transform.localScale = new Vector3(1f, 0f, 1f);
                playerPassedText.text = turnPlayer.name + GameTranslations.PASSED.Get().ToLower();
                playerPassedObject.transform.ScaleTo(new Vector3(1f, 1f, 1f), 0.5f, EasingEquations.EaseInOutElastic);
                SceneController.LockPanel();
                yield return new WaitForSeconds(0.8f);
                playerPassedObject.transform.ScaleTo(new Vector3(1f, 0f, 1f), 0.5f, EasingEquations.EaseInElastic);
                yield return new WaitForSeconds(0.5f);
                SceneController.UnlockPanel();
            }
        }
        ChangeTurn();
    }

    public override void GameExit()
    {
        task?.Cancel();
        StopAllCoroutines();
    }

    Task task;
    IEnumerator AITurn()
    {

        if (aiTurnTimeIndicator != null)
            aiTurnTimeIndicator.SetActive(true);

        canClick = false;
        ReversiAI ai = turnPlayer as ReversiAI;
        if (ai != null)
        {
            this.StartCoroutineAsync(ai.CalculateBestMove(), out task);
            yield return task.Wait();
            //yield return ai.CalculateBestMove();
            if (aiTurnTimeIndicator != null)
                aiTurnTimeIndicator.SetActive(false);
            yield return MakeAMove(ai.bestMove);
        }


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
            int player1Score = board.GetScore(board.player1);
            int player2Score = board.GetScore(board.player2);

            string winner;
            if (player1Score > player2Score)
                winner = board.player1.name + " " + GameTranslations.WON.Get();
            else if (player2Score > player1Score)
                winner = board.player2.name + " " + GameTranslations.WON.Get();
            else
                winner = GameTranslations.DRAW.Get();
            victoryMsg.text = winner;
            victoryMsg.gameObject.SetActive(true);
        }

    }
    public void ChangeTurn()
    {


        turnPlayer = board.OtherPlayer(turnPlayer);


        StartTurn();
    }

    bool CheckForEnd()
    {
        Player otherPlayer = board.OtherPlayer(turnPlayer);
        return board.GetValidMoves(turnPlayer).Count == 0 && board.GetValidMoves(otherPlayer).Count == 0;

    }


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

    public void RenderHints()
    {
        if (hintsRender)
        {
            List<Position> moves = board.GetValidMoves(turnPlayer);
            List<Vector3> sqrPos = new List<Vector3>();
            if (moves.Count > 0)
            {
                foreach (var item in moves)
                {
                    if (ValidCoordinate(item))
                        sqrPos.Add(tiles[item.x, item.y].transform.position);
                }
            }
            hintsRender.RenderCircles(sqrPos, (tileRenderScale / 2f) * 0.5f, 20);
        }
    }

    public void RenderLastTurn()
    {
        if (lastMoveRender)
        {
            if (movesLog != null ? movesLog.Count > 0 : false)
            {

                Position position = movesLog[movesLog.Count - 1].movePos;

                if (board.ValidCoordinate(position))
                {
                    Vector3 pos = tiles[position.x, position.y].transform.position;
                    lastMoveRender.RenderSquaresArea(new List<Vector3> { pos }, tileRenderScale, tileRenderScale);
                }
            }

        }
    }

    public void RenderFlippedPieces()
    {
        if (renderFlippedPieces)
        {
            if (movesLog != null ? movesLog.Count > 0 : false)
            {
                List<Vector3> sqrPos = new List<Vector3>();
                List<Position> positions = movesLog[movesLog.Count - 1].flippedPieces;
                foreach (var item in positions)
                {


                    Vector3 pos = tiles[item.x, item.y].transform.position;
                    sqrPos.Add(pos);

                }
                renderFlippedPieces.RenderSquaresArea(sqrPos, tileRenderScale, tileRenderScale);
            }

        }
    }

    /// <summary>
    ///  Clears all renders.
    /// </summary>
    public void ClearRenders()
    {
        if (renderFlippedPieces != null)
            renderFlippedPieces.Clear();
        if (lastMoveRender != null)
            lastMoveRender.Clear();
        if (hintsRender != null)
            hintsRender.Clear();

    }

    public override void OnClick(Position pos)
    {
        if (!canClick)
        {
            renderFlippedPieces.Clear();
            return;
        }
        if (!board.ValidCoordinate(pos))
            return;

        var moves = board.GetValidMoves(turnPlayer);

        if (moves.Count > 0)
        {
            foreach (var item in moves)
            {

                if (item == pos)
                {
                    StartCoroutine(MakeAMove(pos));
                    renderFlippedPieces.Clear();
                    return;
                }
            }

        }
    }

}

