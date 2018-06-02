using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class DotsAndBoxesBoardImage : BoardImage
{
    List<Position> player1Pos;
    List<Position> player2Pos;

    public override void BuildBoard()
    {
        base.BuildBoard();
    }

    public override void PlacePiecesNormal()
    {
        player1Pos = new List<Position>();
        player2Pos = new List<Position>();
        //base.PlacePiecesNormal();
        List<Position> randomPositions = Position.GenerateRandomPositions((columns * rows) / 2, columns, rows);
        for (int i = 0; i < randomPositions.Count; i++)
        {
            Color c = i % 2 == 0 ? bottomPieceColor : topPieceColor;

            tiles[randomPositions[i].x, randomPositions[i].y].GetComponent<Image>().color = c;

            if (i % 2 == 0)
                player1Pos.Add(new Position(randomPositions[i]));
            else
                player2Pos.Add(new Position(randomPositions[i]));

        }
    }

    public override void ChangeTileColor()
    {
        if (player1Pos != null)
            foreach (var item in player1Pos)
            {
                if (tiles.ValidCoordinates(item))
                {
                    Image sr = tiles[item.x, item.y].GetComponent<Image>();
                    if (sr)
                    {
                        sr.color = bottomPieceColor;
                    }

                }
            }
        if (player2Pos != null)
            foreach (var item in player2Pos)
            {
                if (tiles.ValidCoordinates(item))
                {
                    Image sr = tiles[item.x, item.y].GetComponent<Image>();
                    if (sr)
                    {
                        sr.color = topPieceColor;
                    }

                }
            }
    }

}
