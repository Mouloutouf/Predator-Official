using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class Bresenham : MonoBehaviour
    {
        Grid grid { get => Grid.instance; }
        private List<PathNode> displayList = new List<PathNode>();

        public void Line(int startX, int startY, int endX, int endY)
        {
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

            PutPixel(startX, startY);

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

                PutPixel(currentX, currentY);
            }

            PutPixel(endX, endY);
        }
        private void PutPixel(int x, int y)
        {
            PathNode node = grid._cells[x, y].pathNode;
            displayList.Add(node);
            grid.pathfinding.DisplayNode(node, node.nodeDisplay.greenColor);
        }
        private void ClearAllPixels()
        {
            grid.pathfinding.ResetNodesDisplay(displayList);
        }
    }
}
