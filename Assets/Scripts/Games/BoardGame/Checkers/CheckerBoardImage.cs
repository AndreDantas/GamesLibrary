using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;

public class CheckerBoardImage : BoardImage
{

    [Title("Pieces")]
    [AssetsOnly]
    public GameObject piecePrefab;
    public Color topPieceColor = new Color(0.877f, 0.087f, 0.173f, 1f);
    public Color bottomPieceColor = new Color(0.331f, 0.304f, 0.304f, 1f);
    public int rowsWithPieces = 3;
    private GameObject piecesParent;
    private GameObject bottomPiecesParent;
    private GameObject topPiecesParent;
    [ShowInInspector]
    public GameObject[,] pieces { get; internal set; }

    protected virtual void OnValidate()
    {
        UpdateGrid();

    }

    public CheckerBoardImage UpdateGrid()
    {
        ChangePiecesColor();
        ChangeTileColor();
        return this;
    }

    public void ChangePiecesColor()
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
    public void ChangeTileColor()
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
    public virtual void PlacePieces()
    {
        if (piecePrefab == null || tiles == null)
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
        bottomPiecesParent = new GameObject("Dark Pieces", typeof(RectTransform));
        if (topPiecesParent)
        {
            topPiecesParent.transform.DestroyChildren();
            Destroy(topPiecesParent);
        }
        topPiecesParent = new GameObject("Light Pieces", typeof(RectTransform));

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
        float tilesWidth = rect.sizeDelta.x / columns;
        float tilesHeight = rect.sizeDelta.y / rows;
        GameObject piece;

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
                piece = Instantiate(piecePrefab);
                rect = piece.transform as RectTransform;
                piece.name = "Piece +" + pos;
                rect.SetParent(bottomPiecesParent.transform);
                rect.localScale = Vector3.one;
                rect.anchoredPosition = (tiles[pos.x, pos.y].transform as RectTransform).anchoredPosition;
                rect.sizeDelta = new Vector2(tilesWidth, tilesHeight);
                Image img = piece.GetComponent<Image>();
                if (img)
                {
                    img.color = bottomPieceColor;
                }
                pieces[pos.x, pos.y] = piece;

            }
        }


        for (int i = rows - 1; i > rows - 1 - rowsWithPieces; i--)
        {
            bool oddRow = i % 2 == 0;
            for (int j = 0; j < columns; j += 2)
            {
                Position pos = new Position(oddRow ? j : j + 1, i);
                piece = Instantiate(piecePrefab);
                rect = piece.transform as RectTransform;
                rect.SetParent(topPiecesParent.transform);
                rect.localScale = Vector3.one;
                rect.anchoredPosition = (tiles[pos.x, pos.y].transform as RectTransform).anchoredPosition;
                rect.sizeDelta = new Vector2(tilesWidth, tilesHeight);
                Image img = piece.GetComponent<Image>();
                if (img)
                {
                    img.color = topPieceColor;
                }
                pieces[pos.x, pos.y] = piece;
            }
        }
    }


}
