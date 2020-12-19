using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class AIManager : MonoBehaviour
    {
        public List<EnemyManager> enemies = new List<EnemyManager>();

        void Start()
        {
            foreach (EnemyManager enemy in enemies)
            {
                enemy.GetEnemyPosition(out int eX, out int eY);

                Grid.instance.cells[eX, eY].enemy = enemy;
            }
        }
    } 
}
