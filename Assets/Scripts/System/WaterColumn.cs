using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterColumn
{
    public float xPos, height, targetHeight, k, velocity, drag;
    public Vector3 rayVector;
    public Ray ray;

    public WaterColumn(float xPos, float targetHeight, float k, float drag)
    {
        this.xPos = xPos;
        this.height = targetHeight;
        this.targetHeight = targetHeight;
        this.k = k;
        this.drag = drag;
        this.rayVector = new Vector3(xPos, height, 0.0f);
        this.ray = new Ray(rayVector, new Vector3(0.0f, 100.0f, 0.0f));
    }


    public void UpdateColumn()
    {
        float acc = -k * (height - targetHeight);
        velocity += acc;
        velocity -= drag * velocity;
        height += velocity;
    }
}
