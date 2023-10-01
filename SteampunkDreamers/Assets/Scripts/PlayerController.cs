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

    public Rigidbody rb;
    private float flightForce = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initialRotation = new Quaternion(0, 0, 30f, 1);
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            // ���� ���� ����
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
