using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Predator
{
    public class LevelEditorWindow : EditorWindow
    {
        Level level;

        SerializedProperty serializedElements;

        Environment[,] currentEnvironments;

        EnvironmentType selectedType;
        Environment selectedEnvironment;

        Color hoverColor = Color.grey;
        bool isMousePressed = false;

        bool levelSaved = false;

        // Not Used
        public static void Init()
        {
            LevelEditorWindow window = GetWindow(typeof(LevelEditorWindow)) as LevelEditorWindow;

            window.Show();
        }

        public static void InitWithContent(Level levelProfile)
        {
            LevelEditorWindow window = GetWindow(typeof(LevelEditorWindow)) as LevelEditorWindow;

            window.level = levelProfile;

            window.currentEnvironments = window.level.cellsEnvironments;

            //window.serializedElements = window.level.serializedObject.FindProperty(nameof(window.level.cellsEnvironments));

            Undo.RecordObject(window.level, "Edit Level");

            window.Show();
        }

        public void OnGUI()
        {
            if (level == null)
            {
                EditorGUILayout.LabelField("Currently displayed profile is null");
                return;
            }

            if (level.cellsEnvironments.Length > 0)
            {
                Buttons();

                DisplayLevelGrid();

                Repaint();
            }
        }

        void Buttons()
        {
            if (GUILayout.Button(EnvironmentType.None.ToString())) selectedType = EnvironmentType.None;
            foreach (EnvironmentType type in level.EnvironmentDataBase.environments.Keys)
            {
                if (GUILayout.Button(type.ToString())) selectedType = type;
            }

            if (selectedType == EnvironmentType.None) selectedEnvironment = new Environment();
            else selectedEnvironment = level.EnvironmentDataBase.environments[selectedType];

            EditorGUILayout.LabelField("Current Brush Selected : " + selectedType.ToString());

            string message = levelSaved ? "Level was saved !" : "Save your modifications";
            if (GUILayout.Button("Save Level"))
            {
                level.cellsEnvironments = currentEnvironments;
                levelSaved = true;

                EditorUtility.SetDirty(level);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            EditorGUILayout.LabelField(message);
        }

        void DisplayLevelGrid()
        {
            float tileWidth = 25f;
            float tileHeight = 25f;

            int rowAmount = currentEnvironments.GetLength(0);
            int columnAmount = currentEnvironments.GetLength(1);

            float extraOffset = 170f;
            float offset = 30f;
            float increment = 2f;

            Event currentEvent = Event.current;

            if (currentEvent.type == EventType.MouseDown) isMousePressed = true;

            if (currentEvent.type == EventType.MouseUp) isMousePressed = false;

            for (int i = 0; i < rowAmount; i++)
            {
                for (int u = 0; u < columnAmount; u++)
                {
                    Rect squareRect = new Rect(offset + (increment * i) + (tileWidth * i), extraOffset + offset + (increment * u) + (tileHeight * u), tileWidth, tileHeight);

                    EditorGUIUtility.AddCursorRect(squareRect, MouseCursor.Arrow);

                    if (squareRect.Contains(currentEvent.mousePosition))
                    {
                        EditorGUI.DrawRect(squareRect, hoverColor);

                        if (isMousePressed)
                        {
                            currentEnvironments[i, u] = selectedEnvironment;
                            levelSaved = false;
                        }
                    }
                    else EditorGUI.DrawRect(squareRect, currentEnvironments[i, u].color);
                }
            }
        }
    } 
}
