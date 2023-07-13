using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasBarSlider : MonoBehaviour
{

        public Slider gasSlider_Car; // Reference to the Slider UI element
        public Slider gasSlider_Player; // Reference to the Slider UI element
        public GameObject vehicle;
        private CarMovementController _carMovementController;// Reference to the vehicle object
        private PlayerMovement _playMovementController;
        public GameObject player;
        public bool isPlayerInCar = false;

        private void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        
        {
            _carMovementController = GameObject.FindWithTag("Car").GetComponent<CarMovementController>();
            isPlayerInCar = _carMovementController.isPlayerInCar;
            
            //如果玩家不存在，那么玩家的UI不显示
            if (GameObject.FindWithTag("Player") == null) 
            {
                //那么显示车辆中的gasMass
                gasSlider_Player.value = _carMovementController.gasMass;
                gasSlider_Player.maxValue = 10f;
            }
            else
            {
                _playMovementController = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
                gasSlider_Player.value = _playMovementController.gasMass;
                gasSlider_Player.maxValue = _playMovementController.maxGasMass;
            }

            gasSlider_Car.maxValue =15; // Set the maximum value of the slider

            gasSlider_Car.value = _carMovementController.carGas; // Update the slider value with the current GasMass
        }
        
        //当玩家进入车辆
        private void OnPlayerEnterCar()
        {
            isPlayerInCar = true;
        }
    }  
