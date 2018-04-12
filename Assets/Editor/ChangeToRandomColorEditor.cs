using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ChangeToRandomColor))]
public class ChangeToRandomColorEditor : Editor
{
    SerializedProperty colorList;
    private void OnEnable()
    {
        colorList = serializedObject.FindProperty("colorList");
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        ChangeToRandomColor obj = (ChangeToRandomColor)target;
        obj.changeColor = EditorGUILayout.Toggle("Change Colors", obj.changeColor);
        if (obj.changeColor)
        {
            obj.random = EditorGUILayout.Toggle("Random Colors", obj.random);
            obj.changeTime = EditorGUILayout.FloatField("Time to Change", obj.changeTime);
            if (!obj.random)
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(colorList, new GUIContent("List of Colors"), true);
                serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ColorField("Start Color", obj.StartColor);
            EditorGUILayout.ColorField("End Color", obj.EndColor);
            EditorGUI.EndDisabledGroup();
        }

    }

}
