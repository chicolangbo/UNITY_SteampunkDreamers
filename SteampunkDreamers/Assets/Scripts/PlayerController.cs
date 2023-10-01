using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // 유한상태머신
    public StateMachine stateMachine { get; private set; }

    public float initialFrontSpeed;
    public Rigidbody rb;
    private float flightForce = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        InitStateMachine();
        //StartCoroutine(DistanceLog());
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
            // 상태 패턴 변경
            stateMachine?.ChangeState(StateName.Landing);
        }
    }

    public void InitStateMachine()
    {
        // 나중에 Ready로 바꿔야 함
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
