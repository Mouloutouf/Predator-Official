using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    public CellDefinition _definition { get; set; }

    public GameObject _object { get; set; }
    public GameObject _display { get; set; }

    //\

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
