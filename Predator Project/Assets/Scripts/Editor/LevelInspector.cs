using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Predator
{
    [CustomEditor(typeof(Level))]
    public class LevelInspector : Editor
    {
        Level level;

        SerializedProperty environmentDataBase;
        SerializedProperty map;

        private void OnEnable()
        {
            level = target as Level;

            environmentDataBase = serializedObject.FindProperty(nameof(level.EnvironmentDataBase));
            map = serializedObject.FindProperty(nameof(level.map));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(environmentDataBase);

            EditorGUILayout.BeginHorizontal();

            level.width = EditorGUILayout.IntField(level.width);
            level.height = EditorGUILayout.IntField(level.height);

            EditorGUILayout.EndHorizontal();

            GUI.enabled = level.width > 0 ? level.height > 0 ? true : false : false;

            if (GUILayout.Button("Create Level"))
            {
                level.InitializeLevel();
            }

            GUI.enabled = true;

            bool isLevel = level.map.environmentArrays != null ? true : false;
            GUI.enabled = isLevel;

            if (GUILayout.Button("Open Level Editor"))
                LevelEditorWindow.InitWithContent(target as Level);

            GUI.enabled = true;

            serializedObject.ApplyModifiedProperties();
        }
    } 
}
