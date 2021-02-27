using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public abstract class EnemyBehavior : MonoBehaviour
    {
        public EnemyManager enemy;

        public float waitTime;
        protected float currentTime;

        public List<EnemyAction> Actions { get; protected set; }

        public int actionCount { get => Actions.Count; }
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

                    DoAction();
                }
                currentTime -= Time.deltaTime;
            }
        }

        protected virtual void DoAction()
        {
            Actions[actionIndex].Do();
        }
    }

    public class EnemyMoveAction : EnemyAction
    {
        public EnemyBehavior behavior;
        private EnemyManager enemy { get => behavior.enemy; }

        public List<(int x, int y)> positions = new List<(int x, int y)>();
        private int positionIndex; private (int x, int y) currentPos { get => positions[positionIndex]; }

        public void Execute()
        {
            if (positionIndex >= positions.Count)
            {
                behavior.active = false; enemy.next = true; return;
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

        protected override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}
