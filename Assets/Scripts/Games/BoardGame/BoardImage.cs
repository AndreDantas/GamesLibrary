using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
public class BoardImage : SerializedMonoBehaviour
{
    [Title("Board")]
    [AssetsOnly]
    public GameObject tilePrefab;
    public Color darkTile = Color.black;
    public Color lightTile = Color.white;

    [Range(1, 64)]
    public int columns = 8;
    [Range(1, 64)]
    public int rows = 8;
    protected float tilesWidth;
    protected float tilesHeight;
    protected GameObject tilesParent;
    public GameObject[,] tiles { get; internal set; }
    [Title("Pieces")]
    public Color topPieceColor = new Color(0.877f, 0.087f, 0.173f, 1f);
    public Color bottomPieceColor = new Color(0.331f, 0.304f, 0.304f, 1f);
    [ShowInInspector, OdinSerialize]
    public Dictionary<string, GameObject> piecesPrefab = new Dictionary<string, GameObject>(StringComparer.InvariantCultureIgnoreCase);
    protected GameObject piecesParent;
    protected GameObject bottomPiecesParent;
    protected GameObject topPiecesParent;
    public GameObject[,] pieces { get; internal set; }

    protected virtual void OnValidate()
    {
        UpdateGrid();
    }

    public virtual void UpdateGrid()
    {
        ChangePiecesColor();
        ChangeTileColor();
    }

    public virtual void ChangePiecesColor()
    {
        if (piecesParent != null)
        {
            Image i;
            if (bottomPiecesParent)
                foreach (Transform t in bottomPiecesParent.transform)
                {
                    i = t?.GetComponent<Image>();
                    if (i)
                    {
                        i.color = bottomPieceColor;
                    }

                }
            if (topPiecesParent)
                foreach (Transform t in topPiecesParent.transform)
                {
                    i = t?.GetComponent<Image>();
                    if (i)
                    {
                        i.color = topPieceColor;
                    }

                }
        }
    }
    public void SetLightColor(Color color)
    {
        lightTile = color;
    }
    public void SetDarkColor(Color color)
    {
        darkTile = color;
    }

    public virtual void CreateGrid()
    {
        if (tilePrefab == null)
            return;
        if (tilesParent != null)
        {
            tilesParent.transform.DestroyChildren();
            Destroy(tilesParent);
        }
        columns = UtilityFunctions.ClampMin(columns, 1);
        rows = UtilityFunctions.ClampMin(rows, 1);


        tilesParent = new GameObject("Tiles", typeof(RectTransform));
        RectTransform parent = transform as RectTransform;
        RectTransform rect = tilesParent.transform as RectTransform;
        rect.SetParent(transform);
        rect.localScale = Vector3.one;
        rect.anchoredPosition = Vector3.zero;
        rect.sizeDelta = parent.sizeDelta;
        tiles = new GameObject[columns, rows];

        tilesWidth = rect.sizeDelta.x / columns;
        tilesHeight = rect.sizeDelta.y / rows;
        GameObject tile;
        Position pos;
        bool tileColor = false;
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {


                tile = Instantiate(tilePrefab);
                rect = tile.transform as RectTransform;
                pos = new Position(i, j);
                tile.name = "Tile" + pos;

                Image sr = tile.GetComponent<Image>();
                if (sr)
                {
                    sr.color = tileColor ? lightTile : darkTile;
                }
                // Set tile's position

                rect.SetParent(tilesParent.transform);
                rect.localScale = Vector3.one;
                rect.anchoredPosition = new Vector3(i * tilesWidth + tilesWidth / 2f - (columns * tilesWidth) / 2f,
                                                j * tilesHeight + tilesHeight / 2f - (rows * tilesHeight) / 2f, tilesParent.transform.localPosition.z);
                rect.sizeDelta = new Vector2(tilesWidth, tilesHeight);

                if (j < this.rows - 1)
                    tileColor = !tileColor;
                else if (this.rows % 2 != 0)
                    tileColor = !tileColor;
                tiles[i, j] = tile;
            }
        }

    }
    public virtual void ChangeTileColor()
    {
        if (tiles != null ? tiles.GetLength(0) > 0 && tiles.GetLength(1) > 0 : false)
        {
            bool tileColor = false;
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    Image sr = tiles[i, j].GetComponent<Image>();
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

    public virtual void PlacePiecesNormal()
    {
        if (piecesPrefab == null || tiles == null)
            return;
        if (piecesParent != null)
        {
            piecesParent.transform.DestroyChildren();
            Destroy(piecesParent);
        }
        piecesParent = new GameObject("Pieces", typeof(RectTransform));
        if (bottomPiecesParent)
        {
            bottomPiecesParent.transform.DestroyChildren();
            Destroy(bottomPiecesParent);
        }
        bottomPiecesParent = new GameObject("Bottom Pieces", typeof(RectTransform));
        if (topPiecesParent)
        {
            topPiecesParent.transform.DestroyChildren();
            Destroy(topPiecesParent);
        }
        topPiecesParent = new GameObject("Top Pieces", typeof(RectTransform));

        RectTransform parent = transform as RectTransform;
        RectTransform rect = piecesParent.transform as RectTransform;
        rect.SetParent(transform);
        rect.localScale = Vector3.one;
        rect.anchoredPosition = Vector3.zero;
        rect.sizeDelta = parent.sizeDelta;

        parent = rect;

        // Bottom pieces
        rect = bottomPiecesParent.transform as RectTransform;
        rect.SetParent(piecesParent.transform);
        rect.localScale = Vector3.one;
        rect.anchoredPosition = Vector3.zero;
        rect.sizeDelta = parent.sizeDelta;

        // Top pieces
        rect = topPiecesParent.transform as RectTransform;
        rect.SetParent(piecesParent.transform);
        rect.localScale = Vector3.one;
        rect.anchoredPosition = Vector3.zero;
        rect.sizeDelta = parent.sizeDelta;

        columns = UtilityFunctions.ClampMin(columns, 1);
        rows = UtilityFunctions.ClampMin(rows, 1);

        pieces = new GameObject[columns, rows];
        tilesWidth = rect.sizeDelta.x / columns;
        tilesHeight = rect.sizeDelta.y / rows;
    }
}
