using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Environments { None, HighGrass, SwampWater, DeepWater, Patio, Mud }

public class Case : MonoBehaviour
{
    public Environments environment;

    public bool visible;
    public bool noise;
    public bool path;
    public bool traces;
    
    public GameObject caseObject;

    public GameObject hoverObject;

    public GameObject actionAreaObject;

    public Transform enemy { get; set; }
    public Transform player { get; set; }

    public bool inActionArea { get => inActionArea; set { inActionArea = value; SetToActionArea(value); } }

    private void SetToActionArea(bool value)
    {
        actionAreaObject.SetActive(value);
    }
}
