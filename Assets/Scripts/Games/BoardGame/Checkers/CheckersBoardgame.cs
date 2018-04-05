using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Make promotion
// Highlight attack pieces
// Checkers's rules

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
    public CheckerPlayer turnPlayer;
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
    public GameObject checkerPrefab;
    public GameObject checkerKingPrefab;
    public Color topPlayerColor = Color.red;
    public Color bottomPlayerColor = Color.black;
    [Space(10)]
    [HideInInspector]
    public CheckersBoard board;
    public CheckerPlayer turnPlayer { get; internal set; }
    protected Checker selectedPiece;
    [Header("Renders")]
    public AreaRangeRenderer movementsRender;
    public AreaRangeRenderer captureRender;
    public AreaRangeRenderer lastMoveRender;
    public AreaRangeRenderer selectedPieceRender;
    public TextMeshProUGUI victoryMsg;
    public GameObject resetMatchButton;
    public bool canClick = true;
    public List<CheckersMoveInfo> movesLog;
    private CheckersMoveInfo currentMoveInfo;
    private float tileRenderScale = 0.89f;
    private GameObject tilesParentObj;
    private GameObject piecesParentObj;
    private GameObject player1PiecesParent;
    private GameObject player2PiecesParent;
    public CheckerTile[,] tiles { get; internal set; }
    private CheckerMove previousCaptureMove;
    private bool capturing;
    private void Start()
    {
        //PrepareGame();
    }
    private void OnValidate()
    {
        ChangePiecesColor(true);
        ChangePiecesColor(false);
        ChangeTileColor();
    }
    public void PrepareGame()
    {
        gameSettings = new CheckersSettingsData(CheckersSettings.instance.settings);
        // Board settings
        columns = gameSettings.columns;
        rows = gameSettings.rows;
        board = new CheckersBoard();
        board.columns = columns;
        board.rows = rows;
        (board.playerTop = new CheckerPlayer()).orientation = Orientation.UP;
        (board.playerBottom = new CheckerPlayer()).orientation = Orientation.DOWN;
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
        Destroy(tiles[pos.x, pos.y].checkerPiece);
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

            float columns = this.columns;
            float rows = this.rows;
            float width = MathOperations.ScreenWidth;
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
        SaveLoad.SaveFile("/checkers_game1v1_data.dat", save);
    }

    public void LoadBoardState()
    {
        CheckerBoardSaveData load = SaveLoad.LoadFile<CheckerBoardSaveData>("/checkers_game1v1_data.dat");
        if (load != null)
            if (load.board != null)
            {
                ReconstructBoard(load);
            }
    }


    void ReconstructBoard(CheckerBoardSaveData data, bool playerVsplayer = true)
    {
        ClearRenders();
        if (data.board != null)
        {
            board = data.board;

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
                tiles[node.pos.x, node.pos.y].checkerPiece = obj;

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
                    if ((moves.IndexOf(move) != moves.Count - 1))
                        yield return new WaitForSeconds(0.2f);

                }
            }
        canClick = true;
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
    public bool CheckForDefeat()
    {

        return board.GetPossibleMovements(turnPlayer).Count == 0;

    }
    public void StartTurn()
    {
        RenderLastTurn();
        List<Checker> attackPieces = board.GetPiecesWithCapture(turnPlayer);
        if (attackPieces != null ? attackPieces.Count > 0 : false)
        {
            List<GameObject> objects = new List<GameObject>();
            foreach (var item in attackPieces)
            {
                objects.Add(tiles[item.pos.x, item.pos.y].checkerPiece);
            }
            RenderAttackPieces(objects);
        }
        if (victoryMsg)
            victoryMsg.gameObject.SetActive(false);
        if (resetMatchButton)
            resetMatchButton.SetActive(false);
        if (CheckForDefeat())
        {
            EndGame();
            return;
        }

        currentMoveInfo = new CheckersMoveInfo();
        currentMoveInfo.moves = new List<CheckerMove>();

    }
    public void EndGame()
    {
        canClick = false;
        if (victoryMsg)
        {
            string winner = turnPlayer == board.playerTop ? "Jogador 1" : "Jogador 2";
            victoryMsg.text = winner + " venceu!";
            victoryMsg.gameObject.SetActive(true);
        }

        if (resetMatchButton)
            resetMatchButton.SetActive(true);
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
                if (piece.player == board.playerBottom && !piece.isKing)
                {
                    return piece.pos;
                }
            }

            piece = board.nodes[i, 0].checkerOnNode;
            if (piece != null)
            {
                if (piece.player == board.playerTop && !piece.isKing)
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

    public void OnClick(Position pos)
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
