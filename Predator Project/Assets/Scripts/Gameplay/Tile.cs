using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileTypes { None, Patio, Grass, Water, Mud, Grid }

public class Tile : MonoBehaviour
{
    public TileTypes tileType;
    public GameObject tilePrefab;
}
