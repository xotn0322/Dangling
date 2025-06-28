using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class SceneButtonUtil : MonoBehaviour
{

    public UnityEvent onClick;

    public void OnMouseUpAsButton()
    {
        onClick?.Invoke();
    }
}
