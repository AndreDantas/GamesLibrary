using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racket : MonoBehaviour
{
    public int orientation = -1;
    public float racketSpeed = 0.1f;
    // Use this for initialization
    void Start()
    {
        SetUpRacket();
    }

    private void OnValidate()
    {
        SetUpRacket();
    }

    public void SetUpRacket()
    {
        transform.localScale = new Vector3(UtilityFunctions.ScreenWidth / 4f, 0.25f, 1f);
        transform.position = new Vector3(0, Mathf.Sign(orientation) * (UtilityFunctions.ScreenHeight - UtilityFunctions.ScreenHeight / 2.5f) / 2f, 0);

    }


}
