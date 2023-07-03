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

    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float rotationCoefficient = 10;
    public bool isPlayerInCar = true;
    public float maxRotationSpeed = 30.0f;
    
    public float bucketMass = 0f; // 水桶的质量
    public float playerMass = 2f; // 玩家的质量
    public bool hasBucket = false; // 是否拿着水桶

    private Rigidbody2D rb;

    void Start()
    {
        EventManager.OnPlayerEnterCar += onPlayerEnterCar;
        EventManager.OnPlayerExitCar += onPlayerExitCar;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (isPlayerInCar == false)
        {
            moveHorizontal = 0;
            moveVertical = 0;
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
        Debug.Log("Player Enter Car by CarMovementController");
        isPlayerInCar = true;
        
        //存储玩家的质量、是否携带桶、桶质量
        playerMass = player.GetComponent<PlayerMovement>().playerMass;
        hasBucket = player.GetComponent<PlayerMovement>().hasBucket;
        bucketMass = player.GetComponent<PlayerMovement>().bucketMass;
        
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

        // 恢复玩家的质量、是否携带桶、桶质量
        player.GetComponent<PlayerMovement>().playerMass = playerMass;
        player.GetComponent<PlayerMovement>().hasBucket = hasBucket;
        player.GetComponent<PlayerMovement>().bucketMass = bucketMass;



    }
    
    void OnDisable()
    {
        EventManager.OnPlayerEnterCar -= onPlayerEnterCar;
        EventManager.OnPlayerExitCar -= onPlayerExitCar;
    }
}
