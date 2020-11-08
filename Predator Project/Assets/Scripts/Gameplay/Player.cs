using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int actions;
    public int Actions { get => actions; set { actions = value; /*if (actions <= 0) TurnManager.EnemyTurn();*/ } }

    [SerializeField] private int health;
    public int Health { get => health; set { health = value; if (health <= 0) Death(); } }
    [SerializeField] private int energy;
    public int Energy { get => energy; set { energy = value; if (energy <= 0) Death(); } }
    
    public void Death()
    {
        // Player dies
    }
}
