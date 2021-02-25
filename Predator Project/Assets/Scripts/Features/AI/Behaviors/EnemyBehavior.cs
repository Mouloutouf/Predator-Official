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
}
