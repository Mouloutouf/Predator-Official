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

        public Orientations Orientation { get => Enemy.orientation; }

        public Bresenham bresenham = new Bresenham();

        public VisionArea visionArea = new VisionArea();

        public void CreateDetectionArea()
        {
            Enemy.GetEnemyPosition(out eX, out eY);

            DetectedCells.Clear();

            BresenhamRayCheck();
        }

        private void BresenhamRayCheck()
        {
            foreach (Vector2Int position in visionArea.areaVisions[Orientation])
            {
                int pX = eX + position.x;
                int pY = eY + position.y;

                if (Grid.instance.GetCell(pX, pY) == null) continue;

                List<Cell> cellsInLine = bresenham.Line(eX, eY, pX, pY);

                bool obstructed = false;
                foreach (Cell cell in cellsInLine)
                {
                    if (!cell._environment.Visible) obstructed = true;
                }
                if (!obstructed)
                {
                    AddVisibleCell(pX, pY);
                }
            }
            AddVisibleCell(eX, eY);
        }
        private void AddVisibleCell(int x, int y)
        {
            Cell visibleCell = Grid.instance._cells[x, y];
            DetectedCells.Add(visibleCell);

            // Display
            visibleCell.SetToDetectionArea(detectionBehavior.detectionColor);
        }
    } 
}
