using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public StateMachine stateMachine { get; private set; }

    public float initialSpeed;
    public Quaternion initialAngle;
    public float maxAngle { get; private set; } = 80f;
    public float minAngle { get; private set; } = -80f;
    public float maxSpeed; // �ʱ� �ӷ� X, �÷��� �� �ִ� �ӷ�
    public float frontSpeed; // ������

    public Rigidbody rb { get; private set; } // �浹 ó����
    public bool launchSuccess = false;

    public GameObject speedBar { get; private set; }
    public GameObject angleBar { get; private set; }

    public Vector3 velocity = new Vector3 (0, 0, 0);
    public float distance = 0f;
    public float altitude = 1f;
    public float altitudeRatio = 10f; // ���ؾ� ��
    public float fuelTimer = 5f;


    private AirflowSpwaner airflowSpwaner;
    private ObstacleSpawner birdSpawner;
    public LinkedList<AirflowSystem> airflows = new LinkedList<AirflowSystem>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        speedBar = GameObject.FindWithTag("SpeedBar");
        angleBar = transform.GetChild(transform.childCount - 1).GetChild(transform.childCount - 1).gameObject;
        GameManager.instance.SetBoardLength(maxSpeed);
        airflowSpwaner = GetComponent<AirflowSpwaner>();
        birdSpawner = GetComponent<ObstacleSpawner>();
        birdSpawner.enabled = false;
        airflowSpwaner.enabled = false;
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
        UIManager.instance.UpdateVelocityText(velocity.x);
        UIManager.instance.UpdateAltitudeText(altitude);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Jump"))
        {
            // ���� ���� (Ready -> Gliding)
            stateMachine.AddState(StateName.Gliding, new StateGliding(this));
            StateGliding stateGliding = (StateGliding)stateMachine.GetState(StateName.Gliding);
            angleBar.SetActive(false);
            stateMachine?.ChangeState(StateName.Gliding);
            airflowSpwaner.enabled = true;
            birdSpawner.enabled = true;
        }

        if (other.CompareTag("Floor"))
        {
            // ���� ���� ( Gliding -> Landing )
            stateMachine.AddState(StateName.Landing, new StateLanding(this));
            stateMachine?.ChangeState(StateName.Landing);
            airflowSpwaner.spawnStop = true;
            birdSpawner.spawnStop = true;
        }

        if (other.CompareTag("Airflow"))
        {
            if(!airflows.Contains(other.GetComponent<AirflowSystem>()))
            {
                airflows.AddLast(other.GetComponent<AirflowSystem>());
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Airflow"))
        {
            if (airflows.Contains(other.GetComponent<AirflowSystem>()))
            {
                airflows.Remove(other.GetComponent<AirflowSystem>());
            }
        }
    }

    public void InitStateMachine()
    {
        // original code
        stateMachine = new StateMachine(StateName.Ready, new StateReady(this));

        
    }
}
