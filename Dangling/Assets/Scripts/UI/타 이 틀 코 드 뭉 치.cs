using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    public GameObject ũ_��_��_ȭ_��;
    public List<GameObject> Ÿ_��_Ʋ_��_�� = new();
    public float ��_¥_��_��_��_�� = 1f;

    public void Awake()
    {
        foreach (var item in Ÿ_��_Ʋ_��_��)
        {
            item.SetActive(false);
        }
        ũ_��_��_��_��();
    }

    public void ��_��_��_��_��_��_��(int ��_��_��)
    {
        Ÿ_��_Ʋ_��_��[��_��_��].SetActive(true);
    }

    public void ��_��_��_��_��_��_��(int ��_��_��)
    {
        Ÿ_��_Ʋ_��_��[��_��_��].SetActive(false);
    }


    public void ��_��_��_��()
    {
        var ��_��_��_�� = Task.Run(() => GameManager.Instance.InitMonoBehaviourGameEngine());

        CSceneManager.Instance.LoadScene("MainScene", ��_��_��_��, ��_¥_��_��, ��_¥_��_��_��_��);
    }

    public void ��_¥_��_��()
    {

    }

    public void ��_��_��_��()
    {
        GameManager.Instance.QuitGame();
    }

    public void ũ_��_��_��_��()
    {
        ũ_��_��_ȭ_��.SetActive(true);
    }

    public void ũ_��_��_��_��()
    {
        ũ_��_��_ȭ_��.SetActive(false);
    }
}
