using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public enum EnvironmentType { None, Patio, Grass, Water, Mud, Grid, Wall }

    [System.Serializable]
    public class Environment
    {
        [SerializeField] private EnvironmentType type; public EnvironmentType Type { get => type; private set => type = value; }
        [SerializeField] private Color color = Color.white; public Color Color { get => color; private set => color = value; }
        [SerializeField] private bool visible; public bool Visible { get => visible; private set => visible = value; }
    } 
}
