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
    private float minRotSpeed = 1f;
    private float maxRotSpeed = 50f;
    private float rotSpeed;

    // Resistance & Speed
    // x up
    public float xSpeed;
    private float minXSpeed = 0.2f;
    private float maxXSpeed = 2f;
    // x down
    private float airResistance;
    private float minAirResistance = 3f;
    private float maxAirResistance = -5f;
    // y up
    public float ySpeed;
    private float minYSpeed = 5f;
    private float maxYSpeed = 15f;
    // y down
    private float gravity;
    private float mingravity = 10f;
    private float maxgravity = -15f;

    public StateGliding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        // �ʱ� ���� ����
        controller.transform.localRotation = Quaternion.Euler(0, 0, EulerToAngle(controller.initialAngle.z));

        // velocity ���� -> �߻�
        if (launchSuccess)
        {
            direction = controller.transform.right;
            controller.velocity = direction * controller.initialSpeed;
        }
        minAngle = controller.minAngle;
        maxAngle = controller.maxAngle;
    }

    public override void OnExitState()
    {
    }

    public override void OnFixedUpdateState()
    {
        RotatePlane(Input.GetMouseButton(0));
    }

    public override void OnUpdateState()
    {
        // ���� ������Ʈ
        direction = controller.transform.right;

        // ȸ�� �ӵ� ������Ʈ
        SetRotSpeed(controller.velocity.x);

        //resistance �� ������Ʈ
        SetResistance(controller.transform.localEulerAngles);
        controller.velocity += new Vector3(airResistance, gravity, 0) * Time.deltaTime;
    }

    public void RotatePlane(bool up)
    {   
        if (up && launchSuccess)
        {
            controller.transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
            // minAirResistance, minGravity ����
            minAirResistance -= Time.deltaTime;
            mingravity -= Time.deltaTime;
            Debug.Log(mingravity);
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

        // �ޱ� - ���װ� ����
        airResistance = anglePercentage / 100 * (maxAirResistance - minAirResistance) + minAirResistance;
        gravity = (1 - anglePercentage / 100) * (maxgravity -  mingravity) + mingravity;
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
