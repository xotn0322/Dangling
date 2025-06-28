using UnityEngine;

public class TimeEventManager : MonoBehaviour, IEngineComponent
{
    public static TimeEventManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new TimeEventManager();
            return _instance;
        }
    }
    private static TimeEventManager _instance;

    private GameObject BackGround_1;
    private GameObject BackGround_2;

    public IEngineComponent Init()
    {
        BackGround_1 = GameObject.Find("Map/BackGround/배경 1");
        BackGround_2 = GameObject.Find("Map/BackGround/배경 2");

        BackGround_2.SetActive(false);
        return this;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // 중복 인스턴스 파괴
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);
    }

    public void ChangeBackGround()
    {
        BackGround_1.SetActive(false);
        BackGround_2.SetActive(true);
    }
}