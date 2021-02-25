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
            EnemyManager enemy = selectedCell._enemy;

            // Kill the enemy
            enemy.Die();

            // Drag the enemy's body and changes the cells enemy contents
            enemy.characterDisplay.transform.position = player.playerDisplay.transform.position;
            player.GetPlayerPosition(out int _pX, out int _pY);
            grid._cells[_pX, _pY]._enemy = enemy;
            selectedCell._enemy = null;

            // Drains the enemy's energy
            player._CurrentEnergy += enemy.energyAmount;
        }
    } 
}
