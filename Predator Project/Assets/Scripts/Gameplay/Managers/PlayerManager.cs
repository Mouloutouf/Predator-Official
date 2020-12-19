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
        public Toggle actionToggle;

        public ActionType actionType;

        public Color actionColor;

        public float energyCost;

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
        public GameManager gameManager;

        public Transform playerInterface;

        public Slider healthSlider;
        public Slider energySlider;

        [SerializeField] private float health;
        private float ch;
        public float currentHealth { get => ch; set { ch = value; healthSlider.value = value; } }

        [SerializeField] private float maxEnergy;
        [SerializeField] private float startEnergy;
        private float ce;
        public float currentEnergy { get => ce; set { ce = Mathf.Clamp(value, 0, startEnergy); energySlider.value = value; } }

        public Text actionDisplayText;

        [SerializeField] private int actionPoints;
        private int cp;
        public int currentPoints { get => cp; set { cp = value; actionDisplayText.text = value.ToString(); if (value <= 0) gameManager.ChangeTurn(); } }

        public Image playerDisplay;

        public Dictionary<ActionType, Action> actions = new Dictionary<ActionType, Action>();

        public void GetPlayerPosition(out int x, out int y)
        {
            Grid.instance.ConvertWorldPositionToGrid(playerDisplay.transform.position, out x, out y);
        }

        void Start()
        {
            Grid.instance.cells[0, 0].player = this;

            healthSlider.maxValue = health;
            energySlider.maxValue = maxEnergy;
            energySlider.value = startEnergy;

            currentHealth = health;
            currentEnergy = startEnergy;
            SetPlayerTurn();
        }

        public void SetPlayerTurn()
        {
            currentPoints = actionPoints;

            foreach (Action action in actions.Values)
            {
                action.actionToggle.isOn = false;
            }
        }
    } 
}
