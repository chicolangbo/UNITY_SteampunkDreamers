using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

public class StateGliding : BaseState
{
    public bool launchSuccess = false;
    
    private Vector3 resistance = new Vector3(-1f, -9.8f, 0f);

    public StateGliding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        controller.transform.localRotation = Quaternion.Euler(0,0,EulerToAngle(controller.initialAngle.z));
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
        controller.velocity += resistance;
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
}
