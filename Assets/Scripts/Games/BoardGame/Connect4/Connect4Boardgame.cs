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
    public Player turnPlayer { get; internal set; }
    public bool hitConnect = false;
    [Header("Renders")]
    public ProceduralMeshRenderer lastMoveRender;
    public ProceduralMeshRenderer hintsRender;
    [Space(10)]
    public AudioClip piecePlacement;
    [Space(10)]
    public TextMeshProUGUI victoryMsg;
    public GameObject aiTurnTimeIndicator;
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
        base.Start();
        if (victoryMsg)
            victoryMsg.gameObject.SetActive(false);
        if (aiTurnTimeIndicator)
            aiTurnTimeIndicator.SetActive(false);

        gameObject.AddAudio(piecePlacement);
        // PrepareGame();
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
        board.ConnectTarget = gameSettings.connectTarget;
        topPlayerColor = gameSettings.topPieceColor;
        bottomPlayerColor = gameSettings.bottomPieceColor;
        tileColor = gameSettings.darkTileColor;
        board = new Connect4Board(columns, rows);
        board.player1 = new Player("Jogador 1");
        board.player2 = new Player("Jogador 2");
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
        board.ConnectTarget = gameSettings.connectTarget;
        topPlayerColor = gameSettings.topPieceColor;
        bottomPlayerColor = gameSettings.bottomPieceColor;
        tileColor = gameSettings.darkTileColor;
        board = new Connect4Board(columns, rows);
        board.InitBoard();
        board.player1 = new Player("Jogador 1");
        board.player2 = new Connect4AI(board);
        board.player2.name = "Computador";

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
        piece.transform.localPosition = tiles[pos.x, rows - 1].transform.localPosition + Vector3.up;
        yield return new WaitForSeconds(0.1f);
        piece.transform.MoveToLocal(tiles[pos.x, pos.y].transform.localPosition, 1f, EasingEquations.EaseOutBounce);
        yield return new WaitForSeconds(1f);
        //gameObject.PlayAudio(piecePlacement);

        canClick = true;
        ChangeTurn();

    }


    public void StartTurn()
    {
        hitConnect = false;
        // RenderLastTurn();
        //RenderFlippedPieces();
        if (victoryMsg)
            victoryMsg.gameObject.SetActive(false);
        if (aiTurnTimeIndicator)
            aiTurnTimeIndicator.SetActive(false);

        IndicateTurnPlayer(turnPlayer == board.player1 ? -1 : 1);
        if (CheckForEnd() || CheckForConnect())
        {
            EndGame();
            return;
        }
        if (turnPlayer is ReversiAI)
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
        ReversiAI ai = turnPlayer as ReversiAI;
        if (ai != null)
        {

            yield return ai.CalculateBestMove();
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
            string winner;
            if (hitConnect)
            {
                winner = turnPlayer == board.player1 ? "Jogador 2" : "Jogador 1";
                winner += " venceu!";
            }
            else
                winner = "Empate!";
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
            List<Connect4Node> result = board.CheckForConnect(board.OtherPlayer(turnPlayer), movesLog[movesLog.Count - 1].dropPos);
            if (result != null)
            {
                // Render connect
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
                    lastMoveRender.RenderCircles(new List<Vector3> { pos }, tileRenderScale / 2f * 0.9f, 20);
                }
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
