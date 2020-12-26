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

        int inputWidth, inputHeight;

        SerializedProperty environmentDataBase;
        SerializedProperty map;

        private void OnEnable()
        {
            level = target as Level;

            inputWidth = level.width;
            inputHeight = level.height;

            environmentDataBase = serializedObject.FindProperty(nameof(level.EnvironmentDataBase));
            map = serializedObject.FindProperty(nameof(level.map));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(environmentDataBase);

            EditorGUILayout.BeginHorizontal();

            inputWidth = EditorGUILayout.IntField(inputWidth);
            inputHeight = EditorGUILayout.IntField(inputHeight);

            EditorGUILayout.EndHorizontal();

            bool hasEnvironmentData = level.EnvironmentDataBase != null;

            bool hasChangedSize = inputWidth != level.width || inputHeight != level.height;
            bool hasMinimumSize = inputWidth > 0 && inputHeight > 0;
            GUI.enabled = hasMinimumSize && hasChangedSize && hasEnvironmentData;

            if (GUILayout.Button("Create New Level"))
            {
                level.width = inputWidth;
                level.height = inputHeight;
                level.InitializeLevel();
            }
            GUI.enabled = true;

            bool hasInitializedLevel = level.map.environmentArrays != null;
            GUI.enabled = hasInitializedLevel && hasEnvironmentData;

            if (GUILayout.Button("Open Level Editor"))
            {
                LevelEditorWindow.InitWithContent(target as Level);
            }
            GUI.enabled = true;

            serializedObject.ApplyModifiedProperties();
        }
    } 
}
