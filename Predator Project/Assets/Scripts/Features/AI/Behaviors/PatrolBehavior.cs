using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Predator
{
    [System.Serializable]
    public class Waypoint
    {
        public Transform point;

        public (int x, int y) pos
        {
            get {
                (int x, int y) _pos;
                Grid.instance.ConvertWorldPositionToGrid(point.position, out _pos.x, out _pos.y);
                return _pos;
            }
        }
    }

    public class PatrolBehavior : MonoBehaviour
    {
        public bool pathfinding;

        public List<Waypoint> path;

        public EnemyManager enemy;

        public bool isMoving { get; set; }

        public float waitTime;
        private float currentTime;

        private List<Vector2Int> positionsInPath = new List<Vector2Int>();
        private int posIndex;

        private int pointIndex;

        public void StartMovement()
        {
            if (pointIndex >= path.Count) pointIndex = 0;

            MoveTo(path[pointIndex].pos.x, path[pointIndex].pos.y);
            pointIndex++;
        }
        public void MoveTo(int x, int y)
        {
            if (pathfinding) StartCoroutine(StartPathfindingMove(x, y));

            else StartMove(x, y);
        }
        private IEnumerator StartPathfindingMove(int x, int y)
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

            Init();
        }
        private void StartMove(int x, int y)
        {
            positionsInPath.Clear();

            Debug.Log("destination Pos : " + x + " " + y);
            int eX, eY; enemy.GetEnemyPosition(out eX, out eY);
            Debug.Log("enemy Pos : " + eX + " " + eY);
            int deltaX, deltaY;

            int cX = eX; int cY = eY;

            Debug.Log("current Pos : " + cX + " " + cY);
            Debug.Log(!(cX == x && cY == y));

            while (!(cX == x && cY == y))
            {
                deltaX = x - cX;
                cX = deltaX > 0 ? cX + 1 : deltaX < 0 ? cX - 1 : cX;

                deltaY = y - cY;
                cY = deltaY > 0 ? cY + 1 : deltaY < 0 ? cY - 1 : cY;

                positionsInPath.Add(new Vector2Int(cX, cY));
                Debug.Log("path Pos : " + cX + " " + cY);
            }

            Init();
        }
        private void Init()
        {
            posIndex = 0;
            isMoving = true;
        }

        void Update()
        {
            if (isMoving)
            {
                if (currentTime <= 0.0f)
                {
                    currentTime = waitTime;

                    Move();
                }
                currentTime -= Time.deltaTime;
            }
        }

        private void Move()
        {
            Debug.Log("Movement");

            if (posIndex == positionsInPath.Count)
            {
                isMoving = false;
                enemy.next = true;
                return;
            }

            int _x = positionsInPath[posIndex].x;
            int _y = positionsInPath[posIndex].y;

            enemy.orientation = ChangeOrientation(_x, _y);

            enemy.characterDisplay.transform.position = Grid.instance._cells[_x, _y].transform.position;

            enemy.gameManager.UpdateCheckEnemies();

            posIndex++;
        }

        private Orientations ChangeOrientation(int x, int y)
        {
            int eX, eY; enemy.GetEnemyPosition(out eX, out eY);

            if (x > eX)
            {
                if (y > eY) return Orientations.UpRight;
                else if (y < eY) return Orientations.DownRight;
                else return Orientations.Right;
            }
            else if (x < eX)
            {
                if (y > eY) return Orientations.UpLeft;
                else if (y < eY) return Orientations.DownLeft;
                else return Orientations.Left;
            }
            else
            {
                if (y > eY) return Orientations.Up;
                else if (y < eY) return Orientations.Down;

                else return enemy.orientation;
            }
        }
    }
}
