using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class PathfindingMovement : Movement
    {
        public EnemyManager enemy;

        public bool isMoving { get; set; }

        public float waitTime;
        private float currentTime;

        private List<Vector2Int> positionsInPath = new List<Vector2Int>();
        private int posIndex;

        public override void MoveTo(int x, int y)
        {
            StartCoroutine(FindPath(x, y));
        }

        IEnumerator FindPath(int x, int y)
        {
            positionsInPath.Clear();

            int eX, eY; enemy.GetEnemyPosition(out eX, out eY);

            Debug.Log("Starting Path Finding !");

            StartCoroutine(Grid.instance.pathfinding.FindPath(eX, eY, x, y));
            yield return new WaitUntil(() => Grid.instance.pathfinding.foundPath);

            List<PathNode> path = Grid.instance.pathfinding.Path;

            Debug.Log("The Path has been Found !");

            // Debug Gizmos
            #region Debug
            if (path != null)
            {
                Vector3 offset = new Vector3(Grid.instance._width / 2, Grid.instance._height / 2);

                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(
                        (new Vector3(path[i]._x, path[i]._y)) + (Vector3.one * 0.5f) - offset,
                        (new Vector3(path[i + 1]._x, path[i + 1]._y)) + (Vector3.one * 0.5f) - offset,
                        Color.green,
                        10f
                    );
                }
            }
            #endregion

            foreach (PathNode pathNode in path)
            {
                positionsInPath.Add(new Vector2Int(pathNode._x, pathNode._y));
            }

            posIndex = 0;

            isMoving = true;
        }

        void Update()
        {
            if (isMoving)
            {
                if (currentTime <= 0.0f)
                {
                    MoveNext();
                }
                currentTime -= Time.deltaTime;
            }
        }

        private void MoveNext()
        {
            Debug.Log("Movement");

            currentTime = waitTime;

            if (posIndex == positionsInPath.Count)
            {
                isMoving = false;
                enemy.waitNext = true;

                Grid.instance.pathfinding.ResetNodesDisplay(Grid.instance.pathfinding.displayList);
                return;
            }

            int _x = positionsInPath[posIndex].x;
            int _y = positionsInPath[posIndex].y;
            enemy.enemyDisplay.transform.position = Grid.instance._cells[_x, _y].transform.position;

            enemy.gameManager.CheckAtEachAction();

            posIndex++;
        }
    }  
}
