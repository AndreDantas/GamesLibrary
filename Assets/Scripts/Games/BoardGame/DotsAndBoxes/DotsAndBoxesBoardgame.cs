using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CielaSpike;
using Sirenix.OdinInspector;
public enum EdgePosition
{
    Vertical, Horizontal
}
[System.Serializable]
public class DotsAndBoxesBoardData
{
    public DotsAndBoxesBoard board;
    public Player turnPlayer;
    public DotsAndBoxesSettingsData settings;
}
public class DotsAndBoxesBoardgame : Boardgame
{

    [Header("Box Settings")]
    public GameObject boxPrefab;
    public GameObject edgePrefab;
    public GameObject dotPrefab;
    public float dotSize = 0.1f;
    public float dotSizeMulti = 1.15f;
    public float edgeWidth = 0.1f;
    [Space(10)]
    public Color topPlayerColor = Colors.Blueberry;
    public Color bottomPlayerColor = Colors.PersianRed;
    public Color tileColor = Color.white;
    [Space(10)]
    public DotsAndBoxesSettingsData gameSettings;
    public DotsAndBoxesBoard board;
    public bool vsAI;

    public Player turnPlayer { get; internal set; }

    [Header("Renders")]
    public ProceduralMeshRenderer lastMoveRender;
    [Space(10)]
    public AudioClip edgeCreated;
    public AudioClip squareFilled;
    [Space(10)]
    public TextMeshProUGUI victoryMsg;
    public GameObject aiTurnTimeIndicator;
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public Image player1ScoreImage;
    public Image player2ScoreImage;
    public bool canClick = true;
    private float tileRenderScale = 0.89f;
    private GameObject tilesParentObj;
    private GameObject indicatorParent;

    public BoxTile[,] tiles;
    public EdgeObject[,] edgesX;
    public EdgeObject[,] edgesY;

    protected override void Start()
    {

        base.Start();
        // PrepareGame();

        gameObject.AddAudio(edgeCreated);
        gameObject.AddAudio(squareFilled);
    }

    public void PrepareGame()
    {
        task?.Cancel();
        StopAllCoroutines();
        gameSettings = new DotsAndBoxesSettingsData(BoardGameSettings.instance.settings as DotsAndBoxesSettingsData);
        columns = gameSettings.columns;
        rows = gameSettings.rows;
        topPlayerColor = gameSettings.topPieceColor;
        bottomPlayerColor = gameSettings.bottomPieceColor;
        tileColor = gameSettings.lightTileColor;
        board = new DotsAndBoxesBoard(columns, rows);

        board.player1 = new Player("Jogador 1");
        board.player2 = new Player("Jogador 2");
        board.InitBoard();
        turnPlayer = board.player1;
        RenderMap();

        //ClearRenders();
        //movesLog = new List<Connect4MoveInfo>();
        canClick = true;

        StartTurn();
    }

