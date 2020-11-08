using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    
}

public class Action
{
    public virtual void SetGridState() {}

    public virtual void Execute(int x, int y) {}
}

public class Movement : Action
{
    public Transform player { get; set; }

    public Grid grid { get; set; }

    public Movement(Grid grid, Transform player)
    {
        this.grid = grid;
        this.player = player;
    }

    public override void Execute(int x, int y)
    {
        player.position = grid.Cases[x, y].gameObject.transform.position;
    }
}
