﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Predator
{
    public class Grid : MonoBehaviour
    {
        public static Grid instance;

        public Level level;

        public RectTransform levelCanvas;

        public RectTransform displayOrigin;
        public Transform cellsOrigin;
        public Vector3 originPosition { get => cellsOrigin.position; }

        public CellDefinition definition { get => level.definition; }

        public int width { get => level.width; }
        public int height { get => level.height; }

        public const float cellSize = 1;

        public GameObject cellPrefab;
        public GameObject displayPrefab;

        public CellBehaviour[,] cells { get; private set; }

        void OnEnable()
        {
            if (instance == null) instance = this;
        }

        void Awake()
        {
            CreateGrid();
        }

        #region Creation
        //[Button]
        public void CreateGrid()
        {
            cells = new CellBehaviour[width, height];

            levelCanvas.sizeDelta = new Vector2(width, height);

            cellsOrigin.position = displayOrigin.transform.position;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 position;
                    ConvertCoordinatesToLocalPosition(x, y, out position);

                    GameObject cellObject = Instantiate(cellPrefab);
                    cellObject.transform.parent = cellsOrigin;
                    cellObject.transform.localPosition = position;

                    GameObject cellDisplay = Instantiate(displayPrefab);
                    cellDisplay.transform.parent = displayOrigin;
                    cellDisplay.transform.localPosition = position;

                    CellBehaviour cell = cellObject.GetComponent<CellBehaviour>();
                    cell._definition = definition;
                    cell._display = cellDisplay.transform.GetChild(0).GetComponent<Image>();

                    cells[x, y] = cell;
                }
            }
        }
        #endregion

        #region Conversion
        public void ConvertWorldPositionToGrid(Vector3 worldPosition, out int x, out int y)
        {
            ConvertLocalPositionToGrid(worldPosition - originPosition, out x, out y);
        }

        public void ConvertLocalPositionToGrid(Vector3 localPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(localPosition.x / cellSize);
            y = Mathf.FloorToInt(localPosition.y / cellSize);
        }

        public void ConvertCoordinatesToWorldPosition(int x, int y, out Vector3 worldPosition)
        {
            ConvertCoordinatesToLocalPosition(x, y, out worldPosition);

            worldPosition += originPosition;
        }

        public void ConvertCoordinatesToLocalPosition(int x, int y, out Vector3 localPosition)
        {
            float _x = (x * cellSize) + (cellSize / 2);
            float _y = (y * cellSize) + (cellSize / 2);

            localPosition = new Vector3(_x, _y);
        }
        #endregion
    } 
}
