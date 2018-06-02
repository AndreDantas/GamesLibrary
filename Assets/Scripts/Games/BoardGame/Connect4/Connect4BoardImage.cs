using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class Connect4BoardImage : BoardImage
{

    public override void PlacePiecesNormal()
    {
        if (!ContainsPiece("piece"))
            return;
        base.PlacePiecesNormal();

        for (int i = 0; i < (columns * rows) / 2 - columns / 2; i++)
        {
            int rand = Random.Range(0, columns);

            Color c = i % 2 == 0 ? bottomPieceColor : topPieceColor;

            var parent = i % 2 == 0 ? bottomPiecesParent.transform : topPiecesParent.transform;
            int indexY = 0;
            while (pieces.ValidCoordinates(new Position(rand, indexY)) ? pieces[rand, indexY] != null : false)
            {
                indexY++;

            }
            if (indexY >= rows)
                continue;
            PlacePiece(Instantiate(GetPiece("piece")), new Position(rand, indexY), c, parent);

        }

        tilesParent.transform.SetAsLastSibling();
    }
}
