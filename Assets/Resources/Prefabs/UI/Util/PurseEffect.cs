using DG.Tweening;
using UnityEngine;
using System;

public class PurseEffect : IUIEffect
{
    public float MinScale;
    public float MaxScale;

    void Start()
    {
        if (PlayOnStart)
        {
            Apply();
        }
    }

    private void Purse()
    {
        transform.DOScale(MaxScale, EffectInterval)
                 .SetLoops(IsLoop ? -1 : 1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine)
                 .From(MinScale)
                 .OnComplete(() => 
                 {
                     if (SetMinValueOnComplete)
                     {
                         transform.DOScale(MinScale, EffectInterval).SetEase(Ease.InQuad);
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
        IsPlaying = true;
    }

    public override void Stop()
    {
        transform.DOKill();
        if (SetMinValueOnComplete)
        {
            transform.localScale = new Vector3(MinScale, MinScale, MinScale);
        }
        IsPlaying = false;
    }
}