using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TintEffect : IUIEffect
{
    public Color FromColor;
    public Color ToColor;

    private List<Graphic> _graphics = new List<Graphic>();
    private List<Tween> _tweens = new List<Tween>();

    void Start()
    {
        if (PlayOnStart)
        {
            Apply();
        }
    }

    private void Tint()
    {
        _graphics.Clear();
        _graphics.AddRange(GetComponentsInChildren<Graphic>(includeInactive: true));

        foreach (var graphic in _graphics)
        {
            Tween t = graphic.DOColor(ToColor, EffectInterval)
                .SetEase(Ease.InOutSine)
                .SetLoops(IsLoop ? -1 : 1, LoopType.Yoyo)
                .From(FromColor)
                .OnComplete(() =>
                {
                    if (SetMinValueOnComplete)
                    {
                        Stop();
                    }
                    OnComplete.RunExt();
                    IsPlaying = false;
                });

            _tweens.Add(t);
        }
    }

    public void OnDestroy()
    {
        Stop();
    }

    public override void Stop()
    {
        foreach (var tween in _tweens)
        {
            tween.Kill();
        }

        _tweens.Clear();

        if (SetMinValueOnComplete)
        {

            foreach (var graphic in _graphics)
            {
                graphic.color = FromColor;
            }
        }
        IsPlaying = false;
    }

    public override void Apply()
    {
        Stop();
        Tint();
        IsPlaying = true;
    }
}