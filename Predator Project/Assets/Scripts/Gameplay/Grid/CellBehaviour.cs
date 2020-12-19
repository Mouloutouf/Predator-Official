using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Predator
{
    public class CellBehaviour : MonoBehaviour
    {
        public CellDefinition _definition { get; set; }
        public Image _display { get; set; }

        public Color moveAreaColor;
        public Color attackAreaColor;

        public EnemyManager enemy { get; set; }
        public PlayerManager player { get; set; }

        public void SetToActionArea(Color color)
        {
            _display.color = color;
        }
    } 
}
