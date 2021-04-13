using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoldierBehavior : MonoBehaviour
{
    public UnityEvent behaviorEnd;

    public List<SoldierAction> actions { get; set; } = new List<SoldierAction>();
    protected int currentAction;

    public bool repeat;

    public virtual void GenerateBehavior(ActionTemplate actionTemplate) { }

    public void StartBehavior()
    {
        currentAction = 0;
        NextAction();
    }

    public void StartAction(SoldierAction action)
    {
        action.actionEnd.AddListener(delegate { NextAction(); });

        action.StartAction();
    }
    public void NextAction()
    {
        if (currentAction >= actions.Count)
        {
            if (repeat) currentAction = 0;

            else { behaviorEnd.Invoke(); return; }
        }

        actions[currentAction].actionEnd.RemoveAllListeners();

        StartAction(actions[currentAction]);

        currentAction++;
    }
}
