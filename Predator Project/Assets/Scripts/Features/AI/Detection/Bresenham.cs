using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class Bresenham
    {
        Grid grid { get => Grid.instance; }
        private List<PathNode> displayList = new List<PathNode>();

        private List<Cell> capturedCells;

        public List<Cell> Line(int startX, int startY, int endX, int endY)
        {
            capturedCells = new List<Cell>();

            ClearAllPixels();
            displayList.Clear();

            int width = endX - startX;
            int height = endY - startY;

            int incrementX = width > 0 ? +1 : -1;
            int incrementY = height > 0 ? +1 : -1;

            width = Mathf.Abs(width); height = Mathf.Abs(height);

            bool isWidthLongest = width >= height;

            int _long = isWidthLongest ? width : height;
            int _short = isWidthLongest ? height : width;

            int currentLong = isWidthLongest ? startX : startY;
            int currentShort = isWidthLongest ? startY : startX;

            int incrementLong = isWidthLongest ? incrementX : incrementY;
            int incrementShort = isWidthLongest ? incrementY : incrementX;

            float fault = _long / 2;

            AddCell(startX, startY);

            for (int i = 0; i < _long; i++)
            {
                currentLong += incrementLong;
                fault -= _short;

                if (fault < 0)
                {
                    currentShort += incrementShort;
                    fault += _long;
                }

                int currentX = isWidthLongest ? currentLong : currentShort;
                int currentY = isWidthLongest ? currentShort : currentLong;

                AddCell(currentX, currentY);
            }

            AddCell(endX, endY);

            return capturedCells;
        }
        private void AddCell(int x, int y)
        {
            Cell cell = grid.GetCell(x, y);
            if (cell == null) return;

            PutPixel(cell);

            capturedCells.Add(cell);
        }
        private void PutPixel(Cell cell)
        {
            PathNode node = cell.pathNode;
            displayList.Add(node);
            grid.pathfinding.DisplayNode(node, node.nodeDisplay.greenColor);
        }
        private void ClearAllPixels()
        {
            grid.pathfinding.ResetNodesDisplay(displayList);
        }
    }
}
