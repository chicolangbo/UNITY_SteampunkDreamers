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

    private float minRotSpeed = 1f;
    private float maxRotSpeed = 50f;
    private float rotSpeed;

    //private float airResistance = -1f;
    //private float gravity = -10f;

    public StateGliding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        controller.transform.localRotation = Quaternion.Euler(0,0,EulerToAngle(controller.initialAngle.z));
        // velocity 적용
        if(launchSuccess)
        {
            direction = controller.transform.right;
            controller.velocity = direction * controller.initialSpeed;
        }
    }

    public override void OnExitState()
    {
    }

    public override void OnFixedUpdateState()
    {
        MovePlane(Input.GetMouseButton(0));
    }

    public override void OnUpdateState()
    {
        // 방향 업데이트
        direction = controller.transform.right;

        // 회전 속도 업데이트
        SetRotSpeed(controller.velocity.x);

        // resistance 적용
        //controller.velocity += resistance * Time.deltaTime;
    }

    public void MovePlane(bool up)
    {
        Debug.Log(launchSuccess);

        if (up && launchSuccess)
        {
            controller.transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
        }
        else
        {
            var change = Vector3.forward * -rotSpeed * Time.deltaTime;
            controller.transform.Rotate((change.z < 0) ? change : -change);
        }

        ClampRotation(controller.transform.localEulerAngles);
        controller.distance += controller.velocity.x * Time.deltaTime;
    }

    public void SetRotSpeed(float currSpeed)
    {
        float speedRatio = currSpeed / controller.maxSpeed; // 0~1
        rotSpeed = Mathf.Clamp(maxRotSpeed * (1 - speedRatio), minRotSpeed, maxRotSpeed);
        //Debug.Log(rotSpeed);
    }

    public void ClampRotation(Vector3 localEulerAngle)
    {
        //Vector3 targetAngles = controller.transform.localEulerAngles;
        localEulerAngle.z = EulerToAngle(localEulerAngle.z);
        localEulerAngle.z = Mathf.Clamp(localEulerAngle.z, controller.minAngle, controller.maxAngle);
        controller.transform.localRotation = Quaternion.Euler(localEulerAngle);
    }

    public float EulerToAngle(float z)
    {
        return (z > 180f) ? z - 360f : z;
    }
}
