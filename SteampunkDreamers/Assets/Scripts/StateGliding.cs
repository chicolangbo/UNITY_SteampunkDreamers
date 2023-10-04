using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

public class StateGliding : BaseState
{
    private bool launchSuccess = true;

    public float rotUpSpeed = 10f;
    public float rotDownSpeed = 30f;
    public float airResistance = 10f;

    private float standardFrontSpeed;

    public StateGliding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        controller.angleBar.SetActive(false);
        controller.transform.localRotation = controller.initialAngle;
        standardFrontSpeed = controller.initialSpeed;
        //StateLaunch launch = (StateLaunch)controller.stateMachine.GetState(StateName.Launch);
        //launchSuccess = launch.launchSuccess;
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
        standardFrontSpeed = Mathf.Clamp(standardFrontSpeed -Time.deltaTime, 0, standardFrontSpeed);
        var angle = EulerToAngle(controller.transform.localEulerAngles.z);
        var sin = Mathf.Sin(angle);
        controller.speed = Mathf.Sqrt((1 - Mathf.Abs(sin)) * Mathf.Pow(standardFrontSpeed, 2));
    }

    private void MovePlane(bool up)
    {
        if (up && launchSuccess)
        {
            controller.transform.Rotate(Vector3.forward * rotUpSpeed * Time.deltaTime);
            controller.rb.AddForce(new Vector3(0, 10f, 0), ForceMode.Force);
        }
        else
        {
            var change = Vector3.forward * rotDownSpeed * Time.deltaTime;
            controller.transform.Rotate((change.z < 0) ? change : -change);
        }

        ClampRotation();
        controller.distance += controller.initialSpeed * Time.deltaTime;
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
}
