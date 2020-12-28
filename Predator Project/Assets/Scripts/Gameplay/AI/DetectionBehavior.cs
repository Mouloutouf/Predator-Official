using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class DetectionBehavior : MonoBehaviour
    {
        public EnemyManager enemy;
        private int eX, eY;

        public List<Cell> DetectedCells { get; set; } = new List<Cell>();
        public List<Cell> ObstructedCells { get; set; } = new List<Cell>();

        public Orientations Orientation { get; set; }

        public int range;

        public void OnEnemyMove()
        {
            enemy.GetEnemyPosition(out eX, out eY);

            DetectedCells.Clear();
            DetectionArea();

            DetectionCheck();
        }

        void DetectionArea()
        {
            int x = eX;
            int y = eY;

            int layer = Orientation == Orientations.Up || Orientation == Orientations.Right ? 1 : -1;
            int[] zone = new int[3] { -1, 0, 1 };

            GetDetectedCells(x, y, layer, zone, 1);
        }

        private void GetDetectedCells(int _x, int _y, int layer, int[] zone, int recursion)
        {
            int a, b, B;
            if (Orientation == Orientations.Right || Orientation == Orientations.Left) { a = _x; b = _y; }
            else { a = _y; b = _x; }

            a += layer;
            B = b;

            for (int u = 0; u < zone.Length; u++)
            {
                b = B + zone[u];

                Cell _cell;
                if (Orientation == Orientations.Right || Orientation == Orientations.Left) _cell = Grid.instance._cells[a, b];
                else _cell = Grid.instance._cells[b, a];


                if (_cell._environment.Visible)
                {
                    if (!ObstructedCells.Contains(_cell))
                    {
                        DetectedCells.Add(_cell);

                        if (recursion < range)
                        {
                            recursion++;
                            GetDetectedCells(_x, _y, layer, zone, recursion);
                        }
                    }
                }
                else // Obstacle
                {
                    RemoveObstructedCells(_x, _y, layer, zone, recursion);
                }
            }
        }

        private void RemoveObstructedCells(int _x, int _y, int layer, int[] zone, int recursion)
        {
            int a, b, B;
            if (Orientation == Orientations.Right || Orientation == Orientations.Left) { a = _x; b = _y; }
            else { a = _y; b = _x; }

            a += layer;
            B = b;

            for (int u = 0; u < zone.Length; u++)
            {
                b = B + zone[u];

                Cell _cell;
                if (Orientation == Orientations.Right || Orientation == Orientations.Left) _cell = Grid.instance._cells[a, b];
                else _cell = Grid.instance._cells[b, a];

                ObstructedCells.Add(_cell);

                if (DetectedCells.Contains(_cell))
                {
                    DetectedCells.Remove(_cell);
                }

                if (recursion < range)
                {
                    recursion++;
                    RemoveObstructedCells(_x, _y, layer, zone, recursion);
                }
            }
        }

        public void DetectionCheck()
        {
            foreach (Cell cell in DetectedCells)
            {
                if (cell._player != null)
                {
                    // Player is detected !
                    // Game Over (for now)
                }
                else if (cell._enemy != null)
                {
                    if (cell._enemy.status == Status.Dead)
                    {
                        // Search Behavior
                        // Alert goes up
                        // Any other idea you want to try
                    }
                }
            }
        }
    } 
}
