using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class MovementAction : Action
    {
        public int maxDistance;

        public override void EnableAction(bool active)
        {
            inputManager.hoverDisplay.color = actionColor;

            player.GetPlayerPosition(out int pX, out int pY);

            for (int x = pX - maxDistance; x <= pX + maxDistance; x++)
            {
                for (int y = pY - maxDistance; y <= pY + maxDistance; y++)
                {
                    if (x >= 0 && y >= 0 && x < grid.width && y < grid.height)
                    {
                        Vector3 destination = new Vector3(x, y) - new Vector3(pX, pY);
                        int distance = (int)(Mathf.Abs(destination.x) + Mathf.Abs(destination.y));

                        if (distance <= maxDistance)
                        {
                            CellBehaviour cell = grid.cells[x, y];

                            if (cell.enemy == null) cell.SetToActionArea(cell.moveAreaColor);
                        }
                    }
                }
            }

            base.EnableAction(active);
        }

        public override void Execute(int x, int y)
        {
            player.GetPlayerPosition(out int pX, out int pY);
            Vector3 destination = new Vector3(x, y) - new Vector3(pX, pY);
            int distance = (int)(Mathf.Abs(destination.x) + Mathf.Abs(destination.y));

            if (distance <= maxDistance)
            {
                grid.cells[pX, pY].player = null;

                player.playerDisplay.transform.position = grid.cells[x, y].transform.position;

                grid.cells[x, y].player = player;

                foreach (CellBehaviour _cell in grid.cells)
                {
                    _cell._display.color = Color.white;
                }

                player.currentPoints--;
                player.currentEnergy -= energyCost;

                if (player.currentPoints > 0) EnableAction(true);
            }
        }
    } 
}
