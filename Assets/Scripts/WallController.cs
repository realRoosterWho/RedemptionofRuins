using UnityEngine;

public class WallController : MonoBehaviour
{
    //定义墙的初始生命值
    public float wallHealth = 100f;
    //引用墙的Rigidbody2D和BoxCollider2D组件
    private Rigidbody2D wallRb;
    private BoxCollider2D wallCollider;

    //在开始的时候获取Rigidbody2D和BoxCollider2D组件
    void Start()
    {
        wallRb = GetComponent<Rigidbody2D>();
        wallCollider = GetComponent<BoxCollider2D>();
    }

    // 当物体与墙碰撞时，将碰撞带来的动量的大小减去生命值
    void OnCollisionEnter2D(Collision2D collision)
    {
        var momentum = collision.relativeVelocity.magnitude * collision.rigidbody.mass;
        wallHealth -= momentum;
        //触发InvokeOnWallHit
        EventManager.InvokeOnWallHit();
        if (wallHealth <= 0)
        {
            DestroyWall();
        }
    }

    //销毁墙的函数
    void DestroyWall()
    {
        //在这里触发事件
        EventManager.InvokeOnWallDestroyed();

        //移除墙的Rigidbody2D和BoxCollider2D组件，并且设置其为非活跃状态
        Destroy(wallRb);
        Destroy(wallCollider);
        this.gameObject.SetActive(false);
    }
}