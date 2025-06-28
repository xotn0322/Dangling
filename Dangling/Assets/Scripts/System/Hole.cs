using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole
{
    public float xPosition { get; private set; }
    public float yPosition { get; private set; }
    public Hole(float x, float y)
    {
        this.xPosition = x;
        this.yPosition = y;       
    }

}
