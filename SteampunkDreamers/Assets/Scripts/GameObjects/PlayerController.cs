using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    public StateMachine stateMachine { get; private set; }

    public float initialSpeed;
    public Quaternion initialAngle;
    public float maxAngle { get; private set; } = 80f;
    public float minAngle { get; private set; } = -80f;
    public float minSpeed;
    public float maxSpeed;
    public float frontSpeed; // ������

    public Rigidbody rb { get; private set; } // �浹 ó����
    public bool launchSuccess = false;
    public bool airshipColiide = false;

    public GameObject speedBar { get; private set; }
    public GameObject angleBar { get; private set; }

    public Vector3 velocity = new Vector3 (0, 0, 0);
    public float distance = 0f;
    public float altitude = 1f;
    public float altitudeRatio = 10f; // ���ؾ� ��
    public float fuelTimer = 10f;

    public ParticleSystem electronicParticle; // Nimbus collide
    public ParticleSystem birdParticle; // Bird collide
    public ParticleSystem explosionParticle; // Airship collide
    public ParticleSystem fireParticle; // Airship collide

    public LinkedList<AirflowSystem> airflows = new LinkedList<AirflowSystem>();

    private List<Spawner> spawners = new List<Spawner>();
    public bool shieldOn;
    public GameObject shield;
    public bool once;

    public float boosterSpeed;
    public bool boosterOn;

    // ����
    public int coinCount;
    public float maxSpeedReached;
    public float maxAltitudeReached;

    public float propellerSpeed { get; private set; }
    private Transform propeller;

    public ReinforceTable table { get; private set; }

    private void Awake()
    {
        PlayDataManager.Init();

        // �� �ʱ�ȭ
        maxSpeedReached = 0;
        maxAltitudeReached = 0;
        GameManager.instance.SetPlayer(this.gameObject);
        GameManager.instance.SetBoardLength(maxSpeed);

        // ������Ʈ ����
        rb = GetComponent<Rigidbody>();
        speedBar = GameObject.FindGameObjectWithTag("SpeedBar");
        angleBar = transform.GetChild(0).GetChild(1).gameObject;
        shield = transform.GetChild(1).gameObject;
        propeller = transform.GetChild(3).GetChild(0).GetChild(2).transform;

        // ������ �ʱ�ȭ
        var mapObjectCount = ObjectPoolManager.instance.GetComponents<MapObjectSpawner>().Length;
        spawners.Add(ObjectPoolManager.instance.GetComponent<AirflowSpwaner>());
        spawners[0].enabled = false;
        for(int i = 1; i< mapObjectCount+1; ++i)
        {
            spawners.Add(ObjectPoolManager.instance.GetComponents<MapObjectSpawner>()[i - 1]);
            spawners[i].enabled = false;
            spawners[i].spawnStop = false;
        }
        spawners.Add(ObjectPoolManager.instance.GetComponent<CoinSpawner>());
        spawners[spawners.Count - 1].enabled = false;
    }

    private void Start()
    {
        table = new ReinforceTable();
        // �ʱ� �ӷ� ���׷��̵�
        var initialSpeedUpValue = table.GetData(PlayDataManager.data.reinforceDatas["StartSpeedUpgrade"].id).VALUE;
        minSpeed += initialSpeedUpValue;
        maxSpeed += initialSpeedUpValue;
        //Debug.Log("�ʱ� �ӷ� ���� : " + PlayDataManager.data.reinforceDatas["StartSpeedUpgrade"].level + " / ���� min�� : " + minSpeed + " / ���� max�� : " + maxSpeed);

        // ���� ���� ���׷��̵�
        var fuelUpValue = table.GetData(PlayDataManager.data.reinforceDatas["MoreFuelUpgrade"].id).VALUE;
        if(fuelUpValue > 0)
        {
            fuelTimer = fuelUpValue;
        }
        //Debug.Log("���� ���� ���� : " + PlayDataManager.data.reinforceDatas["MoreFuelUpgrade"].level + " / ���� ���ᰪ : " + fuelTimer);

        InitStateMachine();
    }

    private void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
        transform.position += velocity * Time.deltaTime;
        RotatePropeller();
    }

    private void Update()
    {
        stateMachine?.UpdateState();

        if(shieldOn && once)
        {
            shield.SetActive(true);
            once = false;
            Invoke("ShieldRemove", 5f);
        }
        if(boosterOn)
        {
            Booster(boosterSpeed);
            if(once)
            {
                Invoke("BoosterRemove", 5f);
                once = false;
            }
        }

        if(velocity.x > maxSpeedReached)
        {
            maxSpeedReached = velocity.x;
        }
        if(altitude > maxAltitudeReached)
        {
            maxAltitudeReached = altitude;
        }

        // �ΰ��� ���� UI ������Ʈ
        altitude = transform.position.y * altitudeRatio;
        UIManager.instance.UpdateDistanceText(distance);
        UIManager.instance.UpdateVelocityText(velocity.x);
        UIManager.instance.UpdateAltitudeText(altitude);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Jump") && angleBar.activeSelf)
        {
            // ���� ���� (Ready -> Gliding)
            stateMachine.AddState(StateName.Gliding, new StateGliding(this));
            StateGliding stateGliding = (StateGliding)stateMachine.GetState(StateName.Gliding);
            angleBar.SetActive(false);
            stateMachine?.ChangeState(StateName.Gliding);
            foreach(var s in spawners)
            {
                s.enabled = true;
                s.spawnStop = false;
            }

        }

        if (other.CompareTag("Floor"))
        {
            // ���� ���� ( Gliding -> Landing )
            stateMachine.AddState(StateName.Landing, new StateLanding(this));
            stateMachine?.ChangeState(StateName.Landing);
            foreach (var s in spawners)
            {
                s.spawnStop = true;
            }
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

    public void ShieldRemove()
    {
        shieldOn = false;
        shield.SetActive(false);
    }

    public void Booster(float boosterSpeed)
    {
        frontSpeed += boosterSpeed;
    }

    public void BoosterRemove()
    {
        boosterOn = false;
        fireParticle.Stop();
        fireParticle.gameObject.GetComponent<AudioSource>().enabled = false;
    }

    public void RotatePropeller()
    {
        propeller.Rotate(0, 0, propellerSpeed * Time.deltaTime);
    }

    public void ChangePropellerSpeed(float speed)
    {
        propellerSpeed = speed;
    }
}
