using UnityEngine;
using System.Collections; // 你需要这个命名空间来使用协程

public class WallController : MonoBehaviour
{
    // 定义墙的初始生命值
    public float wallHealth = 100f;
    // 引用墙的Rigidbody2D和BoxCollider2D组件
    private Rigidbody2D wallRb;
    private BoxCollider2D wallCollider;
    // 增加对SpriteRenderer的引用
    private SpriteRenderer wallSpriteRenderer;

    // 在开始的时候获取Rigidbody2D、BoxCollider2D和SpriteRenderer组件
    void Start()
    {
        wallRb = GetComponent<Rigidbody2D>();
        wallCollider = GetComponent<BoxCollider2D>();
        wallSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 当物体与墙碰撞时，将碰撞带来的动量的大小减去生命值
    void OnCollisionEnter2D(Collision2D collision)
    {
        var momentum = collision.relativeVelocity.magnitude * collision.rigidbody.mass;
        wallHealth -= momentum;
        
        // 如果碰撞的物体的tag是"Car"，那么触发InvokeOnWallHit
        if (!collision.gameObject.CompareTag("Player"))
        {
            EventManager.InvokeOnWallHit();
        }

        // 设置颜色为红色，并开始协程将颜色在0.3秒后恢复为白色
        wallSpriteRenderer.color = Color.red;
        StartCoroutine(ResetColorAfterTime(0.3f));

        if (wallHealth <= 0)
        {
            DestroyWall();
        }
    }

    // 销毁墙的函数
    void DestroyWall()
    {
        // 在这里触发事件
        EventManager.InvokeOnWallDestroyed();

        // 移除墙的Rigidbody2D和BoxCollider2D组件，并且设置其为非活跃状态
        Destroy(wallRb);
        Destroy(wallCollider);
        this.gameObject.SetActive(false);
    }

    IEnumerator ResetColorAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        wallSpriteRenderer.color = Color.white;
    }
}