using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapObject : PoolAble
{
    [SerializeField]
    private Transform bg1;
    private Transform bg2;
    private float bgWidth;
    private float borderX;
    public new string name;
    public float speed;
    public float effectTimer;
    public float effectStrength;
    public AudioSource audioSource;
    public Rigidbody rb;

    public PlayerController playerController;

    public event Action onDisappear;

    public void Awake()
    {
        bg1 = GameObject.FindGameObjectWithTag("Bg1").transform;
        bg2 = GameObject.FindGameObjectWithTag("Bg2").transform;
        bgWidth = GameObject.FindGameObjectWithTag("Bg1").GetComponent<BoxCollider>().size.x;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        playerController = GameManager.instance.player.GetComponent<PlayerController>();
    }

    public void FixedUpdate()
    {
        //rb.AddForce(speed,0,0);
        var tempPos = new Vector3(speed, 0, 0);
        transform.position += tempPos * Time.deltaTime;
    }

    private void Update()
    {
        borderX = (bg1.position.x > bg2.position.x)? bg1.position.x + bgWidth : bg2.position.x + bgWidth;
        if(transform.position.x > borderX)
        {
            Debug.Log("¸Ê ¹Ù±ù¿¡¼­ »èÁ¦");
            ReleaseObject();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Shield"))
        {
            CollideEffect();
            onDisappear();
        }
    }

    public virtual void CollideEffect()
    {

    }
}
