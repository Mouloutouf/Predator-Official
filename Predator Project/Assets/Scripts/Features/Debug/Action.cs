using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public abstract class Action
    {
        public int actionCost;

        public virtual void Do()
        {
            Update();
        }
        protected abstract void Update();
    }

    public class MoveAction : Action
    {
        public EnemyManager enemy;
        public int x, y;

        public override void Do()
        {
            Move(x, y);

            Update();
        }
        protected override void Update()
        {
            enemy.gameManager.UpdateCheckEnemies();
        }

        private void Move(int x, int y)
        {
            Debug.Log("Movement");

            enemy.orientation = ChangeOrientation(x, y);

            enemy.characterDisplay.transform.position = Grid.instance._cells[x, y].transform.position;
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
