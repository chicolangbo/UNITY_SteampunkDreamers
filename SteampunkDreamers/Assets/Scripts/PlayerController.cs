using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // ���ѻ��¸ӽ�
    public StateMachine stateMachine { get; private set; }

    public float initialFrontSpeed;
    public Quaternion initialRotation;
    public float maxAngle { get; private set; } = 50f;
    public float minAngle { get; private set; } = -50f;

    public Rigidbody rb;
    private float flightForce = 10f;

    public float distance = 0f;
    public float frontSpeed;
    public float altitude = 1f;
    public float altitudeRatio = 10f; // ���ؾ� ��

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initialRotation = new Quaternion(0, 0, 0, 1);
    }

    private void Start()
    {
        //StartCoroutine(DistanceLog());
        InitStateMachine();
    }

    private void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }

    private void Update()
    {
        stateMachine?.UpdateState();

        altitude = rb.position.y * altitudeRatio;
        UIManager.instance.UpdateDistanceText(distance);
        UIManager.instance.UpdateVelocityText(initialFrontSpeed);
        UIManager.instance.UpdateAltitudeText(altitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            // ���� ���� ����

            stateMachine.AddState(StateName.Landing, new StateLanding(this));
            stateMachine?.ChangeState(StateName.Landing);
        }
    }

    public void InitStateMachine()
    {
        // ���߿� Ready�� �ٲ�� ��
        stateMachine = new StateMachine(StateName.Gliding, new StateGliding(this));
    }

    private IEnumerator DistanceLog()
    {
        while(true)
        {
            //Debug.Log("distance : " + distance);
            //yield return new WaitForSeconds(1f);
        }
    }
}
