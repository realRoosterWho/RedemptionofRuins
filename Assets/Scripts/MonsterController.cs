using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float speed = 5.0f;
    public float attackRange = 1.0f;
    public float damagePerSecond = 10.0f;

    private Rigidbody2D rb;
    private GameObject player;
    private GameObject car;
    private bool isPlayerInRange = false; // 新增标志

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        player = GameObject.FindWithTag("Player");
        car = GameObject.FindWithTag("Car");
        GameObject target = GetNearestTarget();
        if (target != null)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            rb.AddForce(direction * speed); // 使用动力学驱动怪物运动

            if (target == player)
            {
                float distance = Vector2.Distance(transform.position, target.transform.position);
                isPlayerInRange = distance <= attackRange; // 更新标志
            }
            else
            {
                isPlayerInRange = false; // 更新标志
            }

            if (isPlayerInRange) // 如果玩家在攻击范围内
            {
                // 在这里调用玩家脚本的受伤方法
                EventManager.InvokeOnPlayerAttacked(damagePerSecond * Time.deltaTime);
            }
        }
    }

    GameObject GetNearestTarget()
    {
        if (player != null)
        {
            // 如果玩家存在，优先追踪玩家
            Debug.Log("Player");
            return player;
        }
        else if (car != null)
        {
            // 如果玩家不存在，但汽车存在，追踪汽车
            Debug.Log("Car");
            return car;
        }
        else
        {
            // 如果玩家和汽车都不存在，返回null
            return null;
        }
    }
}