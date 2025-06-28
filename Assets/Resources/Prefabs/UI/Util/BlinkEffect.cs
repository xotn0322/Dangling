using DG.Tweening;
using UnityEngine;

public class BlinkEffect : IUIEffect
{
    public float MinAlpha;
    public float MaxAlpha;

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if (PlayOnStart)
        {
            Apply();
        }
    }

    private void Blink()
    {
        canvasGroup.DOFade(MinAlpha, EffectInterval)
                   .SetLoops(IsLoop ? -1 : 1, LoopType.Yoyo)
                   .SetEase(Ease.InOutSine)
                   .From(MaxAlpha)
                   .OnComplete(() =>
                   {
                       if (SetMinValueOnComplete)
                       {
                           canvasGroup.DOFade(MinAlpha, EffectInterval).SetEase(Ease.InQuad);
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
        Blink();
        IsPlaying = true;
    }


    public override void Stop()
    {
        canvasGroup?.DOKill();
        if (SetMinValueOnComplete)
        {
            canvasGroup.alpha = MinAlpha;
        }
        IsPlaying = false;
    }
}