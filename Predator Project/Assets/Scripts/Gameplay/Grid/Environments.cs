using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    [CreateAssetMenu(fileName = "New Environments File", menuName = "Scriptables/Environments")]
    public class Environments : SerializedScriptableObject
    {
        public Dictionary<EnvironmentType, Environment> environments = new Dictionary<EnvironmentType, Environment>();
    } 
}
