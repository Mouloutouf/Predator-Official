using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    [System.Serializable]
    public class Waypoint
    {

    }

    public class PatrolBehavior : MonoBehaviour
    {
        public List<Waypoint> path;

        public EnemyManager enemy;


    }
}
