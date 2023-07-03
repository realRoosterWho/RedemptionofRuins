using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class CarPhysicsController : MonoBehaviour
{
    public float maxSpeed = 20.0f;
    public float frictionCoefficient = 0.05f;

    public float sideFrictionCoefficient = 0.05f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //监听玩家进入车内事件
        EventManager.OnPlayerEnterCar += onPlayerEnterCar;
        //监听玩家离开车内事件
        EventManager.OnPlayerExitCar += onPlayerExitCar;
        
        //更新车质量
        rb.mass += PlayerMovement.playerMass;
    }

    void FixedUpdate()
    {
        // 控制旋转速度
        float currentSpeed = rb.velocity.magnitude;

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
    
    // 玩家进入车内时，将玩家的PlayerMass加到车的刚体质量上
    void onPlayerEnterCar()
    {
        rb.mass += PlayerMovement.playerMass;
    }
    
    // 玩家离开车内时，将玩家的PlayerMass从车的刚体质量上减去
    void onPlayerExitCar()
    {
        rb.mass -= PlayerMovement.playerMass;
    }
    
    //取消订阅
    void OnDestroy()
    {
        EventManager.OnPlayerEnterCar -= onPlayerEnterCar;
        EventManager.OnPlayerExitCar -= onPlayerExitCar;
    }
    

}


