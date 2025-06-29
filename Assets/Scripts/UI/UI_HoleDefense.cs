using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_HoleDefense : UI_Base
{
    enum Texts
    {
        TIMER,

    }

    enum Sliders
    {
        WATERLEVEL,
    }

    enum Images
    {
    }

    enum GameObjects
    {

    }

    [Header("component")]
    UIManager ui = new UIManager();

    [Header("Bind")]
    private Text timer;
    private Slider waterLevel;
    private Image iceBlock1;
    private float elapsedTime = 0f;
    private float elapsedTime1 = 0f;
    private bool isMeltingStarted = false;
    private float duration=3f;

    void Start()
    {
        init();
        //mapping
        timer = Get<Text>((int)Texts.TIMER);
        waterLevel = Get<Slider>((int)Sliders.WATERLEVEL);
    }

    private void init()
    {
        ui.SetCanvas(gameObject, false);

        Bind<Slider>(typeof(Sliders));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));


        FindComponent();
    }


    // Update is called once per frame
    void Update()
    {
        UpdateTimerUI();
        UpdateWaterLevelSlider();
    }


    private void UpdateTimerUI()
    {
        elapsedTime += Time.deltaTime;
        timer.text = $"{elapsedTime:F2}";
    }

    private void UpdateWaterLevelSlider()
    {
        float _currentWaterValue = HoleDefenseManager.Instance.GetCurrentWaterValue();
        float _maxWaterValue = HoleDefenseManager.Instance.GetMaxWaterValue();

        if (_maxWaterValue <= 0f) return;

        float normalizedValue = Mathf.Clamp01(_currentWaterValue / _maxWaterValue);
        waterLevel.value = normalizedValue;
    }

    private void UpdateIceBlockUI()
    {
       
    }



    private void FindComponent()
    {
        
                
    }

    

    public float GetUIValue()
    {
        return 0.0f;
    }
}
