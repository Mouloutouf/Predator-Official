using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public enum Environments { None, Patio, Grass, Water, Mud, Grid }

    [CreateAssetMenu(fileName = "New Cell Definition", menuName = "Scriptables/Cell Definition")]
    public class CellDefinition : ScriptableObject
    {
        public Environments environment;

        public bool visible;
        public bool noise;
        public bool path;
        public bool traces;
    } 
}
