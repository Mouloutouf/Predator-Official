using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class DetectionBehavior : MonoBehaviour
    {
        public EnemyManager enemy;

        public DetectionArea detectionArea;

        public List<Cell> DetectedCells { get => detectionArea.DetectedCells; }

        public void Initialize()
        {
            int x, y;
            enemy.GetEnemyPosition(out x, out y);

            Debug.Log("yes");
            Cell[] cellArray = { 
                Grid.instance._cells[x + 1, y], 
                Grid.instance._cells[x, y + 1], 
                Grid.instance._cells[x, y - 1] 
            };

            detectionArea.DetectedCells.Clear();
            detectionArea.DetectedCells.AddRange(cellArray);

            //detectionArea.CreateDetectionArea();
        }

        public void DetectionCheck()
        {
            foreach (Cell cell in DetectedCells)
            {
                if (cell._player != null)
                {
                    // Player is detected !
                    // Game Over (for now)
                    enemy.gameManager.GameOver();
                }
                else if (cell._enemy != null)
                {
                    if (cell._enemy.status == Status.Dead)
                    {
                        // Search Behavior
                        // Alert goes up
                        // Any other ideas you want to try
                    }
                }
            }
        }
    }
}
