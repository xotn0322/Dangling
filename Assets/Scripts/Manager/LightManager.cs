using System.Collections;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour, IEngineComponent
{
    public static LightManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private static LightManager _instance;

    private Color redColor = Color.red;
    private Color orangeColor = new Color(1f, 0.5f, 0f, 1f); // 주황색
    private Color goalColor;
    private GameObject Alert;
    private Light2D globalLight;
    private bool isCoroutine = false;

    [SerializeField] private float transitionDuration = 0.7f; // 색상 전환 시간

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // 중복 인스턴스 파괴
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);
    }

    public IEngineComponent Init()
    {
        Alert = GameObject.Find("Alert");
        globalLight = Alert.GetComponent<Light2D>();

        return this;
    }

    public void StartAlert(string color)
    {
        if (!isCoroutine)
        {
            isCoroutine = true;
            Alert.SetActive(true);
            
            switch(color)
            {
                case "Red":
                    goalColor = redColor;
                    StartCoroutine(OnAlert4F());
                    break;

                case "Orange":
                    goalColor = orangeColor;
                    StartCoroutine(OnAlert8F());
                    break;
            }
        }
    }

    private IEnumerator OnAlert4F()
    {        
        for (int i = 0; i < 2; i++) // 2번 반복
        {
            yield return StartCoroutine(TransitionIntensity(0f, 4f, transitionDuration));
            
            yield return StartCoroutine(TransitionIntensity(4f, 0f, transitionDuration));
        }

        StopAlert();
    }

    private IEnumerator OnAlert8F()
    {        
        for (int i = 0; i < 2; i++) // 2번 반복
        {
            yield return StartCoroutine(TransitionIntensity(0f, 8f, transitionDuration));
            
            yield return StartCoroutine(TransitionIntensity(8f, 0f, transitionDuration));
        }

        StopAlert();
    }
    
    private IEnumerator TransitionIntensity(float startIntensity, float endIntensity, float transitionTime)
    {
        float elapsed = 0f;
        
        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionTime;
            
            // 부드러운 intensity 전환
            globalLight.intensity = Mathf.Lerp(startIntensity, endIntensity, t);
            
            yield return null;
        }
        
        // 정확한 최종 intensity 설정
        globalLight.intensity = endIntensity;
    }
    
    public void StopAlert()
    {
        if (isCoroutine)
        {
            StopAllCoroutines();
            isCoroutine = false;
            Alert.SetActive(false);
        }
    }
}