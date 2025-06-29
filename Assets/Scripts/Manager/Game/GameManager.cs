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
using UnityEngine.Playables;


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
    private PlayableDirector playableDirector;
    private int Phase;

    //public
    public int totalPlayTimeMS = 50;


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
        {610000, typeof(TimeEventManager)}, 
        {620000, typeof(LightManager)},
        {630000, typeof(SoundManager)}

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

        Phase = 1;

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

    public int GetPhase()
    {
        return Phase;
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
        InitMonoBehaviourGameEngine();
        SpawnPlayer();
        SetPlayableDirector();
        StartGameTimer();
    }

    private void SpawnPlayer()
    {
        var resource = ResourcesManager.Instance.Load<GameObject>("Prefabs/Player");
        var player = Instantiate(resource);
        playerComponent = player.GetComponent<PlayerComponent>();
    }

    private void SetPlayableDirector()
    {
        GameObject playableDirectorObject = GameObject.Find("TimeLine");
        playableDirector = playableDirectorObject.GetComponent<PlayableDirector>();
    }

    private void StartGameTimer()
    {
        long halfMs = (long)(totalPlayTimeMS * 0.5f);  // a = T/2
        long bMs = (long)(totalPlayTimeMS * 0.3f);     // b = T*0.3 (30% 구간)
        long cMs = (long)(totalPlayTimeMS * 0.2f);     // c = T*0.2 (20% 구간)

        Timer tideTimer = new Timer();
        tideTimer.SetTimer(ETimerType.GameTime, false, false, 13000, 1000, actionOnExpire: (t) => {
            SoundManager.Instance.PlaySFX("파도바다1");
            t.CurrentTimeMs = 13000;
        });
        // b 타이머 (첫 번째 감속 구간)
        Timer bTimer = new Timer();
        bTimer.SetTimer(ETimerType.GameTime, false, false, bMs, Constant.FloatingPoint.FLOATING_POINT_MULTIPLIER, (bt) =>
        {
            Debug.Log("b 만료: 첫 번째 감속 완료 (85% 속도)");

            ChangeBackground();

            Phase = 2;
            
            // c 타이머 시작 (두 번째 감속 구간)
            Timer cTimer = new Timer();
            cTimer.SetTimer(ETimerType.GameTime, false, false, cMs, Constant.FloatingPoint.FLOATING_POINT_MULTIPLIER, (ct) =>
            {
                Debug.Log("c 만료: 두 번째 감속 완료 (70% 속도)");

                Phase = 3;
                
                // a 타이머 (최종 구간 - 70% 속도 유지)
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

        // b 타이머 구간 감속 (1.0 → 0.85)
        StartCoroutine(SmoothFreeze(1f, 0.85f, bMs / 1000f, playerComponent));
        
        // c 타이머 구간 감속 (0.85 → 0.7) - bMs 후에 시작
        StartCoroutine(SmoothFreezeDelayed(0.85f, 0.7f, cMs / 1000f, playerComponent, bMs / 1000f));

        TimeManager.Instance.ResisterTimer(bTimer);
    }

    private void ChangeBackground() 
    {
        TimeEventManager.Instance.ChangeBackGround();
        Timer windTimer = new Timer();
        Timer breathTimer = new Timer();
        windTimer.SetTimer(ETimerType.GameTime, false, false, 15000, 1000, actionOnExpire: (t) => {
            SoundManager.Instance.PlaySFX("바람1");
            t.CurrentTimeMs = 15000;
        });
        breathTimer.SetTimer(ETimerType.GameTime, false, false, 12000, 1000, actionOnExpire: (t) => {
            SoundManager.Instance.PlaySFX("입김2");
            t.CurrentTimeMs = 12000;
        });
        TimeManager.Instance.ResisterTimer(windTimer);
        TimeManager.Instance.ResisterTimer(breathTimer);
    }

    public void EndGame() 
    {
        playerComponent.gameObject.SetActive(false);
        TimeManager.Instance.ClearTimers();
        SoundManager.Instance.SetMixerSFXVolume(0);
        SoundManager.Instance.SetMixerVoiceVolume(0);

        SoundManager.Instance.PlayMusic("빨리 정해라");
        playableDirector.Play();
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

    private IEnumerator SmoothFreezeDelayed(float from, float to, float duration, PlayerComponent freezeTarget, float delay)
    {
        yield return new WaitForSeconds(delay);
        
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