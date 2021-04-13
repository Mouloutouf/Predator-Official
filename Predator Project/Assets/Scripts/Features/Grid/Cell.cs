using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Predator
{
    public class Cell : MonoBehaviour
    {
        public PathNode pathNode { get; set; }

        public Environment _environment { get; set; }

        public CellDisplay _cellDisplay { get; set; }

        public Image _environmentDisplay { get => _cellDisplay.environmentDisplay; }
        public Image _actionDisplay { get => _cellDisplay.actionDisplay; }
        public Image _detectionDisplay { get => _cellDisplay.detectionDisplay; }

        public EnemyManager _enemy { get; set; }
        public PlayerManager _player { get; set; }

        public bool isBloody { get; set; }

        void Start()
        {
            _environmentDisplay.color = _environment.Color;
            _actionDisplay.color = DisableDisplay();
            _detectionDisplay.color = DisableDisplay();
        }

        public void SetToActionArea(Color color)
        {
            _actionDisplay.color = ChangeDisplay(color);
        }

        public void SetToDetectionArea(Color color)
        {
            _detectionDisplay.color = ChangeDisplay(color);
        }

        public Color ChangeDisplay(Color color)
        {
            Color displayColor = color;
            displayColor.a = 0.5f;

            return displayColor;
        }

        public Color DisableDisplay()
        {
            Color displayColor = Color.white;
            displayColor.a = 0.0f;
            return displayColor;
        }
    } 
}
