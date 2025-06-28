using DG.Tweening;
using UnityEngine;
using System;

public class RotateEffect : IUIEffect
{
    public Vector3 StartRotation;
    public Vector3 EndRotation;

    void Start()
    {
        if (PlayOnStart)
        {
            Apply();
        }
    }

    private void Purse()
    {
        transform.DORotate(EndRotation, EffectInterval)
                 .SetLoops(IsLoop ? -1 : 1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine)
                 .From(StartRotation)
                 .OnComplete(() =>
                 {
                     if (SetMinValueOnComplete)
                     {
                         transform.DORotate(StartRotation, EffectInterval).SetEase(Ease.InQuad);
                     }
                     OnComplete.RunExt();
                     IsPlaying = false;
                 });
    }

    public void OnDestroy()
    {
        Stop();
    }

    public override void Apply()
    {
        Stop();
        Purse();
    }

    public override void Stop()
    {
        transform.DOKill();
        if (SetMinValueOnComplete)
        {
            transform.rotation = Quaternion.Euler(StartRotation);
        }
        IsPlaying = false;
    }
}