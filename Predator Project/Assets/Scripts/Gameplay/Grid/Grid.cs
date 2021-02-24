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

        [SerializeField] private Level level;

        [SerializeField] private RectTransform levelCanvas;

        [SerializeField] private RectTransform displayOrigin;
        [SerializeField] private Transform cellsOrigin;
        public Vector3 OriginPosition { get => cellsOrigin.position; }

        public Environment[,] _environments { get; private set; }

        public int _width { get => level.width; }
        public int _height { get => level.height; }

        public const float cellSize = 1;

        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject displayPrefab;

        public Cell[,] _cells { get; private set; }

        public Pathfinding pathfinding { get; set; }

        void OnEnable()
        {
            if (instance == null) instance = this;
        }

        void Awake()
        {
            CreateLevel();
        }

        #region Creation
        private void CreateLevel()
        {
            GetLevelMap();

            CreateGrid();
        }

        private void GetLevelMap()
        {
            _environments = new Environment[_width, _height];

            EnvironmentMap levelMap = level.map;

            for (int i = 0; i < _environments.GetLength(0); i++)
            {
                EnvironmentArray levelArray = levelMap.environmentArrays[i];

                List<Environment> orderedEnvironments = new List<Environment>();
                foreach (Environment environment in levelArray.environments) orderedEnvironments.Add(environment);
                orderedEnvironments.Reverse();

                for (int u = 0; u < _environments.GetLength(1); u++)
                {
                    _environments[i, u] = orderedEnvironments[u];
                }
            }
        }

        private void CreateGrid()
        {
            _cells = new Cell[_width, _height];

            levelCanvas.sizeDelta = new Vector2(_width, _height);

            cellsOrigin.position = displayOrigin.transform.position;

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    ConvertCoordinatesToLocalPosition(x, y, out Vector3 position);

                    GameObject cellObject = Instantiate(cellPrefab);
                    cellObject.transform.parent = cellsOrigin;
                    cellObject.transform.localPosition = position;

                    GameObject cellDisplay = Instantiate(displayPrefab);
                    cellDisplay.transform.SetParent(displayOrigin);
                    cellDisplay.transform.localPosition = position;

                    Cell cell = cellObject.GetComponent<Cell>();
                    cell._environment = _environments[x, y];
                    cell._environmentDisplay = cellDisplay.GetComponent<Image>();
                    cell._actionDisplay = cellDisplay.transform.GetChild(0).GetComponent<Image>();
                    cell._detectionDisplay = cellDisplay.transform.GetChild(1).GetComponent<Image>();

                    cell.pathNode = new PathNode(x, y);

                    _cells[x, y] = cell;
                }
            }

            pathfinding = new Pathfinding(_width, _height);
        }
        #endregion

        #region Conversion
        public void ConvertWorldPositionToGrid(Vector3 worldPosition, out int x, out int y)
        {
            ConvertLocalPositionToGrid(worldPosition - OriginPosition, out x, out y);
        }

        public void ConvertLocalPositionToGrid(Vector3 localPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(localPosition.x / cellSize);
            y = Mathf.FloorToInt(localPosition.y / cellSize);
        }

        public void ConvertCoordinatesToWorldPosition(int x, int y, out Vector3 worldPosition)
        {
            ConvertCoordinatesToLocalPosition(x, y, out worldPosition);

            worldPosition += OriginPosition;
        }

        public void ConvertCoordinatesToLocalPosition(int x, int y, out Vector3 localPosition)
        {
            float _x = (x * cellSize) + (cellSize / 2);
            float _y = (y * cellSize) + (cellSize / 2);

            localPosition = new Vector3(_x, _y);
        }
        #endregion

        #region Utility
        public Cell GetCell(int x, int y)
        {
            if (IsInsideGrid(x, y))
            {
                return _cells[x, y];
            }
            else
            {
                Debug.Log("out of range coordinates, position is outside the grid");
                return null;
            }
        }

        public bool IsInsideGrid(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _width && y < _height;
        } 
        #endregion
    } 
}
