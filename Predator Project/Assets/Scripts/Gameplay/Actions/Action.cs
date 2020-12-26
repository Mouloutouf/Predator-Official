using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Predator
{
    public enum ActionType { Move, StealthKill, Heal }

    [System.Serializable]
    public class ActionEvent : UnityEvent<ActionType> { }

    public abstract class Action : MonoBehaviour
    {
        protected abstract ActionType actionType { get; set; }

        public Toggle actionToggle;
        public Color actionColor;

        public const int actionCost = 1;
        public float energyCost;
        public int range;

        public PlayerManager player;
        protected int pX, pY;
        public InputManager inputManager;

        public Grid grid { get; set; }

        protected List<Cell> enabledCells = new List<Cell>();
        protected Cell selectedCell = null;

        void Start()
        {
            grid = Grid.instance;
        }

        public void EnableAction(bool enable)
        {
            if (enable)
            {
                player.GetPlayerPosition(out pX, out pY);

                for (int x = pX - range; x <= pX + range; x++)
                {
                    for (int y = pY - range; y <= pY + range; y++)
                    {
                        if (grid.IsInsideGrid(x, y))
                        {
                            EnableAt(x, y);
                        }
                    }
                }

                inputManager.ChangeAction(actionType);
                inputManager.hoverDisplay.color = actionColor;
            }
            else
            {
                inputManager.ResetAction();
            }
        }
        protected abstract void EnableAt(int x, int y);

        public void ExecuteAction(int x, int y)
        {
            // Get the selected cell and check if it is valid, then execute
            selectedCell = grid._cells[x, y];

            bool execute = enabledCells.Contains(selectedCell);
            if (execute)
            {
                ExecuteAt(x, y);

                ResetAction();
            }
        }
        protected abstract void ExecuteAt(int x, int y);

        protected void ResetAction()
        {
            enabledCells.Clear();

            foreach (Cell _cell in grid._cells) _cell._actionDisplay.color = _cell.ChangeActionDisplay(_cell._environment.Color);

            player._CurrentPoints -= actionCost;

            // re enables the same action if the player has any action points left
            if (player._CurrentPoints > 0) EnableAction(true);
        }
    } 
}
