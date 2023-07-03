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

        Destroy(player);

    }

    private void onPlayerExitCar()
    {
        Debug.Log("Player Exit Car by CarMovementController");
        isPlayerInCar = false;
        // 玩家离开汽车时，生成Player并恢复Player的状态，生成Player，位置在车辆旁边
        player = Instantiate(playerPrefab, transform.position + transform.up * 2, Quaternion.identity); 
        


        //Unsubscribe from events
         void OnDisable()
        {
            EventManager.OnPlayerEnterCar -= onPlayerEnterCar;
            EventManager.OnPlayerExitCar -= onPlayerExitCar;
        }
         
         
    }
}
