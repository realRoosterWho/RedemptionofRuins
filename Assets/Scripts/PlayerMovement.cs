using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public Slider healthSlider;
    public Vector3 healthSliderOffset; // 滑块相对玩家位置的偏移
    public float speed = 5.0f; // 玩家移动的速度
    public float interactRadius = 1.5f; // 玩家可以与之交互的桶的最大距离
    
    public float bucketMass = 0f; // 水桶的质量
    public float playerMass = 2f; // 玩家的质量
    public float emptyBucketMass = 0.0f;
    public float gasMass = 0.0f;
    
    // 设置布尔值：玩家是否获得桶
    public bool hasBucket = false;
    public bool isAddingGas = false;
    public bool canAddGas = false;
    
    
    public float hasBucketTime = 0f; // 玩家拥有水桶的时间
    public float addGasTime = 0f; // 玩家加油的时间
    
    //获取空桶的Prefab
    public GameObject emptyBucketPrefab;
    
    public Sprite playerSpriteUp;     // 玩家向上移动的贴图
    public Sprite playerSpriteDown;   // 玩家向下移动的贴图
    public Sprite playerSpriteLeft;   // 玩家向左移动的贴图
    public Sprite playerSpriteRight;  // 玩家向右移动的贴图
    public Sprite playerSpriteIdle;   // 玩家静止时的贴图

    public float health = 100;
    void onTakeDamage(float damage) 
    {
        Debug.Log("TakeDamage");
        health -= damage;
        healthSlider.value = health;  // 更新血条
        if (health <= 0) 
        {
            //在GameController里面调用onGameOver
            EventManager.TriggerEventGameOver(); 
        }
    }



    private void Start()
    {
        // 订阅水桶事件
        EventManager.OnPlayerGetBucket += onPlayerGetBucket;
        // 定义丢桶事件
        EventManager.OnPlayerDropBucket += onPlayerDropBucket;
        // 订阅玩家受伤
        EventManager.OnPlayerAttacked += onTakeDamage;
        
        healthSlider.maxValue = 100;
        healthSlider.value = health;
        
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
        // 更新血条的位置
        healthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.5f, 0)); // 更新血条的位置,具体而言，就是将世界坐标转换为屏幕坐标，然后加上偏移量，再加上一个向上的偏移量，这样血条就会显示在玩家头顶上方了
        healthSlider.value = health;  // 更新血条
        bucketMass = emptyBucketMass + gasMass;
        
        // 获取玩家的SpriteRenderer组件
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

