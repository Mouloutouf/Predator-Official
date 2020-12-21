using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Scriptables/Level")]
    public class Level : ScriptableObject
    {
        public int width;
        public int height;

        public Environments EnvironmentDataBase;

        public Environment[,] cellsEnvironments;

        [ContextMenu("Initialize Level")]
        public void InitializeLevel()
        {
            cellsEnvironments = new Environment[width, height];

            for (int i = 0; i < cellsEnvironments.GetLength(0); i++)
            {
                for (int u = 0; u < cellsEnvironments.GetLength(1); u++)
                {
                    cellsEnvironments[i, u] = new Environment();
                }
            }
        }
    } 
}
