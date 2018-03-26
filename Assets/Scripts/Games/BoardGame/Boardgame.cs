using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boardgame : MonoBehaviour
{
    public Board board { get; internal set; }
    public int columns = 8;
    public int rows = 8;
    protected virtual void Start()
    {
        board = new Board(columns, rows);
        board.InitBoard();
    }

    /// <summary>
    /// Function to render the map. 
    /// </summary>
    public abstract void RenderMap();


#if UNITY_EDITOR
    protected void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;

        Vector3 scale = Vector3.one * (6.25f / columns);
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Gizmos.DrawWireCube(new Vector2(i + 0.5f + transform.position.x - columns / 2, j + 0.5f + transform.position.y - rows / 2) * scale.x, scale);
            }

        }

    }
#endif
}
