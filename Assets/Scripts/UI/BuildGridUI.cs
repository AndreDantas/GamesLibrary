using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct GridTileInfo
{
    public GameObject tilePrefab;
    public Color color;
}
public class BuildGridUI : MonoBehaviour
{

    public List<GridTileInfo> tilePrefabs;

    [Range(1, 64)]
    public int columns = 8;
    [Range(1, 64)]
    public int rows = 8;
    protected GameObject tilesParent;
    public GameObject[,] tiles { get; internal set; }

    public void CreateGrid()
    {
        if (tilePrefabs == null ? true : tilePrefabs.Count == 0)
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

        int index = 0;
        float tilesWidth = rect.sizeDelta.x / columns;
        float tilesHeight = rect.sizeDelta.y / rows;
        GameObject tile;
        Position pos;

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {


                tile = Instantiate(tilePrefabs[index].tilePrefab);
                rect = tile.transform as RectTransform;
                pos = new Position(i, j);
                tile.name = "Tile" + pos;

                Image sr = tile.GetComponent<Image>();
                if (sr)
                {
                    sr.color = tilePrefabs[index].color;
                }
                // Set tile's position

                rect.SetParent(tilesParent.transform);
                rect.localScale = Vector3.one;
                rect.anchoredPosition = new Vector3(i * tilesWidth + tilesWidth / 2f - (columns * tilesWidth) / 2f,
                                                j * tilesHeight + tilesHeight / 2f - (rows * tilesHeight) / 2f, tilesParent.transform.localPosition.z);
                rect.sizeDelta = new Vector2(tilesWidth, tilesHeight);

                if (j < rows - 1)
                    index = (index + 1) % tilePrefabs.Count;
                else if (rows % 2 != 0)
                    index = (index + 1) % tilePrefabs.Count;
            }
        }

    }

}
