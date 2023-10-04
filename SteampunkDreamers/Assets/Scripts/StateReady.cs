using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateReady : BaseState
{
    private Transform player;

    //speedBar
    private bool toRight = true;
    private bool selectSpeed = false;
    private float controllSpeed_1 = 66.67f; // 0->100���� 1.5��, �պ� 3��
    private float accelerator;
    private float timer = 0f;

    //angleBar
    private bool startAngleMove = false;
    public bool selectAngle = false;
    private float controllSpeed_2 = 33.33f; // 0->100���� 3��, �պ� X
    private float transitionTime = 3f;
    private float minAngle = 1f;
    private float maxAngle = 90f;

    public StateReady(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        player = controller.transform;
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
            SpeedBarMoving();
            if (Input.GetMouseButtonDown(0))
            {
                selectSpeed = true;
            }
        }
        else
        {
            // 1�� �� �ӷ°����� ��Ȱ��ȭ
            timer += Time.deltaTime;
            if(timer > 1f && controller.speedBar.active)
            {
                controller.speedBar.SetActive(false);
            }

            if(!controller.speedBar.active)
            {
                SetVelocity();

                if (controller.velocity.x > controller.initialSpeed - accelerator * transitionTime && !controller.angleBar.active)
                {
                    StartAngleBar();
                }

                if(startAngleMove)
                {
                    AngleBarMoving();
                }

                if(controller.angleBar.active && Input.GetMouseButtonDown(0))
                {
                    selectAngle = true;
                    startAngleMove = false;
                    SetAngle();
                }
            }
        }
    }

    public void SpeedBarMoving()
    {
        if (toRight)
        {
            // 1.5�� ����
            float tempValue = controller.speedBarController.value;
            controller.speedBarController.value = Mathf.Clamp(tempValue + Time.deltaTime * controllSpeed_1, 0, 100f);

            if (controller.speedBarController.value >= 100f)
            {
                toRight = false;
            }
        }
        // 100->0
        else
        {
            float tempValue = controller.speedBarController.value;
            controller.speedBarController.value = Mathf.Clamp(tempValue - Time.deltaTime * controllSpeed_1, 0, 100f);

            if (controller.speedBarController.value <= 0)
            {
                toRight = true;
            }
        }
    }

    public void AngleBarMoving()
    {
        // 3�� ����
        float tempValue = controller.angleBarController.value;
        controller.angleBarController.value = Mathf.Clamp(tempValue + Time.deltaTime * controllSpeed_2, 0, 100f);

        if (controller.angleBarController.value >= 100f) // ��ȸ �Ϸ�
        {
            controller.angleBar.SetActive(false);
        }
    }

    public void SetVelocity()
    {
        // ����(�ִ�) �ӷ� ����
        // value 0~70 : �ִ� �ӷ��� 50%
        // value 70~80, 90~100 : �ִ� �ӷ��� 70%
        // vlaue 80~90 : �ִ� �ӷ��� 90%
        var value = controller.speedBarController.value;
        if (value < 70)
        {
            controller.initialSpeed = controller.maxSpeed * 0.5f;
        }
        else if ((value >= 70 && value < 80) || (value >= 90 && value <= 100))
        {
            controller.initialSpeed = controller.maxSpeed * 0.7f;
        }
        else if (value >= 80 && value < 90)
        {
            controller.initialSpeed = controller.maxSpeed * 0.9f;
        }

        accelerator = Mathf.Pow(controller.initialSpeed, 2) / 220f; // 220 = board ����(110) * 2
        controller.velocity += new Vector3(accelerator * Time.deltaTime, 0, 0);
    }

    public void SetAngle()
    {
        // ���� ���� ����
        // value 0~60, 91~100 : 1���� ����
        // value 61~70, 81~90
        // value 71~80
        var value = controller.angleBarController.value;
        controller.initialAngle = new Quaternion(0, 0, (value<1)?1:value - 30f, 1);
    }

    public void StartAngleBar()
    {
        controller.angleBar.SetActive(true);
        startAngleMove = true;
    }
}
