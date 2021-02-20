using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Predator
{
    public class AIManager : MonoBehaviour
    {
        public bool aIIsActive { get; set; }

        private float waitTime = 2f;
        private float currentTime;

        public GameManager gameManager;

        public Transform aIInterface;

        public static List<EnemyManager> enemies = new List<EnemyManager>();

        void Start()
        {
            foreach (EnemyManager enemy in enemies)
            {
                enemy.gameManager = gameManager;

                enemy.GetEnemyPosition(out int eX, out int eY);

                Grid.instance._cells[eX, eY]._enemy = enemy;
            }
        }

        void Update()
        {
            if (aIIsActive)
            {
                if (currentTime <= 0.0f)
                {
                    gameManager.ChangeTurn();
                }

                currentTime -= Time.deltaTime;
            }
        }

        public void SetAITurn() => currentTime = waitTime;
    } 
}
