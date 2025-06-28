using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleDefenseManager : IEngineComponent
{
    #region �������̽�
    public static HoleDefenseManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new HoleDefenseManager();
            return _instance;
        }
    }
    private static HoleDefenseManager _instance;
    #endregion

    [Header("component")]
    private HoleBlock _hole = new();
    private UI_HoleDefense _ui;

    [Header("value")]
    public Camera mainCam;
    public LayerMask playerMask;
    public int sampleCount = 25;
    public float rayLength = 100f;

    public float averageCoverRatio { get; private set; }

    public IEngineComponent Init()
    {
        playerMask=LayerMask.GetMask("Player");

        if(mainCam == null )
        {
            GameObject go = GameObject.Find("Main Camera");
            if (go != null)
                mainCam = Util.GetOrAddComponent<Camera>(go);
        }

        StartHoleDefense();
        return this;
    }

    public void StartHoleDefense()
    {
        _hole.InitializeHoles();
    }

    public float CheckCoverRatio(Hole hole)
    {
        Vector2 worldPos = new Vector2(hole.xPosition, hole.yPosition);
        int hitCount = 0;

        foreach (var point in SamplePoints(worldPos, sampleCount))
        {
            Vector2 dir = (mainCam.transform.position - (Vector3)point).normalized;
            var hit = Physics2D.Raycast(point, dir, rayLength, playerMask);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
                hitCount++;
        }

        return (float)hitCount / sampleCount;
    }

    public void UpdateHoleState(float ratio)
    {
        switch (ratio)
        {
            case 0f:
                Debug.Log("��� ������ ������ �����");
                // ���� �������� �ӵ��� �ִ� �ӵ�
                break;

            case float r when r > 0f && r < 1f:
                Debug.Log($"�κ������� ����: {r:P0}");
                // ���� ������
                break;

            case float r when r >= 1f:
                Debug.Log("��� ������ ������ ����");
                // ���� �������� �ӵ��� ����
                break;

            default:
                Debug.LogWarning($"��ȿ���� ���� ���� ����: {ratio}");
                // �̰� �α� ������ ���� 
                break;
        }
    }

    private IEnumerable<Vector2> SamplePoints(Vector2 center, int count)
    {        
        yield return center;
    }

    public void Tick()
    {
        int holeCount = _hole.holes.Count;
        if (holeCount == 0)
            return;

        float totalCover = 0f;
        foreach (var hole in _hole.holes)
        {
            totalCover += CheckCoverRatio(hole);
        }

        averageCoverRatio = totalCover / holeCount;
        UpdateHoleState(averageCoverRatio);
    }

    public void StopHoleDefense()
    {
        // ���� ���� �� ����
    }
}