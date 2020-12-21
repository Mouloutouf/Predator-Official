using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Predator
{
    //[CustomEditor(typeof(Level))]
    public class LevelInspector : Editor
    {
        Level level;

        SerializedProperty environmentsDataBase;
        SerializedProperty environments;

        private void OnEnable()
        {
            level = target as Level;

            environmentsDataBase = serializedObject.FindProperty(nameof(level.EnvironmentDataBase));
            environments = serializedObject.FindProperty(nameof(level.cellsEnvironments));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(environmentsDataBase);

            #region Dimensions
            EditorGUILayout.BeginHorizontal();

            level.width = EditorGUILayout.IntField(level.width);
            level.height = EditorGUILayout.IntField(level.height);

            EditorGUILayout.EndHorizontal();
            #endregion

            #region Level
            GUI.enabled = level.width > 0 ? level.height > 0 ? true : false : false;

            if (GUILayout.Button("Create Level"))
            {
                level.InitializeLevel();
            }

            GUI.enabled = true;
            #endregion

            #region Window
            bool isLevel = level.cellsEnvironments != null ? true : false;
            GUI.enabled = isLevel;

            if (GUILayout.Button("Open Level Editor"))
                LevelEditorWindow.InitWithContent(target as Level);

            GUI.enabled = true;
            #endregion

            EditorGUILayout.BeginVertical();

            if (level.cellsEnvironments != null)
            {
                foreach (Environment environment in level.cellsEnvironments)
                {
                    EditorGUILayout.LabelField(environment.type.ToString());
                }
            }

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    } 
}
