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
        public Image enemyDisplay;

        public float energyAmount;

        void OnEnable() => AIManager.enemies.Add(this);
        void OnDisable() => AIManager.enemies.Remove(this);

        public Status status { get; set; }

        public Orientations orientation;

        public DetectionBehavior detectionBehavior;

        void Start()
        {
            status = Status.Normal;

            detectionBehavior.Initialize(this);
        }

        public void GetEnemyPosition(out int x, out int y)
        {
            Grid.instance.ConvertWorldPositionToGrid(enemyDisplay.transform.position, out x, out y);
        }

        public void Die()
        {
            enemyDisplay.color = Color.black;
            status = Status.Dead;
        }
    } 
}
