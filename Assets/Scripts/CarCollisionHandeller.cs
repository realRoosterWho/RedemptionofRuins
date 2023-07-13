using UnityEngine;

public class CarCollisionHandler : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 检查碰撞对象的标签是否为 "Wall"
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Invoke the OnWallHit event
            EventManager.InvokeOnWallHit();
        }
    }
}