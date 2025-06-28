using UnityEngine;

public class WaterSplashTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        WaterMovementController water = other.GetComponent<WaterMovementController>();
        if (water != null)
        {
            float splashX = transform.position.x;
            float splashForce = 2.0f;
            water.SplashAtPosition(splashX, splashForce);
        }
    }
}