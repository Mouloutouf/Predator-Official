using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Predator
{
    public delegate void _Action_(int x, int y);

    public class InputManager : MonoBehaviour
    {
        public bool inputActive { get; set; }

        public Grid grid { get; private set; }

        private _Action_ clickAction_;

        public Image hoverDisplay;
        public Image selectDisplay;

        private Cell selectedCell;

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

            if (inputActive)
            {
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
        }

        private void Action(Vector2 worldPosition, _Action_ action_)
        {
            grid.ConvertWorldPositionToGrid(worldPosition, out int x, out int y);

            if (x >= 0 && y >= 0 && x < grid._width && y < grid._height)
            {
                action_(x, y);
            }
        }

        public void ResetAction()
        {
            clickAction_ = SelectCell;
            hoverDisplay.color = Color.white;

            foreach (Cell _cell in grid._cells) _cell._actionDisplay.color = _cell.DisableDisplay();
        }

        public void ChangeAction(ActionType actionType)
        {
            selectDisplay.gameObject.SetActive(false);
            clickAction_ = actions[actionType].ExecuteAction;
        }

        private void HoverCell(int x, int y)
        {
            Cell hoverCell = grid._cells[x, y];

            hoverDisplay.transform.position = hoverCell.transform.position;
        }

        private void SelectCell(int x, int y)
        {
            selectedCell = grid._cells[x, y];

            selectDisplay.gameObject.SetActive(true);
            selectDisplay.transform.position = selectedCell.transform.position;
        }
    } 
}
