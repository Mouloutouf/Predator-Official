using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Predator
{
    public class AIManager : MonoBehaviour
    {
        public GameManager gameManager;

        public Transform aIInterface;

        public static List<EnemyManager> enemies = new List<EnemyManager>();

        public static bool next { get; set; }

        private float waitTime = 0.2f;
        private float currentTime;

        private int currentIndex;

        public bool aIIsActive { get; set; }

        void Start()
        {
            foreach (EnemyManager enemy in enemies)
            {
                enemy.gameManager = gameManager;

                enemy.UpdateEnemy();
            }
        }

        public void StartAI()
        {
            currentIndex = 0;
            StartEnemy(currentIndex);
        }

        void Update()
        {
            if (aIIsActive)
            {
                if (next)
                {
                    if (currentTime <= 0.0f)
                    {
                        currentTime = waitTime;

                        NextEnemy();
                    }
                    currentTime -= Time.deltaTime; 
                }
            }
        }

        private void NextEnemy()
        {
            next = false;

            if (currentIndex < enemies.Count)
            {
                StartEnemy(currentIndex);
            }
            else gameManager.ChangeTurn();
        }
        private void StartEnemy(int index)
        {
            Debug.Log("Starting Enemy : " + enemies[index]);
            enemies[index].StartEnemy();
            currentIndex++;
        }
    } 
}
