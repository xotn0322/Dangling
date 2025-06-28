using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleDefenseManager : MonoBehaviour, IEngineComponent
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

    private HoleBlock _block = new();
    private Hole[] holes;

    [Header("value")]
    public Camera mainCam;
    public LayerMask playerMask;
    public int sampleCount = 25;
    public float rayLength = 100f;
    public long time;
    private int _nextSpawnIndex =0;

    public float averageCoverRatio { get; private set; }

    public IEngineComponent Init()
    {
        playerMask=LayerMask.GetMask("Player");

        if (mainCam == null )
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
        _block.InitializeHoles();

        holes = _block.PickRandomPositions(1, false);

        Timer spawnTimer = new Timer();
        spawnTimer.SetTimer(ETimerType.GameTime, false, false, time, actionOnExpire: (t) =>
        {
            SpawnNextHole();
            t.CurrentTimeMs = time;
        });
        TimeManager.Instance.ResisterTimer(spawnTimer);
    }

    private void SpawnNextHole()
    {
        if (holes == null || _nextSpawnIndex >= holes.Length)
        {
            return;
        }

        Hole h = holes[_nextSpawnIndex++];
        Vector3 spawnPos = new Vector3(h.xPosition, h.yPosition, 0f);

        GameObject holePrefab = ResourcesManager.Instance.Load<GameObject>("Prefabs/Hole");
        if (holePrefab != null)
            Instantiate(holePrefab, spawnPos, Quaternion.identity);
    }

    public float CheckCoverRatio(Hole hole)
    {
        Vector2 worldPos = new Vector2(hole.xPosition, hole.yPosition);
        int hitCount = 0;

        foreach (var point in SamplePoints(worldPos, sampleCount))
        {
            Vector2 dir = (mainCam.transform.position - (Vector3)point).normalized;
            
            var hit = Physics2D.Raycast(point, dir, rayLength, playerMask);

            Color rayColor = (hit.collider != null && hit.collider.CompareTag("Player"))
                             ? Color.green   
                             : Color.red;
            Debug.DrawRay(point, dir * rayLength, rayColor, 1.0f);


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

    public void Update()
    {
        int holeCount = _block.holes.Count;
        if (holeCount == 0)
            return;

        float totalCover = 0f;
        foreach (var hole in _block.holes)
        {
            totalCover += CheckCoverRatio(hole);
        }

        averageCoverRatio = totalCover / holeCount;
        Debug.Log($"��� ���� : {averageCoverRatio}");
        UpdateHoleState(averageCoverRatio);
    }

    public void StopHoleDefense()
    {
        // ���� ���� �� ����
    }

    #region Get
    public Hole[] GetHole()
    {
        if (holes.Length < 3)
        {
            Debug.Log("holes is null");
            return null;
        }
        else
            return holes;
    }
    #endregion
}