    public void PrepareGameAI()
    {
        task?.Cancel();
        StopAllCoroutines();
        gameSettings = new DotsAndBoxesSettingsData(BoardGameSettings.instance.settings as DotsAndBoxesSettingsData);
        columns = gameSettings.columns;
        rows = gameSettings.rows;
        topPlayerColor = gameSettings.topPieceColor;
        bottomPlayerColor = gameSettings.bottomPieceColor;
        tileColor = gameSettings.lightTileColor;
        board = new DotsAndBoxesBoard(columns, rows);

        board.player1 = new Player("Jogador 1");
        board.player2 = new DotsAndBoxesAI(board);
        board.player2.name = "Computador";
        board.InitBoard();
        turnPlayer = board.player1;
        RenderMap();

        //ClearRenders();
        //movesLog = new List<Connect4MoveInfo>();
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


            if (!indicatorParent)
                indicatorParent = new GameObject("Indicator");
            indicatorParent.transform.SetParent(transform.parent);
            indicatorParent.transform.localScale = Vector3.one;
            indicatorParent.transform.localPosition = Vector3.zero;


            GameObject tile;

            float columns = this.columns;
            float rows = this.rows;
            float width = UtilityFunctions.ScreenWidth;
            boardWidth = boardHeight = width;
            tileRenderScale = (width * 1.0f) / (columns * 1.0f) * 0.85f;

            transform.localScale = Vector3.one * tileRenderScale;

            tiles = new BoxTile[this.columns, this.rows];
            edgesX = new EdgeObject[this.columns + 1, this.rows];
            edgesY = new EdgeObject[this.columns, this.rows + 1];
            for (int i = 0; i < this.columns; i++)
            {
                for (int j = 0; j < this.rows; j++)
                {
                    // Create tile object.
                    tile = Instantiate(boxPrefab);
                    //tile = new GameObject();
                    tile.name = "Tile(" + i + "," + j + ")";

                    // Add Tile component 
                    BoxTile t = tile.AddComponent<BoxTile>();
                    t.pos = new Position(i, j);
                    t.Edges = new BoxStruct<EdgeObject>();
                    // Set tile's position
                    SetBoxObjects(ref t, tile);
                    tile.transform.SetParent(tilesParentObj.transform);
                    tile.transform.localScale = Vector3.one;
                    tile.transform.localPosition = new Vector3(i + 0.5f - columns / 2f, j + 0.5f - rows / 2f, tilesParentObj.transform.localPosition.z);

                    SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                    if (sr)
                    {
                        sr.color = tileColor;
                    }

                    //t.boardGame = this;
                    tiles[i, j] = t;

                }
            }


        }
    }
    void SetBoxObjects(ref BoxTile box, GameObject tileObj)
    {

        int x = box.pos.x;
        int y = box.pos.y;

        //Left Edge
        GameObject temp;

        if (ValidCoordinate(x - 1, y) ? tiles[x - 1, y] == null : true)
        {
            temp = Instantiate(edgePrefab);
            temp.transform.SetParent(tileObj.transform);
            temp.transform.localScale = new Vector3(edgeWidth, 1f - dotSize, 1f);
            temp.transform.localPosition = new Vector3(-0.5f, 0f, 0);


            temp.GetComponent<BoxCollider2D>().size = new Vector2(dotSize * 20f, 1f);

            EdgeObject edge = temp.AddComponent<EdgeObject>();
            edge.orientation = EdgePosition.Horizontal;
            edge.pos = new Position(x, y);
            edge.board = this;
            edge.start = new Vector3(-0.5f, -0.5f, 0);
            edge.end = new Vector3(-0.5f, +0.5f, 0);

            temp = Instantiate(dotPrefab);
            temp.transform.SetParent(tileObj.transform);
            temp.transform.localScale = new Vector3(dotSize * 1.1f, dotSize * dotSizeMulti, 1f);
            temp.transform.localPosition = new Vector3(-0.5f, -0.5f, 0);

            box.Edges.left = edge;

            edgesX[x, y] = box.Edges.left;
        }
        else
        {
            box.Edges.left = tiles[x - 1, y].Edges.right;
        }

        //Right Edge
        if (ValidCoordinate(x + 1, y) ? tiles[x + 1, y] == null : true)
        {
            temp = Instantiate(edgePrefab);
            temp.transform.SetParent(tileObj.transform);
            temp.transform.localScale = new Vector3(edgeWidth, 1f - dotSize, 1f);
            temp.transform.localPosition = new Vector3(+0.5f, 0f, 0);

            temp.GetComponent<BoxCollider2D>().size = new Vector2(dotSize * 20f, 1f);

            EdgeObject edge = temp.AddComponent<EdgeObject>();
            edge.orientation = EdgePosition.Horizontal;
            edge.pos = new Position(x + 1, y);
            edge.board = this;
            edge.start = new Vector3(+0.5f, -0.5f, 0);
            edge.end = new Vector3(+0.5f, +0.5f, 0);

            temp = Instantiate(dotPrefab);
            temp.transform.SetParent(tileObj.transform);
            temp.transform.localScale = new Vector3(dotSize * 1.1f, dotSize * dotSizeMulti, 1f);
            temp.transform.localPosition = new Vector3(0.5f, -0.5f, 0);

            box.Edges.right = edge;
            edgesX[x + 1, y] = box.Edges.right;
        }
        else
        {
            box.Edges.right = tiles[x + 1, y].Edges.left;
        }

        //Top Edge
        if (ValidCoordinate(x, y + 1) ? tiles[x, y + 1] == null : true)
        {
            temp = Instantiate(edgePrefab);
            temp.transform.SetParent(tileObj.transform);
            temp.transform.localScale = new Vector3(1f - dotSize, edgeWidth, 1f);
            temp.transform.localPosition = new Vector3(0f, +0.5f, 0);

            temp.GetComponent<BoxCollider2D>().size = new Vector2(1f, dotSize * 20f);

            EdgeObject edge = temp.AddComponent<EdgeObject>();
            edge.orientation = EdgePosition.Vertical;
            edge.pos = new Position(x, y + 1);
            edge.board = this;
            edge.start = new Vector3(-0.5f, +0.5f, 0);
            edge.end = new Vector3(+0.5f, +0.5f, 0);

            temp = Instantiate(dotPrefab);
            temp.transform.SetParent(tileObj.transform);
            temp.transform.localScale = new Vector3(dotSize * 1.1f, dotSize * dotSizeMulti, 1f);
            temp.transform.localPosition = new Vector3(-0.5f, 0.5f, 0);

            box.Edges.top = edge;
            edgesY[x, y + 1] = box.Edges.top;
        }
        else
        {
            box.Edges.top = tiles[x, y + 1].Edges.bottom;
        }

        //Bottom Edge
        if (ValidCoordinate(x, y - 1) ? tiles[x, y - 1] == null : true)
        {
            temp = Instantiate(edgePrefab);
            temp.transform.SetParent(tileObj.transform);
            temp.transform.localScale = new Vector3(1f - dotSize, edgeWidth, 1f);
            temp.transform.localPosition = new Vector3(0f, -0.5f, 0);

            temp.GetComponent<BoxCollider2D>().size = new Vector2(1f, dotSize * 20f);
            EdgeObject edge = temp.AddComponent<EdgeObject>();

            edge.orientation = EdgePosition.Vertical;
            edge.pos = new Position(x, y);
            edge.board = this;
            edge.start = new Vector3(-0.5f, -0.5f, 0);
            edge.end = new Vector3(+0.5f, -0.5f, 0);

            temp = Instantiate(dotPrefab);
            temp.transform.SetParent(tileObj.transform);
            temp.transform.localScale = new Vector3(dotSize * 1.1f, dotSize * dotSizeMulti, 1f);
            temp.transform.localPosition = new Vector3(0.5f, 0.5f, 0);

            box.Edges.bottom = edge;
            edgesY[x, y] = box.Edges.bottom;
        }
        else
        {
            if (x + 1 == columns && y + 1 == rows)
            {
                temp = Instantiate(dotPrefab);
                temp.transform.SetParent(tileObj.transform);
                temp.transform.localScale = new Vector3(dotSize, dotSize, 1f);
                temp.transform.localPosition = new Vector3(0.5f, 0.5f, 0);

            }
            box.Edges.bottom = tiles[x, y - 1].Edges.top;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            board.PrintNodes();
    }

    public void ConfirmRestartMatch()
    {
        if (vsAI)
            ModalWindow.Choice("Reiniciar jogo?", PrepareGameAI);
        else
            ModalWindow.Choice("Reiniciar jogo?", PrepareGame);
    }
    public void SaveBoardState()
    {
        if (board == null)
            return;

        DotsAndBoxesBoardData save = new DotsAndBoxesBoardData();
        save.board = board;
        save.turnPlayer = turnPlayer;
        save.settings = gameSettings;
        string saveName = "";
        if (!vsAI)
            saveName = "1v1";
        else
            saveName = "AI";

        SaveLoad.SaveFile("/dots_and_boxes_game_" + saveName + "_data.dat", save);
        ModalWindow.Message("Jogo Salvo.");
    }

    public void LoadBoardState()
    {
        string saveName = "";
        if (!vsAI)
            saveName = "1v1";
        else
            saveName = "AI";
        DotsAndBoxesBoardData load = SaveLoad.LoadFile<DotsAndBoxesBoardData>("/dots_and_boxes_game_" + saveName + "_data.dat");
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


    void ReconstructBoard(DotsAndBoxesBoardData data, bool playerVsplayer = true)
    {
        task?.Cancel();
        StopAllCoroutines();
        if (data.board != null ? data.board.isInit : false)
        {

            tileColor = gameSettings.lightTileColor;
            bottomPlayerColor = gameSettings.bottomPieceColor;
            topPlayerColor = gameSettings.topPieceColor;
            board = data.board;
            columns = board.columns;
            rows = board.rows;

            turnPlayer = data.turnPlayer;
            RenderMap();

            foreach (var node in data.board.GetNodes())
            {
                if (node.box == null)
                    continue;
                if (!node.box.filled)
                    continue;
                Position pos = node.box.pos;

                var boxObj = tiles[pos.x, pos.y];

                SpriteRenderer sr = boxObj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    if (node.box.owner == board.player1)
                    {
                        sr.color = bottomPlayerColor * 0.85f;

                    }
                    else
                    {
                        sr.color = topPlayerColor * 0.85f;
                    }

                }

            }

            List<Edge> edges = board.GetFilledEdges();
            for (int i = 0; i < edges.Count; i++)
            {

                Position pos = edges[i].pos;

                Color c = edges[i].owner == board.player1 ? bottomPlayerColor : topPlayerColor;

                EdgeObject edge;

                if (edges[i].orientation == EdgePosition.Horizontal)
                    edge = edgesX[pos.x, pos.y];
                else
                    edge = edgesY[pos.x, pos.y];

                edge.sr.enabled = true;
                edge.sr.color = c;
                edge.owner = edges[i].owner;
            }

            StartTurn();
            canClick = true;
        }
        else
            ModalWindow.Message("Sem jogos salvos.");
    }

    public IEnumerator MakeAMove(Edge edge)
    {
        if (edge == null)
            yield break;

        canClick = false;
        bool extraTurn = false;

        Color c = turnPlayer == board.player1 ? bottomPlayerColor : topPlayerColor;

        //board.PrintNodes();
        //Debug.Log("--------------------------");
        //var g = new DotsAndBoxesBoard(board);
        //g.PrintNodes();
        //Debug.Log("--------------------------");
        //var b = board.BoardAfterMove(turnPlayer, edge);
        //b.PrintNodes();

        board.TraceEdge(edge, turnPlayer);

        float animTime = edgeCreated != null ? edgeCreated.length : 0;

        if (edge.orientation == EdgePosition.Horizontal)
        {
            gameObject.PlayAudio(edgeCreated);
            yield return edgesX[edge.pos.x, edge.pos.y].Activate(c, turnPlayer, animTime);
            Position pos = edge.pos - Position.Right;

            //animTime = squareFilled != null ? squareFilled.length : 0;
            if (board.CheckForFill(pos))
            {
                //board.FillBox(pos, turnPlayer);

                tiles[pos.x, pos.y].sr.ChangeColorTo(c * 0.85f, animTime);
                gameObject.PlayAudio(edgeCreated);
                yield return new WaitForSeconds(animTime);
                extraTurn = true;
            }
        }
        else
        {
            gameObject.PlayAudio(edgeCreated);
            yield return edgesY[edge.pos.x, edge.pos.y].Activate(c, turnPlayer, animTime);
            Position pos = edge.pos - Position.Up;

            //animTime = squareFilled != null ? squareFilled.length : 0;
            if (board.CheckForFill(pos))
            {
                //board.FillBox(pos, turnPlayer);

                tiles[pos.x, pos.y].sr.ChangeColorTo(c * 0.85f, animTime);
                gameObject.PlayAudio(edgeCreated);
                yield return new WaitForSeconds(animTime);

                extraTurn = true;
            }
        }



        if (board.CheckForFill(edge.pos))
        {
            //board.FillBox(edge.pos, turnPlayer);

            tiles[edge.pos.x, edge.pos.y].sr.ChangeColorTo(c * 0.85f, animTime);
            gameObject.PlayAudio(edgeCreated);
            yield return new WaitForSeconds(animTime);

            extraTurn = true;
        }


        canClick = true;
        if (!extraTurn)
            ChangeTurn();
        else
            StartTurn();
    }

    public void UpdateScore()
    {
        if (player1ScoreText)
            player1ScoreText.text = board.GetFilledSquaresCount(board.player1).ToString();
        if (player2ScoreText)
            player2ScoreText.text = board.GetFilledSquaresCount(board.player2).ToString();

        if (player1ScoreImage)
            player1ScoreImage.color = bottomPlayerColor;
        if (player2ScoreImage)
            player2ScoreImage.color = topPlayerColor;


    }

    public void StartTurn()
    {

        //RenderLastTurn();
        //RenderFlippedPieces();
        UpdateScore();

        if (victoryMsg)
            victoryMsg.gameObject.SetActive(false);

        if (aiTurnTimeIndicator)
            aiTurnTimeIndicator.SetActive(false);

        IndicateTurnPlayer(turnPlayer == board.player1 ? -1 : 1);
        if (CheckForEnd())
        {
            EndGame();
            return;
        }
        if (turnPlayer is DotsAndBoxesAI)
        {
            if (playerTurnIndicator)
                playerTurnIndicator?.SetActive(false);
            if (playerTurnBorder)
                playerTurnBorder?.SetActive(false);
            StartCoroutine(AITurn());
            return;
        }


    }

    Task task;
    IEnumerator AITurn()
    {

        if (aiTurnTimeIndicator != null)
            aiTurnTimeIndicator.SetActive(true);

        canClick = false;
        yield return null;
        DotsAndBoxesAI ai = turnPlayer as DotsAndBoxesAI;
        if (ai != null)
        {

            this.StartCoroutineAsync(ai.CalculateBestMove(), out task);

            while (task.State != TaskState.Done)
                yield return null;

            if (aiTurnTimeIndicator != null)
                aiTurnTimeIndicator.SetActive(false);
            yield return MakeAMove(ai.bestMove);
        }

        else
        {
            ChangeTurn();
        }

    }
    bool CheckForEnd()
    {
        if (board.GetValidEdges().Count == 0)
            return true;

        return false;
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

            int score1 = 0;
            int score2 = 0;
            foreach (var item in board.GetNodes())
            {
                if (item.box.filled)
                {
                    if (item.box.owner == board.player1)
                        score1++;
                    else
                        score2++;
                }
            }
            if (score1 > score2)
                winner = board.player1.name + " venceu!";
            else if (score2 > score1)
                winner = board.player2.name + " venceu!";
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
        playerTurnIndicator.transform.localPosition = new Vector3(0f, (rows * tileRenderScale / 2f + playerTurnIndicator.transform.localScale.y * 1.5f) * Mathf.Sign(orientation) + transform.position.y, 0f);
        playerTurnIndicator.transform.eulerAngles = new Vector3(0, 0, 180 * (Mathf.Sign(orientation) >= 1 ? 0 : 1));

        if (playerTurnBorder)
        {
            playerTurnBorder.SetActive(true);
            playerTurnBorder.transform.SetParent(indicatorParent.transform);
            playerTurnBorder.transform.localScale = new Vector3(columns * tileRenderScale, indicatorScale, 1f);
            playerTurnBorder.transform.localPosition = new Vector3(0f, (rows * tileRenderScale / 2f + playerTurnBorder.transform.localScale.y * 1.5f) * Mathf.Sign(orientation) + transform.position.y, 0f);
            playerTurnBorder.transform.eulerAngles = new Vector3(0, 0, 180 * (Mathf.Sign(orientation) >= 1 ? 0 : 1));

            sr = playerTurnBorder.GetComponent<SpriteRenderer>();

            if (sr)
            {
                sr.color = Mathf.Sign(orientation) >= 1 ? topPlayerColor : bottomPlayerColor;
            }
        }


    }


    public void OnClick(EdgeObject edge)
    {
        if (!canClick)
            return;

        Position pos = edge.pos;

        if (edge.orientation == EdgePosition.Horizontal)
        {
            // Debug.Log("Click on Horizontal edge at " + edge.pos);
            if (!board.edgesX[pos.x, pos.y].active)
            {
                StartCoroutine(MakeAMove(board.edgesX[pos.x, pos.y]));
            }
        }
        else
        {
            if (!board.edgesY[pos.x, pos.y].active)
            {
                StartCoroutine(MakeAMove(board.edgesY[pos.x, pos.y]));
            }
            // Debug.Log("Click on Vertical edge at " + edge.pos);
        }
    }

}
