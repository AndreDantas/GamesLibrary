using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SpriteSwap))]
public class SpriteSwapEditor : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        SpriteSwap obj = (SpriteSwap)target;
        if (GUILayout.Button("Swap Sprite"))
        {
            obj.SwapSprite();
        }
    }
}
