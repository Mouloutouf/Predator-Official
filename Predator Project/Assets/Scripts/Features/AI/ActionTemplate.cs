using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType { Move, Shoot, Search, Heal, Aim, Interact }
[Serializable]
public class ActionData
{
    public ActionType actionType;
}
public class MoveActionData : ActionData
{
    public Transform destination;
}

public class ActionTemplate : MonoBehaviour
{
    public bool manual;

    [EnableIf("manual")]
    public List<ActionData> actionDatas = new List<ActionData>();
}
