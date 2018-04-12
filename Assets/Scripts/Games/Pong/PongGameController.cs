using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongGameController : MonoBehaviour
{

    public RacketController topPlayer;
    public RacketController bottomPlayer;

    // Use this for initialization
    void Start()
    {


        if (topPlayer)
        {
            topPlayer.racket.orientation = 1;
            topPlayer.racket.SetUpRacket();

            topPlayer.touchArea = new Bounds();
            topPlayer.touchArea.center = new Vector3(0, UtilityFunctions.ScreenHeight / 4f, 0);
            topPlayer.touchArea.size = new Vector3(UtilityFunctions.ScreenWidth, UtilityFunctions.ScreenHeight / 2f, 0);

        }
        if (bottomPlayer)
        {
            bottomPlayer.racket.orientation = -1;
            bottomPlayer.racket.SetUpRacket();

            bottomPlayer.touchArea = new Bounds();
            bottomPlayer.touchArea.center = new Vector3(0, -UtilityFunctions.ScreenHeight / 4f, 0);
            bottomPlayer.touchArea.size = new Vector3(UtilityFunctions.ScreenWidth, UtilityFunctions.ScreenHeight / 2f, 0);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
