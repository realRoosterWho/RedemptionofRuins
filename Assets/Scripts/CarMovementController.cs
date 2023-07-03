using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//Event List In EventManager
//public static event Action OnPlayerEnterCar;
//public static event Action OnPlayerExitCar;
//public static event Action OnPlayerNearCar;

public class CarMovementController : MonoBehaviour
{

    public GameObject playerPrefab; // Player的预制体
    private GameObject player; // 当前的Player实例

    public float maxSpeed = 20.0f;
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float rotationCoefficient = 10;
    public bool isPlayerInCar = true;
    public float maxRotationSpeed = 30.0f;
    
    public float bucketMass = 0f; // 水桶的质量
    public float playerMass = 2f; // 玩家的质量
    public bool hasBucket = false; // 是否拿着水桶
    public float gasMass = 0.0f;
    public float carMass = 8.00f;
    public float emptyBucketMass = 0.0f;

    private Rigidbody2D rb;

    void Start()
    {
        EventManager.OnPlayerEnterCar += onPlayerEnterCar;
        EventManager.OnPlayerExitCar += onPlayerExitCar;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // 如果玩家在车内，那么车的燃料在不断减少
        if (isPlayerInCar == true)
        {
            gasMass -= 0.005f;
        }

        // 如果燃料耗尽，那么触发事件
        if (gasMass <= 0)
        {
            //锁死燃料到0
            gasMass = 0;
            
            EventManager.InvokeOnCarOutOfGas();
        }
        
        bucketMass = emptyBucketMass + gasMass;
        rb.mass = bucketMass + playerMass + carMass;
        
        //如果燃料耗尽，汽车不可以移动，汽车不接受输入；不然就可以移动，汽车接受输入；
        //如果燃料耗尽，汽车可以移动；否则，汽车不接受输入
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");


        if (isPlayerInCar == false || gasMass <= 0)
        {
            moveHorizontal = 0;
            moveVertical = 0;
        }


        // 汽车的刚体质量越大，最大速度越小
        maxSpeed = 90f / rb.mass;
        // 控制旋转速度
        float currentSpeed = rb.velocity.magnitude;
        // 控制最大速度
        if (currentSpeed > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        //这个比值需要可以调整，现在是10
        rotationSpeed = rb.velocity.magnitude * rotationCoefficient;
        if (rb.velocity.magnitude == 0)
        {
            rotationSpeed = 0;
        }

        //汽车的旋转速度不能超过最大旋转速度maxRotationSpeed
        if (rotationSpeed > maxRotationSpeed)
        {
            rotationSpeed = maxRotationSpeed;
        }

        Vector2 speed = transform.up * moveVertical * this.speed;
        float direction = -moveHorizontal * rotationSpeed;

        rb.AddForce(speed);
        rb.angularVelocity = direction;


    }

    private void onPlayerEnterCar()
    {
        // 如果玩家在加油，玩家不能上车
        if (player.GetComponent<PlayerMovement>().isAddingGas == true)
        {
            return;
        }
        
        
        
        Debug.Log("Player Enter Car by CarMovementController");
        isPlayerInCar = true;
        
        //存储玩家的质量、是否携带桶、桶质量
        playerMass = player.GetComponent<PlayerMovement>().playerMass;
        hasBucket = player.GetComponent<PlayerMovement>().hasBucket;
        bucketMass = player.GetComponent<PlayerMovement>().bucketMass;
        emptyBucketMass = player.GetComponent<PlayerMovement>().emptyBucketMass;
        gasMass = player.GetComponent<PlayerMovement>().gasMass;
        
        // Debug 记录汽车存储的玩家质量
        Debug.Log("Player Mass: " + playerMass);
        Debug.Log("Has Bucket: " + hasBucket);
        Debug.Log("Bucket Mass: " + bucketMass);

        Destroy(player);

    }

    private void onPlayerExitCar()
    {
        Debug.Log("Player Exit Car by CarMovementController");
        isPlayerInCar = false;
        // 玩家离开汽车时，生成Player并恢复Player的状态，生成Player，位置在车辆旁边
        player = Instantiate(playerPrefab, transform.position + transform.right * -1, Quaternion.identity);

        // 恢复玩家的质量、是否携带桶、桶质量、汽油质量、空桶质量
        player.GetComponent<PlayerMovement>().playerMass = playerMass;
        player.GetComponent<PlayerMovement>().hasBucket = hasBucket;
        player.GetComponent<PlayerMovement>().bucketMass = bucketMass;
        player.GetComponent<PlayerMovement>().gasMass = gasMass;
        player.GetComponent<PlayerMovement>().emptyBucketMass = emptyBucketMass;



    }
    
    void OnDisable()
    {
        EventManager.OnPlayerEnterCar -= onPlayerEnterCar;
        EventManager.OnPlayerExitCar -= onPlayerExitCar;
    }
}
