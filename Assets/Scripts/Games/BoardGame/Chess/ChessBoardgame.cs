using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoardgame : Boardgame
{
    public GameObject tilePrefab;
    public Color lightColor = Color.white;
    public Color darkColor = Color.black;

    public GameObject pawnPrefab;

    public ChessTile[,] tiles { get; internal set; }
    private GameObject tilesParentObj;
    private GameObject piecesParentObj;
    private Chess chessBoard;
    protected override void Start()
    {
        board = new Board(columns, rows);
        board.InitBoard();


        chessBoard = new Chess(board);
        chessBoard.player1 = new ChessPlayer(-1);
        chessBoard.player2 = new ChessPlayer(1);

        RenderMap();
        PlacePieces();
    }
    public override void RenderMap()
    {
        if (columns > 0 && rows > 0)
        {
            tilesParentObj = new GameObject("Tiles");
            tilesParentObj.transform.SetParent(transform);
            tilesParentObj.transform.localScale = Vector3.one;
            tilesParentObj.transform.localPosition = Vector3.zero;

            piecesParentObj = new GameObject("Pieces");
            piecesParentObj.transform.SetParent(transform);
            piecesParentObj.transform.localScale = Vector3.one;
            piecesParentObj.transform.localPosition = Vector3.zero;

            bool tileColor = false;
            SpriteRenderer sr;
            GameObject tile;

            float width = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
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
                        sr.color = tileColor ? lightColor : darkColor;
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

    public void PlacePiece(ChessPiece p, Position pos, ChessPlayer player)
    {
        if (!chessBoard.ValidCoordinate(pos))
            return;
        p.gameObject.transform.SetParent(piecesParentObj.transform);
        p.player = player;
        p.startPosition = p.pos = pos;
        p.board = chessBoard;

        p.transform.localPosition = tiles[pos.x, pos.y].transform.localPosition;
        chessBoard.nodes[pos.x, pos.y].pieceOnNode = p;
    }

    public void PlacePieces()
    {
        //TEST
        for (int i = 0; i < columns; i++)
        {
            Pawn p = Instantiate(pawnPrefab).AddComponent<Pawn>();
            PlacePiece(p, new Position(i, 0), chessBoard.player1);
        }
    }

    public void OnClick(Position pos)
    {
        if (!chessBoard.ValidCoordinate(pos))
            return;
        ChessPiece piece = chessBoard.GetPiece(pos) as ChessPiece;

        if (piece != null)
        {
            //TEST
            Debug.Log("Destinations:");
            foreach (Move m in piece.GetPossibleMovement())
            {
                Debug.Log("(" + m.end.x + ", " + m.end.y + ")");
            }
        }
    }
}
