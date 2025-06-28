using DG.Tweening;
using UnityEngine;
using System;

public class ShakeEffect : IUIEffect
{
    public float ShakeStrength;
    public int ShakeVibrato;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Logger.Error($"RectTransform Required for ShakeEffect", GetType());
            return;
        }

        if (PlayOnStart)
        {
            Apply();
        }
    }

    private void Shake()
    {
        rectTransform.DOShakeAnchorPos(EffectInterval, ShakeStrength, ShakeVibrato)
                     .SetLoops(IsLoop ? -1 : 1)
                     .OnComplete(() =>
                     {
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
        Shake();
        IsPlaying = true;
    }

    public override void Stop()
    {
        rectTransform.DOKill();
        IsPlaying = false;
    }
}