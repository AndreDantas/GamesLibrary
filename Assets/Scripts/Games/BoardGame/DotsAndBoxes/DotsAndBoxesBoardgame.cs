using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public enum EdgePosition
{
    Vertical, Horizontal
}

public class DotsAndBoxesBoardgame : Boardgame
{

    [Header("Box Settings")]
    public GameObject boxPrefab;
    public GameObject edgePrefab;

    [Space(10)]
    public Color topPlayerColor = Colors.BlackChocolate;
    public Color bottomPlayerColor = Colors.GhostWhite;
    public GameObject dotObj;
    [Space(10)]
    public DotsAndBoxesSettingsData gameSettings;
    public DotsAndBoxesBoard board;
    public bool vsAI;

    public Player turnPlayer { get; internal set; }

    [Header("Renders")]
    public ProceduralMeshRenderer lastMoveRender;
    [Space(10)]
    public AudioClip edgeCreated;
    [Space(10)]
    public TextMeshProUGUI victoryMsg;
    public GameObject aiTurnTimeIndicator;
    public bool canClick = true;
    private float tileRenderScale = 0.89f;
    private GameObject tilesParentObj;
    private GameObject indicatorParent;

    public BoxTile[,] tiles;
    public EdgeObject[,] edgesX;
    public EdgeObject[,] edgesY;

