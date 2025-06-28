using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Resources;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private static GameManager _instance;

    //private
    private bool _LoadComplete = false;
    private float _LoadProgress = 0f;
    private string _LoadProgressText;

    private PlayerComponent playerComponent;

    //public
    public int totalPlayTimeMS = 180000;


    //functions

    private Dictionary<int, IEngineComponent>_engineComponents = new Dictionary<int, IEngineComponent>()
    {
        {150, ResourcesManager.Instance },
        {500, RandomManager.Instance },
        {1100, FileIOManager.Instance},
        {100000, GameDataManager.Instance},
       
    };

    private Dictionary<int, Type>_monoBehaviorEngineComponents = new Dictionary<int, Type>()
    {
        {550000, typeof(TimeManager)},
        {600000, typeof(HoleDefenseManager)},

    };

    private void Awake()
    {
        // 이미 인스턴스가 존재하는 경우
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);  // this 대신 gameObject 사용

        foreach (var engineComponent in _engineComponents)
        {
            engineComponent.Value.Init();
        }
    }

    public async void LoadGame() //SceneManager에서 관리
    {
        _LoadComplete = false;

        Debug.Log("Start Loading Game...");
        _LoadProgress = 0f;
        _LoadProgressText = "초기화 중...";

        Debug.Log("Load Game Data...");
        _LoadProgress = 0.55f;
        _LoadProgressText = "게임 데이터 로딩 중...";
        await GameDataManager.Instance.LoadDataManager();
        await Task.Delay(200);

        _LoadProgress = 0.99f;
        _LoadProgressText = "리소스 적용 중...";
        await Task.Delay(300);
        
        _LoadComplete = true;
    }

    public bool IsLoadComplete()
    {
        return _LoadComplete;
    }

    public float GetLoadProgress()
    {
        return _LoadProgress;
    }

    public string GetLoadProgressText()
    {
        return _LoadProgressText;
    }

    public void InitMonoBehaviourGameEngine()
    {
        LoadMonoBehaviourEngineComponent();
    }

    private void LoadMonoBehaviourEngineComponent()
    {
        foreach(var engineComponent in _monoBehaviorEngineComponents)
        {
            var resource = ResourcesManager.Instance.Load<GameObject>(IOUtil.CombinePath(Constant.Path.RESOURCE_ENGINECOMPONENT_PATH, engineComponent.Value.Name));
            var gameObject = Instantiate(resource);
            var monoBehaviourEngineComponent = (IEngineComponent)gameObject.GetComponent(engineComponent.Value);
            monoBehaviourEngineComponent.Init();

            _engineComponents.Add(engineComponent.Key, monoBehaviourEngineComponent);
        }
        _monoBehaviorEngineComponents.Clear();
    }

    public void StartGame()
    {
        Debug.Log("Game Loading Complete!");
        
        StartGameInternal();
    }

    public void StartGameInternal()
    {
        SpawnPlayer();
        //StartGameTimer();
    }

    private void SpawnPlayer()
    {
        var resource = ResourcesManager.Instance.Load<GameObject>("Prefabs/Player");
        Instantiate(resource);
        playerComponent = resource.GetComponent<PlayerComponent>(); 
    }

    private void StartGameTimer()
    {
        long halfMs = (long)(totalPlayTimeMS * 0.5f);  // a = T/2
        long bMs = (long)(halfMs * (2f / 3f));                 // b = T/3
        long cMs = (long)(halfMs * (1f / 3f));                 // c = T/6

        // b 타이머
        Timer bTimer = new Timer();
        bTimer.SetTimer(ETimerType.GameTime, false, false, bMs, Constant.FloatingPoint.FLOATING_POINT_MULTIPLIER, (bt) =>
        {
            Debug.Log("b 만료: 배경 전환 + 감속1 시작");
            ChangeBackground();

            // b 타이머 구간 감속 (1.0 → 0.85)
            StartCoroutine(SmoothFreeze(1f, 0.85f, bMs / 1000f, playerComponent));

            // c 타이머 시작
            Timer cTimer = new Timer();
            cTimer.SetTimer(ETimerType.GameTime, false, false, cMs, Constant.FloatingPoint.FLOATING_POINT_MULTIPLIER, (ct) =>
            {
                Debug.Log("c 만료: 감속2 시작 + a 시작");

                // c 타이머 구간 감속 (0.85 → 0.7)
                StartCoroutine(SmoothFreeze(0.85f, 0.7f, cMs / 1000f, playerComponent));

                // a 타이머
                Timer aTimer = new Timer();
                aTimer.SetTimer(ETimerType.GameTime, false, false, halfMs, Constant.FloatingPoint.FLOATING_POINT_MULTIPLIER, (at) =>
                {
                    Debug.Log("a 만료: 게임 종료");
                    EndGame();
                });

                TimeManager.Instance.ResisterTimer(aTimer);
            });

            TimeManager.Instance.ResisterTimer(cTimer);
        });

        TimeManager.Instance.ResisterTimer(bTimer);
    }

    private void ChangeBackground() 
    {

    }

    private void EndGame() 
    {

    }

    private IEnumerator SmoothFreeze(float from, float to, float duration, PlayerComponent freezeTarget)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float freezeFactor = Mathf.Lerp(from, to, t);
            freezeTarget.SetFreezeFactor(freezeFactor);
            yield return null;
        }

        freezeTarget.SetFreezeFactor(to);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Destroy(gameObject);
        Application.Quit();
#endif
    }
}