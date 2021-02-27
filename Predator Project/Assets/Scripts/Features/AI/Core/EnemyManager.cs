using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Predator
{
    public enum Status { Normal, Searching, Scared, Dead }

    public enum Orientations { Up, Right, Down, Left, UpRight, DownRight, DownLeft, UpLeft }

    public class EnemyManager : MonoBehaviour
    {
        public Status status { get; set; }
        public GameManager gameManager { get; set; }

        public DetectionBehavior detectionBehavior;
        public PatrolBehavior patrolBehavior;

        public EnemyDisplay _enemyDisplay;
        public Image characterDisplay { get => _enemyDisplay.characterDisplay; }
        public Image visionConeDisplay { get => _enemyDisplay.visionConeDisplay; }

        public Orientations startOrientation;
        public Orientations orientation { get; set; }

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
            Grid.instance.ConvertWorldPositionToGrid(characterDisplay.transform.position, out x, out y);
        }

        #region Core
        public void InitEnemy()
        {
            status = Status.Normal;
            orientation = startOrientation;
        }
        public void StartEnemy()
        {
            if (status == Status.Dead) { AIManager.next = true; return; }

            actionIndex = 0;
            StartBehavior(patrolBehavior);
        }
        private void KillEnemy()
        {
            characterDisplay.color = Color.black;
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

            UpdateVision(orientation);
        }
        private void UpdatePosition()
        {
            if (enemyCell != null) enemyCell._enemy = null;

            GetEnemyPosition(out int _x, out int _y);
            enemyCell = Grid.instance._cells[_x, _y];
            enemyCell._enemy = this;
        }
        private void UpdateVision(Orientations orientation)
        {
            ChangeVisionConeAngle(orientation);

            detectionBehavior.CreateDetectionArea();
        }
        private void ChangeVisionConeAngle(Orientations orientation)
        {
            Vector3 newRotation = visionConeDisplay.transform.rotation.eulerAngles;

            switch (orientation)
            {
                case Orientations.Up: newRotation.z = 180; break;
                case Orientations.Right: newRotation.z = 90; break;
                case Orientations.Down: newRotation.z = 0; break;
                case Orientations.Left: newRotation.z = -90; break;
                case Orientations.UpRight: newRotation.z = 135; break;
                case Orientations.DownRight: newRotation.z = 45; break;
                case Orientations.DownLeft: newRotation.z = -45; break;
                case Orientations.UpLeft: newRotation.z = -135; break;
                default: break;
            }

            Quaternion quaternion = Quaternion.identity;
            quaternion.eulerAngles = newRotation;

            visionConeDisplay.transform.rotation = quaternion;
        }
        #endregion
    }
}
