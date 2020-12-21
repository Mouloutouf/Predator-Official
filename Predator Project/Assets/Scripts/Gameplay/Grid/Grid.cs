using Sirenix.OdinInspector;
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

        public Environment[,] environments;

        public int width { get => level.width; }
        public int height { get => level.height; }

        public const float cellSize = 1;

        public GameObject cellPrefab;
        public GameObject displayPrefab;

        public Cell[,] cells { get; private set; }

        void OnEnable()
        {
            if (instance == null) instance = this;
        }

        void Awake()
        {
            GetLevelMap();

            CreateGrid();
        }

        public void GetLevelMap()
        {
            environments = new Environment[width, height];

            EnvironmentMap levelMap = level.map;

            for (int i = 0; i < environments.GetLength(0); i++)
            {
                EnvironmentArray levelArray = levelMap.environmentArrays[i];

                for (int u = 0; u < environments.GetLength(1); u++)
                {
                    environments[i, u] = levelArray.environments[u];
                }
            }
        }

        #region Creation
        //[Button]
        public void CreateGrid()
        {
            cells = new Cell[width, height];

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

                    Cell cell = cellObject.GetComponent<Cell>();
                    cell._environment = environments[x, y];
                    cell._environmentDisplay = cellDisplay.GetComponent<Image>();
                    cell._actionDisplay = cellDisplay.transform.GetChild(0).GetComponent<Image>();

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
