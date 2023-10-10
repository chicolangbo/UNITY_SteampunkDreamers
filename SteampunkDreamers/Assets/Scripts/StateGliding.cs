using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.WSA;

public class StateGliding : BaseState
{
    public Vector3 direction = Vector3.zero;

    private float minAngle;
    private float maxAngle;
    private float minRotSpeed = 10f; // 비행체 회전 속도 minimum
    private float maxRotSpeed = 50f; // 비행체 회전 속도 maximum
    private float rotSpeed;
    private Vector3 initialPos;

    // Resistance & Speed
    private bool isRotPossible = true;
    private float airResistance;
    private float airResistanceDown = 5f; // 비행체 앵글이 50% 이하일 시 내려가는 가속도
    private float airResistanceUp = -20f; // 비행체 앵글이 50% 이상일 시 올라가는 감속도

    private float inputLimit = 5f; // frontSpeed가 inputLimit 이하일 시 클릭 제한
    private float inputLimitRelease = 15f; // frontSpeed가 inputLimitRelease 이상일 시 클릭 제한 해제

    public StateGliding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        // 초기 각도 세팅
        controller.transform.localRotation = Quaternion.Euler(0, 0, EulerToAngle(controller.initialAngle.z));

        // velocity 적용 -> 발사
        if (controller.launchSuccess)
        {
            direction = controller.transform.right;
            controller.frontSpeed = controller.initialSpeed;
            //controller.velocity = direction * controller.frontSpeed;
            initialPos = controller.transform.position;
        }
        minAngle = controller.minAngle;
        maxAngle = controller.maxAngle;
    }     

    public override void OnExitState()
    {
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
        // 방향 업데이트
        direction = controller.transform.right;

        // 비행체 속도 업데이트
        controller.frontSpeed += airResistance * Time.deltaTime;
        controller.velocity = direction * ((controller.frontSpeed <= 0f)? 0f : controller.frontSpeed);
        
        // 비행체 회전
        RotatePlane(Input.GetMouseButton(0));

        // frontSpeed -> 앵글 회전 속도 업데이트
        SetRotSpeed(controller.frontSpeed);

        // 앵글 -> 저항값 세팅
        SetResistance(controller.transform.localEulerAngles);

        // distance 업데이트
        controller.distance = controller.transform.position.x - initialPos.x;
    }

    public void RotatePlane(bool up)
    {   
        if (up && controller.launchSuccess && isRotPossible)
        {
            controller.fuelTimer -= Time.deltaTime;
            controller.transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
        }
        else
        {
            var change = Vector3.forward * -rotSpeed * Time.deltaTime;
            controller.transform.Rotate((change.z < 0) ? change : -change);
        }
        ClampRotation(controller.transform.localEulerAngles);
    }

    public void SetResistance(Vector3 localEulerAngle)
    {
        // 앵글 -> 백분율
        localEulerAngle.z = EulerToAngle(localEulerAngle.z);
        var anglePercentage = (localEulerAngle.z - minAngle) / (maxAngle - minAngle) * 100f;

        // 앵글 - airResistance
        if (anglePercentage >= 50)
        {
            airResistance = (anglePercentage - 50) / 50 * airResistanceUp;
        }
        else
        {
            airResistance = (1 - anglePercentage / 50) * airResistanceDown;
        }

        // 입력 제한
        if (controller.fuelTimer <= 0 || controller.frontSpeed + airResistance <= inputLimit)
        {
            controller.fuelTimer = 0;
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

    public void SetRotSpeed(float currSpeed)
    {
        float speedRatio = currSpeed / controller.maxSpeed; // 0~1
        rotSpeed = Mathf.Clamp(maxRotSpeed * (1 - speedRatio), minRotSpeed, maxRotSpeed);
    }

    public void ClampRotation(Vector3 localEulerAngle)
    {
        //Vector3 targetAngles = controller.transform.localEulerAngles;
        localEulerAngle.z = EulerToAngle(localEulerAngle.z);
        localEulerAngle.z = Mathf.Clamp(localEulerAngle.z, minAngle, maxAngle);
        controller.transform.localRotation = Quaternion.Euler(localEulerAngle);
    }

    public float EulerToAngle(float z)
    {
        return (z > 180f) ? z - 360f : z;
    }

    public void ApplicationAirflow()
    {
        var airflow = controller.airflows.First.Value;
        if(airflow.airflowType == AirflowType.Front)
        {
            airResistance = Mathf.Lerp(0, controller.maxSpeed * 0.4f, Time.deltaTime * 10f);
        }
        else // 기체 흔들리는 연출?
        {
            airResistance -= Time.deltaTime * 100f;
        }
    }
}