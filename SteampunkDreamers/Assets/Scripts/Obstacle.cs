using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Speed,
    Angle,
    NoControll
}

public class Obstacle : PoolAble
{
    [SerializeField]
    public string name;
    public float speed;
    public EffectType type;
    public float effectTimer;
    public float effectStrength;
    public ParticleSystem particle;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public Rigidbody rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        rb.AddForce(speed,0,0);
    }

    public void ActivateEffect()
    {

    }

    public void Effect()
    {

    }
}
