#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PuzzleMap))]
[CanEditMultipleObjects]
public class PuzzleMapEditor : Editor
{
    private PuzzleMap myPuzzleMap;
    private int buttonSize = 50;

    private void OnEnable()
    {
        myPuzzleMap = (target as PuzzleMap);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //serializedObject.Update();

        for (int i = 0; i < myPuzzleMap.row; i++)
        {
            GUILayout.BeginHorizontal();
            for (int j = 0; j < myPuzzleMap.column; j++)
            {
                GUILayout.Button("button", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize));
            }
            GUILayout.EndHorizontal();
        }        
    }
}
#endif
