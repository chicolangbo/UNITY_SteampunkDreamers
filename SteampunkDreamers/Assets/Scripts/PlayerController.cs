using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public StateMachine stateMachine { get; private set; }

    public float initialFrontSpeed;
    public Quaternion initialRotation;
    public float maxAngle { get; private set; } = 50f;
    public float minAngle { get; private set; } = -50f;

    public Rigidbody rb { get; private set; }
    private float flightForce = 10f;

    public float distance = 0f;
    public float frontSpeed;
    public float altitude = 1f;
    public float altitudeRatio = 10f; // 정해야 함

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initialRotation = new Quaternion(0, 0, 0, 1);
    }

    private void Start()
    {
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
        UIManager.instance.UpdateVelocityText(frontSpeed);
        UIManager.instance.UpdateAltitudeText(altitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            // 상태 패턴 변경

            stateMachine.AddState(StateName.Landing, new StateLanding(this));
            stateMachine?.ChangeState(StateName.Landing);
        }
    }

    public void InitStateMachine()
    {
        // 나중에 Ready로 바꿔야 함
        stateMachine = new StateMachine(StateName.Gliding, new StateGliding(this));
    }
}
