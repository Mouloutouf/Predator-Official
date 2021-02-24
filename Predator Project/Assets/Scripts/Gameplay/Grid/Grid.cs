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

        public Pathfinding pathfinding;

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
            CreateMap();

            CreateGrid();
        }

        private void CreateMap()
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
                    ConvertCoordinatesToWorldPosition(x, y, out Vector3 position);

                    GameObject cellObject = Instantiate(cellPrefab, position, Quaternion.identity, cellsOrigin);

                    GameObject cellDisplay = Instantiate(displayPrefab, position, Quaternion.identity, displayOrigin);

                    Cell cell = cellObject.GetComponent<Cell>();
                    cell._cellDisplay = cellDisplay.GetComponent<CellDisplay>();

                    cell._environment = _environments[x, y];

                    cell.pathNode = new PathNode(x, y) { _x = x, _y = y, obstacle = cell._environment.Type == EnvironmentType.Wall ? true : false };
                    pathfinding.allPathNodes.Add(cell.pathNode);
                    cell.pathNode.nodeDisplay = cellDisplay.GetComponentInChildren<PathNodeDisplay>();
                    cell.pathNode.nodeDisplay.gameObject.SetActive(pathfinding.debugMode);

                    _cells[x, y] = cell;
                }
            }

            pathfinding.ResetNodesDisplay(pathfinding.allPathNodes);
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
                Debug.Log("Cell does not exist, given coordinates were outside the bounds of the grid");
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
