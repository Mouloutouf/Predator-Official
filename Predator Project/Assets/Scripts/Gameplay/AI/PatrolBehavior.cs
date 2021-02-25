using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Predator
{
    [System.Serializable]
    public class Waypoint
    {
        public Transform point;

        public (int, int) pos
        {
            get {
                (int, int) _pos;
                Grid.instance.ConvertWorldPositionToGrid(point.position, out _pos.Item1, out _pos.Item2);
                return _pos;
            }
        }
    }

    public class PatrolBehavior : MonoBehaviour
    {
        public List<Waypoint> path;

        public EnemyManager enemy;
        public Movement movement;

        private int pointIndex;

        public void StartMovement()
        {
            if (pointIndex >= path.Count) pointIndex = 0;

            movement.MoveTo(path[pointIndex].pos.Item1, path[pointIndex].pos.Item2);
            pointIndex++;
        }
    }
}
