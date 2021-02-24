using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class Pathfinding : MonoBehaviour
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        private int _width { get => Grid.instance._width; }
        private int _height { get => Grid.instance._height; }

        private List<PathNode> openList = new List<PathNode>();
        private List<PathNode> closedList = new List<PathNode>();

        private List<PathNode> discoveredList = new List<PathNode>();

        public bool foundPath { get; private set; }
        private float timer = 0.01f;

        public List<PathNode> _PathList { get; private set; } = null;

        public PathNode GetNode(int x, int y)
        {
            Cell cell = Grid.instance.GetCell(x, y);
            return cell.pathNode;
        }

        private List<PathNode> CalculatePath(PathNode enddNode)
        {
            // Display all discovered path nodes in grey
            foreach (PathNode pathNode in discoveredList) pathNode.nodeDisplay.colorImage.color = pathNode.nodeDisplay.greyColor;

            List<PathNode> path = new List<PathNode>();
            path.Add(enddNode);
            PathNode currentNode = enddNode;

            while (currentNode.previousNode != null)
            {
                // Display currentNode in green
                currentNode.nodeDisplay.colorImage.color = currentNode.nodeDisplay.greenColor;

                path.Add(currentNode);
                currentNode = currentNode.previousNode;
            }
            path.Reverse();

            return path;
        }

        public void ResetDiscoveredNodes()
        {
            foreach (PathNode pathNode in discoveredList)
            {
                pathNode.nodeDisplay.colorImage.color = Color.white;
                pathNode.nodeDisplay.gText.text = "";
                pathNode.nodeDisplay.hText.text = "";
                pathNode.nodeDisplay.FText.text = "";
            }
        }

        public IEnumerator FindPath(int startX, int startY, int endX, int endY)
        {
            _PathList = new List<PathNode>();
            foundPath = false;

            ResetDiscoveredNodes();

            PathNode startNode = GetNode(startX, startY);
            PathNode endNode = GetNode(endX, endY);

            openList = new List<PathNode> { startNode };
            closedList.Clear();

            discoveredList = new List<PathNode> { startNode };

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

            // Display G Cost Value
            startNode.gCost = 0; startNode.nodeDisplay.gText.text = startNode.gCost.ToString();
            // Display H Cost Value
            startNode.hCost = CalculateDistanceCost(startNode, endNode); startNode.nodeDisplay.hText.text = startNode.hCost.ToString();
            // Display F Cost Value
            startNode.CalculateFCost(); startNode.nodeDisplay.FText.text = startNode.FCost.ToString();

            while (openList.Count > 0)
            {
                // Display current node in green

                PathNode currentNode = GetLowestFCostNode(openList);

                currentNode.nodeDisplay.colorImage.color = currentNode.nodeDisplay.greenColor;

                Debug.Log("Moving To Node : " + currentNode.ToString());

                if (currentNode == endNode)
                {
                    /// Input to end the search
                    /// //WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                    yield return new WaitForSeconds(timer);

                    Debug.Log("Reached the end of the path !");

                    // Reached the End
                    foundPath = true;
                    _PathList = CalculatePath(endNode);
                    yield break;
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    /// Input to go through for each
                    yield return new WaitForSeconds(timer);

                    Debug.Log("Checking Neighbour Node : " + neighbourNode.ToString());

                    if (closedList.Contains(neighbourNode)) continue;

                    if (neighbourNode.obstacle)
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    // Display neighbour node in blue
                    neighbourNode.nodeDisplay.colorImage.color = neighbourNode.nodeDisplay.blueColor;

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.previousNode = currentNode;
                        // Display G Cost Value
                        neighbourNode.gCost = tentativeGCost; neighbourNode.nodeDisplay.gText.text = neighbourNode.gCost.ToString();
                        // Display H Cost Value
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode); neighbourNode.nodeDisplay.hText.text = neighbourNode.hCost.ToString();
                        // Display F Cost Value
                        neighbourNode.CalculateFCost(); neighbourNode.nodeDisplay.FText.text = neighbourNode.FCost.ToString();

                        if (!openList.Contains(neighbourNode)) openList.Add(neighbourNode);
                    }

                    if (!discoveredList.Contains(neighbourNode)) discoveredList.Add(neighbourNode);
                }

                /// Input to continue the while
                yield return new WaitForSeconds(timer);

                // Display current node in red
                currentNode.nodeDisplay.colorImage.color = currentNode.nodeDisplay.redColor;
            }

            // Out of nodes to search
            Debug.Log("Could not find a path :/");
            _PathList = null;
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
