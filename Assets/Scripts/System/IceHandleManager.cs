using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constant;

public class IceHandleManager : MonoBehaviour
{
    List<Ice> ices = new();
    Queue<Ice> recycledPositions = new();
    IceInstance[] _ices;
    private GameObject selectedIce = null;
    public float followZOffset = 5f;
    int _iceCount = 0;
    public long time = 5000;
    public int maxIceCount = 3;

    int currentIceIndex = 0;

    void Awake()
    {
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

        // 1. ������ ��ǥ �켱 ����
        while (recycledPositions.Count > 0)
        {
            var candidate = recycledPositions.Dequeue();
            if (!IsPositionOccupied(candidate))
            {
                spawnData = candidate;
                break;
            }
        }

        // 2. ���� ices ����Ʈ���� ������ ��ǥ ����
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
                Debug.Log("��� ��ǥ�� �̹� ��� ���Դϴ�.");
                return;
            }
        }

        Vector3 spawnPos = new Vector3(spawnData.xPosition, spawnData.yPosition, 0f);
        GameObject icePrefab = ResourcesManager.Instance.Load<GameObject>("Prefabs/Ice");

        if (icePrefab == null)
        {
            Debug.LogWarning("Ice �������� ã�� �� �����ϴ�.");
            return;
        }

        GameObject newIce = Instantiate(icePrefab, spawnPos, Quaternion.identity);
        _ices[_iceCount++] = new IceInstance(newIce, spawnData);

        Debug.Log($"Ice ������ at ({spawnData.xPosition}, {spawnData.yPosition}) / ����: {_iceCount}/{_ices.Length}");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                GameObject hitObject = hitInfo.collider.gameObject;

                if (hitObject.CompareTag("Ice"))
                {
                    if (selectedIce == null)
                    {
                        // �������� ���� ���¿��� Ŭ���ϸ� ����
                        selectedIce = hitObject;
                        Debug.Log("Ice ���õ�: " + selectedIce.name);
                    }
                    else if (selectedIce == hitObject)
                    {
                        // �̹� ���õ� Ice�� �ٽ� Ŭ���ϸ� ���� ����
                        selectedIce = null;
                        Debug.Log("Ice ���� ����");
                    }
                }
            }
        }

        // ���õ� Ice�� ������ ���콺�� ����ٴ�
        if (selectedIce != null)
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseScreenPosition.z = followZOffset; // �󸶳� ������ ������

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            selectedIce.transform.position = worldPos;
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

        Debug.Log($"�迭 ���� �Ϸ�. ���� ����: {_iceCount}/{_ices.Length}");
    }
}