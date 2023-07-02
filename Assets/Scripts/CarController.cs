using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//todo 模拟真实环境，让汽车的旋转速度和速度有关
//todo 减少汽车漂移

public class CarController : MonoBehaviour
{
    // Start is called before the first frame update
    
    //汽车的参数
    public float speed = 10.0f; //移动速度
    public float rotationSpeed  = 100.0f; //旋转速度
    public float maxSpeed = 20.0f; //最大速度
    public float maxRotationSpeed = 30.0f; //最大旋转速度
    public float frictionCoefficient  = 0.05f; //摩擦力系数
    public float rotationCoefficient = 10; //旋转系数，用来调整旋转速度和汽车速度的关系
    public float sideFrictionCoefficient = 0.05f; //侧向摩擦力系数
    private float moveVertical;
    private float moveHorizontal;
    
    
    //汽车的行为状态变量
    public bool isBeingEntered = false; //是否正在被玩家控制
    public bool isWithBucket = false; //是否有水桶
    
    

    //汽车的刚体组件
    private Rigidbody2D rb;
    
    void Start()
    {
        //获取汽车的Rigitbody2D组件
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //Update the car's parameters according to the status
        
        
        //如果玩家在车上，那么读取玩家的输入
        if (true)
        {
            //读取玩家的输入
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            //汽车的速度越大，旋转速度越大；速度为零时，旋转速度为零
            }
        else
        {
            //如果玩家不在车上，那么汽车的速度和旋转速度都为零
            rotationSpeed = 0;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }
        
        //打印beignEntered的状态
        //Debug.Log("isBeingEntered: " + isBeingEntered);
        
        //这个比值需要可以调整，现在是10
        rotationSpeed = rb.velocity.magnitude * rotationCoefficient;
        if (rb.velocity.magnitude == 0)
        {
            rotationSpeed = 0;
        }
        
        //Max rotation angle
        //汽车的旋转速度不能超过最大旋转速度maxRotationSpeed
        if (rotationSpeed > maxRotationSpeed)
        {
            rotationSpeed = maxRotationSpeed;
        }
        //用侧向摩擦力来减少汽车的漂移
//todo 侧向摩擦力的大小和汽车的速度有关
        //计算侧向摩擦力的大小
        float sideFrictionMagnitude = sideFrictionCoefficient * rb.mass;
        //计算侧向摩擦力的方向
        Vector2 sideFriction = -sideFrictionMagnitude * rb.velocity.normalized;
        //应用侧向摩擦力
        rb.AddForce(sideFriction);
        

        //计算汽车新的速度和方向,写代码并注释
        Vector2 speed = transform.up * moveVertical * this.speed;
        float direction = -moveHorizontal * rotationSpeed;
        
        // 应用新的速度和方向
        rb.AddForce(speed);
        rb.angularVelocity = direction;
        
        //限制汽车的最大速度
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        if (moveHorizontal == 0)
        {
            // Apply friction
            float frictionMagnitude = frictionCoefficient * rb.mass;
            Vector2 friction = -frictionMagnitude * rb.velocity.normalized;
            rb.AddForce(friction);
        }
    }
}
