using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Environments { None, Patio, Grass, Water, Mud, Grid }

public class Tile : MonoBehaviour
{
    public Tile(Environments enviro)
    {
        switch (enviro)
        {
            case Environments.None:
                break;
            case Environments.Patio:
                break;
            case Environments.Grass:
                break;
            case Environments.Water:
                break;
            case Environments.Mud:
                break;
            case Environments.Grid:
                break;
            default:
                break;
        }
    }
}
