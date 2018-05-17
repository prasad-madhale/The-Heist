using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable <Node>
{
    public bool isobstacle;
    public Vector3 position;

    public int x;
    public int y;
    public float gscore;   // cost from start to node  
    public float hscore;   // cost from node to goal (euclidean)
    public float fscore
    {
        get {
            return gscore + hscore;
        }
    }
    public Node parent;

    public Node(bool _isobstacle, Vector3 _position, int _x, int _y)
    {
        isobstacle = _isobstacle;
        position = _position;
        x = _x;
        y = _y;
    }

    public override string ToString()
    {
        return "( Node " + this.x + " , " + this.y + " )";
    }

    public int CompareTo(Node nd)
    {
        if (this.fscore < nd.fscore)
            return -1;
        else if (this.fscore > nd.fscore)
            return 1;
        else
            return 0;
    }
}
