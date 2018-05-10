using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Linq;
public class ReversiBoardImage : BoardImage
{
    private void Start()
    {
        BuildBoard();
        PlacePiecesNormal();
    }

    public override void PlacePiecesNormal()
    {

        if (!ContainsPiece("Reversi"))
            return;
        base.PlacePiecesNormal();
        PlacePiece(Instantiate(GetPiece("Reversi")), new Position(columns / 2 - 1, rows / 2), bottomPieceColor, bottomPiecesParent.transform);
        PlacePiece(Instantiate(GetPiece("Reversi")), new Position(columns / 2, rows / 2 - 1), bottomPieceColor, bottomPiecesParent.transform);
        PlacePiece(Instantiate(GetPiece("Reversi")), new Position(columns / 2, rows / 2), topPieceColor, topPiecesParent.transform);
        PlacePiece(Instantiate(GetPiece("Reversi")), new Position(columns / 2 - 1, rows / 2 - 1), topPieceColor, topPiecesParent.transform);


    }

}
