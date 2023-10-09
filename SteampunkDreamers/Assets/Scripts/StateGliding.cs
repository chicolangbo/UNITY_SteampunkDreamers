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
    private float minRotSpeed = 10f;
    private float maxRotSpeed = 50f;
    private float rotSpeed;

    private Vector3 initialPos;

    // Resistance & Speed
    private bool isRotPossible = false;

    private float airResistance;
    private float airResistanceFront = 3f;
    private float airResistanceReverse = -5f;

    private float gravity = -30f;
    private float upForce;
    private float maxUpForce = 30f;

    public StateGliding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        // �ʱ� ���� ����
        controller.transform.localRotation = Quaternion.Euler(0, 0, EulerToAngle(controller.initialAngle.z));

        //test code
        //controller.transform.localRotation = Quaternion.Euler(0, 0, EulerToAngle(50f));

        // velocity ���� -> �߻�
        if (controller.launchSuccess)
        {
            direction = controller.transform.right;
            controller.velocity = direction * controller.initialSpeed;
            initialPos = controller.transform.position;
        }
        minAngle = controller.minAngle;
        maxAngle = controller.maxAngle;

        // test code
        //controller.velocity = direction * controller.maxSpeed;
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

        // x�� �ӵ� -> �ޱ� ȸ�� �ӵ� ������Ʈ
        SetRotSpeed(controller.velocity.x);

        // �ޱ� -> ���װ� ����
        SetResistance(controller.transform.localEulerAngles);

        // speed ���� ó��
        if (controller.velocity.x < 0)
        {
            controller.velocity.x = 0;
            upForce = 0;
            isRotPossible = false;
        }
        else if (controller.velocity.x > direction.x * controller.maxSpeed)
        {
            controller.velocity.x = direction.x * controller.maxSpeed;
        }
        else if (controller.velocity.x > 10f)
        {
            isRotPossible = true;
        }

        if(controller.velocity.y > direction.y * controller.maxSpeed)
        {
            controller.velocity.y = direction.y * controller.maxSpeed;
        }

        controller.velocity += new Vector3(airResistance, gravity + upForce, 0) * Time.deltaTime;
        controller.distance = controller.transform.position.x - initialPos.x;
    }

    public void RotatePlane(bool up)
    {   
        if (up && controller.launchSuccess && isRotPossible)
        {
            controller.transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);

            // �ּҰ� ����
            //if (controller.velocity.x <= controller.velocity.y) // y�� ���� �Ѱ�
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
            //upForce = 0;
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
            airResistance = anglePercentage / 50 * airResistanceReverse;
        }
        else
        {
            airResistance = anglePercentage / 50 * airResistanceFront;
        }

        // �ޱ� - upForce
        if (controller.altitude > 10000)
        {
            upForce = 0;
        }
        else if(anglePercentage >= 50)
        {
            upForce = anglePercentage / 50 * maxUpForce;
        }

        // ��� ����
        if (controller.airflows.Count != 0)
        {
            ApplicationAirflow();
        }

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
            // ��ǳ �ִ� : airResistance �ִ� = direction.x * controller.maxSpeed * 0.4f
            // 0 ~ ��ǳ �ִ� lerp, 4��
            airResistance = Mathf.Lerp(0, controller.maxSpeed * 0.4f, Time.deltaTime * 10f);
            Debug.Log("��ǳ : " + airResistance);
        }
        else // ��ü ��鸮�� ����?
        {
            airResistance -= Time.deltaTime * 100f;
            Debug.Log("��ǳ : " + airResistance);
        }
    }
}
