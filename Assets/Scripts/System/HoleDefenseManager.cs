using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleDefenseManager : MonoBehaviour, IEngineComponent
{
    #region instance
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
    public GameObject[] _holePrefabs;

    [Header("value")]
    public Camera mainCam;
    public LayerMask playerMask;
    public int sampleCount = 25;
    public float rayLength = 100f;

    public float _maxWaterY = 100f;
    public float _currentWaterY = 0.0f;
    public float openHoleWaterAmount;
    public float halfOpenHoleWaterAmount;
    public float closeHoleWaterAmount;

    public long time;
    private int _nextSpawnIndex =0;
    private bool isActiveHole;

    private bool isSpill = false;

    public float averageCoverRatio { get; private set; }

    public void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }

    public IEngineComponent Init()
    {
        playerMask=LayerMask.GetMask("Player");

        if (mainCam == null )
        {
            GameObject go = GameObject.Find("Main Camera");
            if (go != null)
                mainCam = Util.GetOrAddComponent<Camera>(go);
            else
                Debug.Log("ã�� �� ���� ");
        }


        StartHoleDefense();
        return this;
    }

    public void StartHoleDefense()
    {
        _block.InitializeHoles();
        isActiveHole = false;
        holes = _block.PickRandomPositions(8, false);
        _holePrefabs = new GameObject[holes.Length];
        SpawnNextHole();
        Timer spawnTimer = new Timer();
        spawnTimer.SetTimer(ETimerType.GameTime, false, false, time, actionOnExpire: (t) =>
        {
            SpawnNextHole();
            //SoundManager.Instance.PlaySFX("구멍 생김2");
            if (GameManager.Instance.GetPhase() == 1)
                LightManager.Instance.StartAlert("Orange");
            t.CurrentTimeMs = time;
        });
        TimeManager.Instance.ResisterTimer(spawnTimer);
    }

    private void SpawnNextHole()
    {
        if (holes == null || _nextSpawnIndex >= holes.Length)
            return;

        int index = _nextSpawnIndex;       
        Hole h = holes[_nextSpawnIndex++];
        Vector3 spawnPos = new Vector3(h.xPosition, h.yPosition, .5f);
        Quaternion rot = Quaternion.Euler(0f, 180f, 0f);

        GameObject holePrefab = ResourcesManager.Instance.Load<GameObject>("Prefabs/Hole");
        if (holePrefab != null)
        {
            GameObject instance = Instantiate(holePrefab, spawnPos, rot);
            _holePrefabs[index] = instance;
            isActiveHole = true;
        }
    }

    public float CheckCoverRatio(GameObject holeGO)
    {
        if (holeGO == null) return 0f;

        int hitCount = 0;
        foreach (var point in SamplePoints2D(holeGO.transform.position, sampleCount))
        {
            Vector3 origin = new Vector3(point.x, point.y, holeGO.transform.position.z - 0.1f);
            Vector3 dir = holeGO.transform.forward;
            Ray ray = new Ray(origin, dir);

            RaycastHit2D hit2D = Physics2D.GetRayIntersection(
                ray,
                rayLength,
                playerMask    // LayerMask.GetMask("Player")���� ��
            );

            bool isHit = (hit2D.collider != null);

            if (hit2D.collider != null)
            {
                GameObject hitObject = hit2D.collider.gameObject;
            }
            Debug.DrawRay(origin, dir * rayLength, isHit ? Color.green : Color.red, 0.5f);

            if (isHit)
                hitCount++;
        }
        return (float)hitCount / sampleCount;
    }

    public void UpdateHoleState(float ratio)
    {
        switch (ratio)
        {
            case 0f:
                // hole is 100%
                UpdateWaterLevel(openHoleWaterAmount);
                
                break;

            case float r when r > 0f && r < 1f:
                // hole is > 0 %  && hole is < 100% open
                UpdateWaterLevel(halfOpenHoleWaterAmount);
                break;

            case float r when r >= 1f:
                // hole is 0% open
                UpdateWaterLevel(closeHoleWaterAmount);
                break;

            default:
                Debug.Log("sibal fuck");
                break;
        }
    }

    public void UpdateWaterLevel(float amount)
    {        
        if (!isSpill)
        {
            isSpill = true;
            SoundManager.Instance.PlaySFX("물들어옴");
            Timer spillTimer = new Timer();
            spillTimer.SetTimer(ETimerType.GameTime, false, false, 20000, 1000, actionOnExpire: (t) => {
                isSpill = false;
            });
            TimeManager.Instance.ResisterTimer(spillTimer);
        }
        _currentWaterY += amount * Time.deltaTime;
        _currentWaterY = Mathf.Clamp(_currentWaterY, 0.0f, _maxWaterY);        

        if(_currentWaterY >=80)
        {
            LightManager.Instance.StartAlert("Red");
        }
        if( _currentWaterY >= _maxWaterY)
        {
            //GameOver
            //CSceneManager.Instance.LoadScene("GameOver");
            Debug.Log("GameOver");
        }
    }

    private IEnumerable<Vector2> SamplePoints2D(Vector3 holePos, int count)
    {
        yield return new Vector2(holePos.x, holePos.y);
        float radius = 0.25f;
        for (int i = 0; i < count - 1; i++)
        {
            float angle = (360f / (count - 1) * i) * Mathf.Deg2Rad;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            yield return new Vector2(holePos.x, holePos.y) + offset;
        }
    }

    public void Update()
    {
        if(isActiveHole ==false)
        {
            return;
        }
        else
        {
            int holeCount = _holePrefabs.Length;
            if (holeCount == 0)
                return;

            float totalCover = 0f;
            foreach (var hole in _holePrefabs)
            {
                totalCover += CheckCoverRatio(hole);
            }

            averageCoverRatio = totalCover / holeCount;
            //Debug.Log($"��� ���� : {averageCoverRatio}");
            UpdateHoleState(averageCoverRatio);
        }
        
    }

    public void StopHoleDefense()
    {
        
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

    public float GetCurrentWaterValue()
    {
        return _currentWaterY;
    }

    public float GetMaxWaterValue()
    {
        return _maxWaterY;
    }

    #endregion
}