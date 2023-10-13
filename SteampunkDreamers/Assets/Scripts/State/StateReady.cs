using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
            if (Input.GetMouseButtonDown(0))
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

                if(controller.angleBar.activeSelf == true && Input.GetMouseButtonDown(0))
                {
                    controller.launchSuccess = true;
                    startAngleMove = false;
                    angleBarController.SetAngle();
                }
            }
        }
    }

    public void StartAngleBar()
    {
        //toRight = false; // 재활용
        controller.angleBar.SetActive(true);
        startAngleMove = true;
    }
}
