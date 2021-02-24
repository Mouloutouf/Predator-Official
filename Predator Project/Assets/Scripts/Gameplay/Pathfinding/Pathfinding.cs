using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class Pathfinding
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        private int _width;
        private int _height;

        private List<PathNode> openList;
        private List<PathNode> closedList;

        public Pathfinding(int width, int height)
        {
            _width = width;
            _height = height;

            Debug.Log(_width + " & " + _height);
        }

        public PathNode GetNode(int x, int y)
        {
            Debug.Log(x + ", " + y);
            Cell cell = Grid.instance.GetCell(x, y);
            return cell.pathNode;
        }

        private List<PathNode> CalculatePath(PathNode enddNode)
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(enddNode);
            PathNode currentNode = enddNode;

            while (currentNode.previousNode != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.previousNode;
            }
            path.Reverse();

            return path;
        }

        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = GetNode(startX, startY);
            PathNode enddNode = GetNode(endX, endY);

            openList = new List<PathNode> { startNode };
            closedList = new List<PathNode>();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    PathNode pathNode = GetNode(x, y);

                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.previousNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, enddNode);
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);

                if (currentNode == enddNode)
                {
                    // Reached the End
                    return CalculatePath(enddNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    if (closedList.Contains(neighbourNode)) continue;

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.previousNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, enddNode);
                        neighbourNode.CalculateFCost();

                        if (!openList.Contains(neighbourNode)) openList.Add(neighbourNode);
                    }
                }
            }

            // Out of nodes in the Open List
            return null;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a._x - b._x);
            int yDistance = Mathf.Abs(a._y - b._y);
            int distance = Mathf.Abs(xDistance - yDistance);

            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * distance;
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostNode = pathNodeList[0];

            for (int i = 0; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].FCost < lowestFCostNode.FCost)
                {
                    lowestFCostNode = pathNodeList[i];
                }
            }

            return lowestFCostNode;
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            if (currentNode._x - 1 >= 0)
            {
                // Left
                neighbourList.Add(GetNode(currentNode._x - 1, currentNode._y));
                // Left Down
                if (currentNode._y - 1 >= 0) neighbourList.Add(GetNode(currentNode._x - 1, currentNode._y - 1));
                // Left Up
                if (currentNode._y + 1 < _height) neighbourList.Add(GetNode(currentNode._x - 1, currentNode._y + 1));
            }
            if (currentNode._x + 1 < _width)
            {
                // Right
                neighbourList.Add(GetNode(currentNode._x + 1, currentNode._y));
                // Right Down
                if (currentNode._y - 1 >= 0) neighbourList.Add(GetNode(currentNode._x + 1, currentNode._y - 1));
                // Right Up
                if (currentNode._y + 1 < _height) neighbourList.Add(GetNode(currentNode._x + 1, currentNode._y + 1));
            }
            // Down
            if (currentNode._y - 1 >= 0) neighbourList.Add(GetNode(currentNode._x, currentNode._y - 1));
            // Up
            if (currentNode._y + 1 < _height) neighbourList.Add(GetNode(currentNode._x, currentNode._y + 1));

            return neighbourList;
        }
    }
}
