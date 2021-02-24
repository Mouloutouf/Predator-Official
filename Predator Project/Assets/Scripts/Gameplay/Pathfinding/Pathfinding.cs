using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class Pathfinding : MonoBehaviour
    {
        private const int Move_Line_Cost = 10;
        private const int Move_Diagonal_Cost = 14;

        private int Width { get => Grid.instance._width; }
        private int Height { get => Grid.instance._height; }

        private List<PathNode> openList = new List<PathNode>();
        private List<PathNode> closedList = new List<PathNode>();

        public List<PathNode> displayList { get; private set; } = new List<PathNode>();

        public bool foundPath { get; private set; }
        private float timer { get { if (debugMode) return 0.01f; else return 0.0f; } }

        public List<PathNode> allPathNodes = new List<PathNode>();

        public List<PathNode> Path { get; private set; } = null;

        private PathNodeDisplay displayTemplate { get => Grid.instance.GetCell(0, 0).pathNode.nodeDisplay; }

        public bool debugMode;

        public PathNode GetNode(int x, int y)
        {
            Cell cell = Grid.instance.GetCell(x, y);
            return cell.pathNode;
        }

        public void DisplayNode(PathNode node, Color color)
        {
            node.nodeDisplay.colorImage.color = color;
        }
        public void DisplayNodes(List<PathNode> nodes, Color color)
        {
            foreach (PathNode node in nodes) node.nodeDisplay.colorImage.color = color;
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            DisplayNodes(displayList, displayTemplate.greyColor); // Display all discovered path nodes in grey

            List<PathNode> path = new List<PathNode> { endNode };

            PathNode currentNode = endNode;

            while (currentNode.previousNode != null)
            {
                DisplayNode(currentNode, displayTemplate.greenColor); // Display currentNode in green

                path.Add(currentNode);
                currentNode = currentNode.previousNode;
            }
            path.Reverse();

            return path;
        }

        public void ResetNodesDisplay(List<PathNode> pathNodeList)
        {
            foreach (PathNode pathNode in pathNodeList)
            {
                Color transparent = Color.white; transparent.a = 0.0f;
                pathNode.nodeDisplay.colorImage.color = transparent;

                pathNode.nodeDisplay.gText.text = pathNode.nodeDisplay.hText.text = pathNode.nodeDisplay.FText.text = "";
            }
        }

        public IEnumerator FindPath(int startX, int startY, int endX, int endY)
        {
            Path = new List<PathNode>();
            foundPath = false;

            ResetNodesDisplay(displayList);

            PathNode startNode = GetNode(startX, startY);
            PathNode endNode = GetNode(endX, endY);

            openList = new List<PathNode> { startNode };
            closedList.Clear();

            displayList = new List<PathNode> { startNode };

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
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
                PathNode currentNode = GetLowestFCostNode(openList);

                DisplayNode(currentNode, displayTemplate.greenColor); // Display current node in green

                Debug.Log("Moving To Node : " + currentNode.ToString());

                if (currentNode == endNode)
                {
                    /// Input to end the search
                    /// //WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                    yield return new WaitForSeconds(timer);

                    Debug.Log("Reached the end of the path !");

                    // Reached the End
                    foundPath = true;
                    Path = CalculatePath(endNode);
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

                    DisplayNode(neighbourNode, displayTemplate.blueColor); // Display neighbour node in blue

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

                    if (!displayList.Contains(neighbourNode)) displayList.Add(neighbourNode);
                }

                /// Input to continue the while
                yield return new WaitForSeconds(timer);

                DisplayNode(currentNode, displayTemplate.redColor); // Display current node in red
            }

            // Out of nodes to search
            Debug.Log("Could not find a path :/");
            Path = null;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a._x - b._x);
            int yDistance = Mathf.Abs(a._y - b._y);
            int distance = Mathf.Abs(xDistance - yDistance);

            return Move_Diagonal_Cost * Mathf.Min(xDistance, yDistance) + Move_Line_Cost * distance;
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
                if (currentNode._y + 1 < Height) neighbourList.Add(GetNode(currentNode._x - 1, currentNode._y + 1));
            }
            if (currentNode._x + 1 < Width)
            {
                // Right
                neighbourList.Add(GetNode(currentNode._x + 1, currentNode._y));
                // Right Down
                if (currentNode._y - 1 >= 0) neighbourList.Add(GetNode(currentNode._x + 1, currentNode._y - 1));
                // Right Up
                if (currentNode._y + 1 < Height) neighbourList.Add(GetNode(currentNode._x + 1, currentNode._y + 1));
            }
            // Down
            if (currentNode._y - 1 >= 0) neighbourList.Add(GetNode(currentNode._x, currentNode._y - 1));
            // Up
            if (currentNode._y + 1 < Height) neighbourList.Add(GetNode(currentNode._x, currentNode._y + 1));

            return neighbourList;
        }
    }
}
