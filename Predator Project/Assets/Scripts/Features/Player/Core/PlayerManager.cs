using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Predator
{
    public class PlayerManager : SerializedMonoBehaviour
    {
        public GameManager gameManager;

        public Transform playerInterface;

        public Slider healthSlider;
        public Slider energySlider;

        [SerializeField] private float health;
        private float _currentHealth; public float _CurrentHealth { get => _currentHealth; set { _currentHealth = value; healthSlider.value = value; } }

        [SerializeField] private float maxEnergy;
        [SerializeField] private float startEnergy;
        private float _currentEnergy; public float _CurrentEnergy 
        {
            get => _currentEnergy;
            set {
                _currentEnergy = Mathf.Clamp(value, 0, startEnergy);
                energySlider.value = value;
                if (value <= 0) gameManager.GameOver();
            }
        }

        public Text actionDisplayText;

        [SerializeField] private int actionPoints;
        private int _currentPoints; public int _CurrentPoints
        {
            get => _currentPoints;
            set {
                _currentPoints = value;
                actionDisplayText.text = value.ToString();
                if (value <= 0) gameManager.ChangeTurn();
            }
        }

        public Image playerDisplay;
        public Color visibleColor, hiddenColor;

        public Dictionary<ActionType, PlayerAction> actions = new Dictionary<ActionType, PlayerAction>();

        public Cell playerCell { get; set; }

        public void GetPlayerPosition(out int x, out int y)
        {
            Grid.instance.ConvertWorldPositionToGrid(playerDisplay.transform.position, out x, out y);
        }

        void Start()
        {
            InitPlayer();

            StartPlayer();

            UpdatePlayerPosition();
        }

        private void InitPlayer()
        {
            healthSlider.maxValue = health;
            energySlider.maxValue = maxEnergy;
            energySlider.value = startEnergy;

            _CurrentHealth = health;
            _CurrentEnergy = startEnergy;
        }

        public void StartPlayer()
        {
            _CurrentPoints = actionPoints;

            foreach (PlayerAction action in actions.Values) action.actionToggle.isOn = false;
        }

        public void UpdatePlayerPosition()
        {
            if (playerCell != null) playerCell._player = null;

            GetPlayerPosition(out int _x, out int _y);
            playerCell = Grid.instance._cells[_x, _y];
            playerCell._player = this;

            playerDisplay.color = playerCell._environment.Visible == true ? visibleColor : hiddenColor;

            gameManager.aIManager.DetectionCheck();
        }
    } 
}
