using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_HoleDefense : UI_Base
{
    enum Sliders
    {
        
    }

    enum Images
    {

    }

    enum GameObjects
    {

    }

    [Header("component")]
    UIManager ui = new UIManager();



    void Start()
    {
        init();
    }

    private void init()
    {
        ui.SetCanvas(gameObject, false);

        Bind<Slider>(typeof(Sliders));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        FindComponent();
    }


    // Update is called once per frame
    void Update()
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
