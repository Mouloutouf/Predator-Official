using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public abstract class Movement : MonoBehaviour
    {
        public abstract void MoveTo(int x, int y);
    }

    public class PathMovement : Movement
    {
        public EnemyManager enemy;

        public bool isMoving { get; set; }

        public float waitTime;
        private float currentTime;

        private List<Vector2Int> positionsInPath = new List<Vector2Int>();
        private int posIndex;

        public override void MoveTo(int x, int y)
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
                return;
            }

            int _x = positionsInPath[posIndex].x;
            int _y = positionsInPath[posIndex].y;
            enemy.enemyDisplay.transform.position = Grid.instance._cells[_x, _y].transform.position;

            enemy.SetEnemyCell();

            enemy.gameManager.CheckAtEachAction();

            posIndex++;
        }
    } 
}
