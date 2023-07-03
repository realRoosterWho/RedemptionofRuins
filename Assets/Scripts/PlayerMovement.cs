using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f; // 玩家移动的速度
    public float interactRadius = 1.5f; // 玩家可以与之交互的桶的最大距离
    public float bucketMass = 0f; // 水桶的质量
    public static float playerMass = 2f; // 玩家的质量
    
    public float hasBucketTime = 0f; // 玩家拥有水桶的时间
    
    //获取空桶的Prefab
    public GameObject emptyBucketPrefab;


    // 设置布尔值：玩家是否获得桶
    public bool hasBucket = false;

    private void Start()
    {
        // 订阅水桶事件
        EventManager.OnPlayerGetBucket += onPlayerGetBucket;
        // 定义丢桶事件
        EventManager.OnPlayerDropBucket += onPlayerDropBucket;
        
    }

    // 定义一个枚举，用于跟踪玩家的移动状态，有上下左右静止多个状态
    public enum MovementState
    {
        Idle, // 静止状态
        Up, // 向上移动
        Down, // 向下移动
        Left, // 向左移动
        Right // 向右移动

    }

    public MovementState currentState; // 当前的移动状态

    // 用于每一帧的更新
    void Update()
    {
        // 读取玩家的键盘输入
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 创建一个表示移动方向的向量
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);

        //log移动向量
        //Debug.Log(movement);
        
        //log桶的质量
        Debug.Log(bucketMass);

        // 根据玩家的输入，更新玩家的移动状态
        if (moveHorizontal > 0)
        {
            currentState = MovementState.Right;
        }
        else if (moveHorizontal < 0)
        {
            currentState = MovementState.Left;
        }
        else if (moveVertical > 0)
        {
            currentState = MovementState.Up;
        }
        else if (moveVertical < 0)
        {
            currentState = MovementState.Down;
        }
        else
        {
            currentState = MovementState.Idle;
        }

        //保持玩家的物体不旋转
        transform.rotation = Quaternion.identity;

        // 应用移动向量来驱动玩家的移动
        transform.position += movement * speed * Time.deltaTime;
        
        
        // 如果玩家拥有桶，计时器开始计时
        if (hasBucket)
        {
            hasBucketTime += Time.deltaTime;
        }
        
        // 如果玩家没有桶，计时器归零
        if (!hasBucket)
        {
            hasBucketTime = 0f;
        }
        
        //Log Timer
//        Debug.Log(hasBucketTime);
        
    }

    void onPlayerGetBucket()
    {
        hasBucket = true;
        Debug.Log("Player has got the bucket");

        //找到最近的桶
        // 通过Physics2D.OverlapCircleAll()方法找到所有与玩家重叠的碰撞器
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRadius);

        /*
        // 找到最近的桶
        Transform closestBucket = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider2D collider in colliders)
                {
                    // 如果碰撞器的标签是Bucket
                    if (collider.CompareTag("Bucket"))
                    {
                        // 计算玩家和桶之间的距离
                        float distance = Vector3.Distance(transform.position, collider.transform.position);

                        // 如果距离比最近的距离还要小
                        if (distance < closestDistance)
                        {
                            // 更新最近的距离
                            closestDistance = distance;

                            // 更新最近的桶
                            closestBucket = collider.transform;
                        }
                    }
                }
            */
        
        // 从EventManager中获取最近的桶
        GameObject closestBucket = null;
        closestBucket = EventManager.FindNearestBucket();
        
        
        // 对最近的桶进行操作
        Debug.Log("The closest bucket is " + closestBucket.name);
        
        // 记录最近的桶的bucketMass
        BucketStatus bucketStatus = closestBucket.GetComponent<BucketStatus>();
        this.bucketMass = bucketStatus.bucketMass;
        Debug.Log("The mass of the closest bucket is " + bucketMass);
        
        // 记录玩家的质量（为玩家的质量加上最近的桶的质量）
        playerMass += bucketMass;
        Debug.Log("The mass of the player is " + playerMass);
        
        
        
        // 摧毁最近的桶
        Destroy(closestBucket.gameObject);

    }
    
    // 定义丢桶事件
    void onPlayerDropBucket()
    {
        hasBucket = false;
        Debug.Log("Player has dropped the bucket");
        
        // 生成一个新的桶
        GameObject bucket = Instantiate(emptyBucketPrefab);
        
        // 设置新生成的桶的质量
        bucket.GetComponent<BucketStatus>().bucketMass = bucketMass;

        // 设置桶的位置,桶生成在玩家左侧
        bucket.transform.position = transform.position + new Vector3(-1, 0, 0);

        // 设置桶的质量
        bucket.GetComponent<Rigidbody2D>().mass = bucketMass;
        
        // 设置桶的序号
        bucket.GetComponent<BucketStatus>().bucketNumber = 1;
        
        // 设置桶的质量
        bucket.GetComponent<BucketStatus>().bucketMass = bucketMass;
        
        // 减少玩家的质量
        playerMass -= bucketMass;

    }
    
    // 取消订阅事件
    private void OnDestroy()
    {
        EventManager.OnPlayerGetBucket -= onPlayerGetBucket;
        EventManager.OnPlayerDropBucket -= onPlayerDropBucket;
    }
}