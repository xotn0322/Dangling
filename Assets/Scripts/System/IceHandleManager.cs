using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constant;

public class IceHandleManager : MonoBehaviour
{
    public static IceHandleManager Instance { get; private set; }

    List<Ice> ices = new();
    Queue<Ice> recycledPositions = new();
    IceInstance[] _ices;
    private GameObject selectedIce = null;
    public float followZOffset = 5f;
    int _iceCount = 0;
    public long time = 5000;
    public int maxIceCount = 3;

    public float duration;
    private int click;
    private bool isDoubleClick;
    int currentIceIndex = 0;

    void Awake()
    {
        // 2. 싱글톤 초기화
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _ices = new IceInstance[maxIceCount];
    }

    void Start()
    {
        ices.Add(new Ice(-5f, -3f));
        ices.Add(new Ice(0f, -3f));
        ices.Add(new Ice(5f, -3f));

        Timer spawnTimer = new Timer();
        spawnTimer.SetTimer(ETimerType.GameTime, false, false, time, actionOnExpire: (t) =>
        {
            GenerateIceBlock();
            t.CurrentTimeMs = time;
        });
        TimeManager.Instance.ResisterTimer(spawnTimer);
    }

    public void GenerateIceBlock()
    {
        if (_iceCount >= _ices.Length)
            return;

        Ice spawnData = null;

        // 1. 삭제된 좌표 우선 재사용
        while (recycledPositions.Count > 0)
        {
            var candidate = recycledPositions.Dequeue();
            if (!IsPositionOccupied(candidate))
            {
                spawnData = candidate;
                break;
            }
        }

        // 2. 기존 ices 리스트에서 순차적 좌표 선택
        if (spawnData == null)
        {
            int attempts = 0;
            while (attempts < ices.Count)
            {
                var candidate = ices[currentIceIndex];
                currentIceIndex = (currentIceIndex + 1) % ices.Count;

                if (!IsPositionOccupied(candidate))
                {
                    spawnData = candidate;
                    break;
                }

                attempts++;
            }

            if (spawnData == null)
            {
                Debug.Log("모든 좌표가 이미 사용 중입니다.");
                return;
            }
        }

        Vector3 spawnPos = new Vector3(spawnData.xPosition, spawnData.yPosition, 0f);
        GameObject icePrefab = ResourcesManager.Instance.Load<GameObject>("Prefabs/Ice");

        if (icePrefab == null)
        {
            Debug.LogWarning("Ice 프리팹을 찾을 수 없습니다.");
            return;
        }

        GameObject newIce = Instantiate(icePrefab, spawnPos, Quaternion.identity);
        _ices[_iceCount++] = new IceInstance(newIce, spawnData);

        Debug.Log($"Ice 생성됨 at ({spawnData.xPosition}, {spawnData.yPosition}) / 현재: {_iceCount}/{_ices.Length}");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseWorldPos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            RaycastHit2D hitInfo = Physics2D.Raycast(mouseWorldPos2D, Vector2.zero);
            if (hitInfo.collider != null && hitInfo.collider.CompareTag("Ice"))
            {
                GameObject hitObject = hitInfo.collider.gameObject;

                if (selectedIce == null)
                {
                    selectedIce = hitObject;
                    click = 1;
                    isDoubleClick = false;
                    Debug.Log("Ice 선택됨: " + hitObject.name);
                }
                else if (selectedIce == hitObject && click == 1)
                {
                    click = 2;
                    isDoubleClick = true;
                    
                    Debug.Log("두 번째 클릭! Ice 파괴: " + hitObject.name);

                    StartCoroutine(DestroyWithAlpaCoroutine(hitObject, duration));

                    for (int i = 0; i < _ices.Length; i++)
                    {
                        var inst = _ices[i];
                        if (inst != null && inst.gameObject == hitObject)
                        {
                            recycledPositions.Enqueue(inst.data);
                            _ices[i] = null;
                            break;
                        }
                    }
                    CompactIces();

                    // 3) 선택 해제, 클릭 카운트 리셋
                    selectedIce = null;
                    click = 0;
                }
            }
        }

        // 선택된 Ice가 있으면 커서를 따라다님
        if (selectedIce != null)
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseScreenPosition.z = followZOffset;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            selectedIce.transform.position = new Vector3(worldPos.x, worldPos.y, selectedIce.transform.position.z);
        }
    }

    private bool IsPositionOccupied(Ice target)
    {
        foreach (var instance in _ices)
        {
            if (instance == null) continue;
            if (Mathf.Approximately(instance.data.xPosition, target.xPosition) &&
                Mathf.Approximately(instance.data.yPosition, target.yPosition))
            {
                return true;
            }
        }
        return false;
    }
    private IEnumerator DestroyWithAlpaCoroutine(GameObject go, float duration)
    {
        var sprite = go.GetComponent<SpriteRenderer>();
        if (sprite == null)
        {
            Destroy(go);
            yield break;
        }

        Color original = sprite.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += UnityEngine.Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float a = Mathf.Lerp(original.a, 0f, t);
            sprite.color = new Color(original.r, original.g, original.b, a);
            yield return null;
        }

        // 완전 투명 보장
        sprite.color = new Color(original.r, original.g, original.b, 0f);
        Destroy(go);
    }

    private void CompactIces()
    {
        int index = 0;
        IceInstance[] newArray = new IceInstance[maxIceCount];

        foreach (var instance in _ices)
        {
            if (instance != null)
            {
                newArray[index++] = instance;
            }
        }

        _ices = newArray;
        _iceCount = index;

        Debug.Log($"배열 정리 완료. 현재 개수: {_iceCount}/{_ices.Length}");
    }

    public void ClearSelectedIce()
    {
        selectedIce = null;
        click = 0;
    }

    public void RemoveIceFromArray(GameObject go)
    {
        for (int i = 0; i < _ices.Length; i++)
        {
            var inst = _ices[i];
            if (inst != null && inst.gameObject == go)
            {
                recycledPositions.Enqueue(inst.data);
                _ices[i] = null;
                CompactIces();
                break;
            }
        }
    }
}