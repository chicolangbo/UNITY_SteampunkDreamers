using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

public class StateGliding : BaseState
{
    public bool launchSuccess = false;
    public float accelerator;
    public Vector3 direction = Vector3.zero;

    private float minAngle;
    private float maxAngle;
    private float minRotSpeed = 10f;
    private float maxRotSpeed = 50f;
    private float rotSpeed;

    private Vector3 initialPos;

    // Resistance & Speed
    private bool isRotPossible = false;

    private float airResistance;
    private float minAirResistance = 10f;
    private float maxAirResistance = -15f;

    private float gravity = -15f;
    private float upForce;
    private float minUpForce = 0f;
    private float maxUpForce = 40f;

    public StateGliding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        // 초기 각도 세팅
        //controller.transform.localRotation = Quaternion.Euler(0, 0, EulerToAngle(controller.initialAngle.z));
        controller.transform.localRotation = Quaternion.Euler(0, 0, EulerToAngle(50f)); // test code
        // velocity 적용 -> 발사
        if (launchSuccess)
        {
            direction = controller.transform.right;
            controller.velocity = direction * controller.initialSpeed;
            initialPos = controller.transform.position;
        }
        minAngle = controller.minAngle;
        maxAngle = controller.maxAngle;

        // test code
        controller.velocity = direction * controller.maxSpeed;

        //airflowSpwaner.StartCreateAirflow();
    }     

    public override void OnExitState()
    {
    }

    public override void OnFixedUpdateState()
    {
        RotatePlane(Input.GetMouseButton(0));
        controller.velocity += new Vector3(airResistance, gravity + upForce, 0) * Time.deltaTime;

        //resistance 값 업데이트
        SetResistance(controller.transform.localEulerAngles);


        // x speed 예외 처리
        if (controller.velocity.x < 0)
        {
            controller.velocity.x = 0;
            upForce = 0;
            isRotPossible = false;
        }
        else if(controller.velocity.x > controller.maxSpeed)
        {
            controller.velocity.x = controller.maxSpeed;
        }
        else if( controller.velocity.x > 5f)
        {
            isRotPossible = true;
        }
    }

    public override void OnUpdateState()
    {
        // 방향 업데이트
        direction = controller.transform.right;

        // 회전 속도 업데이트
        SetRotSpeed(controller.velocity.x);

        // 기류 적용
        if (controller.airflows.Count != 0)
        {
            ApplicationAirflow();
        }


        controller.distance = controller.transform.position.x - initialPos.x;
    }

    public void RotatePlane(bool up)
    {   
        if (up && launchSuccess && isRotPossible)
        {
            controller.transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);

            // 최소값 세팅
            //if (controller.velocity.x <= controller.velocity.y) // y값 증가 한계
            //{
            //    Debug.Log("upForce 0");
            //    upForce = 0;
            //    isRotPossible = false;
            //}
        }
        else
        {
            var change = Vector3.forward * -rotSpeed * Time.deltaTime;
            controller.transform.Rotate((change.z < 0) ? change : -change);
            upForce = 0;
        }



        ClampRotation(controller.transform.localEulerAngles);
    }

    public void SetResistance(Vector3 localEulerAngle)
    {
        // 앵글 -> 백분율
        localEulerAngle.z = EulerToAngle(localEulerAngle.z);
        var anglePercentage = (localEulerAngle.z - minAngle) / (maxAngle - minAngle) * 100f;

        // 앵글 - 저항값, upForce 맵핑
        airResistance = anglePercentage / 100 * (maxAirResistance - minAirResistance) + minAirResistance;
        upForce = anglePercentage / 100 * (maxUpForce - minUpForce) + minUpForce;

        //Debug.Log("gravity : " + gravity);
        //Debug.Log("air : " + airResistance);
        //Debug.Log("velocity : " + controller.velocity);
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
            Debug.Log("front");
            airResistance = 0f;
        }
        else
        {
            Debug.Log("reverse");
        }
    }
}
