using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBounds : ScreenBounds
{
    public GameObject midline { get; internal set; }
    public override void CreateWalls()
    {
        base.CreateWalls();
        if (!wallPrefab)
            return;
        midline = Instantiate(wallPrefab, wallsParent.transform);
        midline.transform.localPosition = Vector3.zero;
        midline.transform.localScale = new Vector3(UtilityFunctions.ScreenWidth, 0.1f);
        Destroy(midline.GetComponent<Collider2D>());
    }
}
