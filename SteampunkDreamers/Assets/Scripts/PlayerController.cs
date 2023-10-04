using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public StateMachine stateMachine { get; private set; }

    public float initialSpeed;
    public Quaternion initialAngle;
    public float maxAngle { get; private set; } = 50f;
    public float minAngle { get; private set; } = -50f;
    public float maxSpeed;
    public Vector3 velocity = new Vector3 (0, 0, 0);

    public Rigidbody rb { get; private set; } // �浹 ó����
    public GameObject speedBar;
    public GameObject angleBar;
    public SpeedBarController speedBarController { get; private set; }
    public AngleBarController angleBarController { get; private set; }
    //private float flightForce = 10f;

    public float distance = 0f;
    public float speed = 0f; // velocity�� x��
    public float altitude = 1f;
    public float altitudeRatio = 10f; // ���ؾ� ��

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        speedBarController = speedBar.GetComponent<SpeedBarController>();
        angleBarController = angleBar.GetComponent<AngleBarController>();
    }

    private void Start()
    {
        InitStateMachine();
    }

    private void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
        transform.position += velocity * Time.deltaTime;
    }

    private void Update()
    {
        stateMachine?.UpdateState();

        // �ΰ��� ���� UI ������Ʈ
        altitude = transform.position.y * altitudeRatio;
        UIManager.instance.UpdateDistanceText(distance);
        UIManager.instance.UpdateVelocityText(speed);
        UIManager.instance.UpdateAltitudeText(altitude);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            // ���� ���� ���� ( Gliding -> Landing )
            stateMachine.AddState(StateName.Landing, new StateLanding(this));
            stateMachine?.ChangeState(StateName.Landing);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Jump"))
        {
            // ���� ���� ���� (Angle -> Gliding)
            stateMachine.AddState(StateName.Gliding, new StateGliding(this));
            stateMachine?.ChangeState(StateName.Gliding);
        }
    }

    public void InitStateMachine()
    {
        stateMachine = new StateMachine(StateName.Ready, new StateReady(this));
    }
}
