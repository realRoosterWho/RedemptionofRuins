using UnityEngine;

public class CarCollisionHandler : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Invoke the OnWallHit event
        EventManager.InvokeOnWallHit();
    }
}