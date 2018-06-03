using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
class Connect4BoardSaveData
{
    public Connect4Board board;
    public List<Connect4MoveInfo> movesLog;
    public Player turnPlayer;
    public Connect4SettingsData settings;
}
[System.Serializable]
public struct Connect4MoveInfo
{
    public Position dropPos;
    public Connect4MoveInfo(Position pos)
    {
        dropPos = pos;

    }
}
public class Connect4Boardgame : Boardgame
{
    [Header("Tile Settings")]
    public GameObject tilePrefab;
    public Color tileColor = Colors.BlueBell;
    [Space(10)]
    [Header("Pieces")]
    public GameObject piecePrefab;
    public Color topPlayerColor = Colors.BlackChocolate;
    public Color bottomPlayerColor = Colors.GhostWhite;
    [Space(10)]
    public Connect4SettingsData gameSettings;
    public Connect4Board board;
    public bool vsAI;
    public bool hasAnimation = true;
    public Player turnPlayer { get; internal set; }
    public bool hitConnect = false;
    [Header("Renders")]
    public ProceduralMeshRenderer lastMoveRender;
    public ProceduralMeshRenderer connectRender;
    [Space(10)]
    public AudioClip piecePlacement;
    [Space(10)]
    public TextMeshProUGUI victoryMsg;
    public GameObject aiTurnTimeIndicator;
    public TextMeshProUGUI titleText;
    public string gameName = GameTranslations.CONNECT.Get();
    public bool canClick = true;
    private float tileRenderScale = 0.89f;
    private GameObject tilesParentObj;
    private GameObject piecesParentObj;
    private GameObject player1PiecesParent;
    private GameObject player2PiecesParent;
    private GameObject indicatorParent;
    public BoardgameTile[,] tiles { get; internal set; }
    public List<Connect4MoveInfo> movesLog;

    protected override void Start()
    {
        gameName = GameTranslations.CONNECT.Get();
        base.Start();
        if (victoryMsg)
            victoryMsg.gameObject.SetActive(false);
        if (aiTurnTimeIndicator)
            aiTurnTimeIndicator.SetActive(false);
        hasAnimation = true;
        gameObject.AddAudio(piecePlacement);
        // PrepareGame();
    }

    public void ToggleAnimation()
    {
        hasAnimation = !hasAnimation;
    }

    public void AddPiece(Player player, Position pos)
    {
        if (!ValidCoordinate(pos))
            return;
        board.AddPiece(player, pos);
        GeneratePiece(player, pos);
    }

