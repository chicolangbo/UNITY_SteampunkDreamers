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
    private float airResistance;
    private float minAirResistance = 10f;
    private float maxAirResistance = -15f;
    private float gravity = -15f;
    private float upForce;
    private float minUpForce = 0f;
    private float maxUpForce = 60f;

    public StateGliding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        // original code
        // �ʱ� ���� ����
        //controller.transform.localRotation = Quaternion.Euler(0, 0, EulerToAngle(controller.initialAngle.z));
        controller.transform.localRotation = Quaternion.Euler(0, 0, EulerToAngle(50f)); // test code
        // velocity ���� -> �߻�
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
    }     

    public override void OnExitState()
    {
    }

    public override void OnFixedUpdateState()
    {
        // ���� ������Ʈ
        direction = controller.transform.right;

        RotatePlane(Input.GetMouseButton(0));

        //resistance �� ������Ʈ
        SetResistance(controller.transform.localEulerAngles);
        controller.velocity += new Vector3(airResistance, gravity, 0) * Time.deltaTime;

        // ȸ�� �ӵ� ������Ʈ
        SetRotSpeed(controller.velocity.x);
    }

    public override void OnUpdateState()
    {
        controller.distance = controller.transform.position.x - initialPos.x;
    }

    public void RotatePlane(bool up)
    {   
        if (up && launchSuccess)
        {
            controller.transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
            controller.velocity.y += upForce * Time.deltaTime;
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
        // �ޱ� -> �����
        localEulerAngle.z = EulerToAngle(localEulerAngle.z);
        var anglePercentage = (localEulerAngle.z - minAngle) / (maxAngle - minAngle) * 100f;

        // �ޱ� - ���װ�, upForce ����
        airResistance = anglePercentage / 100 * (maxAirResistance - minAirResistance) + minAirResistance;
        upForce = anglePercentage / 100 * (maxUpForce - minUpForce) + minUpForce;

        // �ּҰ� ����
        if(controller.velocity.x < 0) // �ڷΰ��� �� ����
        {
            airResistance = 0;
            upForce = 0;
        }
        if (controller.velocity.x <= controller.velocity.y) // y�� ���� �Ѱ輱
        {
            upForce = 0;
            //upLimited = true;
            //y�� �� �ö󰡵��� ����(y���װ� �ִ�) &x���װ��� �ּ�
            //gravity = maxgravity;
            //UnityEditor.EditorApplication.isPaused = true;
        }
        else
        {
            //upLimited = false;
        }
        Debug.Log("gravity : " + gravity);
        Debug.Log("air : " + airResistance);
        Debug.Log("velocity : " + controller.velocity);
    }

    public void SetXYSpeed()
    {
        // ���� ó��

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
}
