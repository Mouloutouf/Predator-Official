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

        public Color detectionColor;

        public void DetectCells()
        {
            int x, y;
            enemy.GetEnemyPosition(out x, out y);

            ResetDetectedCells();

            //if (Grid.instance.IsInsideGrid(x + 1, y))
            //{ detectionArea.DetectedCells.Add(Grid.instance._cells[x + 1, y]); Grid.instance._cells[x + 1, y].SetToDetectionArea(detectionColor); }
            //if (Grid.instance.IsInsideGrid(x - 1, y))
            //{ detectionArea.DetectedCells.Add(Grid.instance._cells[x - 1, y]); Grid.instance._cells[x - 1, y].SetToDetectionArea(detectionColor); }
            //if (Grid.instance.IsInsideGrid(x, y + 1))
            //{ detectionArea.DetectedCells.Add(Grid.instance._cells[x, y + 1]); Grid.instance._cells[x, y + 1].SetToDetectionArea(detectionColor); }
            //if (Grid.instance.IsInsideGrid(x, y - 1))
            //{ detectionArea.DetectedCells.Add(Grid.instance._cells[x, y - 1]); Grid.instance._cells[x, y - 1].SetToDetectionArea(detectionColor); }

            detectionArea.CreateDetectionArea();
        }

        public void ResetDetectedCells()
        {
            foreach (Cell _cell in detectionArea.DetectedCells) _cell._detectionDisplay.color = _cell.DisableDisplay();

            detectionArea.DetectedCells.Clear();
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
