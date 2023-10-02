using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateReady : BaseState
{
    private bool toRight = true;
    private bool select = false;
    private float controllSpeed = 100f;
    private float timer = 0f;

    public StateReady(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
    }

    public override void OnExitState()
    {
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
        if(!select)
        {
            BarWaiting();
            if (Input.GetMouseButtonDown(0))
            {
                select = true;
            }
        }
        else
        {
            // 1초 뒤 비활성화
            timer += Time.deltaTime;
            if(timer > 1f && controller.setBar.active)
            {
                controller.setBar.SetActive(false);
            }

            if(!controller.setBar.active)
            {
                // 최초 속도에 넣어주고
                controller.initialFrontSpeed = controller.setBarController.value / 100f * controller.frontSpeedMax;

                // frontSpeed = 0 -> 최초 속도로 Lerp
                var tempSpeed = controller.frontSpeed;
                controller.frontSpeed = Mathf.Lerp(tempSpeed, controller.initialFrontSpeed, Time.deltaTime);
            }
        }
    }

    public void BarWaiting()
    {
        if (toRight)
        {
            float tempValue = controller.setBarController.value;
            controller.setBarController.value = Mathf.Clamp(tempValue + Time.deltaTime * controllSpeed, 0, 100f);

            if (controller.setBarController.value >= 100f)
            {
                toRight = false;
            }
        }
        // 100->0
        else
        {
            float tempValue = controller.setBarController.value;
            controller.setBarController.value = Mathf.Clamp(tempValue - Time.deltaTime * controllSpeed, 0, 100f);

            if (controller.setBarController.value <= 0)
            {
                toRight = true;
            }
        }
    }
}
