using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovement : MonoBehaviour
{
    private float speed = 1f;
    private float length = 1f;

    private float runningTime = 0f;
    private float yPos = 0f;

    // Use this for initialization
    void Start()
    {
        runningTime += Time.deltaTime * speed;
        yPos = Mathf.Sin(runningTime) * length;
        Debug.Log(yPos);
        this.transform.position = new Vector2(0, yPos);
    }

    private void Update()
    {
        runningTime += Time.deltaTime * speed;
        yPos = Mathf.Sin(runningTime) * length;
        Debug.Log(yPos);
        this.transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}