﻿using System.Collections;
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
                        Cell cell = grid.cells[x, y];

                        cell.SetToActionArea(cell.attackAreaColor);
                    }
                }
            }

            base.EnableAction(active);
        }

        public override void Execute(int x, int y)
        {
            Cell cell = grid.cells[x, y];

            player.GetPlayerPosition(out int pX, out int pY);

            bool inRange = x >= pX - range && y >= pY - range && x <= pX + range && y <= pY + range;

            if (cell.enemy != null && inRange)
            {
                cell.enemy.enemyDisplay.transform.position = player.playerDisplay.transform.position;
                cell.enemy.Die();
                
                foreach (Cell _cell in grid.cells)
                {
                    _cell._actionDisplay.color = _cell.ChangeActionDisplay(_cell._environment.color);
                }

                player.currentPoints--;
                player.currentEnergy += cell.enemy.energyAmount;

                cell.enemy = null;

                if (player.currentPoints > 0) EnableAction(true);
            }
        }
    } 
}
