using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public enum EnvironmentType { None, Patio, Grass, Water, Mud, Grid, Wall }

    [System.Serializable]
    public class Environment
    {
        [SerializeField] private EnvironmentType enviroType; public EnvironmentType EnviroType { get => enviroType; private set => enviroType = value; }
        [SerializeField] private Color color = Color.white; public Color Color { get => color; private set => color = value; }
        [SerializeField] private bool visible; public bool Visible { get => visible; private set => visible = value; }
    }

    [CreateAssetMenu(fileName = "New Environment Data File", menuName = "Scriptables/Environment Data")]
    public class EnvironmentData : SerializedScriptableObject
    {
        public Dictionary<EnvironmentType, Environment> environments = new Dictionary<EnvironmentType, Environment>();
    } 
}
