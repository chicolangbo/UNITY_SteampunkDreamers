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
    private float minRotSpeed = 10f; // ����ü ȸ�� �ӵ� minimum
    private float maxRotSpeed = 50f; // ����ü ȸ�� �ӵ� maximum
    private float rotSpeed;
    private Vector3 initialPos;

    // Resistance & Speed
    private bool isRotPossible = true;
    private float airResistance;
    private float airResistanceDown = 5f; // ����ü �ޱ��� 50% ������ �� �������� ���ӵ�
    private float airResistanceUp = -20f; // ����ü �ޱ��� 50% �̻��� �� �ö󰡴� ���ӵ�

    private float inputLimit = 5f; // frontSpeed�� inputLimit ������ �� Ŭ�� ����
    private float inputLimitRelease = 15f; // frontSpeed�� inputLimitRelease �̻��� �� Ŭ�� ���� ����

    public StateGliding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        // �ʱ� ���� ����
        controller.transform.localRotation = Quaternion.Euler(0, 0, EulerToAngle(controller.initialAngle.z));

        // velocity ���� -> �߻�
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
        // ���� ������Ʈ
        direction = controller.transform.right;

        // ����ü �ӵ� ������Ʈ
        controller.frontSpeed += airResistance * Time.deltaTime;
        controller.velocity = direction * ((controller.frontSpeed <= 0f)? 0f : controller.frontSpeed);
        
        // ����ü ȸ��
        RotatePlane(Input.GetMouseButton(0));

        // frontSpeed -> �ޱ� ȸ�� �ӵ� ������Ʈ
        SetRotSpeed(controller.frontSpeed);

        // �ޱ� -> ���װ� ����
        SetResistance(controller.transform.localEulerAngles);

        // distance ������Ʈ
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
        // �ޱ� -> �����
        localEulerAngle.z = EulerToAngle(localEulerAngle.z);
        var anglePercentage = (localEulerAngle.z - minAngle) / (maxAngle - minAngle) * 100f;

        // �ޱ� - airResistance
        if (anglePercentage >= 50)
        {
            airResistance = (anglePercentage - 50) / 50 * airResistanceUp;
        }
        else
        {
            airResistance = (1 - anglePercentage / 50) * airResistanceDown;
        }

        // �Է� ����
        if (controller.fuelTimer <= 0 || controller.frontSpeed + airResistance <= inputLimit)
        {
            controller.fuelTimer = 0;
            isRotPossible = false;
        }
        else if(controller.frontSpeed + airResistance >= inputLimitRelease)
        {
            isRotPossible = true;
        }

        // ��� ����
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
        else // ��ü ��鸮�� ����?
        {
            airResistance -= Time.deltaTime * 100f;
        }
    }
}