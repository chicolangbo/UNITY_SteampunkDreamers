using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateGliding : BaseState
{
    public float rotUpSpeed = 10f;
    public float rotDownSpeed = 30f;
    private float frontSpeed;
    private float frontSpeedMax;
    public float maxAngle = 50f;
    public float minAngle = -50f;
    private float altitude = 1f;
    public float altitudeRatio;
    private float distance = 0f;

    public StateGliding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        frontSpeedMax = controller.initialFrontSpeed + 100f;
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
        if (up)
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
        altitude = controller.rb.position.y * altitudeRatio;
        distance += controller.initialFrontSpeed * Time.deltaTime;
    }

    public void ClampRotation()
    {
        Vector3 targetAngles = controller.transform.localEulerAngles;
        targetAngles.z = (targetAngles.z > 180f) ? targetAngles.z - 360f : targetAngles.z;
        targetAngles.z = Mathf.Clamp(targetAngles.z, minAngle, maxAngle);
        controller.transform.localRotation = Quaternion.Euler(targetAngles);
    }
}