    private void Awake()
    {
        RenderMap();
    }
    public override void RenderMap()
    {

        if (columns > 0 && rows > 0)
        {
            if (tilesParentObj != null)
                tilesParentObj.transform.DestroyChildren();
            Destroy(tilesParentObj);
            tilesParentObj = new GameObject("Tiles");
            tilesParentObj.transform.SetParent(transform);
            tilesParentObj.transform.localScale = Vector3.one;
            tilesParentObj.transform.localPosition = Vector3.zero;


            if (!indicatorParent)
                indicatorParent = new GameObject("Indicator");
            indicatorParent.transform.SetParent(transform.parent);
            indicatorParent.transform.localScale = Vector3.one;
            indicatorParent.transform.localPosition = Vector3.zero;


            GameObject tile;

            float columns = this.columns;
            float rows = this.rows;
            float width = UtilityFunctions.ScreenWidth;
            boardWidth = boardHeight = width;
            tileRenderScale = (width * 1.0f) / (columns * 1.0f);

            transform.localScale = Vector3.one * ((width * 1.0f) / (columns * 1.0f));

            tiles = new BoxTile[this.columns, this.rows];
            edgesX = new EdgeObject[this.columns + 1, this.rows];
            edgesY = new EdgeObject[this.columns, this.rows + 1];
            for (int i = 0; i < this.columns; i++)
            {
                for (int j = 0; j < this.rows; j++)
                {
                    // Create tile object.
                    //tile = Instantiate(boxPrefab);
                    tile = new GameObject();
                    tile.name = "Tile(" + i + "," + j + ")";

                    // Add Tile component 
                    BoxTile t = tile.AddComponent<BoxTile>();
                    t.pos = new Position(i, j);
                    t.Edges = new BoxStruct<EdgeObject>();
                    // Set tile's position
                    SetBoxEdges(ref t, tile);
                    tile.transform.SetParent(tilesParentObj.transform);
                    tile.transform.localScale = Vector3.one;
                    tile.transform.localPosition = new Vector3(i + 0.5f - columns / 2f, j + 0.5f - rows / 2f, tilesParentObj.transform.localPosition.z);

                    SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                    if (sr)
                    {
                        sr.color = Colors.Green;
                    }

                    //t.boardGame = this;
                    tiles[i, j] = t;

                }
            }


        }
    }
    void SetBoxEdges(ref BoxTile box, GameObject tileObj)
    {

        int x = box.pos.x;
        int y = box.pos.y;

        //Left Edge
        GameObject temp;
        if (ValidCoordinate(x - 1, y) ? tiles[x - 1, y] == null : true)
        {
            temp = Instantiate(edgePrefab);
            temp.transform.SetParent(tileObj.transform);
            temp.transform.localScale = new Vector3(0.1f, 1f, 1f);
            temp.transform.localPosition = new Vector3(-0.5f, 0f, tileObj.transform.localPosition.z);

            EdgeObject edge = temp.AddComponent<EdgeObject>();
            edge.orientation = EdgePosition.Horizontal;
            edge.pos = new Position(x, y);
            edge.board = this;
            edge.start = new Vector3(-0.5f, -0.5f, tileObj.transform.localPosition.z);
            edge.end = new Vector3(-0.5f, +0.5f, tileObj.transform.localPosition.z);

            box.Edges.left = edge;

            edgesX[x, y] = box.Edges.left;
        }
        else
        {
            box.Edges.left = tiles[x - 1, y].Edges?.right;
        }

        //Right Edge
        if (ValidCoordinate(x + 1, y) ? tiles[x + 1, y] == null : true)
        {
            temp = Instantiate(edgePrefab);
            temp.transform.SetParent(tileObj.transform);
            temp.transform.localScale = new Vector3(0.1f, 1f, 1f);
            temp.transform.localPosition = new Vector3(+0.5f, 0f, tileObj.transform.localPosition.z);

            EdgeObject edge = temp.AddComponent<EdgeObject>();
            edge.orientation = EdgePosition.Horizontal;
            edge.pos = new Position(x + 1, y);
            edge.board = this;
            edge.start = new Vector3(+0.5f, -0.5f, tileObj.transform.localPosition.z);
            edge.end = new Vector3(+0.5f, +0.5f, tileObj.transform.localPosition.z);

            box.Edges.right = edge;
            edgesX[x + 1, y] = box.Edges.right;
        }
        else
        {
            box.Edges.right = tiles[x + 1, y].Edges?.left;
        }

        //Top Edge
        if (ValidCoordinate(x, y + 1) ? tiles[x, y + 1] == null : true)
        {
            temp = Instantiate(edgePrefab);
            temp.transform.SetParent(tileObj.transform);
            temp.transform.localScale = new Vector3(1f, 0.1f, 1f);
            temp.transform.localPosition = new Vector3(0f, +0.5f, tileObj.transform.localPosition.z);

            EdgeObject edge = temp.AddComponent<EdgeObject>();
            edge.orientation = EdgePosition.Vertical;
            edge.pos = new Position(x, y + 1);
            edge.board = this;
            edge.start = new Vector3(-0.5f, +0.5f, tileObj.transform.localPosition.z);
            edge.end = new Vector3(+0.5f, +0.5f, tileObj.transform.localPosition.z);

            box.Edges.top = edge;
            edgesY[x, y + 1] = box.Edges.top;
        }
        else
        {
            box.Edges.top = tiles[x, y + 1].Edges?.bottom;
        }

        //Bottom Edge
        if (ValidCoordinate(x, y - 1) ? tiles[x, y - 1] == null : true)
        {
            temp = Instantiate(edgePrefab);
            temp.transform.SetParent(tileObj.transform);
            temp.transform.localScale = new Vector3(1f, 0.1f, 1f);
            temp.transform.localPosition = new Vector3(0f, -0.5f, tileObj.transform.localPosition.z);

            EdgeObject edge = temp.AddComponent<EdgeObject>();
            edge.orientation = EdgePosition.Vertical;
            edge.pos = new Position(x, y);
            edge.board = this;
            edge.start = new Vector3(-0.5f, -0.5f, tileObj.transform.localPosition.z);
            edge.end = new Vector3(+0.5f, -0.5f, tileObj.transform.localPosition.z);

            box.Edges.bottom = edge;
            edgesY[x, y] = box.Edges.bottom;
        }
        else
        {
            box.Edges.bottom = tiles[x, y - 1].Edges?.top;
        }
    }
    public void OnClick(Position pos, EdgePosition orientation)
    {
        if (orientation == EdgePosition.Horizontal)
        {
            Debug.Log("Click on Horizontal edge at " + pos);
        }
        else
        {
            Debug.Log("Click on Vertical edge at " + pos);
        }
    }
}
