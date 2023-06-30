using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//todo 模拟真实环境，让汽车的旋转速度和速度有关
public class CarController : MonoBehaviour
{
    // Start is called before the first frame update
    
    //生成汽车的移动速度、旋转速度和最大速度，现在加上摩擦力质量因子
    public float speed = 10.0f;
    public float rotationSpeed  = 100.0f;
    public float maxSpeed = 20.0f;
    public float frictionCoefficient  = 0.05f;
    
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
        //Read the User Input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
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
