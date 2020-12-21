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

        public Color moveAreaColor;
        public Color attackAreaColor;

        public EnemyManager enemy { get; set; }
        public PlayerManager player { get; set; }

        void Start()
        {
            _environmentDisplay.color = _environment.color;
            _actionDisplay.color = ChangeActionDisplay(_environment.color);
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
