using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public abstract class EnemyAction
    {
        public EnemyBehavior enemyBehavior;

        public abstract void Execute();
    }

    public abstract class EnemyBehavior : MonoBehaviour
    {
        public EnemyManager enemy;

        public float waitTime;
        protected float currentTime;

        public List<EnemyAction> enemyActions { get; protected set; }

        public int actionCount { get => enemyActions.Count; }
        protected int actionIndex;

        public bool active { get; set; }

        public virtual void StartBehavior()
        {
            if (actionIndex >= actionCount) actionIndex = 0;

            StartAt(actionIndex);
            actionIndex++;
        }
        protected abstract void StartAt(int index);

        void Update()
        {
            if (active)
            {
                if (currentTime <= 0.0f)
                {
                    currentTime = waitTime;

                    Do();
                }
                currentTime -= Time.deltaTime;
            }
        }

        protected virtual void Do()
        {
            enemyActions[actionIndex].Execute();
        }
    }

    public class EnemyMoveAction : EnemyAction
    {
        private EnemyManager enemy { get => enemyBehavior.enemy; }

        public List<(int x, int y)> positions = new List<(int x, int y)>();
        private int positionIndex; private (int x, int y) currentPos { get => positions[positionIndex]; }

        public override void Execute()
        {
            if (positionIndex >= positions.Count)
            {
                enemyBehavior.active = false; enemy.next = true; return;
            }

            Move(currentPos.x, currentPos.y);

            positionIndex++;
        }

        private void Move(int x, int y)
        {
            Debug.Log("Movement");

            enemy.orientation = ChangeOrientation(x, y);

            enemy.characterDisplay.transform.position = Grid.instance._cells[x, y].transform.position;

            enemy.gameManager.UpdateCheckEnemies();
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
