using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class CarPhysicsController : MonoBehaviour
{
    public float maxSpeed = 20.0f;
    public float frictionCoefficient = 0.05f;

    public float sideFrictionCoefficient = 0.05f;
    private PlayerMovement playerMovement;
    
    public float bucketMass = 0f; // 水桶的质量
    public float playerMass = 2f; // 玩家的质量
    // 设置布尔值：玩家是否获得桶
    public bool hasBucket = false;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //监听玩家进入车内事件
        EventManager.OnPlayerEnterCar += onPlayerEnterCar;
        //监听玩家离开车内事件
        EventManager.OnPlayerExitCar += onPlayerExitCar;
        
        //如果玩家不是空的，那么获取玩家的实例化脚本
        if (GameObject.FindWithTag("Player") != null)
        {
            playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        }

    }

    void FixedUpdate()
    {

        
        // 控制旋转速度
        float currentSpeed = rb.velocity.magnitude;
        
        // 汽车的刚体质量越大，最大速度越小
        maxSpeed = 90f / rb.mass;

        // 控制最大速度
        if (currentSpeed > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // 侧向摩擦力
        float sideFrictionMagnitude = sideFrictionCoefficient * rb.mass;
        Vector2 sideFrictionDirection = -sideFrictionMagnitude * rb.velocity.normalized;
        rb.AddForce(sideFrictionDirection);

        // 当没有输入时施加摩擦力
        if (Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.01f)
        {
            float frictionMagnitude = frictionCoefficient * rb.mass;
            Vector2 frictionDirection = -frictionMagnitude * rb.velocity.normalized;
            rb.AddForce(frictionDirection);
        }
    }
    
    // 玩家进入车内时，读取CarMovementController里面的玩家数据，将玩家的PlayerMass加到车的刚体质量上
    void onPlayerEnterCar()
    {
        //读取CarMovementController里面的玩家数据，将玩家的PlayerMass加到车的刚体质量上
        rb.mass += GameObject.FindWithTag("Car").GetComponent<CarMovementController>().playerMass;
    }

    // 玩家离开车内时，将玩家的PlayerMass从车的刚体质量上减去
    void onPlayerExitCar()
    {
        //将玩家的PlayerMass从车的刚体质量上减去
        rb.mass -= GameObject.FindWithTag("Car").GetComponent<CarMovementController>().playerMass;
    }

    //取消订阅
    void OnDestroy()
    {
        EventManager.OnPlayerEnterCar -= onPlayerEnterCar;
        EventManager.OnPlayerExitCar -= onPlayerExitCar;
    }
    

}
