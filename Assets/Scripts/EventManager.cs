using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    //定义玩家上车事件
    public static event Action OnPlayerEnterCar;
    
    //定义玩家下车事件
    public static event Action OnPlayerExitCar;
    
    //检查玩家是否靠近车辆事件
    public static event Action OnPlayerNearCar;
    
    //检查玩家离最近的桶的距离
    public static event Action OnPlayerNearBucket;
    
    //定义玩家拿桶事件
    public static event Action OnPlayerGetBucket;
    
    //定义玩家丢桶事件
    public static event Action OnPlayerDropBucket;
    
    //检查玩家是否靠近加油站
    public static event Action OnPlayerNearGasStation;
    
    //定义汽车燃料用尽事件
    public static event Action OnCarOutOfGas;
    
    //定义汽车太重了事件
    public static event Action OnCarTooHeavy;
    
    //一堵墙被破坏事件
    public static event Action OnWallDestroyed;
    
    //游戏结束事件
    public static event Action OnGameOver;



    bool isPlayerNearCar = false;
    bool isPlayerInCar = false;
    bool isPlayerNearBucket = false;
    bool isPlayerNearGasStation = false;

    private bool cannotEnterCar = false;
    private bool cannotGetBucket = false;
    private bool cannotDropBucket = false;
    

    
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isPlayerNearCar = PlayerNearCar();
        isPlayerInCar = PlayerInCar();
        isPlayerNearBucket = PlayerNearBucket();
        //Debug.Log(isPlayerNearCar + "isPlayerNearCar");
        //Debug.Log(isPlayerInCar + "isPlayerInCar");

        cannotDropBucket = false;
        cannotEnterCar = false;
        cannotGetBucket = false;
        
        
        
        
        //Debug.Log("EventManager is running");

        
        //检查玩家是否靠近桶事件：如果玩家靠近桶，那么触发检查玩家是否靠近桶事件
        if (isPlayerNearBucket)
        {
            //Debug.Log("Player is near the bucket");
            OnPlayerNearBucket?.Invoke();
        }
        
        
        // 在EventManager里：
        // 如果玩家为空，那么不获取playerMovement
        if (GameObject.FindWithTag("Player") != null)
        {
            PlayerMovement playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
            // 玩家拿桶事件：如果玩家按下了Space，且玩家靠近桶，且玩家没有拿桶，那么触发玩家拿桶事件
            if (Input.GetKeyDown(KeyCode.Space) && isPlayerNearBucket && !playerMovement.hasBucket)
            {
                Debug.Log("Player is trying to get the bucket");
                OnPlayerGetBucket?.Invoke();
            }
    
            // 玩家丢桶事件：如果玩家按下了Space，且玩家在拿了桶后超过一秒钟，那么触发玩家丢桶事件
            if (Input.GetKeyDown(KeyCode.Space) && playerMovement.hasBucket && (playerMovement.hasBucketTime > 1.0f) && !isPlayerNearCar && !cannotDropBucket)
            {
                Debug.Log("Player is trying to drop the bucket");
                OnPlayerDropBucket?.Invoke();
            }
            
            
        }
        
        //玩家上车事件：如果玩家按下了Space，且玩家靠近车辆，且玩家没有在车上，那么触发玩家上车事件
        if (Input.GetKeyDown(KeyCode.Space) && !isPlayerInCar && isPlayerNearCar && !isPlayerNearBucket && !cannotEnterCar)
        {
            Debug.Log("Player is trying to enter the car");
            OnPlayerEnterCar?.Invoke();
        }

        
        //玩家下车事件：如果玩家按下了Space，且玩家在车上，那么触发玩家下车事件
        if (Input.GetKeyDown(KeyCode.Space) && isPlayerInCar)
        {
            Debug.Log("Player is trying to exit the car");
            OnPlayerExitCar?.Invoke();
        }
        
        //检查玩家是否靠近车辆事件：如果玩家靠近车辆，那么触发检查玩家是否靠近车辆事件
        if (isPlayerNearCar)
        {
            //Debug.Log("Player is near the car");
            OnPlayerNearCar?.Invoke();
        }

    }

    public static void InvokeOnCarOutOfGas()
    {
        Debug.Log("Car is out of gas");
        OnCarOutOfGas?.Invoke();
    }

    public static void InvokeOnCarTooHeavy()
    {
        Debug.Log("Car is too heavy");
        OnCarTooHeavy?.Invoke();
    }
    
    public static void InvokeOnWallDestroyed()
    {
        Debug.Log("A wall is destroyed");
        OnWallDestroyed?.Invoke();
    }
    
    public static void TriggerEventGameOver()
    {
        Debug.Log("Game Over");
        OnGameOver?.Invoke();
    }


    // 检查玩家是否在车上
        bool PlayerInCar()
        {
            //Debug.Log("Checking if player is in the car");
            //如果汽车的状态为isBeingEntered，那么玩家在车上
            if (GameObject.FindWithTag("Car").GetComponent<CarMovementController>().isPlayerInCar == true)
            {
                //Debug.Log("Player is in the car");
                return true;
            }
            else
            {
                //Debug.Log("Player is not in the car");
                return false;
            }
            
        }
        
        // 检查玩家是否靠近车辆
        bool PlayerNearCar()
        {
            //如果没有玩家，那么玩家不靠近车上
            if (GameObject.FindWithTag("Player") == null)
            {
                return false;
            }
            
            
            //Debug.Log("Checking if player is near the car");
            //获取玩家的位置
            Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
            //获取车辆的位置
            Vector3 carPosition = GameObject.FindWithTag("Car").transform.position;
            //计算玩家和车辆的距离
            float distance = Vector3.Distance(playerPosition, carPosition);
            
            //打印玩家和车辆的距离
            //Debug.Log("The distance between player and car is " + distance);
            
            //如果玩家和车辆的距离小于1.5，那么玩家靠近车辆
            if (distance < 1.5)
            {
                //log
                //Debug.Log("Player is near the car");
                
                this.cannotDropBucket = true;
                return true;
            }
            else
            {
                //log
                //Debug.Log("Player is not near the car");
                
                this.cannotDropBucket = false;
                return false;
            }
        }
        
        // 检查玩家是否靠近最近的桶
        bool PlayerNearBucket()
        {
            //寻找最近的桶
            GameObject nearestBucket = FindNearestBucket();
    
            //如果没有玩家，那么玩家不靠近桶
            if (GameObject.FindWithTag("Player") == null)
            {
                this.cannotEnterCar = false;
                return false;
            }
    
            //如果没有桶，那么玩家不靠近桶
            if (nearestBucket == null)
            {
                this.cannotEnterCar = false;
                return false;
            }
    
            //获取玩家的位置
            Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
            //获取桶的位置
            Vector3 bucketPosition = nearestBucket.transform.position;
            //计算玩家和桶的距离
            float distance = Vector3.Distance(playerPosition, bucketPosition);
    
            //如果玩家和桶的距离小于1.5，那么玩家靠近桶
            if (distance < 1.5)
            {
                //log
                //Debug.Log("Player is near the bucket");
                this.cannotEnterCar = true;
                return true;
            }
            else
            {
                //log
                //Debug.Log("Player is not near the bucket");
                this.cannotEnterCar = false;
                return false;
            }
        }

        
        //找到最近的桶
        public static GameObject FindNearestBucket()
        {
            //获取所有桶
            GameObject[] buckets = GameObject.FindGameObjectsWithTag("Bucket");
            //如果没有桶，那么返回空
            if (buckets.Length == 0)
            {
                return null;
            }
            
            //如果没有玩家，那么返回空
            if (GameObject.FindWithTag("Player") == null)
            {
                return null;
            }
            
            //获取玩家的位置
            Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
            //初始化最近的桶
            GameObject nearestBucket = buckets[0];
            //初始化最近的桶和玩家的距离
            float nearestDistance = Vector3.Distance(playerPosition, nearestBucket.transform.position);
            
            //遍历所有桶
            foreach (GameObject bucket in buckets)
            {
                //获取桶的位置
                Vector3 bucketPosition = bucket.transform.position;
                //计算玩家和桶的距离
                float distance = Vector3.Distance(playerPosition, bucketPosition);
                
                //如果玩家和桶的距离小于最近的桶和玩家的距离，那么更新最近的桶和玩家的距离
                if (distance < nearestDistance)
                {
                    nearestBucket = bucket;
                    nearestDistance = distance;
                }
            }
            // log nearest bucket
            Debug.Log("The nearest bucket is " + nearestBucket.name);

            //返回最近的桶
            return nearestBucket;
        }
        
        void InvokeNearGasStation()
        {
            OnPlayerNearGasStation?.Invoke();
        }

}
