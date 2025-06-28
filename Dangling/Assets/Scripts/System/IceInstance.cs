using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceInstance
{
    public GameObject gameObject;
    public Ice data;

    public IceInstance(GameObject go, Ice d)
    {
        gameObject = go;
        data = d;
    }
}
