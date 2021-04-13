using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SoldierAction : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent actionEnd;

    public float waitTime;
    protected float currentTime;

    public bool active;

    [Min(1)]
    public int steps;
    protected int currentStep;

    public void StartAction()
    {
        active = true;
        currentTime = 0.0f;

        currentStep = 0;
    }

    void Update()
    {
        if (active)
        {
            if (currentTime <= 0.0f)
            {
                currentTime = waitTime;

                Next();
            }
            currentTime -= Time.deltaTime;
        }
    }

    public abstract void Do();
    public void Next()
    {
        if (currentStep >= steps) { actionEnd.Invoke(); return; }

        Do();

        currentStep++;
    }
}

namespace Predator
{
    public class SoldierMoveAction : SoldierAction
    {
        public EnemyManager enemy;

        public List<Vector2Int> positionsInPath = new List<Vector2Int>();

        public override void Do()
        {
            Debug.Log("Movement");

            int _x = positionsInPath[currentStep].x;
            int _y = positionsInPath[currentStep].y;

            enemy.orientation = enemy.ChangeOrientation(_x, _y);

            enemy.characterDisplay.transform.position = Grid.instance._cells[_x, _y].transform.position;

            enemy.gameManager.aIManager.UpdateEnemies();
        }
    }
}