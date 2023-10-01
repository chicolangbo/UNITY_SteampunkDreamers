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

    public StateGliding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        controller.transform.localRotation = controller.initialRotation;
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
        controller.altitude = controller.rb.position.y * controller.altitudeRatio;
        controller.distance += controller.initialFrontSpeed * Time.deltaTime;
    }

    public void ClampRotation()
    {
        Vector3 targetAngles = controller.transform.localEulerAngles;
        targetAngles.z = (targetAngles.z > 180f) ? targetAngles.z - 360f : targetAngles.z;
        targetAngles.z = Mathf.Clamp(targetAngles.z, controller.minAngle, controller.maxAngle);
        controller.transform.localRotation = Quaternion.Euler(targetAngles);
    }
}
