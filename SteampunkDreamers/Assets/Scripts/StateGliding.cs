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

    private bool upLimited = false;

    // Resistance & Speed
    // x up
    //public float xSpeed;
    //private float minXSpeed = 0.2f;
    //private float maxXSpeed = 2f;
    // x down
    private float airResistance;
    private float minAirResistance = 3f;
    private float maxAirResistance = -5f;
    // y up
    //public float ySpeed;
    //private float minYSpeed = 5f;
    //private float maxYSpeed = 15f;
    // y down
    private float gravity = -15f;
    private float upForce;
    private float minUpForce = 20f;
    private float maxUpForce = 30f;

    public StateGliding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        // 초기 각도 세팅
        controller.transform.localRotation = Quaternion.Euler(0, 0, EulerToAngle(controller.initialAngle.z));

        // velocity 적용 -> 발사
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
        // 방향 업데이트
        direction = controller.transform.right;

        // 회전 속도 업데이트
        SetRotSpeed(controller.velocity.x);

        //resistance 값 업데이트
        SetResistance(controller.transform.localEulerAngles);
        controller.velocity += new Vector3(airResistance, gravity, 0) * Time.deltaTime;
        //controller.velocity += 
    }

    public void RotatePlane(bool up)
    {   
        if (up && launchSuccess && !upLimited)
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
        // 앵글 -> 백분율
        localEulerAngle.z = EulerToAngle(localEulerAngle.z);
        var anglePercentage = (localEulerAngle.z - minAngle) / (maxAngle - minAngle) * 100f;

        // 앵글 - 저항값, upForce 맵핑
        airResistance = anglePercentage / 100 * (maxAirResistance - minAirResistance) + minAirResistance;
        upForce = anglePercentage / 100 * (maxUpForce - minUpForce) + minUpForce;

        // 최소값 세팅
        if(controller.velocity.x < 0) // 뒤로가는 거 막음
        {
            airResistance = 0;
        }
        //if(controller.velocity.x <= controller.velocity.y) // y값 증가 한계선
        //{
        //    upLimited = true;
            // y값 못 올라가도록 세팅(y저항값 최대) & x저항값은 최소
            //gravity = maxgravity;
            //UnityEditor.EditorApplication.isPaused = true;
        //}
        //else
        //{
        //    upLimited = false;
        //}
        Debug.Log("gravity : " + gravity);
        Debug.Log("air : " + airResistance);
        Debug.Log("velocity : " + controller.velocity);
    }

    public void SetXYSpeed()
    {
        // 가속 처리

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
