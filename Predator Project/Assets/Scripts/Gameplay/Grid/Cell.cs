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
        public Environment _environment { get; set; }

        public Image _environmentDisplay { get; set; }
        public Image _actionDisplay { get; set; }
        // I was doing the display thing for the detection
        public EnemyManager _enemy { get; set; }
        public PlayerManager _player { get; set; }

        void Start()
        {
            _environmentDisplay.color = _environment.Color;
            _actionDisplay.color = ChangeActionDisplay(_environment.Color);
        }

        public void SetToActionArea(Color color)
        {
            _actionDisplay.color = ChangeActionDisplay(color);
        }

        public Color ChangeActionDisplay(Color color)
        {
            Color actionColor = color;
            actionColor.a = 0.5f;

            return actionColor;
        }
    } 
}
