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
        public GameManager gameManager { get; set; }

        public Image enemyDisplay;

        public float energyAmount;

        void OnEnable() => AIManager.enemies.Add(this);
        void OnDisable() => AIManager.enemies.Remove(this);

        public Status status { get; set; }

        public Orientations orientation;

        public DetectionBehavior detectionBehavior;

        public PatrolBehavior patrolBehavior;

        public bool waitNext { get; set; }

        public int actionAmount;
        private int actionIndex;

        public float waitTime;
        private float currentTime;

        public Cell enemyCell { get; set; }

        void Start()
        {
            status = Status.Normal;
        }

        public void GetEnemyPosition(out int x, out int y)
        {
            Grid.instance.ConvertWorldPositionToGrid(enemyDisplay.transform.position, out x, out y);
        }

        public void StartEnemy()
        {
            if (status == Status.Dead) { AIManager.waitNext = true; return; }

            actionIndex = 0;
            waitNext = true;
        }

        private void Execute(PatrolBehavior behavior)
        {
            if (behavior.path.Count == 0) { AIManager.waitNext = true; return; }

            behavior.DoMovement();
            actionIndex++;
        }

        public void Next()
        {
            waitNext = false;
            currentTime = waitTime;

            if (actionIndex < actionAmount)
            {
                Debug.Log("Next Move, at action : " + actionIndex);
                Execute(patrolBehavior);
            }

            else AIManager.waitNext = true;
        }

        void Update()
        {
            if (waitNext)
            {
                if (currentTime <= 0.0f)
                {
                    Next();
                }
                currentTime -= Time.deltaTime; 
            }
        }

        public void SetEnemyCell()
        {
            if (enemyCell != null) enemyCell._enemy = null;

            GetEnemyPosition(out int _x, out int _y);
            enemyCell = Grid.instance._cells[_x, _y];
            enemyCell._enemy = this;

            if (detectionBehavior != null) detectionBehavior.DetectCells();
        }

        private void SetEnemyOrientation()
        {

        }

        public void Die()
        {
            detectionBehavior.ResetDetectedCells();

            enemyDisplay.color = Color.black;
            status = Status.Dead;
        }
    } 
}
