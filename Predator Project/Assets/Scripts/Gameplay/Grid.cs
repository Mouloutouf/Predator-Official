using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void _Action_(int x, int y);

public class Grid : MonoBehaviour
{
    public Level level;

    public GameObject levelCanvas;

    public RectTransform displayOrigin;
    public Transform cellsOrigin;
    public Vector3 originPosition { get => cellsOrigin.position; }

    public CellDefinition definition { get => level.definition; }

    public int width { get => level.width; }
    public int height { get => level.height; }

    public float cellSize { get; } = 1;

    public GameObject cellPrefab;
    public GameObject displayPrefab;

    public CellBehaviour[,] cells { get; private set; }

    //\

    public Transform player;

    private _Action_ clickAction_;

    public int moveArea;

    private int currentX;
    private int currentY;

    private CellBehaviour selectedCell;

    void Start()
    {
        CreateGrid();
    }

    #region Creation
    //[Button]
    public void CreateGrid()
    {
        cells = new CellBehaviour[width, height];

        levelCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

        cellsOrigin.position = displayOrigin.transform.position;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = ConvertCoordinatesToLocalPosition(x, y);

                GameObject cellObject = Instantiate(cellPrefab);
                cellObject.transform.parent = cellsOrigin;
                cellObject.transform.position = position;

                GameObject cellDisplay = Instantiate(displayPrefab);
                cellDisplay.transform.parent = displayOrigin;
                cellDisplay.transform.position = position;

                CellBehaviour cell = cellObject.GetComponent<CellBehaviour>();
                cell._definition = definition;
                cell._object = cellObject;
                cell._display = cellDisplay;
            }
        }
    }
    #endregion

    #region Get
    public void GetGrid()
    {
        cells = new CellBehaviour[width, height];

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                GetCellObject(transform, x, y);
            }
        }
    }

    private void GetCellObject(Transform parent, int x, int y)
    {
        Vector3 pos = ConvertCoordinatesToLocalPosition(x, y);

        foreach (Transform child in parent)
        {
            if (child.localPosition == pos)
            {
                cells[x, y] = child.gameObject.GetComponent<CellBehaviour>();
            }
        }
    }
    #endregion

    #region Conversion
    private Vector3 ConvertCoordinatesToLocalPosition(int x, int y)
    {
        float _x = (x * cellSize) + (cellSize / 2);
        float _y = (y * cellSize) + (cellSize / 2);

        return new Vector3(_x, _y);
    }

    //private void ConvertCoordinatesToWorldPosition(int x, int y, out Vector3 worldPosition)
    //{
    //    ConvertCoordinatesToLocalPosition(x, y, out worldPosition);

    //    worldPosition += originPosition;
    //}

    private void ConvertPositionToGridCoordinates(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    #endregion

    #region Draw
    private void DrawGrid()
    {
        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }
    #endregion

    #region Actions
    void Update()
    {
        Action(Functions._GetMouseWorldPosition(), HoverTile);

        if (Input.GetMouseButtonDown(0))
        {
            if (clickAction_ != null) Action(Functions._GetMouseWorldPosition(), clickAction_);
        }

        if (Input.GetMouseButtonDown(1))
        {
            //grid.GetInfoOnTile();
        }
    }

    public void ResetAction()
    {
        clickAction_ = null;
    }

    public void Action(Vector2 worldPosition, _Action_ action_)
    {
        int x, y;
        ConvertPositionToGridCoordinates(worldPosition, out x, out y);

        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            action_(x, y);
        }
    }

    public void HoverTile(int x, int y)
    {
        if (x == currentX && y == currentY) return;

        foreach (CellBehaviour _case in cells) if (_case != selectedCell) _case.hoverObject.SetActive(false);
        cells[x, y].hoverObject.SetActive(true);

        currentX = x;
        currentY = y;
    }

    public void SelectTile(int x, int y)
    {
        foreach (CellBehaviour _case in cells) { _case.hoverObject.SetActive(false); _case.hoverObject.GetComponent<SpriteRenderer>().color = Color.white; }
        cells[x, y].hoverObject.SetActive(true);
        cells[x, y].hoverObject.GetComponent<SpriteRenderer>().color = Color.yellow;

        selectedCell = cells[x, y];
    }

    public void MoveToTile(int x, int y)
    {
        player.position = cells[x, y].gameObject.transform.position;
    }

    public void AttackTile(int x, int y)
    {
        if (cells[x, y].enemy != null)
        {
            // Kill the Enemy
        }
    }

    public void SetTilesToAction(Vector3 playerPosition, int area)
    {
        int x, y;
        ConvertPositionToGridCoordinates(playerPosition, out x, out y);

        int minX, minY, maxX, maxY;
        minX = Mathf.Clamp(x - area, 0, width);
        maxX = Mathf.Clamp(x + area, 0, width);
        minY = Mathf.Clamp(y - area, 0, height);
        maxY = Mathf.Clamp(y + area, 0, height);

        for (int _x = 0; _x < width; _x++)
        {
            for (int _y = 0; _y < height; _y++)
            {
                cells[_x, _y].inActionArea = (_x >= minX && _y >= minY && _x < maxX && _y < maxY) ? true : false;
            }
        }
    } 
    #endregion
}
