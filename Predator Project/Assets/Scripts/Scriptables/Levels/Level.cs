using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Scriptables/Level")]
public class Level : ScriptableObject
{
    public int width;
    public int height;

    public CellDefinition definition;
}
