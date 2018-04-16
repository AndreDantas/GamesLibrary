using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(TestingAudio))]

public class TestingAudioEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Play Audio"))
        {
            TestingAudio obj = (TestingAudio)target;
            obj.PlayAudio();
        }
    }
}
