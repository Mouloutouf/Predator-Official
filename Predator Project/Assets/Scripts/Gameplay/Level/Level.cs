using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    [System.Serializable]
    public class EnvironmentMap
    {
        public EnvironmentArray[] environmentArrays;
    }
    [System.Serializable]
    public class EnvironmentArray
    {
        public Environment[] environments;
    }

    [CreateAssetMenu(fileName = "New Level", menuName = "Scriptables/Level")]
    public class Level : ScriptableObject
    {
        public int width;
        public int height;

        public EnvironmentDataTypes EnvironmentDataBase;

        public EnvironmentMap map { get; set; }

        [ContextMenu("Initialize Level")]
        public void InitializeLevel()
        {
            map = new EnvironmentMap();
            map.environmentArrays = new EnvironmentArray[width];

            for (int i = 0; i < map.environmentArrays.Length; i++)
            {
                map.environmentArrays[i] = new EnvironmentArray();
                map.environmentArrays[i].environments = new Environment[height];

                for (int u = 0; u < map.environmentArrays[i].environments.Length; u++)
                {
                    map.environmentArrays[i].environments[u] = new Environment();
                }
            }
        }
    } 
}
