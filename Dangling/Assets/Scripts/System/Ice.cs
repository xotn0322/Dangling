using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice
{
    public float xPosition { get; private set; }
    public float yPosition { get; private set; }
    public Ice(float x, float y)
    {
        this.xPosition = x;
        this.yPosition = y;
    }
}
