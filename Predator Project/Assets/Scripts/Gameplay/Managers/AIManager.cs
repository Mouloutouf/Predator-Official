using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Predator
{
    public class AIManager : MonoBehaviour
    {
        public bool aIIsActive { get; set; }

        private float waitTime = 0.2f;
        private float currentTime;

        public GameManager gameManager;

        public Transform aIInterface;

        public static List<EnemyManager> enemies = new List<EnemyManager>();
        private int enemyIndex;

        public static bool waitNext { get; set; }

        void Start()
        {
            foreach (EnemyManager enemy in enemies)
            {
                enemy.gameManager = gameManager;

                enemy.SetEnemyCell();
            }
        }

        void Update()
        {
            if (aIIsActive && waitNext)
            {
                if (currentTime <= 0.0f)
                {
                    NextEnemy();
                }
                currentTime -= Time.deltaTime;
            }
        }

        private void NextEnemy()
        {
            waitNext = false;
            currentTime = waitTime;

            if (enemyIndex < enemies.Count)
            {
                Debug.Log("Next Enemy : " + enemies[enemyIndex]);
                enemies[enemyIndex].StartEnemy();
                enemyIndex++;
            }
            else gameManager.ChangeTurn();
        }

        public void SetAITurn()
        {
            enemyIndex = 0;
            waitNext = true;
        }
    } 
}
