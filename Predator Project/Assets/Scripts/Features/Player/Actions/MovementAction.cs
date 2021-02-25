using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class MovementAction : PlayerAction
    {
        protected override ActionType actionType { get; set; } = ActionType.Move;

        protected override void EnableAt(int x, int y)
        {
            Vector3 destination = new Vector3(x, y) - new Vector3(pX, pY);
            int distance = (int)(Mathf.Abs(destination.x) + Mathf.Abs(destination.y));

            if (distance <= range)
            {
                Cell cell = grid._cells[x, y];
                if ((cell._enemy == null || cell._enemy.status == Status.Dead) && cell._player == null && cell._environment.EnviroType != EnvironmentType.Wall)
                {
                    cell.SetToActionArea(actionColor);
                    enabledCells.Add(cell);
                }
            }
        }

        protected override void ExecuteAt(int x, int y)
        {
            // Player movement
            player.playerDisplay.transform.position = grid._cells[x, y].transform.position;

            // Changes the cells player contents
            player.UpdatePlayer();

            // Removes a certain amount of energy
            player._CurrentEnergy -= energyCost;
        }
    } 
}
