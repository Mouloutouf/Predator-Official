using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Predator
{
    public enum Status { Normal, Searching, Scared, Dead }

    public enum Orientations { Up, Right, Down, Left }

    public interface IBehavior
    {

    }

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

        void Start()
        {
            status = Status.Normal;

            if (detectionBehavior != null) detectionBehavior.Initialize();
        }

        public void GetEnemyPosition(out int x, out int y)
        {
            Grid.instance.ConvertWorldPositionToGrid(enemyDisplay.transform.position, out x, out y);
        }

        private void Do()
        {
            // Select Best Actions to Perform depending on the State of the Enemy

            // Execute the first Action with the appropriate Behavior
            Execute(new PatrolBehavior());
        }

        private void Execute(PatrolBehavior behavior)
        {
            // Execute the Behavior with the Action Data
        }

        void Update()
        {
            // when the behavior is finished (by a check condition)
            // -> move to the next Action with the appropriate Behavior
            if (true)
            {
                Execute(new PatrolBehavior());
            }
        }

        public void Die()
        {
            enemyDisplay.color = Color.black;
            status = Status.Dead;
        }
    } 
}