// 根据玩家的移动状态来更改贴图
        switch (currentState)
        {
            case MovementState.Up:
                spriteRenderer.sprite = playerSpriteUp;
                break;
            case MovementState.Down:
                spriteRenderer.sprite = playerSpriteDown;
                break;
            case MovementState.Left:
                spriteRenderer.sprite = playerSpriteLeft;
                break;
            case MovementState.Right:
                spriteRenderer.sprite = playerSpriteRight;
                break;
            case MovementState.Idle:
                spriteRenderer.sprite = playerSpriteIdle;
                break;
        }

        
        
        if (isAddingGas == true)
        {
            EventManager.InvokeAddingGas();
        }

        if (health <= 0) {
            //在GameController里面调用onGameOver
            EventManager.TriggerEventGameOver();
            
        }
        
        // 玩家移动的速度和水桶质量有关，如果玩家携带水桶
        if (hasBucket)
        {
            speed = 5f / (bucketMass/2 + 1); 
        }
        else
        {
            speed = 5f;
        }

        // 读取玩家的键盘输入
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        // 如果玩家正在加油，不移动
        if (isAddingGas)
        {
            moveHorizontal = 0;
            moveVertical = 0;
        }

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
        
        //如果玩家有桶且玩家的桶的GasMass = 0，那么触发事件
        if (hasBucket && gasMass == 0)
        {
            EventManager.InvokeOnEmptyBucket();
        }
        
        //检测玩家是否靠近加油站
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRadius);
        foreach (Collider2D collider in colliders)
        {
            // 如果碰撞器的标签是GasStation
            if (collider.CompareTag("GasStation"))
            {
                // 计算玩家和加油站之间的距离
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                // 如果距离小于交互半径
                if (distance < interactRadius)
                {
                    // 如果玩家拥有桶
                    if (hasBucket)
                    {
                        Debug.Log("Player can add gas");
                        canAddGas = true;
                    }
                    // 如果玩家没有桶
                    else
                    {
                        canAddGas = false;
                    }
                    }
                else
                {
                    canAddGas = false; //如果距离大于交互半径，玩家不可加油
                }
                }
            }
        //记录玩家处于isAddingGas的时间，也就是记录addGas的时间
        if (isAddingGas)
        {
            addGasTime += Time.deltaTime;
        }
        else
        {
            addGasTime = 0f;
        }
        


        //如果玩家可加油，且玩家按下E键，加油
        //具体而言，每秒钟玩家的Gas的质量都增加，直到再次按下E键退出加油
        if (canAddGas && Input.GetKeyDown(KeyCode.E) && (!isAddingGas))
        {
            Debug.Log("Player is adding gas");
            isAddingGas = true;
        }
        // 如果玩家加油时间超过0.5秒钟且按下E键，并且处于加油状态，退出加油
        if (addGasTime > 0.5f && Input.GetKeyDown(KeyCode.E) && isAddingGas)
        {
            Debug.Log("Player is not adding gas");
            isAddingGas = false;
        }
        
        // 如果玩家远离加油站，退出加油状态
        if (!canAddGas)
        {
            isAddingGas = false;
        }
        
        
        // 如果状态是在加油，那么每秒钟玩家的Gas的质量都增加0.2f；但是如果加到上限了，就不能再加了，而且会自动退出加油状态
        if (isAddingGas)
        {
            if (gasMass < 10.0f)
            {
                //每秒(Time)，增加0.2f
                gasMass += 0.8f * Time.deltaTime;
                
            }
            else
            {
                gasMass = 10.0f;
                isAddingGas = false;
            }
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
        
        // 记录最近的桶的bucketMass以及gasMass和emptyMass
        BucketStatus bucketStatus = closestBucket.GetComponent<BucketStatus>();
        this.bucketMass = bucketStatus.bucketMass;
        this.emptyBucketMass = bucketStatus.emptyBucketMass;
        this.gasMass = bucketStatus.gasMass;
        
        Debug.Log("The mass of the closest bucket is " + bucketMass);
        
        
        
        // 摧毁最近的桶
        Destroy(closestBucket.gameObject);

    }
    
    // 定义丢桶事件
    void onPlayerDropBucket()
    {
        // 如果玩家在加油，玩家不能扔桶
        if (isAddingGas)
        {
            return;
        }


        hasBucket = false;
        Debug.Log("Player has dropped the bucket");
        
        // 生成一个新的桶
        GameObject bucket = Instantiate(emptyBucketPrefab);
        
        // 设置新生成的桶的质量
        bucket.GetComponent<BucketStatus>().bucketMass = bucketMass;
        bucket.GetComponent<BucketStatus>().gasMass = gasMass;
        bucket.GetComponent<BucketStatus>().emptyBucketMass = emptyBucketMass;

        // 设置桶的位置,桶生成在玩家移动方向的相反的一侧
        switch (currentState)
        {
            case MovementState.Up:
                bucket.transform.position = transform.position + new Vector3(0, -1, 0);
                break;
            case MovementState.Down:
                bucket.transform.position = transform.position + new Vector3(0, 1, 0);
                break;
            case MovementState.Left:
                bucket.transform.position = transform.position + new Vector3(1, 0, 0);
                break;
            case MovementState.Right:
                bucket.transform.position = transform.position + new Vector3(-1, 0, 0);
                break;
            case MovementState.Idle:
                bucket.transform.position = transform.position + new Vector3(0, 1, 0);
                break;
        }
        
        
        // 设置桶的质量
        bucket.GetComponent<Rigidbody2D>().mass = bucketMass;
        
        // 设置桶的序号
        bucket.GetComponent<BucketStatus>().bucketNumber = 1;
        
        // 设置桶的质量
        bucket.GetComponent<BucketStatus>().bucketMass = bucketMass;
        
    }
    
    // 取消订阅事件
    private void OnDestroy()
    {
        EventManager.OnPlayerGetBucket -= onPlayerGetBucket;
        EventManager.OnPlayerDropBucket -= onPlayerDropBucket;
        EventManager.OnPlayerAttacked -= onTakeDamage;
    }
}
