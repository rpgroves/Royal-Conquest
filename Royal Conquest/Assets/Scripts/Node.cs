using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] Node nextSouth;
    [SerializeField] Node nextNorth;
    bool northEnd = false;
    bool southEnd = false;

    void Start()
    {
        if(getNextNorth() == null)
            northEnd = true;
        if(getNextSouth() == null)
            southEnd = true;
    }

    public bool isNorthEnd()
    {
        return northEnd;
    }

    public bool isSouthEnd()
    {
        return southEnd;
    }

    public Node getNextNorth()
    {
        return nextNorth;
    }

    public Node getNextSouth()
    {
        return nextSouth;
    }
}
