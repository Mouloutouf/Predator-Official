using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap : MonoBehaviour
{
    public Vector2 anchorPoint;
    public float tileSize;

    public Level level;

    public Tile[,] tiles;

    void Start()
    {
        LoadLevel();
    }

    void LoadLevel()
    {
        tiles = level.tiles;
    }
}
