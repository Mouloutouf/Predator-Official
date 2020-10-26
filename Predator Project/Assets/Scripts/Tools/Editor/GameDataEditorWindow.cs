using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameDataEditorWindow : EditorWindow
{
    public static void Open(GameDataObject dataObject)
    {
        GameDataEditorWindow window = GetWindow<GameDataEditorWindow>("Game Data Editor");
    }
}
