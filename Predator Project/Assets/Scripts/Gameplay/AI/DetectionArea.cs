using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class DetectionArea : MonoBehaviour
    {
        public DetectionBehavior detectionBehavior;

        public EnemyManager Enemy { get => detectionBehavior.enemy; }
        private int eX, eY;

        public List<Cell> DetectedCells { get; set; } = new List<Cell>();
        public List<Cell> ObstructedCells { get; set; } = new List<Cell>();

        public Orientations Orientation { get => Enemy.orientation; }

        public int maxDepth;

        public void CreateDetectionArea()
        {
            Enemy.GetEnemyPosition(out eX, out eY);

            DetectedCells.Clear();

            int visionLayer = Orientation == Orientations.Up || Orientation == Orientations.Right ? 1 : -1;
            int[] visionZone = new int[3] { -1, 0, 1 };

            CheckCellsAtVisionArea(eX, eY, visionLayer, visionZone, 1, false);
        }

        private void CheckCellsAtVisionArea(int _x, int _y, int visionLayer, int[] visionZone, int depth, bool obstacle)
        {
            int a, b, B;
            if (Orientation == Orientations.Right || Orientation == Orientations.Left) { a = _x; b = _y; }
            else { a = _y; b = _x; }

            a += visionLayer;
            B = b;

            for (int u = 0; u < visionZone.Length; u++)
            {
                b = B + visionZone[u];

                if (Orientation == Orientations.Right || Orientation == Orientations.Left) { _x = a; _y = b; }
                else { _x = b; _y = a; }

                if (Grid.instance.IsInsideGrid(_x, _y))
                {
                    Cell _cell = Grid.instance._cells[_x, _y];

                    if (obstacle) depth = RemoveObstructedCell(_x, _y, visionLayer, visionZone, depth, _cell);

                    else depth = AddVisibleCell(_x, _y, visionLayer, visionZone, depth, _cell);
                }
            }
        }

        private int AddVisibleCell(int _x, int _y, int visionLayer, int[] visionZone, int depth, Cell _cell)
        {
            if (_cell._environment.Visible)
            {
                if (!ObstructedCells.Contains(_cell))
                {
                    DetectedCells.Add(_cell);
                    _cell.SetToDetectionArea(detectionBehavior.detectionColor);

                    if (depth < maxDepth)
                    {
                        depth++;
                        CheckCellsAtVisionArea(_x, _y, visionLayer, visionZone, depth, false);
                    }
                }
            }
            else // Obstacle
            {
                CheckCellsAtVisionArea(_x, _y, visionLayer, visionZone, depth, true);
            }

            return depth;
        }

        private int RemoveObstructedCell(int _x, int _y, int visionLayer, int[] visionZone, int depth, Cell _cell)
        {
            ObstructedCells.Add(_cell);

            if (DetectedCells.Contains(_cell))
            {
                DetectedCells.Remove(_cell);
                _cell._detectionDisplay.color = _cell.ChangeActionDisplay(_cell._environment.Color);
            }

            if (depth < maxDepth)
            {
                depth++;
                CheckCellsAtVisionArea(_x, _y, visionLayer, visionZone, depth, true);
            }

            return depth;
        }
    } 
}
