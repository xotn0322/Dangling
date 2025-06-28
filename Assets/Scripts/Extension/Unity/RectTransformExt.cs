using UnityEngine;

public enum EAnchorPreset : byte
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottonCenter,
    BottomRight,

    VertStretchLeft,
    VertStretchRight,
    VertStretchCenter,

    HorStretchTop,
    HorStretchMiddle,
    HorStretchBottom,

    StretchAll,
}

public enum EPivotPreset : byte
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottonCenter,
    BottomRight,
}


public static class RectTransformExt
{
    public static void SetPivotPresetExt(this RectTransform source, EPivotPreset pivot)
    {
        switch (pivot)
        {
            case EPivotPreset.TopLeft:
                {
                    source.pivot = new Vector2(0, 0);
                    break;
                }
            case EPivotPreset.TopCenter:
                {
                    source.pivot = new Vector2(0.5f, 0);
                    break;
                }
            case EPivotPreset.TopRight:
                {
                    source.pivot = new Vector2(1, 0);
                    break;
                }
            case EPivotPreset.MiddleLeft:
                {
                    source.pivot = new Vector2(0, 0.5f);
                    break;
                }
            case EPivotPreset.MiddleCenter:
                {
                    source.pivot = new Vector2(0.5f, 0.5f);
                    break;
                }
            case EPivotPreset.MiddleRight:
                {
                    source.pivot = new Vector2(1, 0.5f);
                    break;
                }
            case EPivotPreset.BottomLeft:
                {
                    source.pivot = new Vector2(0, 1);
                    break;
                }
            case EPivotPreset.BottonCenter:
                {
                    source.pivot = new Vector2(0.5f, 1);
                    break;
                }
            case EPivotPreset.BottomRight:
                {
                    source.pivot = new Vector2(1, 1);
                    break;
                }
        }
    }

    public static void SetAnchorPresetExt(this RectTransform source, EAnchorPreset allign, float offsetX = 0, float offsetY = 0)
    {
        source.anchoredPosition = new Vector3(offsetX, offsetY, 0f);
        switch (allign)
        {
            case (EAnchorPreset.TopLeft):
                {
                    source.anchorMin = new Vector2(0, 1);
                    source.anchorMax = new Vector2(0, 1);
                    break;
                }
            case (EAnchorPreset.TopCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 1);
                    source.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (EAnchorPreset.TopRight):
                {
                    source.anchorMin = new Vector2(1, 1);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }

            case (EAnchorPreset.MiddleLeft):
                {
                    source.anchorMin = new Vector2(0, 0.5f);
                    source.anchorMax = new Vector2(0, 0.5f);
                    break;
                }
            case (EAnchorPreset.MiddleCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0.5f);
                    source.anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                }
            case (EAnchorPreset.MiddleRight):
                {
                    source.anchorMin = new Vector2(1, 0.5f);
                    source.anchorMax = new Vector2(1, 0.5f);
                    break;
                }

            case (EAnchorPreset.BottomLeft):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(0, 0);
                    break;
                }
            case (EAnchorPreset.BottonCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0);
                    source.anchorMax = new Vector2(0.5f, 0);
                    break;
                }
            case (EAnchorPreset.BottomRight):
                {
                    source.anchorMin = new Vector2(1, 0);
                    source.anchorMax = new Vector2(1, 0);
                    break;
                }

            case (EAnchorPreset.HorStretchTop):
                {
                    source.anchorMin = new Vector2(0, 1);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }
            case (EAnchorPreset.HorStretchMiddle):
                {
                    source.anchorMin = new Vector2(0, 0.5f);
                    source.anchorMax = new Vector2(1, 0.5f);
                    break;
                }
            case (EAnchorPreset.HorStretchBottom):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(1, 0);
                    break;
                }

            case (EAnchorPreset.VertStretchLeft):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(0, 1);
                    break;
                }
            case (EAnchorPreset.VertStretchCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0);
                    source.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (EAnchorPreset.VertStretchRight):
                {
                    source.anchorMin = new Vector2(1, 0);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }

            case (EAnchorPreset.StretchAll):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }
        }
    }
}