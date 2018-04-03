using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boardgame : MonoBehaviour
{
    [Header("Board Settings")]
    public int columns = 8;
    public int rows = 8;


    /// <summary>
    /// Function to render the map. 
    /// </summary>
    public abstract void RenderMap();

}
