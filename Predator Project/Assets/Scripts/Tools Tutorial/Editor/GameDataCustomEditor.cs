﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetHandler
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceId, int line)
    {
        // Check if the double clicked asset is an asset of type GameDataObject
        GameDataObject obj = EditorUtility.InstanceIDToObject(instanceId) as GameDataObject;

        // If it is Open the Editor Window
        if (obj != null)
        {
            GameDataEditorWindow.Open(obj);
            return true;
        }

        return false;
    }
}

[CustomEditor(typeof(GameDataObject))]
public class GameDataCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Editor"))
        {
            GameDataEditorWindow.Open((GameDataObject)target);
        }
    }
}
