using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Predator
{
    public class PathNode
    {
        public int _x, _y;

        public int gCost;
        public int hCost;
        public int FCost;

        public PathNode previousNode;

        public PathNode(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public override string ToString()
        {
            return _x + ", " + _y;
        }

        public void CalculateFCost()
        {
            FCost = gCost + hCost;
        }
    } 
}
