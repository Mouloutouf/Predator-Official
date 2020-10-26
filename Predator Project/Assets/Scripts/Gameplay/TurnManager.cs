using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public Player player;

    public EnemyManager enemyManager;

    public static void PlayerTurn()
    {
        // Activate Player Controller
    }

    public static void EnemyTurn()
    {
        // Activate Enemy Manager
    }

    void Start()
    {
        PlayerTurn();
    }
}
