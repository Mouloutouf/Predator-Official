﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Predator
{
    public class EnemyManager : MonoBehaviour
    {
        public Image enemyDisplay;

        public float energyAmount;

        void OnEnable() => AIManager.enemies.Add(this);
        void OnDisable() => AIManager.enemies.Remove(this);

        public void GetEnemyPosition(out int x, out int y)
        {
            Grid.instance.ConvertWorldPositionToGrid(enemyDisplay.transform.position, out x, out y);
        }

        public void Die()
        {
            enemyDisplay.color = Color.black;
        }
    } 
}
