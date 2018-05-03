using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class BoardImage : MonoBehaviour
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
    protected GameObject tilesParent;
    public GameObject[,] tiles { get; internal set; }

    public virtual void SetLightColor(Color color)
    {
        lightTile = color;
    }
    public virtual void SetDarkColor(Color color)
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

        float tilesWidth = rect.sizeDelta.x / columns;
        float tilesHeight = rect.sizeDelta.y / rows;
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

}
