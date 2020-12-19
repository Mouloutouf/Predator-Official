using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Predator
{
    public delegate void _Action_(int x, int y);

    public class InputManager : MonoBehaviour
    {
        public Grid grid { get; private set; }

        private _Action_ clickAction_;

        public Image hoverDisplay;
        public Image selectDisplay;

        private CellBehaviour selectedCell;

        public PlayerManager player;

        public Dictionary<ActionType, Action> actions { get => player.actions; }

        void Start()
        {
            grid = Grid.instance;
            clickAction_ = SelectCell;
        }

        void Update()
        {
            Action(Functions.GetMouseWorldPosition(), HoverCell);

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Functions.GetMouseWorldPosition();
                if (clickAction_ != null) Action(mousePosition, clickAction_);
            }

            if (Input.GetMouseButtonDown(1))
            {
                //grid.GetInfoOnTile();
            }
        }

        public void Action(Vector2 worldPosition, _Action_ action_)
        {
            grid.ConvertWorldPositionToGrid(worldPosition, out int x, out int y);

            if (x >= 0 && y >= 0 && x < grid.width && y < grid.height)
            {
                action_(x, y);
            }
        }

        public void ResetAction()
        {
            clickAction_ = SelectCell;
            hoverDisplay.color = Color.white;

            foreach (CellBehaviour _cell in grid.cells)
            {
                _cell._display.color = Color.white;
            }
        }

        public void ChangeAction(ActionType actionType)
        {
            selectDisplay.gameObject.SetActive(false);
            clickAction_ = actions[actionType].Execute;
        }

        public void HoverCell(int x, int y)
        {
            CellBehaviour hoverCell = grid.cells[x, y];

            hoverDisplay.transform.position = hoverCell.transform.position;
        }

        public void SelectCell(int x, int y)
        {
            selectedCell = grid.cells[x, y];

            selectDisplay.gameObject.SetActive(true);
            selectDisplay.transform.position = selectedCell.transform.position;
        }

        //public void MoveToTile(int x, int y)
        //{
        //    player.position = grid.cells[x, y].transform.position;
        //}

        //public void SetTilesToAction(Vector3 playerPosition, int area)
        //{
        //    int x, y;
        //    ConvertPositionToGridCoordinates(playerPosition, out x, out y);

        //    int minX, minY, maxX, maxY;
        //    minX = Mathf.Clamp(x - area, 0, width);
        //    maxX = Mathf.Clamp(x + area, 0, width);
        //    minY = Mathf.Clamp(y - area, 0, height);
        //    maxY = Mathf.Clamp(y + area, 0, height);

        //    for (int _x = 0; _x < width; _x++)
        //    {
        //        for (int _y = 0; _y < height; _y++)
        //        {
        //            cells[_x, _y].inActionArea = (_x >= minX && _y >= minY && _x < maxX && _y < maxY) ? true : false;
        //        }
        //    }
        //}
    } 
}
