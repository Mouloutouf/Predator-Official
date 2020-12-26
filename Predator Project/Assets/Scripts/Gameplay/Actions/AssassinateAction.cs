using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class AssassinateAction : Action
    {
        protected override ActionType actionType { get; set; } = ActionType.StealthKill;

        protected override void EnableAt(int x, int y)
        {
            Cell cell = grid._cells[x, y];
            cell.SetToActionArea(actionColor);

            if (cell._enemy != null)
            {
                enabledCells.Add(cell);
            }
        }

        protected override void ExecuteAt(int x, int y)
        {
            // Kill and drag the enemy's body
            selectedCell._enemy.Die();
            selectedCell._enemy.enemyDisplay.transform.position = player.playerDisplay.transform.position;

            // Drains the enemy's energy
            player._CurrentEnergy += selectedCell._enemy.energyAmount;

            // Removes the cell's enemy content
            selectedCell._enemy = null;
        }
    } 
}
