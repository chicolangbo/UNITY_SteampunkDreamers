using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    None,
    Speed,
    Angle,
    NoControll
}

public abstract class Obstacle : PoolAble
{
    [SerializeField]
    public new string name;
    public float speed;
    public EffectType type;
    public float effectTimer;
    public float effectStrength;
    public ParticleSystem particle;
    public AudioSource audioSource;
    public Rigidbody rb;

    public PlayerController playerController;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void FixedUpdate()
    {
        rb.AddForce(speed,0,0);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            CollideEffect();
            OnDie();
        }
    }

    public abstract void CollideEffect();

    public abstract void OnDie();
}
