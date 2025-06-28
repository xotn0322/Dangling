using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public enum UIEvent
    {
        Click,
        Drag
    }

    [Header("Field")]
    private int _order = 10;
    //static Stack<UI_MainScene> _mainScenesStack = new Stack<UI_MainScene>();
    public static GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
            {
                root = new GameObject { name = "@UI_Root" };
            }

            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);

        canvas.renderMode = RenderMode.ScreenSpaceCamera;

        //SetUICamera(canvas);

        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
            canvas.sortingOrder = 0;
    }

    public void SetUICamera(Canvas canvas)
    {
        GameObject go = GameObject.Find("UI Camera");
        if (go == null)
        {
            Debug.Log("UI Camera Missing");
        }

        Camera uiCamera = Util.GetOrAddComponent<Camera>(go);
        if (uiCamera == null)
        {
            Debug.Log("UI Camera object does not have a Camera component.");
            return;
        }

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            canvas.worldCamera = uiCamera;
        }
    }

    public static T ShowSceneUI<T>(string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        string path = $"{Constant.Path.DEFAULT_RESOURCES_DEFAULT_UI_SCENE_PATH}/{name}";
        GameObject go = Util.Instantiate(path);

        T sceneUI = Util.GetOrAddComponent<T>(go);

        return sceneUI;
    }

    public void Clear()
    {
        ////TODO
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        { return; }

        Object.Destroy(go);
    }
}
