using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class AssassinateAction : Action
    {
        public int range;

        public override void EnableAction(bool active)
        {
            inputManager.hoverDisplay.color = actionColor;

            player.GetPlayerPosition(out int pX, out int pY);

            for (int x = pX - range; x <= pX + range; x++)
            {
                for (int y = pY - range; y <= pY + range; y++)
                {
                    if (x >= 0 && y >= 0 && x < grid.width && y < grid.height)
                    {
                        CellBehaviour cell = grid.cells[x, y];

                        cell.SetToActionArea(cell.attackAreaColor);
                    }
                }
            }

            base.EnableAction(active);
        }

        public override void Execute(int x, int y)
        {
            CellBehaviour cell = grid.cells[x, y];

            player.GetPlayerPosition(out int pX, out int pY);

            bool inRange = x >= pX - range && y >= pY - range && x <= pX + range && y <= pY + range;

            if (cell.enemy != null && inRange)
            {
                cell.enemy.enemyDisplay.transform.position = player.playerDisplay.transform.position;
                cell.enemy.Die();
                cell.enemy = null;

                foreach (CellBehaviour _cell in grid.cells)
                {
                    _cell._display.color = Color.white;
                }

                EnableAction(true);
            }
        }
    } 
}
