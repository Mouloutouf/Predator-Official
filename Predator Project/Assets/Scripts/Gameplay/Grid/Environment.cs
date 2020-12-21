using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public enum EnvironmentType { None, Patio, Grass, Water, Mud, Grid }

    [System.Serializable]
    public class Environment
    {
        public EnvironmentType type;

        public Color color = Color.white;

        public bool visible;
        public bool noise;
        public bool path;
        public bool traces;
    } 
}
