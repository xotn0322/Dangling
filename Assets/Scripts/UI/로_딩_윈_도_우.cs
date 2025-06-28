using DG.Tweening;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class 로_딩_윈_도_우 : MonoBehaviour
{
    public TextMeshProUGUI ProgressText;

    private float _loopTime = 1f;


    public void Awake()
    {
        ProgressText.text = string.Empty;
        ProgressText.DOText("...", _loopTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).SetDelay(0);
    }
}