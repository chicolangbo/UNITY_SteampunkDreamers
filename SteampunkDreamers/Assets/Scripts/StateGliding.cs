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

    //private float airResistance = -1f;
    //private float gravity = -10f;

    public StateGliding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        controller.transform.localRotation = Quaternion.Euler(0,0,EulerToAngle(controller.initialAngle.z));
        // velocity 적용
        direction = controller.transform.right;
        controller.velocity = direction * controller.initialSpeed;
    }

    public override void OnExitState()
    {
    }

    public override void OnFixedUpdateState()
    {
        //MovePlane(Input.GetMouseButton(0));
    }

    public override void OnUpdateState()
    {
        // 방향 업데이트
        direction = controller.transform.right;

        // resistance 적용
        //controller.velocity += resistance * Time.deltaTime;
    }

    private void MovePlane(bool up)
    {
        //if (up && launchSuccess)
        //{
        //    controller.transform.Rotate(Vector3.forward * rotUpSpeed * Time.deltaTime);
        //    controller.rb.AddForce(new Vector3(0, 10f, 0), ForceMode.Force);
        //}
        //else
        //{
        //    var change = Vector3.forward * rotDownSpeed * Time.deltaTime;
        //    controller.transform.Rotate((change.z < 0) ? change : -change);
        //}

        //ClampRotation();
        //controller.distance += controller.initialSpeed * Time.deltaTime;
    }

    public void ClampRotation()
    {
        Vector3 targetAngles = controller.transform.localEulerAngles;
        targetAngles.z = EulerToAngle(targetAngles.z);
        targetAngles.z = Mathf.Clamp(targetAngles.z, controller.minAngle, controller.maxAngle);
        controller.transform.localRotation = Quaternion.Euler(targetAngles);
    }

    public float EulerToAngle(float z)
    {
        return (z > 180f) ? z - 360f : z;
    }

    public void EulerToDirection(float z)
    {
        var angle = z;
        var angleInRadians = angle * Mathf.Deg2Rad; // 각도->라디안 변환
        direction = new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians), 0); // 디렉션 변환
        direction.Normalize();
        Debug.Log(direction);
    }
}
