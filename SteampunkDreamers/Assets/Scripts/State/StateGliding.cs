using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.WSA;

public class StateGliding : BaseState
{
    public Vector3 direction = Vector3.zero;

    private float minAngle;
    private float maxAngle;
    private float minRotSpeed = 5f; // 비행체 회전 속도 minimum, 각도 높아질 때
    private float standardRotSpeed = 10; // 비행체 회전 속도 기준값
    private float maxRotSpeed = 50f; // 비행체 회전 속도 maximum, 각도 낮아질 때
    private float rotSpeed;
    private Vector3 initialPos;

    // Resistance & Speed
    public bool isRotPossible = true;
    private float airResistance;
    private float airResistanceDown = 20f; // 비행체 앵글이 50% 이하일 시 내려가는 가속도
    private float airResistanceUp = -50f; // 비행체 앵글이 50% 이상일 시 올라가는 감속도

    private float minFrontSpeed = 5f;
    private float inputLimit = 5f; // frontSpeed가 inputLimit 이하일 시 클릭 제한
    private float inputLimitRelease = 15f; // frontSpeed가 inputLimitRelease 이상일 시 클릭 제한 해제

    // airflow
    private float airflowFrontRatio = 10; // 순풍
    private float airflowReverseRatio = 100; // 역풍 ( airResistance -= dt*airflowReverseRatio로 적용되어 있음 )

    private bool firstDown;
    private bool jump;
    private float jumpRotateTime = 3f;
    private Quaternion startRotation;
    private Quaternion jumpTargetAngle;
    private float elapsedTime = 0;

    public StateGliding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        // 초기 각도 세팅 & 점프 로테이션 스피드 지정
        jumpTargetAngle = controller.initialAngle;

        // velocity 적용 -> 발사
        if (controller.launchSuccess)
        {
            direction = controller.transform.right;
            initialPos = controller.transform.position;
        }
        controller.frontSpeed = controller.initialSpeed;
        minAngle = controller.minAngle;
        maxAngle = controller.maxAngle;

        startRotation = controller.transform.rotation;
    }     

    public override void OnExitState()
    {
    }

    public override void OnFixedUpdateState()
    {
        if (!jump && controller.launchSuccess)
        {
            Jump(jumpTargetAngle, jumpRotateTime);
        }
    }

    public override void OnUpdateState()
    {
        direction = controller.transform.right;

        if(jump || !controller.launchSuccess)
        {

            if (controller.airshipColiide)
            {
                isRotPossible = false;
            }

            // 비행체 속도 업데이트
            controller.frontSpeed += airResistance * Time.deltaTime;
            if(controller.frontSpeed > controller.maxSpeed)
            {
                controller.frontSpeed = controller.maxSpeed;
            }
            else if(controller.frontSpeed < minFrontSpeed)
            {
                controller.frontSpeed = minFrontSpeed;
            }

            SetRotSpeed(controller.transform.eulerAngles);
            RotatePlane(Input.GetMouseButton(0));
            SetResistance(controller.transform.localEulerAngles);
        }

        // distance 업데이트
        controller.velocity = direction * ((controller.frontSpeed <= 0f) ? 0f : controller.frontSpeed);
        controller.distance = controller.transform.position.x - initialPos.x;
    }

    public void RotatePlane(bool up)
    {
        if (up && controller.launchSuccess && isRotPossible)
        {
            controller.fuelTimer -= Time.deltaTime;
            if( controller.fuelTimer <= 0f )
            {
                controller.fuelTimer = 0f;
            }
            var change = Vector3.forward * rotSpeed * 1.5f * Time.deltaTime;
            controller.transform.Rotate(change);
        }
        else
        {
            var change = Vector3.forward * -rotSpeed * Time.deltaTime;
            controller.transform.Rotate((change.z < 0) ? change : -change);
        }
        controller.transform.localRotation = Utils.ClampRotation(controller.transform.localEulerAngles, minAngle, maxAngle);
    }

    public void SetResistance(Vector3 localEulerAngle)
    {
        // 앵글 -> 백분율
        localEulerAngle.z = Utils.EulerToAngle(localEulerAngle.z);
        var anglePercentage = (localEulerAngle.z - minAngle) / (maxAngle - minAngle) * 100f;

        // 앵글 - airResistance
        if (anglePercentage >= 50 && firstDown)
        {
            airResistance = (anglePercentage - 50) / 50 * airResistanceUp; // -
        }
        else
        {
            if(firstDown == false && anglePercentage < 49)
            {
                firstDown = true;
            }
            airResistance = (1 - anglePercentage / 50) * airResistanceDown; // +
        }

        // 입력 제한
        if (controller.fuelTimer <= 0 || controller.frontSpeed + airResistance <= inputLimit)
        {
            isRotPossible = false;
        }
        else if(controller.frontSpeed + airResistance >= inputLimitRelease)
        {
            isRotPossible = true;
        }

        // 기류 적용
        if (controller.airflows.Count != 0)
        {
            ApplicationAirflow();
        }
    }

    public void SetRotSpeed(Vector3 localEulerAngle)
    {
        localEulerAngle.z = Utils.EulerToAngle(localEulerAngle.z);
        var anglePercentage = (localEulerAngle.z - minAngle) / (maxAngle - minAngle) * 100f;

        if (anglePercentage > 50)
        {
            rotSpeed = standardRotSpeed + (anglePercentage - 50) / 50 * maxRotSpeed; // -
        }
        else
        {
            rotSpeed = standardRotSpeed + (1 - anglePercentage / 50) * minRotSpeed; // +
        }
    }

    public void ApplicationAirflow()
    {
        var airflow = controller.airflows.First.Value;
        if(airflow.airflowType == AirflowType.Front)
        {
            airResistance = Mathf.Lerp(0, controller.maxSpeed * 0.4f, Time.deltaTime * airflowFrontRatio);
        }
        else
        {
            airResistance -= Time.deltaTime * airflowReverseRatio;
        }
    }

    public void Jump(Quaternion targetRotation, float duration)
    {
        if (elapsedTime < duration)
        {
            // 회전 각도를 보간하여 업데이트
            controller.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
        }
        else
        {
            // 최종 목표 회전 적용
            controller.transform.localRotation = Utils.ClampRotation(targetRotation.eulerAngles, minAngle, maxAngle);
            jump = true;
        }
    }
}