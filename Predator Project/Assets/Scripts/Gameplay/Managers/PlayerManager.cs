using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Predator
{
    public enum ActionType { Move, StealthKill, Heal }

    [System.Serializable]
    public class ActionEvent : UnityEvent<ActionType> { }

    public abstract class Action : MonoBehaviour
    {
        public ActionType actionType;

        public Color actionColor;

        void Start()
        {
            grid = Grid.instance;
        }

        public virtual void EnableAction(bool activate)
        {
            if (activate) inputManager.ChangeAction(actionType);

            else inputManager.ResetAction();
        }

        public PlayerManager player;

        public InputManager inputManager;

        public Grid grid { get; set; }

        public abstract void Execute(int x, int y);
    }

    public class PlayerManager : SerializedMonoBehaviour
    {
        public Image playerDisplay;

        public Dictionary<ActionType, Action> actions = new Dictionary<ActionType, Action>();

        public void GetPlayerPosition(out int x, out int y)
        {
            Grid.instance.ConvertWorldPositionToGrid(playerDisplay.transform.position, out x, out y);
        }

        void Start()
        {
            Grid.instance.cells[0, 0].player = this;
        }
    } 
}