    public GameObject GeneratePiece(Player player, Position pos)
    {
        if (board == null || tiles == null || player == null || piecePrefab == null)
            return null;
        if (!ValidCoordinate(pos))
            return null;

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
        return pieceObj;
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
        gameSettings = new Connect4SettingsData(BoardGameSettings.instance.settings as Connect4SettingsData);
        columns = gameSettings.columns;
        rows = gameSettings.rows;
        topPlayerColor = gameSettings.topPieceColor;
        bottomPlayerColor = gameSettings.bottomPieceColor;
        tileColor = gameSettings.darkTileColor;
        board = new Connect4Board(columns, rows);
        board.ConnectTarget = gameSettings.connectTarget;
        board.player1 = new Player(GameTranslations.PLAYER_NAME.Get() + " 1");
        board.player2 = new Player(GameTranslations.PLAYER_NAME.Get() + " 2");
        board.InitBoard();
        turnPlayer = board.player1;
        RenderMap();

        ClearRenders();
        movesLog = new List<Connect4MoveInfo>();
        canClick = true;

        StartTurn();
    }
    public virtual void PrepareGameAI()
    {
        StopAllCoroutines();
        gameSettings = new Connect4SettingsData(BoardGameSettings.instance.settings as Connect4SettingsData);
        columns = gameSettings.columns;
        rows = gameSettings.rows;

        topPlayerColor = gameSettings.topPieceColor;
        bottomPlayerColor = gameSettings.bottomPieceColor;
        tileColor = gameSettings.darkTileColor;
        board = new Connect4Board(columns, rows);
        board.InitBoard();
        board.ConnectTarget = gameSettings.connectTarget;
        board.player1 = new Player(GameTranslations.PLAYER_NAME.Get() + " 1");
        board.player2 = new Connect4AI(board);
        board.player2.name = GameTranslations.AI_NAME.Get();

        turnPlayer = board.player1;
        RenderMap();
        ClearRenders();
        movesLog = new List<Connect4MoveInfo>();
        canClick = true;

        StartTurn();
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

            SpriteRenderer sr;
            GameObject tile;

            float columns = this.columns;
            float rows = this.rows;
            float width = UtilityFunctions.ScreenWidth;
            boardWidth = boardHeight = width;
            tileRenderScale = (width * 1.0f) / (columns * 1.0f);

            transform.localScale = Vector3.one * ((width * 1.0f) / (columns * 1.0f));

            tiles = new BoardgameTile[this.columns, this.rows];
            for (int i = 0; i < this.columns; i++)
            {
                for (int j = 0; j < this.rows; j++)
                {
                    // Create tile object.
                    tile = Instantiate(tilePrefab);
                    tile.name = "Tile(" + i + "," + j + ")";

                    // Add Tile component 
                    BoardgameTile t = tile.AddComponent<BoardgameTile>();
                    t.pos = new Position(i, j);
                    t.boardGame = this;
                    tiles[i, j] = t;

                    // Change sprite color
                    sr = tile.GetComponent<SpriteRenderer>();
                    if (sr)
                    {
                        sr.color = tileColor;
                    }

                    // Set tile's position
                    tile.transform.SetParent(tilesParentObj.transform);
                    tile.transform.localScale = Vector3.one;
                    tile.transform.localPosition = new Vector3(i + 0.5f - columns / 2f, j + 0.5f - rows / 2f, tilesParentObj.transform.localPosition.z);


                }
            }
        }
    }
    public void ClearRenders()
    {

        if (lastMoveRender)
            lastMoveRender.Clear();

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

        Connect4BoardSaveData save = new Connect4BoardSaveData();
        save.board = board;
        save.movesLog = movesLog;
        save.turnPlayer = turnPlayer;
        save.settings = gameSettings;
        string saveName = "";
        if (!vsAI)
            saveName = "1v1";
        else
            saveName = "AI";

        SaveLoad.SaveFile("/connect4_game_" + saveName + "_data.dat", save);
        ModalWindow.Message(GameTranslations.GAME_SAVED.Get());
    }

    public void LoadBoardState()
    {
        string saveName = "";
        if (!vsAI)
            saveName = "1v1";
        else
            saveName = "AI";
        Connect4BoardSaveData load = SaveLoad.LoadFile<Connect4BoardSaveData>("/connect4_game_" + saveName + "_data.dat");
        if (load != null ? load.board != null : false)
        {
            ReconstructBoard(load);
        }
        else
            ModalWindow.Message(GameTranslations.NO_SAVED_GAME.Get());
    }
    public void ConfirmBoardLoad()
    {
        ModalWindow.Choice(GameTranslations.LOAD_GAME_CONFIRM.Get(), LoadBoardState);
    }

    void ReconstructBoard(Connect4BoardSaveData data, bool playerVsplayer = true)
    {
        ClearRenders();
        StopAllCoroutines();
        if (data.board != null ? data.board.isInit : false)
        {

            tileColor = gameSettings.lightTileColor;
            bottomPlayerColor = gameSettings.bottomPieceColor;
            topPlayerColor = gameSettings.topPieceColor;
            board = data.board;
            columns = board.columns;
            rows = board.rows;
            board.ConnectTarget = data.board.ConnectTarget;
            movesLog = data.movesLog;
            turnPlayer = data.turnPlayer;
            RenderMap();

            foreach (Connect4Node node in data.board.GetNodes())
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
            ModalWindow.Message(GameTranslations.NO_SAVED_GAME.Get());
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


    /// <summary>
    /// Changes the color of the tiles at runtime. 
    /// </summary>
    public void ChangeTileColor()
    {
        if (tiles != null ? tiles.GetLength(0) > 0 && tiles.GetLength(1) > 0 : false)
        {

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    SpriteRenderer sr = tiles[i, j].GetComponent<SpriteRenderer>();
                    if (sr)
                    {
                        sr.color = tileColor;
                    }


                }
            }
        }
    }
    public IEnumerator MakeAMove(Position pos)
    {
        canClick = false;
        pos = new Position(pos.x, board.GetRowEmptyPosition(pos.x));
        board.Move(turnPlayer, pos.x);

        movesLog.Add(new Connect4MoveInfo(pos));

        GameObject piece = GeneratePiece(turnPlayer, pos);
        if (hasAnimation)
        {
            piece.transform.localPosition = tiles[pos.x, rows - 1].transform.localPosition + Vector3.up;
            //yield return new WaitForSeconds(0.1f);
            piece.transform.MoveToLocal(tiles[pos.x, pos.y].transform.localPosition, 0.8f, EasingEquations.EaseOutBounce);
            yield return new WaitForSeconds(0.7f);
            gameObject.PlayAudio(piecePlacement);
        }
        else
        {
            piece.transform.localPosition = tiles[pos.x, pos.y].transform.localPosition;

            gameObject.PlayAudio(piecePlacement);
            yield return null;
        }

        canClick = true;
        ChangeTurn();

    }


    public void StartTurn()
    {
        hitConnect = false;
        //RenderLastTurn();
        //RenderFlippedPieces();
        if (victoryMsg)
            victoryMsg.gameObject.SetActive(false);
        if (titleText)
        {
            titleText.gameObject.SetActive(true);
            titleText.text = gameName + " " + board.ConnectTarget;
        }
        if (aiTurnTimeIndicator)
            aiTurnTimeIndicator.SetActive(false);

        IndicateTurnPlayer(turnPlayer == board.player1 ? -1 : 1);
        if (CheckForEnd() || CheckForConnect())
        {
            EndGame();
            return;
        }
        if (turnPlayer is Connect4AI)
        {
            playerTurnIndicator?.SetActive(false);
            playerTurnBorder?.SetActive(false);
            StartCoroutine(AITurn());
            return;
        }


    }


    IEnumerator AITurn()
    {

        if (aiTurnTimeIndicator != null)
            aiTurnTimeIndicator.SetActive(true);

        canClick = false;
        Connect4AI ai = turnPlayer as Connect4AI;
        if (ai != null)
        {

            yield return ai.CalculateBestMove();
            if (aiTurnTimeIndicator != null)
                aiTurnTimeIndicator.SetActive(false);
            yield return MakeAMove(new Position(ai.bestMove, 0));
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
            string winner;
            if (hitConnect)
            {
                winner = board.OtherPlayer(turnPlayer).name;
                winner += " " + GameTranslations.WON.Get() + "!";
            }
            else
                winner = GameTranslations.DRAW.Get() + "!";
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

        for (int i = 0; i < board.rows; i++)
        {
            if (board.ValidColumn(i))
            {
                return false;

            }
        }
        return true;
    }

    bool CheckForConnect()
    {
        if (movesLog != null ? movesLog.Count > 0 : false)
        {
            Position result = board.CheckForConnect(board.OtherPlayer(turnPlayer));
            if (result.x >= 0 && result.y >= 0)
            {
                // Render connect
                RenderConnect(result);
                hitConnect = true;
                return true;
            }
        }

        return false;
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
        playerTurnIndicator.transform.localPosition = new Vector3(0f, (rows * tileRenderScale / 2f + playerTurnIndicator.transform.localScale.y / 2f) * -1 + transform.position.y, 0f);
        playerTurnIndicator.transform.eulerAngles = new Vector3(0, 0, 180);

        if (playerTurnBorder)
        {
            playerTurnBorder.SetActive(true);
            playerTurnBorder.transform.SetParent(indicatorParent.transform);
            playerTurnBorder.transform.localScale = new Vector3(columns * tileRenderScale, indicatorScale, 1f);
            playerTurnBorder.transform.localPosition = new Vector3(0f, (rows * tileRenderScale / 2f + playerTurnBorder.transform.localScale.y / 2f) * -1 + transform.position.y, 0f);
            playerTurnBorder.transform.eulerAngles = new Vector3(0, 0, 180);

            sr = playerTurnBorder.GetComponent<SpriteRenderer>();

            if (sr)
            {
                sr.color = Mathf.Sign(orientation) >= 1 ? topPlayerColor : bottomPlayerColor;
            }
        }



    }
    public void RenderLastTurn()
    {
        if (lastMoveRender)
        {
            if (movesLog != null ? movesLog.Count > 0 : false)
            {

                Position position = movesLog[movesLog.Count - 1].dropPos;

                if (board.ValidCoordinate(position))
                {
                    Vector3 pos = tiles[position.x, position.y].transform.position;
                    //lastMoveRender.RenderCircles(new List<Vector3> { pos }, tileRenderScale / 2f * 0.9f, 20);
                    lastMoveRender.RenderSquaresArea(new List<Vector3> { pos }, tileRenderScale, tileRenderScale);
                }
            }

        }
    }

    public void RenderConnect(Position pos)
    {
        if (connectRender)
        {
            if (ValidCoordinate(pos))
            {
                List<Vector3> renderPos = new List<Vector3>();
                foreach (var item in board.GetConnectPositions(pos, board.OtherPlayer(turnPlayer)))
                {
                    //Debug.Log(item);
                    renderPos.Add(tiles[item.x, item.y].transform.position);
                }
                lastMoveRender.RenderCircles(renderPos, tileRenderScale / 2f, 30);
            }


        }
    }

    public override void OnClick(Position pos)
    {
        if (!canClick)
        {
            return;
        }
        if (!board.ValidCoordinate(pos))
            return;

        var moves = board.GetValidColumns();
        //Debug.Log(pos);
        if (moves.Count > 0)
        {
            foreach (var item in moves)
            {

                if (item == pos.x)
                {

                    StartCoroutine(MakeAMove(pos));
                    return;
                }
            }

        }
    }
}
