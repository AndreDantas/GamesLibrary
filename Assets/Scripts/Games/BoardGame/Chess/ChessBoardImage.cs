﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Linq;
public class ChessBoardImage : BoardImage
{

    private List<string> PiecesNames => new List<string> { "pawn", "rook", "knight", "bishop", "king", "queen" };

    public void PlacePiecesMiniChess(string removePiece)
    {

        foreach (string s in PiecesNames)
        {
            if (!piecesPrefab.ContainsKey(s))
                return;
        }
        base.PlacePiecesNormal();

        // First Player
        for (int i = 0; i < columns; i++)
        {
            Position pos = new Position(i, 1);
            PlacePiece(Instantiate(piecesPrefab["pawn"]), pos, bottomPieceColor, bottomPiecesParent.transform);
        }
        int counter = 1;
        if (removePiece != "rook")
        {
            PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(counter - 1, 0), bottomPieceColor, bottomPiecesParent.transform);
            PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(columns - counter, 0), bottomPieceColor, bottomPiecesParent.transform);
            counter++;
        }
        if (removePiece != "knight")
        {
            PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(counter - 1, 0), bottomPieceColor, bottomPiecesParent.transform);
            PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(columns - counter, 0), bottomPieceColor, bottomPiecesParent.transform);
            counter++;
        }
        if (removePiece != "bishop")
        {
            PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(counter - 1, 0), bottomPieceColor, bottomPiecesParent.transform);
            PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(columns - counter, 0), bottomPieceColor, bottomPiecesParent.transform);
        }

        PlacePiece(Instantiate(piecesPrefab["queen"]), new Position(columns / 2 - 1, 0), bottomPieceColor, bottomPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["king"]), new Position(columns / 2, 0), bottomPieceColor, bottomPiecesParent.transform);

        // Second Player
        for (int i = 0; i < columns; i++)
        {
            Position pos = new Position(i, rows - 2);
            PlacePiece(Instantiate(piecesPrefab["pawn"]), pos, topPieceColor, topPiecesParent.transform);
        }
        counter = 1;
        if (removePiece != "rook")
        {
            PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(counter - 1, rows - 1), topPieceColor, topPiecesParent.transform);
            PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(columns - counter, rows - 1), topPieceColor, topPiecesParent.transform);
            counter++;
        }
        if (removePiece != "knight")
        {
            PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(counter - 1, rows - 1), topPieceColor, topPiecesParent.transform);
            PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(columns - counter, rows - 1), topPieceColor, topPiecesParent.transform);
            counter++;
        }
        if (removePiece != "bishop")
        {
            PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(counter - 1, rows - 1), topPieceColor, topPiecesParent.transform);
            PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(columns - counter, rows - 1), topPieceColor, topPiecesParent.transform);
        }
        PlacePiece(Instantiate(piecesPrefab["queen"]), new Position(columns / 2 - 1, rows - 1), topPieceColor, topPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["king"]), new Position(columns / 2, rows - 1), topPieceColor, topPiecesParent.transform);

    }

    public void PlacePiecesOmegaChess(string addedPiece)
    {

        foreach (string s in PiecesNames)
        {
            if (!piecesPrefab.ContainsKey(s))
                return;
        }
        base.PlacePiecesNormal();

        // First Player
        for (int i = 0; i < columns; i++)
        {
            Position pos = new Position(i, 1);
            PlacePiece(Instantiate(piecesPrefab["pawn"]), pos, bottomPieceColor, bottomPiecesParent.transform);
        }
        int counter = 1;

        PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(counter - 1, 0), bottomPieceColor, bottomPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(columns - counter, 0), bottomPieceColor, bottomPiecesParent.transform);
        counter++;
        if (addedPiece == "rook")
        {
            PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(counter - 1, 0), bottomPieceColor, bottomPiecesParent.transform);
            PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(columns - counter, 0), bottomPieceColor, bottomPiecesParent.transform);
            counter++;
        }

        PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(counter - 1, 0), bottomPieceColor, bottomPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(columns - counter, 0), bottomPieceColor, bottomPiecesParent.transform);
        counter++;
        if (addedPiece == "knight")
        {
            PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(counter - 1, 0), bottomPieceColor, bottomPiecesParent.transform);
            PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(columns - counter, 0), bottomPieceColor, bottomPiecesParent.transform);
            counter++;
        }

        PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(counter - 1, 0), bottomPieceColor, bottomPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(columns - counter, 0), bottomPieceColor, bottomPiecesParent.transform);
        counter++;
        if (addedPiece == "bishop")
        {
            PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(counter - 1, 0), bottomPieceColor, bottomPiecesParent.transform);
            PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(columns - counter, 0), bottomPieceColor, bottomPiecesParent.transform);
        }

        PlacePiece(Instantiate(piecesPrefab["queen"]), new Position(columns / 2 - 1, 0), bottomPieceColor, bottomPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["king"]), new Position(columns / 2, 0), bottomPieceColor, bottomPiecesParent.transform);

        // Second Player
        for (int i = 0; i < columns; i++)
        {
            Position pos = new Position(i, rows - 2);
            PlacePiece(Instantiate(piecesPrefab["pawn"]), pos, topPieceColor, topPiecesParent.transform);
        }
        counter = 1;

        PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(counter - 1, rows - 1), topPieceColor, topPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(columns - counter, rows - 1), topPieceColor, topPiecesParent.transform);
        counter++;

        if (addedPiece == "rook")
        {
            PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(counter - 1, rows - 1), topPieceColor, topPiecesParent.transform);
            PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(columns - counter, rows - 1), topPieceColor, topPiecesParent.transform);
            counter++;
        }

        PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(counter - 1, rows - 1), topPieceColor, topPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(columns - counter, rows - 1), topPieceColor, topPiecesParent.transform);
        counter++;

        if (addedPiece == "knight")
        {
            PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(counter - 1, rows - 1), topPieceColor, topPiecesParent.transform);
            PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(columns - counter, rows - 1), topPieceColor, topPiecesParent.transform);
            counter++;
        }

        PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(counter - 1, rows - 1), topPieceColor, topPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(columns - counter, rows - 1), topPieceColor, topPiecesParent.transform);
        counter++;
        if (addedPiece == "bishop")
        {
            PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(counter - 1, rows - 1), topPieceColor, topPiecesParent.transform);
            PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(columns - counter, rows - 1), topPieceColor, topPiecesParent.transform);
        }
        PlacePiece(Instantiate(piecesPrefab["queen"]), new Position(columns / 2 - 1, rows - 1), topPieceColor, topPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["king"]), new Position(columns / 2, rows - 1), topPieceColor, topPiecesParent.transform);

    }
    public override void PlacePiecesNormal()
    {

        foreach (string s in PiecesNames)
        {
            if (!piecesPrefab.ContainsKey(s))
                return;
        }
        base.PlacePiecesNormal();

        // First Player
        for (int i = 0; i < columns; i++)
        {
            Position pos = new Position(i, 1);
            PlacePiece(Instantiate(piecesPrefab["pawn"]), pos, bottomPieceColor, bottomPiecesParent.transform);
        }

        PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(0, 0), bottomPieceColor, bottomPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(7, 0), bottomPieceColor, bottomPiecesParent.transform);

        PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(1, 0), bottomPieceColor, bottomPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(6, 0), bottomPieceColor, bottomPiecesParent.transform);

        PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(2, 0), bottomPieceColor, bottomPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(5, 0), bottomPieceColor, bottomPiecesParent.transform);

        PlacePiece(Instantiate(piecesPrefab["queen"]), new Position(columns / 2 - 1, 0), bottomPieceColor, bottomPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["king"]), new Position(columns / 2, 0), bottomPieceColor, bottomPiecesParent.transform);

        // Second Player
        for (int i = 0; i < columns; i++)
        {
            Position pos = new Position(i, rows - 2);
            PlacePiece(Instantiate(piecesPrefab["pawn"]), pos, topPieceColor, topPiecesParent.transform);
        }
        PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(0, rows - 1), topPieceColor, topPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["rook"]), new Position(7, rows - 1), topPieceColor, topPiecesParent.transform);

        PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(1, rows - 1), topPieceColor, topPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["knight"]), new Position(6, rows - 1), topPieceColor, topPiecesParent.transform);

        PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(2, rows - 1), topPieceColor, topPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["bishop"]), new Position(5, rows - 1), topPieceColor, topPiecesParent.transform);

        PlacePiece(Instantiate(piecesPrefab["queen"]), new Position(columns / 2 - 1, rows - 1), topPieceColor, topPiecesParent.transform);
        PlacePiece(Instantiate(piecesPrefab["king"]), new Position(columns / 2, rows - 1), topPieceColor, topPiecesParent.transform);

    }

    private void PlacePiece(GameObject piece, Position pos, Color pieceColor, Transform parent)
    {
        if (!tiles.ValidCoordinate(pos.x, pos.y) || !pieces.ValidCoordinate(pos.x, pos.y))
            return;
        RectTransform rect = piece.transform as RectTransform;
        piece.name = "Piece +" + pos;
        rect.SetParent(parent);
        rect.localScale = Vector3.one;
        rect.anchoredPosition = (tiles[pos.x, pos.y].transform as RectTransform).anchoredPosition;
        rect.sizeDelta = new Vector2(tilesWidth, tilesHeight);
        Image img = piece.GetComponent<Image>();
        if (img)
        {
            img.color = pieceColor;
        }
        pieces[pos.x, pos.y] = piece;
    }

}
