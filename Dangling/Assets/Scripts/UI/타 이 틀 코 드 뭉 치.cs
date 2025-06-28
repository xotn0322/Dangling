using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    public GameObject 크_레_딧_화_면;
    public List<GameObject> 타_이_틀_배_경 = new();
    public float 가_짜_로_딩_시_간 = 1f;

    public void Awake()
    {
        foreach (var item in 타_이_틀_배_경)
        {
            item.SetActive(false);
        }
        크_레_딧_닫_기();
    }

    public void 마_우_스_올_렸_을_때(int 인_덱_스)
    {
        타_이_틀_배_경[인_덱_스].SetActive(true);
    }

    public void 마_우_스_내_렸_을_때(int 인_덱_스)
    {
        타_이_틀_배_경[인_덱_스].SetActive(false);
    }


    public void 게_임_시_작()
    {
        var 로_딩_엔_진 = Task.Run(() => GameManager.Instance.InitMonoBehaviourGameEngine());

        CSceneManager.Instance.LoadScene("MainScene", 로_딩_엔_진, 진_짜_시_작, 가_짜_로_딩_시_간);
    }

    public void 진_짜_시_작()
    {

    }

    public void 게_임_종_료()
    {
        GameManager.Instance.QuitGame();
    }

    public void 크_레_딧_열_기()
    {
        크_레_딧_화_면.SetActive(true);
    }

    public void 크_레_딧_닫_기()
    {
        크_레_딧_화_면.SetActive(false);
    }
}
