using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CSceneManager : MonoBehaviour
{
    public static CSceneManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private static CSceneManager _instance;

    private static string _nextScene;

    public GameObject LoadingWindow;

    public void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }

    public void LoadScene(string sceneName, Task loadAction, Action onLoadCompleteAction, float minLoadingTime = 0f)
    {
        LoadingWindow.SetActive(true);

        _nextScene = sceneName;
        SceneManager.LoadScene(Constant.Scene.LOADING_SCENE_NAME);
        StartCoroutine(LoadSceneProcess(loadAction, onLoadCompleteAction, minLoadingTime));
    }

    IEnumerator LoadSceneProcess(Task loadAction, Action onLoadCompleteAction, float minLoadingTime = 0f)
    {
        // Wait 2 frames for LoadingScene
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        AsyncOperation op = SceneManager.LoadSceneAsync(_nextScene);
        op.allowSceneActivation = false;

        float loadingTime = 0f;

        while ((loadAction != null && !loadAction.IsCompleted) ||
               (op.progress < 0.9f) ||
               loadingTime < minLoadingTime)
        {
            yield return new WaitForEndOfFrame();

            loadingTime += Time.deltaTime;

            //// TODO : Show Loading Progress

        }

        op.allowSceneActivation = true;

        // Wait 2 frames for applying new Scene
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        LoadingWindow.SetActive(false);

        onLoadCompleteAction.RunExt();
    }
}
