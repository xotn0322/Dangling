using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    public GameObject CreditWindow;
    public List<GameObject> TitleButtonBack = new();
    public float FakeLoadingTime = 1f;

    public void Awake()
    {
        foreach (var item in TitleButtonBack)
        {
            item.SetActive(false);
        }
        CloseCreditWinodw();
    }

    public void OnMouseHover(int index)
    {
        TitleButtonBack[index].SetActive(true);
    }

    public void OnMouseExit(int index)
    {
        TitleButtonBack[index].SetActive(false);
    }


    public void StartLoading()
    {
        var loadingEngine = Task.Run(() =>
        {
            GameManager.Instance.LoadGame();
        });

        CSceneManager.Instance.LoadScene("MainScene", loadingEngine, StartGame, FakeLoadingTime);
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    public void OpenCreditWinodw()
    {
        CreditWindow.SetActive(true);
    }

    public void CloseCreditWinodw()
    {
        CreditWindow.SetActive(false);
    }
}
