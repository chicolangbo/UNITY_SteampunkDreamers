using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StateReady : BaseState
{
    //speedBar
    private SpeedBarController speedBarController;
    private bool selectSpeed = false;
    public float accelerator;
    private float timer = 0f;

    //angleBar
    private AngleBarController angleBarController;
    private bool startAngleMove = false;
    private float transitionTime = 6f;

    private float propellerSpeed = 10f;
    private float propellerSpeedMax = 3000f;

    public StateReady(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        speedBarController = controller.speedBar.GetComponent<SpeedBarController>();
        angleBarController = controller.angleBar.GetComponent<AngleBarController>();
    }

    public override void OnExitState()
    {
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
        if(!selectSpeed)
        {
            speedBarController.SpeedBarMoving();
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                selectSpeed = true;
            }
        }
        else
        {
            // 1초 뒤 속력게이지 비활성화
            timer += Time.deltaTime;
            if(timer > 1f && controller.speedBar.activeSelf)
            {
                controller.speedBar.SetActive(false);
            }

            if(!controller.speedBar.activeSelf)
            {
                speedBarController.SetVelocity();

                if (controller.velocity.x > controller.initialSpeed - speedBarController.accelerator * transitionTime && !controller.angleBar.activeSelf)
                {
                    StartAngleBar();
                }

                if(startAngleMove)
                {
                    angleBarController.AngleBarMoving();
                }

                if(controller.angleBar.activeSelf == true && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    controller.launchSuccess = true;
                    startAngleMove = false;
                    angleBarController.SetAngle();
                }
            }
        }

        if(propellerSpeed < propellerSpeedMax)
        {
            propellerSpeed += propellerSpeed * Time.deltaTime;
            controller.ChangePropellerSpeed(propellerSpeed);
        }
    }

    public void StartAngleBar()
    {
        //toRight = false; // 재활용
        controller.angleBar.SetActive(true);
        startAngleMove = true;
    }
}
