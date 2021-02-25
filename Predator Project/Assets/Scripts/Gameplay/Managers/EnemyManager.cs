using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Predator
{
    public enum Status { Normal, Searching, Scared, Dead }

    public enum Orientations { Up, Right, Down, Left }

    public class EnemyManager : MonoBehaviour
    {
        public Status status { get; set; }
        public GameManager gameManager { get; set; }

        public DetectionBehavior detectionBehavior;
        public PatrolBehavior patrolBehavior;

        public Image enemyDisplay;

        public Orientations orientation;

        public float energyAmount;
        
        void OnEnable() => AIManager.enemies.Add(this);
        void OnDisable() => AIManager.enemies.Remove(this);

        public bool next { get; set; }

        public int actionAmount;
        private int actionIndex;

        public float waitTime;
        private float currentTime;

        public Cell enemyCell { get; set; }

        public void GetEnemyPosition(out int x, out int y)
        {
            Grid.instance.ConvertWorldPositionToGrid(enemyDisplay.transform.position, out x, out y);
        }

        void Start()
        {
            status = Status.Normal;
        }

        #region Core
        public void StartEnemy()
        {
            if (status == Status.Dead) { AIManager.next = true; return; }

            actionIndex = 0;
            StartBehavior(patrolBehavior);
        }
        private void KillEnemy()
        {
            enemyDisplay.color = Color.black;
            status = Status.Dead;

            detectionBehavior.ClearDetectionArea();
        }
        public void Die() => KillEnemy();
        #endregion

        #region Behaviors
        void Update()
        {
            if (next)
            {
                if (currentTime <= 0.0f)
                {
                    currentTime = waitTime;

                    NextBehavior();
                }
                currentTime -= Time.deltaTime;
            }
        }

        public void NextBehavior()
        {
            next = false;

            if (actionIndex < actionAmount)
            {
                StartBehavior(patrolBehavior);
            }
            else AIManager.next = true;
        }
        private void StartBehavior(PatrolBehavior behavior)
        {
            if (behavior.path.Count == 0) { AIManager.next = true; return; }

            Debug.Log("Starting Behavior, at action : " + actionIndex);
            behavior.StartMovement();
            actionIndex++;
        }
        #endregion

        #region Update
        public void UpdateEnemy()
        {
            UpdatePosition();

            UpdateVision();
        }
        private void UpdatePosition()
        {
            if (enemyCell != null) enemyCell._enemy = null;

            GetEnemyPosition(out int _x, out int _y);
            enemyCell = Grid.instance._cells[_x, _y];
            enemyCell._enemy = this;
        }
        private void UpdateVision()
        {
            // Update Vision in one of 8 directions

            if (detectionBehavior != null) detectionBehavior.CreateDetectionArea();
        }
        #endregion
    }
}
