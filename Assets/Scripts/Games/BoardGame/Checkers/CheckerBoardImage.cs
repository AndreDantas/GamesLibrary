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
    [MinValue(1)]
    public int rowsWithPieces = 3;
    public override void PlacePiecesNormal()
    {

        if (!piecesPrefab.ContainsKey("Checker"))
            return;
        base.PlacePiecesNormal();
        GameObject piece;

        if (rowsWithPieces >= rows / 2)
            rowsWithPieces = (rows / 2) - 1;
        if (rowsWithPieces <= 0)
            rowsWithPieces = 1;

        RectTransform rect;
        for (int i = 0; i < rowsWithPieces; i++)
        {
            bool oddRow = i % 2 == 0;
            for (int j = 0; j < columns; j += 2)
            {
                Position pos = new Position(oddRow ? j : j + 1, i);
                piece = Instantiate(piecesPrefab["Checker"]);
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
                piece = Instantiate(piecesPrefab["Checker"]);
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
