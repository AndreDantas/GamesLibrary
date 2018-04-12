using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBounds : MonoBehaviour
{

    public GameObject wallPrefab;
    public float thickness = 0.2f;
    GameObject wallsParent;
    public GameObject topWall { get; internal set; }
    public GameObject bottomWall { get; internal set; }
    public GameObject leftWall { get; internal set; }
    public GameObject rightWall { get; internal set; }
    private void Start()
    {
        CreateWalls();
    }

    public void CreateWalls()
    {
        if (!wallPrefab)
            return;
        if (wallsParent)
        {
            wallsParent.transform.DestroyChildren();
        }
        else
            wallsParent = new GameObject("Walls Parent");

        float screenWidth = UtilityFunctions.ScreenWidth;
        float screenHeight = UtilityFunctions.ScreenHeight;

        GameObject bottom = Instantiate(wallPrefab);
        bottom.name = "Bottom wall";
        bottom.transform.SetParent(wallsParent.transform);
        bottom.transform.position = new Vector3(0, -screenHeight / 2f, 0);
        bottom.transform.localScale = new Vector3(screenWidth, thickness, 1);
        bottomWall = bottom;

        GameObject top = Instantiate(wallPrefab);
        top.name = "Top wall";
        top.transform.SetParent(wallsParent.transform);
        top.transform.position = new Vector3(0, screenHeight / 2f, 0);
        top.transform.localScale = new Vector3(screenWidth, thickness, 1);
        topWall = top;

        GameObject left = Instantiate(wallPrefab);
        left.name = "Left wall";
        left.transform.SetParent(wallsParent.transform);
        left.transform.position = new Vector3(-screenWidth / 2f, 0, 0);
        left.transform.localScale = new Vector3(thickness, screenHeight, 1);
        leftWall = left;

        GameObject right = Instantiate(wallPrefab);
        right.name = "Right wall";
        right.transform.SetParent(wallsParent.transform);
        right.transform.position = new Vector3(screenWidth / 2f, 0, 0);
        right.transform.localScale = new Vector3(thickness, screenHeight, 1);
        rightWall = right;
    }

}
