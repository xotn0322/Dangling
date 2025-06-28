using System;
using UnityEngine;

public abstract class IUIEffect : MonoBehaviour
{
    public float EffectInterval;
    public bool IsLoop = true;
    public bool PlayOnStart = true;
    public Action OnComplete = null;
    public bool SetMinValueOnComplete = false;

    [HideInInspector]
    public bool IsPlaying = false;

    public abstract void Apply();

    public abstract void Stop();
}